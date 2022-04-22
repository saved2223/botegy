package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.service.BotService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.UUID;

@RestController
public class BotController {
    private final BotService botService;

    public BotController(BotService botService){
        this.botService = botService;
    }

    @PostMapping(value = "/getBotsForPlayer")
    public ResponseEntity<List<Bot>> getBotsByPlayerId (@RequestParam UUID playerId){
        return ResponseEntity.ok(botService.getBotsForPlayer(playerId));
    }

    @PostMapping(value = "/getMatchesForBot")
    public ResponseEntity<BotMatchWrapper> getMatchesForBots(@RequestParam UUID botId){
        return ResponseEntity.ok(botService.getBotAndMathes(botId));
    }

    @PostMapping(value = "/deleteBot")
    public void deleteBot(@RequestParam UUID botId){
        botService.deleteBot(botId);
    }
}
