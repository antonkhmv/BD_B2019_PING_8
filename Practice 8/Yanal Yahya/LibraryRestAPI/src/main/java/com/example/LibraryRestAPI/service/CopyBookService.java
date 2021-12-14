package com.example.LibraryRestAPI.service;

import com.example.LibraryRestAPI.entity.CopyEntity;
import com.example.LibraryRestAPI.repository.CopyBookRepo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class CopyBookService {

    @Autowired
    private CopyBookRepo copyBookRepo;

    public CopyEntity addCopy(CopyEntity copy) {
        return copyBookRepo.save(copy);
    }

    public List<CopyEntity> getCopies() {
        List<CopyEntity> temp = new ArrayList<>();
        var result = copyBookRepo.findAll();
        result.forEach(temp::add);
        return temp;
    }

    public void deleteById(Long id) {
        copyBookRepo.deleteById(id);
    }

    public void update(CopyEntity copy) {
        copyBookRepo.save(copy);
    }
}
