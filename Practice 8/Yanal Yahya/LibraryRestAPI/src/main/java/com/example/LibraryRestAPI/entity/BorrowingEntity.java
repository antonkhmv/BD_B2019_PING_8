package com.example.LibraryRestAPI.entity;

import com.fasterxml.jackson.annotation.JsonFormat;

import javax.persistence.*;
import java.sql.Date;

@Entity
@Table(name = "borrowing")
public class BorrowingEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "reader_id")
    private ReaderEntity reader;

    @ManyToOne
    @JoinColumn(name = "book_copy_id")
    private CopyEntity bookCopy;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "dd-MM-yyyy")
    private Date returnDate;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public ReaderEntity getReader() {
        return reader;
    }

    public void setReader(ReaderEntity reader) {
        this.reader = reader;
    }

    public CopyEntity getBookCopy() {
        return bookCopy;
    }

    public void setBookCopy(CopyEntity bookCopy) {
        this.bookCopy = bookCopy;
    }

    public Date getReturnDate() {
        return returnDate;
    }

    public void setReturnDate(Date returnDate) {
        this.returnDate = returnDate;
    }

    public BorrowingEntity() {
    }
}
