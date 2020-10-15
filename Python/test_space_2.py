while True:
    try:

        a=input()
        b=input()
        short=''
        long=''
        if len(a)>len(b):
            long=a
            short=b
        else:
            long=b
            short=a
        out=''
        for i in range(1,len(short)+1):
            for p in range(len(short)-i+1):
                if short[p:p+i+1] in long:
                    if len(short[p:p+i+1])>len(out):
                        out=short[p:p+i+1]
                        break
        print(out)
    except:
        break