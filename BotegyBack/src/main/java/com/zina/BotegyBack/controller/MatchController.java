package com.zina.BotegyBack.controller;

import com.zina.BotegyBack.container.History;
import com.zina.BotegyBack.entity.Match;
import com.zina.BotegyBack.service.MatchService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
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
    public ResponseEntity<Match> doFight(@RequestParam String bot1Id, @RequestParam String bot2Id) {

        Match m = matchService.playMatch(UUID.fromString(bot1Id), UUID.fromString(bot2Id));
        if (m == null) {
            return ResponseEntity.badRequest().build();
        } else return ResponseEntity.ok(m);
    }

    @GetMapping(value = "/getMatchHistory")
    public ResponseEntity<History> getMatchHistory(@RequestParam String matchId) {
        return ResponseEntity.ok(matchService.getMatchHistory(UUID.fromString(matchId)));
    }


}
