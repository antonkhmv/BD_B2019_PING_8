package com.example.LibraryRestAPI.repository;

import com.example.LibraryRestAPI.entity.BorrowingEntity;
import org.springframework.data.repository.CrudRepository;

public interface BorrowingRepo extends CrudRepository<BorrowingEntity, Long> {
}
