import time

class Context:
    def __init__(self):
        self.history = []
        self.location = "Kyiv"
        self.preferences = set()
        self.time = time.strftime("%Y-%m-%d %H:%M:%S")

    def update_context(self, query):
        self.history.append(query)
        self.time = time.strftime("%Y-%m-%d %H:%M:%S")
        
        # Add to user preferences if a word is repeated multiple times
        for word in query.split():
            if self.history.count(query) > 1:
                self.preferences.add(word)

    def get_adapted_results(self, query):
        # Returning the context information as part of the results
        return f"Results for '{query}' (adapted for {self.location}, {self.time}):\n" \
               f"Personalized for your interest in {', '.join(self.preferences)}.\n"
    
    def get_history(self):
        return "\n".join(self.history)

context = Context()
