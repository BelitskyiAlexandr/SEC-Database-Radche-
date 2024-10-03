import os

# Function to search for a keyword in text files
def search_in_files(query, folder='data/'):
    results = []
    
    for filename in os.listdir(folder):
        if filename.endswith(".txt"):
            with open(os.path.join(folder, filename), 'r') as file:
                content = file.read()
                if query.lower() in content.lower():
                    results.append(f"Found in {filename}")
    
    return results


def main_search():
    query = input("Enter your search query: ")
    results = search_in_files(query)
    
    if results:
        print(f"Found relevant results in files: {results}")
    else:
        print("No relevant results found.")

if __name__ == "__main__":
    main_search()
