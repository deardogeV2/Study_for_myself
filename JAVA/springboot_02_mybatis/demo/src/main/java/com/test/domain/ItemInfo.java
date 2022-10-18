package com.test.domain;

public class ItemInfo {
    private Integer id;
    private String itemName;
    private double perMoney;

    @Override
    public String toString() {
        return "ItemInfo{" +
                "id=" + id +
                ", itemName='" + itemName + '\'' +
                ", perMoney=" + perMoney +
                ", description='" + description + '\'' +
                ", itemStatus=" + itemStatus +
                '}';
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getItemName() {
        return itemName;
    }

    public void setItemName(String itemName) {
        this.itemName = itemName;
    }

    public double getPerMoney() {
        return perMoney;
    }

    public void setPerMoney(double perMoney) {
        this.perMoney = perMoney;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public Integer getItemStatus() {
        return itemStatus;
    }

    public void setItemStatus(Integer itemStatus) {
        this.itemStatus = itemStatus;
    }

    private String description;
    private Integer itemStatus;
}
