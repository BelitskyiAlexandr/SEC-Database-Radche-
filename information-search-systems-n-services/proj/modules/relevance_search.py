def search_books(books, query, genre_vector):
    query_lower = query.lower()
    results = []
    
    for book in books:
        title_match = query_lower in book['Title'].lower()
        author_match = query_lower in book['Author'].lower()
        genre_score = sum(genre_vector.get(genre, 0) for genre in book['genres'])
        
        if title_match or author_match:
            results.append({
                "Book Id": book['Book Id'],
                "Title": book['Title'],
                "Author": book['Author'],
                "genres": book['genres'],
                "relevance": genre_score
            })
    

    results.sort(key=lambda x: x['relevance'], reverse=True)
    return results
