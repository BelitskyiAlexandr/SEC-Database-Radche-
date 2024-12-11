from colorama import Fore, Style, init
from collections import Counter
from modules.data_loader import load_books
from modules.genre_vector import create_genre_vector, update_genre_vector
from modules.relevance_search import search_books

init(autoreset=True)  

def display_menu():
    """Display the main menu."""
    print("\nMain Menu:")
    print("1. Enter genres of interest")
    print("2. Search by query")
    print("3. Find books by genres")
    print("4. Exit")

def display_genres(all_genres, top_n=10):
    """Display the top N genres or all genres."""
    print("\nTop 10 most frequent genres:")
    genre_counts = Counter(all_genres)
    top_genres = genre_counts.most_common(top_n)
    for genre, count in top_genres:
        print(f"{Fore.CYAN}{genre} ({count} books)")
    print(f"\nType 'all genres' to view all genres, or input your selection.")

def display_all_genres(all_genres):
    """Display all genres."""
    print("\nAll available genres:")
    print(f"{Fore.CYAN}{', '.join(sorted(all_genres))}")

def display_results(results, start, end, user_genres):
    """Display search results."""
    print(f"\n{Fore.WHITE}Your genres: {Fore.LIGHTYELLOW_EX}\"{', '.join(user_genres)}\"")
    print("\nSearch Results:")
    for result in results[start:end]:
        print(
            f"{Fore.WHITE}Book Id: {Fore.YELLOW}\"{result['Book Id']}\"\n"
            f"{Fore.WHITE}Title: {Fore.GREEN}\"{result['Title']}\"\n"
            f"{Fore.WHITE}Author: {Fore.RED}\"{result['Author']}\"\n"
            f"{Fore.WHITE}genres:"
        )
        genres = result['genres']
        for i in range(0, len(genres), 3):
            print(f"{Fore.CYAN}{', '.join(genres[i:i+3])}")
        print()

def filter_books_by_genres(books, genre_vector):
    """Filter books based on user's genre preferences."""
    user_genres = [genre for genre, count in genre_vector.items() if count > 0]
    filtered_books = []

    for book in books:
        book_genres = book['genres']
        if any(genre in user_genres for genre in book_genres):
            filtered_books.append(book)

    filtered_books.sort(
        key=lambda book: sum(genre_vector.get(genre, 0) for genre in book['genres']),
        reverse=True
    )
    return filtered_books

def display_current_vector(genre_vector):
    """Display the user's current genre vector."""
    print("\nYour current genres of interest:")
    current_genres = [genre for genre, count in genre_vector.items() if count > 0]
    if current_genres:
        print(f"{Fore.CYAN}{', '.join(current_genres)}")
    else:
        print(f"{Fore.WHITE}The vector is empty.")

def main():
    file_path = 'Goodreads_books_with_genres.csv'
    books = load_books(file_path)
    all_genres = [genre for book in books for genre in book['genres']]
    unique_genres = set(all_genres)
    genre_vector = create_genre_vector(unique_genres)

    current_query_results = []
    current_page = 0
    results_per_page = 5

    while True:
        display_menu()
        choice = input("\nChoose an option: ").strip()

        if choice == '1':
            display_genres(all_genres)
            user_input = input("\nEnter genres (comma-separated), \n'all genres' to view all, \n'print' to view your vector, \n'clear vector' to reset: ").strip()
        
            if user_input.lower() == "all genres":
                display_all_genres(unique_genres)

            elif user_input.lower() == "print":
                display_current_vector(genre_vector)

            elif user_input.lower() == "clear vector":
                genre_vector = create_genre_vector(unique_genres)
                print("\nThe vector was cleared.")

            else:
                user_genres = map(str.strip, user_input.split(','))
                genre_vector = update_genre_vector(genre_vector, user_genres)

        elif choice == '2':
            query = input("\nEnter your search query (book title or author): ").strip()
            current_query_results = search_books(books, query, genre_vector)
            if not current_query_results:
                print("\nNo results found.")
                continue

            user_genres = [genre for genre, count in genre_vector.items() if count > 0]
            current_page = 0
            while True:
                start = current_page * results_per_page
                end = start + results_per_page
                display_results(current_query_results, start, min(end, len(current_query_results)), user_genres)

                print("\nOptions: 'next' for next page, 'prev' for previous page, 'menu' to return to main menu")
                nav_choice = input("Choose an option: ").strip().lower()

                if nav_choice == "next":
                    if end < len(current_query_results):
                        current_page += 1
                    else:
                        print("\nNo more results.")

                elif nav_choice == "prev":
                    if current_page > 0:
                        current_page -= 1
                    else:
                        print("\nAlready on the first page.")

                elif nav_choice == "menu":
                    break

                else:
                    print("\nInvalid option. Please try again.")

        elif choice == '3':
            filtered_books = filter_books_by_genres(books, genre_vector)
            if not filtered_books:
                print("\nNo books found for your selected genres.")
                continue

            user_genres = [genre for genre, count in genre_vector.items() if count > 0]
            current_page = 0
            while True:
                start = current_page * results_per_page
                end = start + results_per_page
                display_results(filtered_books, start, min(end, len(filtered_books)), user_genres)

                print("\nOptions: 'next' for next page, 'prev' for previous page, 'menu' to return to main menu")
                nav_choice = input("Choose an option: ").strip().lower()

                if nav_choice == "next":
                    if end < len(filtered_books):
                        current_page += 1
                    else:
                        print("\nNo more results.")

                elif nav_choice == "prev":
                    if current_page > 0:
                        current_page -= 1
                    else:
                        print("\nAlready on the first page.")

                elif nav_choice == "menu":
                    break

                else:
                    print("\nInvalid option. Please try again.")

        elif choice == '4':
            print("\nGoodbye!")
            break

        else:
            print("\nInvalid choice. Please try again.")

if __name__ == "__main__":
    main()
