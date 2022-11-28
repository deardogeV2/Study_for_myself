-- 锁的key
local key = KEYS[1]
-- 当前线程标识
local threadId = ARGV[1]

-- 锁中的线程标识
local id = redis.call('get',key)

-- 标识与当前线程标识比较
if(id==threadId) then
    --释放锁
    return redis.call('del',key)
end
return 0