import math
def mianji_for_point(a,b,c):
    lena = math.sqrt((a[1] - b[1]) * (a[1] - b[1]) + (a[2] - b[2]) * (a[2] - b[2]) + (a[3] - b[3]) * (a[3] - b[3]))
    lenb = math.sqrt((b[1] - c[1]) * (b[1] - c[1]) + (b[2] - c[2]) * (b[2] - c[2]) + (b[3] - c[3]) * (b[3] - c[3]))
    lenc = math.sqrt((c[1] - a[1]) * (c[1] - a[1]) + (c[2] - a[2]) * (c[2] - a[2]) + (c[3] - a[3]) * (c[3] - a[3]))
    zhouchang = (lena + lenb + lenb) / 2
    num = (zhouchang - lena) * (zhouchang - lenb) * (zhouchang - lenc) * zhouchang
    mianji = math.sqrt(num)
    return mianji

def mianji_for_len(a,b,c):
    zhouchang=(a+b+c)/2
    mianji = math.sqrt((zhouchang - a) * (zhouchang - b) * (zhouchang - c)*zhouchang)
    return mianji

print(mianji_for_len(3*math.sqrt(2),3*math.sqrt(2),3*math.sqrt(2)))
a=[1,0,0,3]
b=[2,0,3,0]
c=[3,3,0,0]
print(mianji_for_point(a,b,c))
print(math.sqrt(2))
print(-5*-5)