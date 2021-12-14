package com.example.LibraryRestAPI.controller;

import com.example.LibraryRestAPI.entity.PublisherEntity;
import com.example.LibraryRestAPI.exception.PublisherAlreadyExistException;
import com.example.LibraryRestAPI.service.PublisherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/publishers")
public class PublisherController {

    @Autowired
    private PublisherService publisherService;

    @PostMapping
    public ResponseEntity addPublisher(@RequestBody PublisherEntity publisher) {
        try {
            publisherService.addPublisher(publisher);
            return ResponseEntity.ok("publisher added!");
        } catch (PublisherAlreadyExistException e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("error while getting publishers");
        }
    }

    @GetMapping
    public ResponseEntity getPublishers() {
        try {
            return ResponseEntity.ok("server works");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("error while getting publishers");
        }
    }
}
