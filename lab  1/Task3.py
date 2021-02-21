import random
print("Enter length of array: ")

#new variables
n = int(input())
sum = 0
positive = []
arr = [random.randint(-50, 50) for b in range(n)]

#sum of odd elements
for i in range(len(arr)):
    if i % 2 != 0 :
        if arr[i] > 0:
            sum = sum + arr[i]
#positive elements
for elem in arr:
    if elem > 0 :
        positive.append(elem)

#Print block
print("Min element of array is", min(arr))
print("Sum of positive elements on odd indexes:", sum)   
print("Positive elements: ", positive)
print("Original array:",arr)