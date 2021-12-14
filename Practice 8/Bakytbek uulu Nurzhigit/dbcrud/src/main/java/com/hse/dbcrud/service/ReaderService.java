package com.hse.dbcrud.service;

import com.hse.dbcrud.entity.ReaderEntity;
import com.hse.dbcrud.repository.ReaderRepository;
import org.springframework.stereotype.Service;

@Service
public class ReaderService {

    private final ReaderRepository rp;

    public ReaderService(ReaderRepository rp) {
        this.rp = rp;
    }

    public void registration(ReaderEntity reader) {

        rp.save(reader);
    }

    public Iterable<ReaderEntity> getAll() {
        return rp.findAll();
    }

    public ReaderEntity getById(Long id) {

        return rp.findById(id).orElse(null);
    }

    public void update(ReaderEntity reader) {
        ReaderEntity r = rp.findById(reader.getId()).orElse(null);
        if(r != null) {
            rp.save(reader);
        }
    }

    public void deleteById(Long id) {
        rp.findById(id).ifPresent(r -> rp.deleteById(r.getId()));
    }
}
