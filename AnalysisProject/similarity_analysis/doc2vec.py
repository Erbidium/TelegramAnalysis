import gensim.downloader as api

def get_pretrained_model_ruscorpora():
    return api.load('word2vec-ruscorpora-300')