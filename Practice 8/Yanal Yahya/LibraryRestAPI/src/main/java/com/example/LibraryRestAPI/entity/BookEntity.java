package com.example.LibraryRestAPI.entity;

import com.fasterxml.jackson.annotation.JsonFormat;

import javax.persistence.*;
import java.sql.Date;
import java.util.List;

@Entity
@Table(name = "book")
public class BookEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Long ISBN;
    private String title;
    private String author;
    private int pagesNum;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy")
    private Date pubYear;

    @ManyToOne
    @JoinColumn(name = "publisher_id")
    private PublisherEntity publisher;

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "book")
    private List<BookCategoryEntity> categories;

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "book")
    private List<CopyEntity> copies;

    public PublisherEntity getPublisher() {
        return publisher;
    }

    public void setPublisher(PublisherEntity publisher) {
        this.publisher = publisher;
    }

    public List<CopyEntity> getCopies() {
        return copies;
    }

    public void setCopies(List<CopyEntity> copies) {
        this.copies = copies;
    }

    public List<BookCategoryEntity> getCategories() {
        return categories;
    }

    public void setCategories(List<BookCategoryEntity> categories) {
        this.categories = categories;
    }

    public BookEntity() {
    }

    public Long getISBN() {
        return ISBN;
    }

    public void setISBN(Long ISBN) {
        this.ISBN = ISBN;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getAuthor() {
        return author;
    }

    public void setAuthor(String author) {
        this.author = author;
    }

    public int getPagesNum() {
        return pagesNum;
    }

    public void setPagesNum(int pagesNum) {
        this.pagesNum = pagesNum;
    }

    public Date getPubYear() {
        return pubYear;
    }

    public void setPubYear(Date pubYear) {
        this.pubYear = pubYear;
    }

}
