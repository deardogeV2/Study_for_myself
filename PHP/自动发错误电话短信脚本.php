<?php

$file='C:/Users/Administrator/Desktop/123456.txt';

function _smsTemplatePost($par)
{
    $url = 'https://sms.langma.cn/api?client_id=70000';
    $params = $par;
    try {
        $signStr = '';
        foreach ([
            'app_id' => 10004,
            'client_id' => 70000,
            'client_key' => 'f4cc344af748d64f8fc12450bffedd67',
            'mobile' => $params['mobile'],
            'template_id' => $params['template_id'],
        ] as $key => $value) {
            $signStr .= "{$key}={$value}&";
        }
        $signStr = trim($signStr, '&');

        $url .= '&sign=' . md5($signStr);
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, $url);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_POST, 1);
        curl_setopt($ch, CURLOPT_POSTFIELDS, 'sms_info=' . json_encode($params));
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
        curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 0);
        curl_setopt($ch, CURLOPT_SSLVERSION, 1);
        curl_setopt($ch, CURLOPT_USERAGENT, 'Mozilla/5.0 (Windows NT 6.1; WOW64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1');
        $data = curl_exec($ch);
        curl_close($ch);
        return $data;
    } catch (Exception $ex) {
        echo '<pre>';
        echo __FILE__ . ':' . __LINE__ . PHP_EOL;
        var_dump($ex->getMessage());
        die;
    }
}

function send_($f){
    $file=$f;
    $handle=fopen($file,"r");
    $contents=fread($handle,filesize($file));

    preg_match_all('/sms_info=({\S+?}\S+?})/', $contents, $matches, PREG_SET_ORDER);
    for ($i=0;$i<count($matches);$i++)
    {
        $str=$matches[$i][1];
        if (check($str)){//字符串筛选条件
            $str=str_replace('\\"','"',$str);
            $t=json_decode($str,true);
            _smsTemplatePost($t);
        }else {
            echo "出现代码错误";
            exit(1);
        }
    }
    fclose($handle);
}

function check($str){
    //$str字符串的条件，用于筛选日期什么的
    //如果需要url编解码请注意大小写。
    return true;
}

echo '<pre>';
echo __FILE__.':'.__LINE__ . PHP_EOL;
send_($file);
die;