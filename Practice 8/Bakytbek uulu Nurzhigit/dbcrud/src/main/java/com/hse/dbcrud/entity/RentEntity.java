package com.hse.dbcrud.entity;

import com.fasterxml.jackson.annotation.JsonFormat;

import javax.persistence.*;
import java.io.Serializable;
import java.util.Date;

@Entity
public class RentEntity {

    @EmbeddedId
    private Key key;

    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    private Date returnDate;

    public Date getReturnDate() {
        return returnDate;
    }

    public void setReturnDate(Date returnDate) {
        this.returnDate = returnDate;
    }

    public Key getKey() {
        return key;
    }

    public void setKey(Key key) {
        this.key = key;
    }


    @Embeddable
    public static class Key implements Serializable {

        public Key() {
        }

        @ManyToOne(cascade=CascadeType.REFRESH)
        @JoinColumn(name = "reader", nullable = false)
        private ReaderEntity reader;

        @ManyToOne(cascade=CascadeType.REFRESH)
        @JoinColumns({
                @JoinColumn(name = "copyNumber", referencedColumnName = "copyNumber", nullable = false),
                @JoinColumn(name = "isbn", referencedColumnName = "isbn", nullable = false)
        })
        private BookCopyEntity bkkey;

        public ReaderEntity getReader() {
            return reader;
        }

        public void setReader(ReaderEntity readerId) {
            this.reader = readerId;
        }

        public BookCopyEntity getBkkey() {
            return bkkey;
        }

        public void setBkkey(BookCopyEntity bkkey) {
            this.bkkey = bkkey;
        }
    }
}
