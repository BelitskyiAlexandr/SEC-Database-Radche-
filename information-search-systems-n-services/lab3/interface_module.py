import search_module
from context_module import context

def run_interface():
    print("Welcome to the Search System! Type 'exit' to quit or 'history' to view past searches.")
    
    while True:
        query = input("Enter your search query: ")
        
        if query == "exit":
            print("Exiting the search system.")
            break
        elif query == "history":
            print("Search history:")
            print(context.get_history())
            continue

        # Basic search
        results = search_module.search_in_files(query)

        # Update context
        context.update_context(query)

        # Contextual search
        adapted_results = context.get_adapted_results(query)
        print(adapted_results)
        
        # Display search results
        print("Relevant search results:")
        for result in results:
            print(f"- {result}")

if __name__ == "__main__":
    run_interface()
