package com.zina.BotegyBack.service;

import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Match;
import com.zina.BotegyBack.repository.BotRepository;
import com.zina.BotegyBack.repository.MatchRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class MatchService {
    private final MatchRepository matchRepository;
    private final BotRepository botRepository;


    public MatchService(MatchRepository matchRepository, BotRepository botRepository){
        this.matchRepository = matchRepository;
        this.botRepository = botRepository;
    }

    public Match playMatch(UUID bot1Id, UUID bot2Id){
        Bot bot1 = botRepository.getById(bot1Id);
        Bot bot2 = botRepository.getById(bot2Id);
        Match match = new Match();
        match.setBot1(bot1);
        match.setBot2(bot2);
        //insert your logic here

        //match.setWinnerBot(winnerBot);
        matchRepository.save(match);
        return match;
    }

    public List<Match> getMatchesByBotId(UUID botId){
        return matchRepository.findByBot1_IdIsOrBot2_IdIs(botId, botId);
    }

}
