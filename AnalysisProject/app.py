from sqlalchemy import create_engine, select
from post import Post
from swagger_gen.lib.wrappers import swagger_metadata
from swagger_gen.swagger import Swagger
from flask import Flask, jsonify, request
from sqlalchemy.orm import sessionmaker
import spacy
import re

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

    if request.method != 'POST':
        print('Request method is not POST')
        return jsonify({"error": "Method not allowed"}), 405

    input_text = request.json.get('text', '')
    print(f'Input text: {input_text}')

    nlp = spacy.load('ru_core_news_md')

    print("loaded spacy")

    processed_input = process_text(input_text, nlp)
    if not processed_input:
        print('Invalid input text')
        return jsonify({"error": "Invalid input text"}), 400

    Session = sessionmaker(bind=engine)
    session = Session()
    stmt = select(Post)
    posts = session.scalars(stmt)

    similar_posts = []

    input_doc = nlp(' '.join(processed_input))

    print('processed input')

    for post in posts:
        processed_post = process_text(post.text, nlp)
        if len(processed_post) == 0:
            continue

        similarity_result = input_doc.similarity(nlp(' '.join(processed_post)))

        if similarity_result > 0.8:
            similar_posts.append({
                "post_id": post.id,
                "text": post.text,
                "similarity": similarity_result
            })

            print(post.text)

    print(f'Similar posts: {similar_posts}')
    return jsonify(similar_posts)

def process_text(text, nlp):
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
    app.run()
