import re
import nltk
from gensim.models import Word2Vec
from sqlalchemy import create_engine, select
from post import Post
from swagger_gen.lib.wrappers import swagger_metadata
from swagger_gen.swagger import Swagger
from flask import Flask, jsonify, request
from sqlalchemy.orm import sessionmaker
import pymorphy2
import spacy

# Download stopwords
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

@app.route('/get_data', methods=['POST'])
@swagger_metadata(
    summary='Data endpoint',
    description='This is a sample endpoint'
)
def get_data():
    print('Request received')

    input_text = request.json.get('text', '')
    print(f'Input text: {input_text}')

    # nlp = spacy.load('ru_core_news_md')
    # print("loaded spacy")

    processed_input = process_text(input_text)
    if not processed_input:
        print('Invalid input text')
        return jsonify({"error": "Invalid input text"}), 400

    Session = sessionmaker(bind=engine)
    session = Session()
    stmt = select(Post)
    posts = session.scalars(stmt)

    similar_posts = []

    # Train the Word2Vec model on the input text
    model = Word2Vec([processed_input], vector_size=100, window=5, min_count=1, workers=8)

    print('processed input')

    posts_ordered_by_date = sorted(posts, key=lambda p: p.createdat)

    # find the oldest post
    oldest_post = None
    oldest_post_index = None
    for index, post in enumerate(posts_ordered_by_date):
        processed_post = process_text(post.text)
        if len(processed_post) == 0:
            continue

        # Compute similarity using Word2Vec
        similarity_result = model.wv.n_similarity(processed_input, processed_post)

        if similarity_result > 0.5:
            oldest_post = post
            oldest_post_index = index

            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result),  # Convert to float
                "created_at": post.createdat,
                "root_id": None
            })

            print(post.text)
            print('oldest post')
            break

    if oldest_post_index is None:
        return jsonify({"error": "Such post is not found"}), 400

    find_spread_by_root(oldest_post, oldest_post_index, posts_ordered_by_date, similar_posts, model)

    print(f'Similar posts: {similar_posts}')
    return jsonify(similar_posts)

def find_spread_by_root(root_post, post_index, posts_ordered_by_date, similar_posts, model):
    processed_post_text = process_text(root_post.text)

    for index, post in enumerate(posts_ordered_by_date):
        if index <= post_index:
            continue

        processed_post = process_text(post.text)
        if len(processed_post) == 0:
            continue

        # Compute similarity using Word2Vec
        similarity_result = model.wv.n_similarity(processed_post_text, processed_post)

        # Convert numpy.float32 to float for JSON serialization
        post_id = post.id
        if similarity_result > 0.5 and not any(post_id == p['post_id'] for p in similar_posts):
            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result),  # Convert to float
                "created_at": post.createdat,
                "root_id": root_post.id
            })

            find_spread_by_root(post, index, posts_ordered_by_date, similar_posts, model)

            print(post.text)
            print('oldest post')

            break

def process_text(text):
    stop_words = set(nltk.corpus.stopwords.words('russian'))

    text = re.sub(r'\*\*\w+\*\*', '', text)
    text = re.sub(r'http\S+', '', text)
    text = re.sub(r'#\S+', '', text)
    text = re.sub(r'@\S+', '', text)
    text = re.sub(r'\b\d+\b(?!\d{4}\b)', '', text)

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
