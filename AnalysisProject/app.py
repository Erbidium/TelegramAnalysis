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

    nlp = spacy.load('ru_core_news_md')
    print("loaded spacy")

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

    for post in posts:
        processed_post = process_text(post.text)
        if len(processed_post) == 0:
            continue

        # Compute similarity using Word2Vec
        similarity_result = model.wv.n_similarity(processed_input, processed_post)

        # Convert numpy.float32 to float for JSON serialization
        if similarity_result > 0.5:
            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": float(similarity_result)  # Convert to float
            })

            print(post.text)

    print(f'Similar posts: {similar_posts}')
    return jsonify(similar_posts)

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
