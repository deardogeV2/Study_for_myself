<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>奥利干</title>
    <style>
        a{
            display: block;
            width: 230px;
            height: 40px;
            background-color: #55585a;
            font-size:14px ;
            color: white;
            text-decoration: none;
            text-indent: 2em;
            line-height: 40px;
        }
        a:hover{
            background-color:orange;
        }
    </style>
</head>
<body>

<?php
$al=array(0,1,2,3,4);
foreach ($al as $item=>$value){?>
    <a href="#"><?php echo $value ?></a>
<?php }?>

</body>
</html>
