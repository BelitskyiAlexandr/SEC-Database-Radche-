import requests
from bs4 import BeautifulSoup
from urllib.parse import urljoin
from collections import defaultdict
import matplotlib.pyplot as plt
import time

# Collect all the links
def get_links_from_page(url):
    try:
        response = requests.get(url)
        if response.status_code == 200:
            soup = BeautifulSoup(response.content, 'html.parser')
            links = []
            for link in soup.find_all('a', href=True):
                full_url = urljoin(url, link['href'])
                links.append(full_url)
            return links
        else:
            print(f"Error: Unable to fetch page {url}")
            return []
    except Exception as e:
        print(f"Error: {e}")
        return []

# Check if link is working
def check_link_status(link):
    try:
        response = requests.head(link, allow_redirects=True)
        return response.status_code
    except Exception as e:
        return None

def filter_https_links(links):
    return [link for link in links if link.startswith('https')]

# Function to get Moz metrics
def get_moz_metrics(url, access_id, secret_key):

    moz_url = f"https://lsapi.seomoz.com/v2/url_metrics"
    auth = (access_id, secret_key)

    data = """{
        "targets": ["omfgdogs.com/#"]
    }"""

    try:
        response = requests.post(moz_url, data=data, auth=auth)
        if response.status_code == 200:
            return response.json()
        else:
            print(f"Error: {response.status_code}, {response.text}")
            return None
    except Exception as e:
        print(f"Error fetching Moz metrics: {e}")
        return None

# DA visualization
def visualize_link_quality(metrics):
    domains = [metric['domain'] for metric in metrics]
    authorities = [metric['domain_authority'] for metric in metrics]
    
    plt.barh(domains, authorities)
    plt.xlabel('Domain Authority')
    plt.ylabel('Domains')
    plt.title('Domain Authority of Links')
    plt.show()


def main():
    url = 'https://www.omfgdogs.com/#'
    
    print("Collecting links...")
    links = get_links_from_page(url)
    print(f"Collected {len(links)} links.")
    
    print("Checking links status...")
    valid_links = [link for link in links if check_link_status(link) == 200]
    print(f"Collected {len(valid_links)} working links.")
    
    print("Filtering HTTPS links...")
    https_links = filter_https_links(valid_links)
    print(f"Collected {len(https_links)} links with HTTPS.")

    MOZ_ACCESS_ID = 'mozscape-fDGRrNj0bD'
    MOZ_SECRET_KEY = 'uHVXo2B1pF4NIBPuaPnJbRuikFnAbprW'
    
    print("Getting authority metrics...")
    link_metrics = []
    for link in https_links:
        metrics = get_moz_metrics(link, MOZ_ACCESS_ID, MOZ_SECRET_KEY)
        if metrics:
            link_metrics.append({'domain': link, 'domain_authority': metrics.get('domain_authority', 0)})
        time.sleep(1)  # API limits timeout
    
    if link_metrics:
        print("Visualization of link authority...")
        visualize_link_quality(link_metrics)
    else:
        print("Failed to get link metrics.")

if __name__ == "__main__":
    main()
