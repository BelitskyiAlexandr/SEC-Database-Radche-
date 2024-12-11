import csv

def load_books(file_path):
    books = []
    with open(file_path, mode='r', encoding='utf-8') as csv_file:
        reader = csv.DictReader(csv_file)
        for row in reader:
            genres = row['genres'] if row['genres'] else ""
            row['genres'] = genres.split(';') if genres else []
            books.append(row)
    return books
