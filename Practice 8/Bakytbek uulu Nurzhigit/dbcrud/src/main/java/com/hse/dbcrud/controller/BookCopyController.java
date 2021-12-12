package com.hse.dbcrud.controller;

import com.hse.dbcrud.entity.BookCopyEntity;
import com.hse.dbcrud.service.BookCopyService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/book-copies")
public class BookCopyController {

    private final BookCopyService bks;

    public BookCopyController(BookCopyService bks) {
        this.bks = bks;
    }

    @PostMapping
    public ResponseEntity createBookCopy(@RequestBody BookCopyEntity bk) {

        bks.addBookCopy(bk);
        return ResponseEntity.ok("book copy created");
    }

    @GetMapping
    public ResponseEntity getBookCopy(@RequestParam Long copyNumber, @RequestParam Long isbn) {

        return ResponseEntity.ok(bks.getById(copyNumber, isbn));
    }

    @DeleteMapping
    public ResponseEntity deleteBookCopy(@RequestParam Long copyNumber, @RequestParam Long isbn) {

        bks.deleteById(copyNumber, isbn);
        return ResponseEntity.ok("deleted");
    }
}
