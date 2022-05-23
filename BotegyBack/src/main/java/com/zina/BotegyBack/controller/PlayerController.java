package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.service.PlayerService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.UUID;

@RestController
public class PlayerController {
    private final PlayerService playerService;

    public PlayerController(PlayerService playerService){
        this.playerService = playerService;
    }

    @PutMapping(value = "/updateNick")
    public ResponseEntity<Player> updatePlayerName(@RequestParam String userId, @RequestParam String nick){
        return ResponseEntity.ok(playerService.updateNick(UUID.fromString(userId), nick));
    }

    @PutMapping(value = "/ban")
    public void banUser(@RequestParam String userId){
        playerService.ban(UUID.fromString(userId));
    }

    @PutMapping(value = "/updatePass")
    public void updatePlayerPass(@RequestParam String userId, @RequestParam String pass){
        playerService.updatePass(UUID.fromString(userId), pass);
    }

    @PutMapping(value = "/doModer")
    public ResponseEntity<Player> doPlayerModer(@RequestParam String userId){
        return ResponseEntity.ok(playerService.doModer(UUID.fromString(userId)));
    }

    @PutMapping(value = "/unDoModer")
    public ResponseEntity<Player> UnDoPlayerModer(@RequestParam String userId){
        return ResponseEntity.ok(playerService.unDoModer(UUID.fromString(userId)));
    }


}
