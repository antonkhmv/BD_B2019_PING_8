package com.example.LibraryRestAPI.service;

import com.example.LibraryRestAPI.entity.PublisherEntity;
import com.example.LibraryRestAPI.exception.PublisherAlreadyExistException;
import com.example.LibraryRestAPI.repository.PublisherRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@Service
public class PublisherService {

    @Autowired
    private PublisherRepository publisherRepository;

    public PublisherEntity addPublisher(PublisherEntity publisher) throws PublisherAlreadyExistException {
        if (publisherRepository.findById(publisher.getName()).isPresent()) {
            throw new PublisherAlreadyExistException("Publisher with this name already exist!");
        }
        return publisherRepository.save(publisher);
    }
}
