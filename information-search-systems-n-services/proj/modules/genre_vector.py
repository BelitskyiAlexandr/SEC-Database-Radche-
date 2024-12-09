def create_genre_vector(genres_list):
    genre_vector = {}
    for genre in genres_list:
        genre_vector[genre] = 0
    return genre_vector

def update_genre_vector(genre_vector, user_genres):
    for genre in user_genres:
        if genre in genre_vector:
            genre_vector[genre] += 1
    return genre_vector
