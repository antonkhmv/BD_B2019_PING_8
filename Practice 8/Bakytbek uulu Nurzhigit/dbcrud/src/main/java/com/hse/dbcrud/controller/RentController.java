package com.hse.dbcrud.controller;

import com.hse.dbcrud.entity.RentEntity;
import com.hse.dbcrud.model.RentModel;
import com.hse.dbcrud.service.RentService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/rents")
public class RentController {

    private final RentService rs;

    public RentController(RentService rs) {
        this.rs = rs;
    }

    @PostMapping
    public ResponseEntity createRent(@RequestBody RentModel rent) {
        rs.addRent(rent);

        return ResponseEntity.ok("rent added");
    }

    @GetMapping
    public ResponseEntity getRent(@RequestParam Long copyNumber,
                                  @RequestParam Long isbn, @RequestParam Long readerNumber) {

        return ResponseEntity.ok(rs.getById(copyNumber, isbn, readerNumber));
    }

    @DeleteMapping
    public ResponseEntity deleteRent(@RequestParam Long copyNumber,
                                  @RequestParam Long isbn, @RequestParam Long readerNumber) {

        rs.deleteById(copyNumber, isbn, readerNumber);
        return ResponseEntity.ok("deleted");
    }


}
