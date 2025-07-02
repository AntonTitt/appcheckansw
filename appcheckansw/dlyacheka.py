from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity
import string
import re
from pymorphy3 import MorphAnalyzer
import sys
import os

os.environ["HF_HOME"] = os.path.join(os.getcwd(), "hf_cache")
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

user_answers =  sys.argv[1].split(',') # ["париж красивый город","теория относительности объясняет гравитацию"] 
correct_answers =  sys.argv[2].split(',') #["Париж является столицей Франции.","Теория относительности описывает гравитацию"]

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

def compare_answer_batches(user_answers, correct_answers, threshold=0.75):
    """
    Сравнивает каждый ответ пользователя с соответствующим эталонным ответом.
    
    Параметры:
        user_answers (list): Список ответов пользователя.
        correct_answers (list): Список эталонных ответов.
        threshold (float): Порог сходства (0.75 по умолчанию).
    
    Возвращает:
        list: Список пар (is_correct, similarity).
    """
    # Нормализация текста (опционально)
    def normalize(text):
        return text.lower().strip().translate(str.maketrans('', '', string.punctuation)).lower()
    
    user_answers = [normalize(ans) for ans in user_answers]
    correct_answers = [normalize(ans) for ans in correct_answers]
    
    # Получаем эмбеддинги для всех ответов
    user_embeddings = model.encode(user_answers, show_progress_bar=True)
    correct_embeddings = model.encode(correct_answers, show_progress_bar=True)
    
    # Считаем попарное косинусное сходство
    similarities = cosine_similarity(user_embeddings, correct_embeddings)
    
    # Для каждого ответа берем соответствующую пару (i, i)
    results = []
    for i in range(len(user_answers)):
        sim = similarities[i, i]
        results.append((sim >= threshold, sim))
    
    return results


results = compare_answer_batches(user_answers, correct_answers, threshold=0.9)

# Выводим результаты
for i, (is_correct, sim) in enumerate(results):
    print(f"{i+1}:{sim:.5f}")
    #print(f"{i+1}: {'True' if is_correct else 'False'} (сходство: {sim:.4f})")
#is_correct, similarity = compare_answers(user_answer, correct_answer, threshold=0.9)
#print(f"{is_correct} (сходство: {similarity:.5f})")
#input()