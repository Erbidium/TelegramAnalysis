import os

import spacy
from flask_cors import cross_origin, CORS
from gensim.models import Word2Vec
from gensim.models.doc2vec import TaggedDocument
from sqlalchemy import create_engine, select
from models.channel import Channel
from models.post import Post
from swagger_gen.lib.wrappers import swagger_metadata
from swagger_gen.swagger import Swagger
from flask import Flask, jsonify, request
from sqlalchemy.orm import sessionmaker
from posts_search import find_spread_by_root
from similarity_analysis.doc2vec import get_pretrained_model_ruscorpora
from similarity_analysis.natasha_navec import get_embedding
from text_processing import process_text_spacy, load_nltk, process_text_nltk

# servername = "localhost"
# username = "sa"
# dbname = os.getenv("SQLSERVER_DB", "ParsingDb")
# password = os.getenv("SQLSERVER_PASSWORD", "your_password")
# connection_string = f"mssql+pyodbc://{username}:{password}@{servername}/{dbname}?driver=ODBC+Driver+17+for+SQL+Server"

servername = "(localdb)\MSSQLLocalDB"
dbname = "parsingdb"
engine = create_engine(
    'mssql+pyodbc://@' + servername + '/' + dbname + '?trusted_connection=yes&TrustServerCertificate=yes&MARS_Connection=Yes&driver=ODBC+Driver+17+for+SQL+Server'
)

app = Flask(__name__)
cors = CORS(app)

load_nltk()

nlp = spacy.load('ru_core_news_md')


@app.route('/get_data', methods=['POST'])
@cross_origin(supports_credentials=True)
@swagger_metadata(
    summary='Data endpoint',
    description='This is a sample endpoint'
)
def get_data():
    input_text = request.json.get('text', '')

    processed_input = process_text_nltk(input_text)
    if not processed_input:
        print('Invalid input text')
        return jsonify({"error": "Invalid input text"}), 400

    # Natasha
    input_embedding = get_embedding(processed_input)

    Session = sessionmaker(bind=engine)
    session = Session()

    posts = session.scalars(select(Post)).all()
    channels = session.scalars(select(Channel)).all()

    posts_ordered_by_date = sorted(posts, key=lambda p: p.createdat)
    processed_posts = []
    for post in posts_ordered_by_date:
        processed_post = process_text_nltk(post.text)
        processed_posts.append(processed_post)


    documents = []
    counter = 0
    #for pr in processed_posts:
    documents.append(TaggedDocument(words=processed_input, tags=[f'doc{counter}']))
    counter += 1
    # Train the Doc2Vec model on posts texts
    # model = Doc2Vec(documents, vector_size=100, window=5, min_count=1, workers=8)

    # Train the Word2Vec model only on searched post
    model = Word2Vec([processed_input], vector_size=100, window=5, min_count=1, workers=8)

    # Train the Word2Vec model on posts texts
    # model = Word2Vec(processed_posts, vector_size=100, window=5, min_count=1, workers=8)

    # model = get_pretrained_model_ruscorpora()

    # find the oldest post
    oldest_post = None
    oldest_post_index = None
    oldest_post_processed = None

    similar_posts = []

    for index, post in enumerate(posts_ordered_by_date):
        processed_post = processed_posts[index]
        if len(processed_post) == 0:
            continue

        # Natasha
        # post_embedding = get_embedding(processed_post)
        # similarity_result = cosine_similarity(input_embedding, post_embedding)

        similarity_result = model.wv.n_similarity(processed_input, processed_post)
        print(similarity_result)

        if similarity_result > 0.5:
            oldest_post = post
            oldest_post_index = index
            oldest_post_processed = processed_post

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

    find_spread_by_root(oldest_post, oldest_post_index, oldest_post_processed, posts_ordered_by_date, similar_posts, model, processed_posts, 0, channels, processed_input)

    print(f'Similar posts: {similar_posts}')
    return jsonify(similar_posts)

swagger = Swagger(
    app=app,
    title='app'
)
swagger.configure()

if __name__ == '__main__':
    app.run(debug=True)
