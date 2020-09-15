'''
因为python没有指针，链表的内定义。
所以一直不懂链表我的我只能在各种百科里面查找链表的写法
在这里留一下我的学习记录
'''

#单链表节点类
class Node(object):
    def __init__(self,data,next=None):
        #self为自己，data为数据（不分数据类型，毕竟是python）
        #next是链表下个指针指向谁，用关键字参数是为了末尾的节点能指向空
        self.data=data
        self.next=next

#上面就实现了节点类的定义，但还不是一个链表。
#下面来生成几个对象试一试

#接下来开始完成链表类的定义
class LinkedList(object):
    def __init__(self):
        self.head=None
        self.length=0
        #初始化头指针指向和链表长度
    #小演示一个链表
    def demo(self):
        node1 = None
        node2 = Node(1, None)
        node3 = Node('2', node2)
        # 这就实现了一个链表3→链表2→链表1(我们设置的为空)

    #实现根据一组输入对象创建链表，
    #返回值是一对数，我使用列表来进行保存:0是头,1是长度
    def createLinkedList(self,*args):#注意，因为Node的写法问题，这里的args的进入链表方式是逆序的
                                     #即(1,2,3)进入是之后是head→3→2→1→None
        for i in args:
            self.head=Node(i,self.head)
            self.length+=1
        return [self.head, self.length]

'''相应的，循环链表和双向链表都是可以通过类来写的，而链表中的方法在python中也是类方法而已。
之后有时间会补齐三种链表的生成方法和其中的查找删除增加各三组方法'''
