package com.example.LibraryRestAPI.controller;

import com.example.LibraryRestAPI.entity.BorrowingEntity;
import com.example.LibraryRestAPI.service.BorrowingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Service
@RequestMapping("borrowing")
public class BorrowingController {

    @Autowired
    private BorrowingService borrowingService;

    @PostMapping
    public ResponseEntity addBorrowing(@RequestBody BorrowingEntity borrowing) {

        var result = borrowingService.addBorrowing(borrowing);
        return ResponseEntity.ok(result);
    }

    @GetMapping
    public ResponseEntity getBorrowing() {
        List<BorrowingEntity> books;
        books = borrowingService.getBorrowings();

        return ResponseEntity.ok(books);
    }

    @DeleteMapping
    public ResponseEntity deleteBook(@RequestParam Long id) {

        borrowingService.deleteById(id);
        return ResponseEntity.ok("deleted");
    }

    @PutMapping
    public ResponseEntity updateBook(@RequestBody BorrowingEntity borrowing) {

        borrowingService.update(borrowing);
        return ResponseEntity.ok("Updated");
    }
}
