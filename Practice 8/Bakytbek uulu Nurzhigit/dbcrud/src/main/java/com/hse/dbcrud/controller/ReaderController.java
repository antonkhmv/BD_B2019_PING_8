package com.hse.dbcrud.controller;

import com.hse.dbcrud.entity.ReaderEntity;
import com.hse.dbcrud.service.ReaderService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/readers")
public class ReaderController {

    private final ReaderService rs;

    public ReaderController(ReaderService rs) {
        this.rs = rs;
    }

    @PostMapping
    public ResponseEntity registration(@RequestBody ReaderEntity reader) {

        rs.registration(reader);
        return ResponseEntity.ok("Reader added");
    }
    @GetMapping
    public ResponseEntity greeting() {
        return ResponseEntity.ok("Hello!");
    }
    @GetMapping("/{id}")
    public ResponseEntity getReader(@PathVariable Long id) {

        return ResponseEntity.ok(rs.getById(id));
    }

    @PutMapping
    public ResponseEntity updateReader(@RequestBody ReaderEntity reader){

        System.out.println(reader.getName());
        rs.update(reader);

        return ResponseEntity.ok("Updated");
    }

    @DeleteMapping("/{id}")
    public ResponseEntity deleteReader(@PathVariable Long id) {

        rs.deleteById(id);
        return ResponseEntity.ok("Deleted");
    }
}
