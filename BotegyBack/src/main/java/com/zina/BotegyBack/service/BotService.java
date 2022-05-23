package com.zina.BotegyBack.service;

import com.zina.BotegyBack.container.BotCodeWrapper;
import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.repository.BotRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.Resource;
import org.springframework.core.io.ResourceLoader;
import org.springframework.stereotype.Service;

import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.UUID;

@Service
public class BotService {
    private final BotRepository botRepository;
    private final ResourceLoader resourceLoader;
    private final MatchService matchService;
    private final PlayerService playerService;
    public BotService(BotRepository botRepository, MatchService matchService, ResourceLoader resourceLoader, PlayerService playerService){
        this.resourceLoader = resourceLoader;
        this.botRepository = botRepository;
        this.matchService = matchService;
        this.playerService = playerService;
    }

    public String getCodeForBot(Bot bot){
        String code = "";
        Resource resource = resourceLoader.getResource("classpath:bots/"+bot.getId() + ".js");
        try {
            InputStream inputStream = resource.getInputStream();
            code = new String(inputStream.readAllBytes(), StandardCharsets.UTF_8);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return code;
    }

    public void addBot(String name, UUID userId, String code){
        Bot bot = new Bot();
        bot.setName(name);
        bot.setPlayer(playerService.getPlayer(userId));
        createCodeFile(bot.getId().toString(), code);
        botRepository.save(bot);
    }

    private void createCodeFile(String name, String code){
        try {
            File file = new File(resourceLoader.getResource("classpath:js/").getFile() + name + ".js");
            BufferedWriter writer = new BufferedWriter(new FileWriter(file));
            writer.write(code);
            writer.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public Bot updateBot(UUID id, String name, String code){
        Bot b = botRepository.getById(id);
        b.setName(name);
        deleteCode(id.toString());
        createCodeFile(id.toString(), code);
        botRepository.save(b);
        return b;
    }

    public BotCodeWrapper getBot(UUID botId){
        BotCodeWrapper bcw = new BotCodeWrapper();
        Bot bot = botRepository.getById(botId);
        bcw.setBot(bot);
        bcw.setCode(getCodeForBot(bot));
        return bcw;
    }

    public List<Bot> getBotsByName(String botName){
        return botRepository.findByName(botName);
    }

    public List<Bot> getBotsForPlayer(UUID id){
       return botRepository.findByPlayer_Id(id);
    }

    public BotMatchWrapper getBotAndMatches(UUID botId){
        return new BotMatchWrapper(botRepository.getById(botId), matchService.getMatchesByBotId(botId));
    }

    private void deleteCode(String name){
        Resource resource = resourceLoader.getResource("classpath:bots/"+ name + ".js");
        try {
            resource.getFile().delete();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void deleteBot(UUID botId){
        deleteCode(botId.toString());
        botRepository.deleteById(botId);
    }




}
