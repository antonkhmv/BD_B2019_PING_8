package com.example.LibraryRestAPI.entity;

import javax.persistence.*;
import java.util.List;

@Entity
@Table(name = "copy_book")
public class CopyEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    @ManyToOne
    @JoinColumn(name = "book_id")
    private BookEntity book;

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "bookCopy")
    private List<BorrowingEntity> readers;

    private int copyNumber;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public BookEntity getBook() {
        return book;
    }

    public void setBook(BookEntity book) {
        this.book = book;
    }

    public int getCopyNumber() {
        return copyNumber;
    }

    public void setCopyNumber(int copyNumber) {
        this.copyNumber = copyNumber;
    }

    public String getPosition() {
        return position;
    }

    public void setPosition(String position) {
        this.position = position;
    }

    private String position;


    public CopyEntity() {

    }
}
