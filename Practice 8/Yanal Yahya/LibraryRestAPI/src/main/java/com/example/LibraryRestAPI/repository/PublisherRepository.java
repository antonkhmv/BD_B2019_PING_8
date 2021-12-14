package com.example.LibraryRestAPI.repository;

import com.example.LibraryRestAPI.entity.PublisherEntity;
import org.springframework.data.repository.CrudRepository;

public interface PublisherRepository extends CrudRepository<PublisherEntity, String> {
}
