<?php
/*
使用时需要注意：
1.windows中使用PHP里面的system等方式使用cmd来执行的时候需要注意cmd的双引号转义的问题，
比如三个双引号在CMD中就是一个双引号

2.PHP中的PHP_EOL换行常量在别的代码中运行时候存在问题，这里直接换成“\n”会好一些


*/
function request_by_curl($remote_server, $post_string)
{
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $remote_server);
    curl_setopt($ch, CURLOPT_POST, 1);
    curl_setopt($ch, CURLOPT_CONNECTTIMEOUT, 5);
    curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json;charset=utf-8'));
    curl_setopt($ch, CURLOPT_POSTFIELDS, $post_string);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    // 线下环境不用开启curl证书验证, 未调通情况可尝试添加该代码
    curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
    $data = curl_exec($ch);
    curl_close($ch);
    return $data;
}

function getdata($par,$errMobiles){
    //$text="#### ".$par['title']."\n";
    $text="#### "."短信模板系统调用失败"."\n";
    foreach ($par as $x=>$x_value) {
        if (!is_array($x_value)){
            $text=$text.">$x:$x_value\n\n";
        }
        else{
            foreach ($par[$x] as $y=>$y_value){
                $text=$text.">>$y:".urldecode($y_value)."\n\n";
            }
        }
    }
    if ($errMobiles['res']) {
        $err = json_decode($errMobiles['res'], true);
        if (is_array($err)) {
            $text = $text . "#### 错误信息：\n";
            foreach ($err as $a => $a_value) {
                $text = $text . ">>$a:" . urldecode($a_value) . "\n\n";
            }
        } else {
            $err_msg = $errMobiles['res'];
            $text = $text . "#### 错误信息：\n" . ">$err_msg\n\n";
        }
    }
    return $text;
}

function auto_markdown_run($title, $par,$errMobiles)
{
    var_dump($title);
    var_dump($par);
    var_dump($errMobiles);

    $msg=json_decode($par,true);
    $err=json_decode($errMobiles,true);

    echo "\n";
    var_dump($msg);
    var_dump($err);

    $text=getdata($msg,$err);

    file_put_contents('/home/wanghuajun/test.log','asaa'.PHP_EOL,FILE_APPEND);
    list($msec, $sec) = explode(' ', microtime());
    $msectime = (float)sprintf('%.0f', (floatval($msec) + floatval($sec)) * 1000);
    $secret = 'SEC63b8487cc0b3d0ebf5b39e68c17b996f3aedc005efa86878d80b311d5cf204bf';
    $sign = urlencode(base64_encode(hash_hmac('sha256', $msectime . "\n" . $secret, $secret, true)));
    $webhook = "https://oapi.dingtalk.com/robot/send?access_token=284397cd7ac0a0ab6dc522fe1492b640f35ca8be6c3b9753452a8040d29f55ab" . "&" . "timestamp=" . $msectime . "&sign=" . $sign;

// 数据内容
    $textString = json_encode([
        'msgtype' => 'markdown',
        'markdown' => [
            "title" => $title,
            "text" => $text
        ],
        'at' => [
            'isAtAll' => true
        ]
    ]);
    var_dump(request_by_curl($webhook, $textString));
}

auto_markdown_run($argv[1],$argv[2],$argv[3]);



die;
///usr/local/lnmp/php/bin/php test.php 'super' '{"p1":"123","p2":{"p21":"321"}}' '{"res":'1234'}'