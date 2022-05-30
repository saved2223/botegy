package com.zina.BotegyBack.controller;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class ConfigController {

    @GetMapping(value = "/isGoogleLogin")
    public ResponseEntity<String> getIsGoogleLogin() {
        String google = System.getenv("googleAuth");
        return ResponseEntity.ok(google);
    }

}
