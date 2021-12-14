package com.example.LibraryRestAPI.repository;

import com.example.LibraryRestAPI.entity.CopyEntity;
import org.springframework.data.repository.CrudRepository;

public interface CopyBookRepo extends CrudRepository<CopyEntity, Long> {
}
