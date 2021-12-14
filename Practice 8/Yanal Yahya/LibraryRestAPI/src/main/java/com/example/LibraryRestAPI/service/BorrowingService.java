package com.example.LibraryRestAPI.service;

import com.example.LibraryRestAPI.entity.BorrowingEntity;
import com.example.LibraryRestAPI.repository.BorrowingRepo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class BorrowingService {
    @Autowired
    private BorrowingRepo borrowingRepo;

    public BorrowingEntity addBorrowing(BorrowingEntity borrowing) {
        return borrowingRepo.save(borrowing);
    }

    public List<BorrowingEntity> getBorrowings() {
        List<BorrowingEntity> temp = new ArrayList<>();
        var result = borrowingRepo.findAll();
        result.forEach(temp::add);
        return temp;
    }

    public void deleteById(Long id) {
        borrowingRepo.deleteById(id);
    }

    public void update(BorrowingEntity borrowing) {
        borrowingRepo.save(borrowing);
    }
}
