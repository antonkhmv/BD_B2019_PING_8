package com.example.LibraryRestAPI.controller;

import com.example.LibraryRestAPI.entity.BookEntity;
import com.example.LibraryRestAPI.service.BookService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Service
@RequestMapping("books")
public class BookController {
    @Autowired
    private BookService bookService;

    @PostMapping
    public ResponseEntity addBook(@RequestBody BookEntity book) {

        var result = bookService.addBook(book);
        return ResponseEntity.ok(result);
    }

    @GetMapping
    public ResponseEntity getBooks() {
        List<BookEntity> books;
        books = bookService.getBooks();

        return ResponseEntity.ok(books);
    }

    @DeleteMapping
    public ResponseEntity deleteBook(@RequestParam Long id) {

        bookService.deleteById(id);
        return ResponseEntity.ok("deleted");
    }

    @PutMapping
    public ResponseEntity updateBook(@RequestBody BookEntity book) {

        bookService.update(book);
        return ResponseEntity.ok("Updated");
    }
}
