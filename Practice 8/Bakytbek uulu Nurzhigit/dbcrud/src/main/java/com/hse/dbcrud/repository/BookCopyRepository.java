package com.hse.dbcrud.repository;

import com.hse.dbcrud.entity.BookCopyEntity;
import org.springframework.data.repository.CrudRepository;

public interface BookCopyRepository extends CrudRepository<BookCopyEntity, BookCopyEntity.Key> {
}
