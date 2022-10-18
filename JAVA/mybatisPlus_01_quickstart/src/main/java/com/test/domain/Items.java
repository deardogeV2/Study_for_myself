package com.test.domain;

import com.baomidou.mybatisplus.annotation.*;
import lombok.*;

//@Setter // lombok 的注解
//@Getter // 代替全部get方法
//@ToString // 代替get方法
//@NoArgsConstructor // 代替无参构造
//@AllArgsConstructor // 代替全参构造
//@EqualsAndHashCode // 代替.equal
@Data // 代替上面全部 除了构造方法
@TableName(value = "items")
public class Items {

    private Integer id;

    private String itemName;

    @TableField(value = "per_money")
    private Double perMoney;

    private String description;
    private Integer itemStatus;

    @TableField(exist = false,select = false)
    private Integer isGood;

    @TableField(value = "is_delete")
    @TableLogic(value = "0",delval = "1") // value代表该值未删除，1代表该值已删除
    private Integer isDelete;

    @Version // 注解标识该字段为乐观锁
    private Integer version; // 用这个字段来标识乐观锁
}
