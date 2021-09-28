from math import*
print("Enter n:")
n = int(input())
print("Enter m:")
m = int(input())
if( m == 0):
    print("Error: division by 0")
else:
    z = (sqrt(2)-sqrt(3*n))/2*m
    print("z =", z)