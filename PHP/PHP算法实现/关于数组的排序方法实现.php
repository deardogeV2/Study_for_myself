<?php
//多个排序方法的PHP实现，很简单可以移植成别的语言。
function maopao($arr,$out){
    global $arr,$out;
    if (count($arr)>1){
        $len=count($arr);
        for($i=0;$i<$len-1;$i++) {
            if ($arr[$i] > $arr[$i + 1]) {
                $cha = $arr[$i];
                $arr[$i] = $arr[$i + 1];
                $arr[$i + 1] = $cha;
            }
        }
        array_unshift($out,array_pop($arr));
//        print_r($out);
        maopao($arr,$out);
    }
    else{
        array_unshift($out,array_pop($arr));
//        print_r($out);
        return($out);
    }
}

function xuanze($arr){
    for ($i=1,$len=count($arr);$i<$len;$i++)
    {
        $temp=$arr[$i];
        $flag=false;
        for ($j=$i-1;$j>=0;$j--)
        {
            if ($arr[$j]>$temp)
            {
                $arr[$j+1]=$arr[$j];
//                $arr[$j]=$temp;
                $flag=true;
            }
            else {//增加效率
                break;
            }
        }
        if ($flag)
        {
           $arr[$j+1]=$temp;//还是不严谨，J应该是局部变量才对，这里居然还能用
        }
    }
    return $arr;
}

function quick($arr)
{
    $len=count($arr);
    if ($len<=1) return $arr;

    //取出某个元素，然后将剩余的数组元素，分散到两个数组中
    $left=$right=array();
    for ($i=1;$i<$len;$i++)
    {
        //小的放left，大的放right
        if ($arr[$i]<$arr[0])
        {
            $left[]=$arr[$i];//数组指针使用
        }
        else{
            $right[]=$arr[$i];
        }
    }
    //递归点
    $left=quick($left);
    $right=quick($right);


    return array_merge($left,(array)$arr[0],$right);
}

function guibing($arr)
{
    $len = count($arr);
    if ($len<=1)return $arr;

    $middle=floor($len/2);
    $left=array_slice($arr,0,$middle);//并未排序
    $right=array_slice($arr,$middle);

    //递归点
    $left=merge($left);
    $right=merge($right);


    $arr3=array();
    while(count($left) && count($right))
    {
        $arr3[]=$left[0]<$right[0] ? array_shift($left):array_shift($right);
    }
    return array_merge($arr3,$left,$right);
}

$out=array();
$arr=array(1,4,2,9,7,5,8);
//maopao($arr,$out);
$arr=guibing($arr);
print_r($arr);