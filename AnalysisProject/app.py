import re
import nltk
from flask_cors import cross_origin, CORS
from gensim.models import Word2Vec, Doc2Vec
from gensim.models.doc2vec import TaggedDocument
from sqlalchemy import create_engine, select
from channel import Channel
from post import Post
from swagger_gen.lib.wrappers import swagger_metadata
from swagger_gen.swagger import Swagger
from flask import Flask, jsonify, request
from sqlalchemy.orm import sessionmaker
import pymorphy2
import spacy


nltk.download('punkt')
nltk.download('stopwords')

# Initialize lemmatizer
morph = pymorphy2.MorphAnalyzer()

servername = "(localdb)\MSSQLLocalDB"
dbname = "parsingdb"
engine = create_engine(
    'mssql+pyodbc://@' + servername + '/' + dbname + '?trusted_connection=yes&TrustServerCertificate=yes&MARS_Connection=Yes&driver=ODBC+Driver+17+for+SQL+Server'
)

app = Flask(__name__)
cors = CORS(app)

@app.route('/get_data', methods=['POST'])
@cross_origin(supports_credentials=True)
@swagger_metadata(
    summary='Data endpoint',
    description='This is a sample endpoint'
)
def get_data():
    input_text = request.json.get('text', '')

    nlp = spacy.load('ru_core_news_md')

    processed_input = process_text(None, input_text, nlp)
    if not processed_input:
        print('Invalid input text')
        return jsonify({"error": "Invalid input text"}), 400

    Session = sessionmaker(bind=engine)
    session = Session()

    posts = session.scalars(select(Post)).all()
    channels = session.scalars(select(Channel)).all()

    posts_ordered_by_date = sorted(posts, key=lambda p: p.createdat)
    processed_posts = []
    for post in posts_ordered_by_date:
        processed_post = process_text(post, post.text, nlp)
        processed_posts.append(processed_post)

    # Train the Word2Vec model on the input text
    documents = []
    counter = 0
    #for pr in processed_posts:
    documents.append(TaggedDocument(words=processed_input, tags=[f'doc{counter}']))
    counter += 1
    # model = Doc2Vec(documents, vector_size=100, window=5, min_count=1, workers=8)
    model = Word2Vec([processed_input], vector_size=100, window=5, min_count=1, workers=8)
    # model = Word2Vec(processed_posts, vector_size=100, window=5, min_count=1, workers=8)

    # find the oldest post
    oldest_post = None
    oldest_post_index = None
    oldest_post_processed = None

    similar_posts = []

    for index, post in enumerate(posts_ordered_by_date):
        processed_post = processed_posts[index]
        if len(processed_post) == 0:
            continue

        similarity_result = model.wv.n_similarity(processed_input, processed_post)

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

    find_spread_by_root(oldest_post, oldest_post_index, oldest_post_processed, posts_ordered_by_date, similar_posts, model, nlp, processed_posts, 0, channels, processed_input)

    print(f'Similar posts: {similar_posts}')
    return jsonify(similar_posts)

def find_spread_by_root(root_post, post_index, processed_post_text, posts_ordered_by_date, similar_posts, model, nlp, processed_posts, depth, channels, processed_input):
    for index, post in enumerate(posts_ordered_by_date):
        if index <= post_index:
            continue

        processed_post = processed_posts[index]
        if len(processed_post) == 0:
            continue

        similarity_result = model.wv.n_similarity(processed_post_text, processed_post)

        post_id = post.id

        channel = next((channel for channel in channels if channel.id == post.channelid), None)

        # replace post in graph if similarity is higher
        post_search_result_in_graph = [p for p in similar_posts if post_id == p['post_id']]
        given_post_in_graph = post_search_result_in_graph[0] if post_search_result_in_graph else None

        if similarity_result > 0.5:
            if given_post_in_graph and given_post_in_graph.similarity >= similarity_result:
                continue
            elif given_post_in_graph and given_post_in_graph.similarity < similarity_result:
                similar_posts = [p for p in similar_posts if p['post_id'] != post_id]

            similarity_result_with_wanted = model.wv.n_similarity(processed_input, processed_post)

            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result),
                "similarity_with_wanted": float(similarity_result_with_wanted),
                "created_at": post.createdat,
                "root_id": root_post.id,
                "channel_title": channel.title
            })

            find_spread_by_root(post, index, processed_post, posts_ordered_by_date, similar_posts, model, nlp, processed_posts, depth + 1, channels, processed_input)
            break

def process_text(post, text, nlp):
    stop_words = set(nltk.corpus.stopwords.words('russian'))

    text = re.sub(r'\*\*\w+\*\*', '', text)
    text = re.sub(r'http\S+', '', text)
    text = re.sub(r'#\S+', '', text)
    text = re.sub(r'@\S+', '', text)
    text = re.sub(r'\b\d+\b(?!\d{4}\b)', '', text)

    if len(text) == 0:
        return []

    # Tokenize and remove stopwords
    tokens = nltk.word_tokenize(text.lower())
    tokens = [word for word in tokens if word.isalpha() and word not in stop_words]

    # Lemmatize tokens
    lemmatized_tokens = [morph.parse(token)[0].normal_form for token in tokens]

    return lemmatized_tokens

def process_text2(text, nlp):
    stop_words = nlp.Defaults.stop_words
    text = re.sub(r'\*\*\w+\*\*', '', text)
    text = re.sub(r'http\S+', '', text)
    text = re.sub(r'\b\d+\b(?!\d{4}\b)', '', text)
    doc = nlp(text)
    lemmatized_words = [token.lemma_ for token in doc]
    processed_words = [word for word in lemmatized_words if
                       word not in stop_words and re.match(r'\w+', word) and len(word) >= 3]
    return processed_words

swagger = Swagger(
    app=app,
    title='app'
)

swagger.configure()

if __name__ == '__main__':
    app.run(debug=True)
