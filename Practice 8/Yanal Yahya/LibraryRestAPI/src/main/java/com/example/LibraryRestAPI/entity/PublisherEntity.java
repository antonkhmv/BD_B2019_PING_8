package com.example.LibraryRestAPI.entity;

import javax.persistence.*;
import java.util.List;

@Entity
@Table(name = "publisher")
public class PublisherEntity {
    @Id
    private String name;
    private String address;

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "publisher")
    private List<BookEntity> books;

    public PublisherEntity() {
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String adress) {
        this.address = adress;
    }
}
