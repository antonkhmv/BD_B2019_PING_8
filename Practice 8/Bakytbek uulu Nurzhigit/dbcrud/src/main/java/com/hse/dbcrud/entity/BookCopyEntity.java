package com.hse.dbcrud.entity;

import javax.persistence.*;
import java.io.Serializable;
import java.util.List;

@Entity
public class BookCopyEntity implements Serializable{

    @EmbeddedId
    private Key key;

    private Long position;

    public Long getPosition() {
        return position;
    }

    public void setPosition(Long position) {
        this.position = position;
    }

    public Key getKey() {
        return key;
    }

    public void setKey(Key key) {
        this.key = key;
    }

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "key.bkkey", fetch=FetchType.LAZY)
    private List<RentEntity> rents;

    @Embeddable
    public static class Key implements Serializable {

        public Key() {
        }

        @Column(nullable = false, name = "copyNumber")
        private Long copyNumber;

        @Column(nullable = false, name = "isbn")
        private Long isbn;

        public Long getCopyNumber() {
            return copyNumber;
        }

        public void setCopyNumber(Long copyNumber) {
            this.copyNumber = copyNumber;
        }

        public Long getIsbn() {
            return isbn;
        }

        public void setIsbn(Long isbn) {
            this.isbn = isbn;
        }
    }
}
