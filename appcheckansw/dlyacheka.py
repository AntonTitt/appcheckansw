from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity
import string
import re
from pymorphy3 import MorphAnalyzer
import sys

model = SentenceTransformer('intfloat/multilingual-e5-large')  #сосал: 'paraphrase-multilingual-MiniLM-L12-v2'
morph = MorphAnalyzer()

#if(sys.argv.__len__!=3):
#    exit("sosal")

user_answer =sys.argv[1]# "париж красивый город"
correct_answer = sys.argv[2]#"Париж является столицей Франции."

def normalize_text(text):
    # Приводим к нижнему регистру + убираем пунктуацию
    text = re.sub(r'[^\w\s]', '', text.lower())
    
    # Лемматизация (опционально, но улучшает точность)
    words = text.split()
    lemmas = [morph.parse(word)[0].normal_form for word in words]
    return " ".join(lemmas)

# Пример
#user_text = normalize_text("Столица Франции — Париж!")  # "столица франция париж"
#correct_text = normalize_text("Париж является столицей Франции.")  # "париж являться столица франция"

def compare_answers(user_answer, correct_answer, threshold=0.75):
    # Нормализация
    user_norm = normalize_text(user_answer).translate(str.maketrans('', '', string.punctuation)).lower()
    correct_norm = normalize_text(correct_answer).translate(str.maketrans('', '', string.punctuation)).lower()
    
    # Эмбеддинги
    embeddings = model.encode([user_norm, correct_norm])
    similarity = cosine_similarity([embeddings[0]], [embeddings[1]])[0][0]
    
    # Проверка (можно добавить доп. логику)
    return similarity >= threshold, similarity



is_correct, similarity = compare_answers(user_answer, correct_answer, threshold=0.9)
print(f"{is_correct} (сходство: {similarity:.5f})")
#input()