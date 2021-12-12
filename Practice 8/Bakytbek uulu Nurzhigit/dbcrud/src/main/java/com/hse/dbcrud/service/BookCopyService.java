package com.hse.dbcrud.service;

import com.hse.dbcrud.entity.BookCopyEntity;
import com.hse.dbcrud.entity.ReaderEntity;
import com.hse.dbcrud.repository.BookCopyRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class BookCopyService {
    private final BookCopyRepository bcr;

    public BookCopyService(BookCopyRepository bcr) {
        this.bcr = bcr;
    }

    public void addBookCopy(BookCopyEntity copy) {

        bcr.save(copy);
    }

    public BookCopyEntity getById(Long copyid, Long isbn) {

        BookCopyEntity.Key key = new BookCopyEntity.Key();
        key.setCopyNumber(copyid);
        key.setIsbn(isbn);
        return bcr.findById(key).orElse(null);
    }

    public void deleteById(Long copyid, Long isbn) {

        BookCopyEntity.Key key = new BookCopyEntity.Key();
        key.setCopyNumber(copyid);
        key.setIsbn(isbn);
        bcr.deleteById(key);
    }
}
