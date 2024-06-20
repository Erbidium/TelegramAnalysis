from flask_cors import cross_origin, CORS
from sqlalchemy import create_engine, select
from models.channel import Channel
from models.post import Post
from swagger_gen.lib.wrappers import swagger_metadata
from swagger_gen.swagger import Swagger
from flask import Flask, jsonify, request
from sqlalchemy.orm import sessionmaker
from posts_search import find_spread_by_root
from sentence_transformers import SentenceTransformer, util
import os
os.environ['HF_HUB_DISABLE_SYMLINKS_WARNING'] = '1'

servername = "(localdb)\MSSQLLocalDB"
dbname = "parsingdb"
engine = create_engine(
    'mssql+pyodbc://@' + servername + '/' + dbname + '?trusted_connection=yes&TrustServerCertificate=yes&MARS_Connection=Yes&driver=ODBC+Driver+17+for+SQL+Server'
)

app = Flask(__name__)
cors = CORS(app)

# Load pre-trained Sentence-BERT model
model = SentenceTransformer('sentence-transformers/paraphrase-multilingual-MiniLM-L12-v2')


@app.route('/get_data', methods=['POST'])
@cross_origin(supports_credentials=True)
@swagger_metadata(
    summary='Data endpoint',
    description='This is a sample endpoint'
)
def get_data():
    input_text = request.json.get('text', '')

    if not input_text:
        print('Invalid input text')
        return jsonify({"error": "Invalid input text"}), 400


    input_embedding = model.encode(input_text, convert_to_tensor=True)

    Session = sessionmaker(bind=engine)
    session = Session()

    posts = session.scalars(select(Post)).all()
    channels = session.scalars(select(Channel)).all()

    posts_ordered_by_date = sorted(posts, key=lambda p: p.createdat)
    processed_posts = [post.text for post in posts_ordered_by_date]

    # Compute embeddings for all posts
    post_embeddings = model.encode(processed_posts, convert_to_tensor=True)

    # Find the most similar posts
    cosine_scores = util.pytorch_cos_sim(input_embedding, post_embeddings)[0]

    # find the oldest post
    oldest_post = None
    oldest_post_index = None

    similar_posts = []

    for index, post in enumerate(posts_ordered_by_date):
        processed_post = processed_posts[index]
        if len(processed_post) == 0:
            continue

        similarity_result = cosine_scores[index]
        print(similarity_result)

        if similarity_result > 0.7:
            oldest_post = post
            oldest_post_index = index

            channel = next((channel for channel in channels if channel.id == post.channelid), None)

            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result),
                "similarity_with_wanted": float(similarity_result),
                "created_at": post.createdat,
                "root_id": None,
                "channel_title": channel.title
            })

            break

    if oldest_post_index is None:
        return jsonify({"error": "Such post is not found"}), 400

    cosine_scores_input = util.pytorch_cos_sim(input_embedding, post_embeddings)[0]
    find_spread_by_root(oldest_post, oldest_post_index, post_embeddings[oldest_post_index], posts_ordered_by_date, similar_posts, model, post_embeddings, channels, cosine_scores_input)

    print(f'Similar posts: {similar_posts}')
    return jsonify(similar_posts)

swagger = Swagger(
    app=app,
    title='app'
)
swagger.configure()

if __name__ == '__main__':
    app.run(debug=True)
