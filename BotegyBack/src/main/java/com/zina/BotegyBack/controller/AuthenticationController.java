package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.service.PlayerService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;

@RestController
public class AuthenticationController {

    private final PlayerService playerService;

    public AuthenticationController(PlayerService playerService){
        this.playerService = playerService;
    }

    @PostMapping(value = "/log")
    public ResponseEntity<Player> logIn (@RequestParam String email, @RequestParam String pass){
        final Optional<Player> player = playerService.getPlayerByEmailAndPass(email, pass);
        return player.map(ResponseEntity::ok).orElseGet(() -> ResponseEntity.notFound().build());
    }

    @PostMapping(value = "/logUp")
    public ResponseEntity<Player> logUpDefault(@RequestParam String nick, @RequestParam String email, @RequestParam String pass){
        Player p = playerService.addPlayer(nick, email, pass);
        return ResponseEntity.ok(p);
    }




}
