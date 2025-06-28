from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity
import string
import re
from pymorphy3 import MorphAnalyzer
import sys
import os

os.environ["TRANSFORMERS_CACHE"] = os.path.join(os.getcwd(), "hf_cache")
#model = SentenceTransformer('intfloat/multilingual-e5-large')  #сосал: 'paraphrase-multilingual-MiniLM-L12-v2'
#model = SentenceTransformer.load("C:\\Users\\123\\Desktop\\pypypypyp\\intfloatmultilinguale5large")
# Указываем свою папку для моделей
MODEL_DIR = "intfloatmultilinguale5large"
MODEL_NAME = "intfloat/multilingual-e5-large"

# Создаем папку, если её нет
os.makedirs(MODEL_DIR, exist_ok=True)
# Загружаем модель и сохраняем в указанную папку
model = SentenceTransformer(MODEL_NAME, cache_folder=MODEL_DIR)
# Проверяем, есть ли модель в папке
if os.path.exists(os.path.join(MODEL_DIR, MODEL_NAME.replace("/", "_"))):
    #print("Модель уже загружена, используем локальную копию.")
    model = SentenceTransformer(os.path.join(MODEL_DIR, MODEL_NAME.replace("/", "_")))
else:
    #print("Модель не найдена, скачиваем...")
    model = SentenceTransformer(MODEL_NAME, cache_folder=MODEL_DIR)

morph = MorphAnalyzer()

#if(sys.argv.__len__!=3):
#    exit("sosal")

user_answer = "париж красивый город"
correct_answer ="Париж является столицей Франции."

def normalize_text(text):
    text = re.sub(r'[^\w\s]', '', text.lower())
    
    words = text.split()
    lemmas = [morph.parse(word)[0].normal_form for word in words]
    return " ".join(lemmas)


def compare_answers(user_answer, correct_answer, threshold=0.75):
    user_norm = normalize_text(user_answer).translate(str.maketrans('', '', string.punctuation)).lower()
    correct_norm = normalize_text(correct_answer).translate(str.maketrans('', '', string.punctuation)).lower()
    
    # ембеддинги
    embeddings = model.encode([user_norm, correct_norm])
    similarity = cosine_similarity([embeddings[0]], [embeddings[1]])[0][0]
    return similarity >= threshold, similarity



is_correct, similarity = compare_answers(user_answer, correct_answer, threshold=0.9)
print(f"{is_correct} (сходство: {similarity:.5f})")
#input()