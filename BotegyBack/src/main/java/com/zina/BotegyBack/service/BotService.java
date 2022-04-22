package com.zina.BotegyBack.service;

import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.repository.BotRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class BotService {
    private final BotRepository botRepository;

    private final MatchService matchService;
    public BotService(BotRepository botRepository, MatchService matchService){
        this.botRepository = botRepository;
        this.matchService = matchService;
    }

    public List<Bot> getBotsForPlayer(UUID id){
       return botRepository.findByPlayer_Id(id);
    }

    public BotMatchWrapper getBotAndMathes(UUID botId){
        return new BotMatchWrapper(botRepository.getById(botId), matchService.getMatchesByBotId(botId));
    }

    public void deleteBot(UUID botId){
        botRepository.deleteById(botId);
    }




}
