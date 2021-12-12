package com.example.LibraryRestAPI.entity;

import com.fasterxml.jackson.annotation.JsonFormat;

import javax.persistence.*;
import java.sql.Date;
import java.util.List;

@Entity
@Table(name = "reader" )
public class ReaderEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE)
    private Long id;
    private String lastName;
    private String firstName;
    private String address;
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy")
    private Date birthdate;

    public List<BorrowingEntity> getReaders() {
        return readers;
    }

    public void setReaders(List<BorrowingEntity> readers) {
        this.readers = readers;
    }

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "reader")
    private List<BorrowingEntity> readers;


    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public Date getBirthdate() {
        return birthdate;
    }

    public void setBirthdate(Date birthdate) {
        this.birthdate = birthdate;
    }

    public ReaderEntity() {
    }
}
