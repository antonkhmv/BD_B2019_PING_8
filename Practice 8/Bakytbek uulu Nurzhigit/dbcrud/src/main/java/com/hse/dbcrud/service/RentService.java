package com.hse.dbcrud.service;

import com.hse.dbcrud.entity.RentEntity;
import com.hse.dbcrud.model.RentModel;
import com.hse.dbcrud.repository.RentRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.PathVariable;

@Service
public class RentService {

    private final RentRepository rp;


    public RentService(RentRepository rp) {
        this.rp = rp;
    }

    @Autowired
    private ReaderService readerService;

    @Autowired
    private BookCopyService bookCopyService;

    public void addRent(RentModel rent) {
        RentEntity re = new RentEntity();

        RentEntity.Key key = new RentEntity.Key();
        key.setReader(readerService.getById(rent.getReaderId()));

        System.out.println(key.getReader());

        key.setBkkey(bookCopyService.getById(rent.getCopyNumber(), rent.getIsbn()));
        re.setKey(key);

        re.setReturnDate(rent.getReturnDate());

        rp.save(re);
    }

    public RentEntity getById(Long cn, Long isbn, Long rn) {

        RentEntity.Key key = new RentEntity.Key();
        key.setReader(readerService.getById(rn));
        key.setBkkey(bookCopyService.getById(cn, isbn));

        return rp.findById(key).orElse(null);
    }

    public void deleteById(Long cn, Long isbn, Long rn) {

        RentEntity.Key key = new RentEntity.Key();
        key.setReader(readerService.getById(rn));
        key.setBkkey(bookCopyService.getById(cn, isbn));

        rp.deleteById(key);
    }
}
