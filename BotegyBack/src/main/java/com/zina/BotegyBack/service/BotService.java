package com.zina.BotegyBack.service;

import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.repository.BotRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.Resource;
import org.springframework.core.io.ResourceLoader;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.UUID;

@Service
public class BotService {
    private final BotRepository botRepository;
    private final ResourceLoader resourceLoader;
    private final MatchService matchService;
    public BotService(BotRepository botRepository, MatchService matchService, ResourceLoader resourceLoader){
        this.resourceLoader = resourceLoader;
        this.botRepository = botRepository;
        this.matchService = matchService;
    }

    public String getCodeForBot(Bot bot){
        String code = "";
        Resource resource = resourceLoader.getResource("classpath:bots/"+bot.getId());
        try {
            InputStream inputStream = resource.getInputStream();
            code = new String(inputStream.readAllBytes(), StandardCharsets.UTF_8);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return code;
    }

    public List<Bot> getBotsForPlayer(UUID id){
       return botRepository.findByPlayer_Id(id);
    }

    public BotMatchWrapper getBotAndMatches(UUID botId){
        return new BotMatchWrapper(botRepository.getById(botId), matchService.getMatchesByBotId(botId));
    }

    public void deleteBot(UUID botId){
        botRepository.deleteById(botId);
    }




}
