package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.entity.Match;
import com.zina.BotegyBack.service.MatchService;
import org.springframework.http.ResponseEntity;
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

    @PostMapping(value = "/fastFight")
    public ResponseEntity<?> doFight(@RequestParam UUID bot1Id, @RequestParam UUID bot2Id) {
        Match m = matchService.playMatch(bot1Id, bot2Id);

        return ResponseEntity.ok(m.getWinnerBot().getName());
    }


}
