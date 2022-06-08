package com.zina.BotegyBack.service;

import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.repository.BotRepository;
import com.zina.BotegyBack.repository.MatchRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.UUID;

@Service
public class BotService {
    private final BotRepository botRepository;
    private final PlayerService playerService;
    private final MatchRepository matchRepository;

    public BotService(BotRepository botRepository, PlayerService playerService, MatchRepository matchRepository) {
        this.botRepository = botRepository;
        this.playerService = playerService;
        this.matchRepository = matchRepository;
    }


    public void addBot(String name, UUID userId, String code) {
        Bot bot = new Bot();
        bot.setName(name);
        bot.setPlayer(playerService.getPlayer(userId));
        bot.setCode(code);
        botRepository.save(bot);


    }

    public List<Bot> getBotsByName(String botName) {
        return botRepository.findByName(botName);
    }

    public Bot getBot(UUID botId) {
        return botRepository.findById(botId).get();
    }

    public Bot updateBot(UUID id, String name, String code) {
        delMatches(id);
        Bot b = botRepository.findById(id).get();
        b.setName(name);
        b.setCode(code);
        botRepository.save(b);
        return b;
    }





    public List<Bot> getBotsForPlayer(UUID id) {
        return botRepository.findByPlayer_Id(id);
    }


    private void delMatches(UUID botId) {
        matchRepository.findByBot1_IdIsAndBot2_IdIs(botId, botId).forEach(match -> {
            match.setWinnerBot(null);
            match.setBot2(null);
            match.setBot1(null);
            matchRepository.save(match);
            matchRepository.delete(match);
        });
        matchRepository.findByBot1_IdIsOrBot2_IdIs(botId, botId).forEach(match -> {
            match.setWinnerBot(null);
            match.setBot2(null);
            match.setBot1(null);
            matchRepository.save(match);
            matchRepository.delete(match);
        });
    }

    public void deleteBot(UUID botId) {
        delMatches(botId);
        Bot b = botRepository.findById(botId).get();
        b.setPlayer(null);
        botRepository.save(b);
        botRepository.delete(b);
    }


}
