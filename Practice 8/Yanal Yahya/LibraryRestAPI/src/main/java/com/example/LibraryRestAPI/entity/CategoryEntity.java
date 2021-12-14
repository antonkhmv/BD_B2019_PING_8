package com.example.LibraryRestAPI.entity;

import javax.persistence.*;
import java.util.List;

@Entity
@Table(name = "category")
public class CategoryEntity {
    @Id
    private String categoryName;

    @OneToOne
    private CategoryEntity parentCategory;

    @OneToMany(cascade = CascadeType.ALL, mappedBy = "category")
    private List<BookCategoryEntity> readers;

    public List<BookCategoryEntity> getReaders() {
        return readers;
    }

    public void setReaders(List<BookCategoryEntity> readers) {
        this.readers = readers;
    }

    public String getCategoryName() {
        return categoryName;
    }

    public void setCategoryName(String categoryName) {
        this.categoryName = categoryName;
    }

    public CategoryEntity getParentCategory() {
        return parentCategory;
    }

    public void setParentCategory(CategoryEntity parentCategory) {
        this.parentCategory = parentCategory;
    }

    public CategoryEntity() {
    }
}
