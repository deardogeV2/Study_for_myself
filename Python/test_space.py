class point():
    def __init__(self, n):
        self.weizi = 1
        self.L1 = 1
        self.L2 = 2
        self.L3 = 3
        self.L4 = 4
        self.max=n

    def D(self):
        self.weizi+=1
        if self.weizi>self.max:
            self.weizi=1
            self.L1 = 1
            self.L2 = 2
            self.L3 = 3
            self.L4 = 4
        elif self.weizi>self.L4:
            self.L1 +=1
            self.L2 +=1
            self.L3 +=1
            self.L4 +=1
    def U(self):
        self.weizi-=1
        if self.weizi<1:
            self.weizi = self.max
            self.L1 = self.max-3
            self.L2 = self.max-2
            self.L3 = self.max-1
            self.L4 = self.max
        elif self.weizi<self.L1:
            self.L1 -= 1
            self.L2 -= 1
            self.L3 -= 1
            self.L4 -= 1

while True:
    try:
        a=int(input())
        A=point(a)
        b=input()
        T=1
        for i in b:
            if i=='U':
                A.U()
                T-=1
            else:
                A.D()
                T+=1
        if a<=4:
            for i in range(a):
                if i == a - 1:
                    print(i+1)
                else:
                    print(i+1,end=' ')
            print(T%a)
        else:
            print(A.L1,A.L2,A.L3,A.L4)
            print(A.weizi)
    except:
        break
