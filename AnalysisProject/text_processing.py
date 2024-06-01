import re
import nltk
import pymorphy2
import spacy

nltk.download('punkt')
nltk.download('stopwords')

# Initialize lemmatizer
morph = pymorphy2.MorphAnalyzer()

nlp = spacy.load('ru_core_news_md')

def process_text_nltk(text):
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

def process_text_spacy(text):
    stop_words = nlp.Defaults.stop_words
    text = re.sub(r'\*\*\w+\*\*', '', text)
    text = re.sub(r'http\S+', '', text)
    text = re.sub(r'\b\d+\b(?!\d{4}\b)', '', text)
    doc = nlp(text)
    lemmatized_words = [token.lemma_ for token in doc]
    processed_words = [word for word in lemmatized_words if
                       word not in stop_words and re.match(r'\w+', word) and len(word) >= 3]
    return processed_words