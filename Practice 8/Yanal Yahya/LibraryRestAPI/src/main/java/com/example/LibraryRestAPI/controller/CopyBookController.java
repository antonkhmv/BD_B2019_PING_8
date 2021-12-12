package com.example.LibraryRestAPI.controller;

import com.example.LibraryRestAPI.entity.CopyEntity;
import com.example.LibraryRestAPI.service.CopyBookService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/copiesBook")
public class CopyBookController {
    @Autowired
    private CopyBookService copyBookService;

    @PostMapping
    public ResponseEntity addCopy(@RequestBody CopyEntity copy) {

        var result = copyBookService.addCopy(copy);
        return ResponseEntity.ok(result);
    }

    @GetMapping
    public ResponseEntity getCopies() {
        List<CopyEntity> copies;
        copies = copyBookService.getCopies();

        return ResponseEntity.ok(copies);
    }

    @DeleteMapping
    public ResponseEntity deleteCopy(@RequestParam Long id) {

        copyBookService.deleteById(id);
        return ResponseEntity.ok("deleted");
    }

    @PutMapping
    public ResponseEntity updateCopy(@RequestBody CopyEntity copy) {

        copyBookService.update(copy);
        return ResponseEntity.ok("Updated");
    }
}
