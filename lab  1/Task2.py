import random
ranNum = random.randint(1, 100)
while True:
    print("Введи своє число: ")
    num  = int(input())
    if(num > ranNum):
        print("Моє число менше \n")
    elif ( num < ranNum ):
        print("Моє число більше\n")
    else:
        print("Вітаю! Шукане число було", ranNum)
        break