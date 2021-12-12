package com.example.LibraryRestAPI.service;

import com.example.LibraryRestAPI.entity.BookEntity;
import com.example.LibraryRestAPI.repository.BookRepo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class BookService {

    @Autowired
    private BookRepo bookRepo;

    public BookEntity addBook(BookEntity book) {
        return bookRepo.save(book);
    }

    public List<BookEntity> getBooks() {
        List<BookEntity> temp = new ArrayList<>();
        var result = bookRepo.findAll();
        result.forEach(temp::add);
        return temp;
    }

    public void deleteById(Long id) {
        bookRepo.deleteById(id);
    }

    public BookEntity update(BookEntity book) {
        return bookRepo.save(book);
    }
}
