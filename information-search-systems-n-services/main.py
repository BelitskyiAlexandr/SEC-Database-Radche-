import os
import nltk
from nltk.corpus import stopwords, wordnet
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer
from collections import defaultdict

nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')

# Part of speech for lemmer
def get_wordnet_pos(word):
    tag = nltk.pos_tag([word])[0][1][0].upper()
    tag_dict = {"J": wordnet.ADJ, "N": wordnet.NOUN, "V": wordnet.VERB, "R": wordnet.ADV}
    return tag_dict.get(tag, wordnet.NOUN)

def preprocess_text(text):
    text = text.lower()
    
    # tokenize + filter stopwords
    tokens = word_tokenize(text)
    stop_words = set(stopwords.words('english'))
    tokens = [token for token in tokens if token.isalnum() and token not in stop_words]
    
    # lemmer
    lemmatizer = WordNetLemmatizer()
    tokens = [lemmatizer.lemmatize(token, get_wordnet_pos(token)) for token in tokens]
    return tokens

def build_inverted_index(docs):
    inverted_index = defaultdict(list)
    for doc_id, doc in enumerate(docs):
        tokens = preprocess_text(doc)
        for token in tokens:
            inverted_index[token].append(doc_id)
    return inverted_index

def search(query, inverted_index, docs):
    query_tokens = preprocess_text(query)
    results = set(range(len(docs)))  
    for token in query_tokens:
        if token in inverted_index:
            results = results.intersection(inverted_index[token])
        else:
            results = set()  
    return results

def main():
    doc_filenames = ['doc1.txt', 'doc2.txt', 'doc3.txt']
    docs = []
    for filename in doc_filenames:
        with open(filename, 'r', encoding='utf-8') as file:
            docs.append(file.read())

    inverted_index = build_inverted_index(docs)

    while True:
        query = input("Enter your search query (or 'exit'): ")
        if query.lower() == 'exit':
            break

        results = search(query, inverted_index, docs)
        if results:
            print(f"Found in docs: {', '.join([doc_filenames[i] for i in results])}")
        else:
            print("Nothing was found.")

if __name__ == "__main__":
    main()
