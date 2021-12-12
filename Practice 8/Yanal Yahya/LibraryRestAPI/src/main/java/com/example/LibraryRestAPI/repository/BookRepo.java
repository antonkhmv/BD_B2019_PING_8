package com.example.LibraryRestAPI.repository;

import com.example.LibraryRestAPI.entity.BookEntity;
import org.springframework.data.repository.CrudRepository;

public interface BookRepo extends CrudRepository<BookEntity, Long> {
}
