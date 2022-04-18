package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.service.PlayerService;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.awt.*;
import java.util.Optional;

@RestController
public class AuthenticationController {

    private final PlayerService playerService;

    public AuthenticationController(PlayerService playerService){
        this.playerService = playerService;
    }

    @PostMapping(value = "/logIn")
    public ResponseEntity<?> logIn (@RequestParam String email, @RequestParam String pass){
        final Optional<Player> player = playerService.getPlayerByEmailAndPass(email, pass);
        return player.isEmpty() ? new ResponseEntity<>(HttpStatus.OK) : new ResponseEntity<>(HttpStatus.NOT_FOUND);
    }

    @PostMapping(value = "/logUp")
    public ResponseEntity<?> logUpDefault(@RequestParam String nick, @RequestParam String email, @RequestParam String pass){
        playerService.addPlayer(nick, email, pass);
        return new ResponseEntity<>(HttpStatus.CREATED);
    }



}
