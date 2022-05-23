package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.container.BotCodeWrapper;
import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.service.BotService;
import com.zina.BotegyBack.service.MatchService;
import com.zina.BotegyBack.service.PlayerService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
public class BotController {
    private final BotService botService;
    private final MatchService matchService;
    public BotController(BotService botService, MatchService matchService){
        this.botService = botService;
        this.matchService = matchService;
    }
    @GetMapping(value = "/getBot")
    public ResponseEntity<BotCodeWrapper> getBot(@RequestParam String botId){
        return ResponseEntity.ok(botService.getBot(UUID.fromString(botId)));
    }

    @PutMapping(value = "/updateBot")
    public ResponseEntity<Bot> updateBot(@RequestParam String botId, @RequestParam String name, @RequestParam String code){
        return ResponseEntity.ok(botService.updateBot(UUID.fromString(botId), name, code));
    }


    @GetMapping(value = "/getBotsForPlayer")
    public ResponseEntity<List<Bot>> getBotsByPlayerId (@RequestParam String playerId){
        return ResponseEntity.ok(botService.getBotsForPlayer(UUID.fromString(playerId)));
    }

    @GetMapping(value = "/getBotsByName")
    public ResponseEntity<List<Bot>> getBotsByName(@RequestParam String botName){
        return ResponseEntity.ok(botService.getBotsByName(botName));
    }

    @PutMapping(value = "/addBot")
    public void addBot(@RequestParam String name, @RequestParam String userId, @RequestParam String code){
        botService.addBot(name, UUID.fromString(userId), code);
    }

    @GetMapping(value = "/getMatchesForBot")
    public ResponseEntity<BotMatchWrapper> getMatchesForBots(@RequestParam String botId){
        return ResponseEntity.ok(matchService.getBotAndMatches(UUID.fromString(botId)));
    }

    @DeleteMapping(value = "/deleteBot")
    public void deleteBot(@RequestParam String botId){
        botService.deleteBot(UUID.fromString(botId));
    }
}
