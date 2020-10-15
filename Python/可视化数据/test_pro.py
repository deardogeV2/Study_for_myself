a={'a':2,'b':1,'z':0,'t':5}
b=sorted(a,key=lambda x:a[x],reverse=True)
print(b)
for i in b:
    print(a[i])