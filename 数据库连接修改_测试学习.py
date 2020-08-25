import MySQLdb

conn=MySQLdb.connect(
    host='127.0.0.1',
    user='root',
    passwd='123456',
    db='zb_test',
    charset='utf8'
)

c=conn.cursor()

c.execute('SELECT * FROM zb_test.name')

all=c.fetchall()
print(all)