package com.hse.dbcrud.repository;

import com.hse.dbcrud.entity.RentEntity;
import org.springframework.data.repository.CrudRepository;

public interface RentRepository extends CrudRepository<RentEntity, RentEntity.Key> {
}
