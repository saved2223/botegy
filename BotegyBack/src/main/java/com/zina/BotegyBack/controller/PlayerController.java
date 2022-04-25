package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.service.PlayerService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.UUID;

@RestController
public class PlayerController {
    private final PlayerService playerService;

    public PlayerController(PlayerService playerService){
        this.playerService = playerService;
    }

    @PostMapping(value = "/updateNick")
    public ResponseEntity<Player> updatePlayerName(@RequestParam UUID userId, @RequestParam String nick){
        return ResponseEntity.ok(playerService.updateNick(userId, nick));
    }

    @PostMapping(value = "/updatePass")
    public void updatePlayerPass(@RequestParam UUID userId, @RequestParam String pass){
        playerService.updatePass(userId, pass);
    }

    @PostMapping(value = "/doModer")
    public ResponseEntity<Player> doPlayerModer(@RequestParam UUID userId){
        return ResponseEntity.ok(playerService.doModer(userId));
    }

    @PostMapping(value = "/UnDoModer")
    public ResponseEntity<Player> UnDoPlayerModer(@RequestParam UUID userId){
        return ResponseEntity.ok(playerService.unDoModer(userId));
    }


}
