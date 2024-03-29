import requests
import re
from bs4 import BeautifulSoup

req = requests.get("https://lumber-tycoon-2.fandom.com/wiki/Changelog")

soup = BeautifulSoup(req.text, "html.parser")
numOfTags = len(soup.find_all())

print("Enter word: ")
word = input()

words = len(re.findall(r'\W'+ word +'\W', req.text))
numberOfLinks = 0
for a_tag in soup.findAll("a"):
    href = a_tag.attrs.get("href")
    if href == "" or href is None:
        continue
    else:
        numberOfLinks += 1


a = len(set(req.text.split()))
print ('Number of all elements of web page : ', a)
img = len(soup.find_all('img'))

print("Number of tags: ", numOfTags, '; Frequency of occurrence: ', numOfTags/a)
print('Number of word ' + word + ':', words, '; Frequency of occurrence: ', words/a)
print('Number of links: ', numberOfLinks)
print('Number of images: ', img)