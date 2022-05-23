package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Match;
import com.zina.BotegyBack.service.MatchService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.UUID;

@RestController
public class MatchController {

    private final MatchService matchService;

    public MatchController(MatchService matchService) {
        this.matchService = matchService;
    }

    @GetMapping(value = "/doFight")
    public ResponseEntity<Bot> doFight(@RequestParam String bot1Id, @RequestParam String bot2Id) {
        Match m = matchService.playMatch(UUID.fromString(bot1Id), UUID.fromString(bot2Id));
        return ResponseEntity.ok(m.getWinnerBot());
    }

    @GetMapping(value = "/getMatchHistory")
    public ResponseEntity<String> getMatchHistory(@RequestParam String matchId){
        return ResponseEntity.ok(matchService.getMatchHistory(UUID.fromString(matchId)));
    }


}
