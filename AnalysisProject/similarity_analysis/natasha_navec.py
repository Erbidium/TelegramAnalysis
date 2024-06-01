import re

import nltk
from natasha import Segmenter, MorphVocab, NewsEmbedding, NewsMorphTagger, Doc
from navec import Navec
import os
import tarfile
import numpy as np

segmenter = Segmenter()
morph_vocab = MorphVocab()
emb = NewsEmbedding()
morph_tagger = NewsMorphTagger(emb)

navec_path = 'C:\\Users\\Acer\\Downloads\\navec_hudlit_v1_12B_500K_300d_100q.tar'

if not os.path.exists(navec_path):
    raise FileNotFoundError(f"File not found: {navec_path}")
if not tarfile.is_tarfile(navec_path):
    raise tarfile.ReadError(f"Invalid tar file: {navec_path}")

navec = Navec.load(navec_path)

def process_text_navec(text):
    stop_words = set(nltk.corpus.stopwords.words('russian'))

    text = re.sub(r'\*\*\w+\*\*', '', text)
    text = re.sub(r'http\S+', '', text)
    text = re.sub(r'#\S+', '', text)
    text = re.sub(r'@\S+', '', text)
    text = re.sub(r'\b\d+\b(?!\d{4}\b)', '', text)

    tokens = nltk.word_tokenize(text.lower())
    tokens = [word for word in tokens if word.isalpha() and word not in stop_words]

    doc = Doc(' '.join(tokens))
    doc.segment(segmenter)
    doc.tag_morph(morph_tagger)
    for token in doc.tokens:
        token.lemmatize(morph_vocab)
    lemmatized_tokens = [token.lemma for token in doc.tokens]

    return lemmatized_tokens

def get_embedding(text_tokens):
    embeddings = [navec[token] for token in text_tokens if token in navec]
    if embeddings:
        return np.mean(embeddings, axis=0)
    else:
        return np.zeros(navec.pq.dim)

def cosine_similarity(vec1, vec2):
    dot_product = np.dot(vec1, vec2)
    norm_vec1 = np.linalg.norm(vec1)
    norm_vec2 = np.linalg.norm(vec2)
    if norm_vec1 == 0 or norm_vec2 == 0:
        return 0.0
    return dot_product / (norm_vec1 * norm_vec2)