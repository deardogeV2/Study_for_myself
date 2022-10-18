package com.example.LearnForDsl.domain;

import lombok.Data;

@Data
public class HotelDoc {
    private Integer id;
    private String name;
    private String address;
    private Integer price;
    private Integer score;
    private String brand;
    private String city;
    private String starName;
    private String business;
    private String location;
    private String pic;


    public HotelDoc() {}
    public HotelDoc(HotelInfo hotelInfo){
        this.id = hotelInfo.getId();
        this.name = hotelInfo.getName();
        this.address = hotelInfo.getAddress();
        this.score = hotelInfo.getScore();
        this.brand = hotelInfo.getBrand();
        this.city = hotelInfo.getCity();
        this.starName = hotelInfo.getStarName();
        this.business = hotelInfo.getBusiness();
        this.location = hotelInfo.getLatitude() +", "+ hotelInfo.getLongitute();
        this.pic = hotelInfo.getPic();

    }
}