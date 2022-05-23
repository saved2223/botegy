package com.zina.BotegyBack.service;

import com.eclipsesource.v8.V8;
import com.eclipsesource.v8.V8Array;
import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Match;
import com.zina.BotegyBack.repository.BotRepository;
import com.zina.BotegyBack.repository.MatchRepository;
import org.springframework.stereotype.Service;

import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.UUID;

@Service
public class MatchService {
    private final MatchRepository matchRepository;
    private final BotRepository botRepository;
    private final BotService botService;

    private final String defaultScript;
    private final V8 runtime = V8.createV8Runtime();

    public MatchService(MatchRepository matchRepository, BotRepository botRepository, BotService botService){
        this.matchRepository = matchRepository;
        this.botRepository = botRepository;
        this.botService = botService;
        this.defaultScript = createEnvironment();
        this.runtime.executeVoidScript(this.defaultScript);
    }

    public BotMatchWrapper getBotAndMatches(UUID botId){
        return new BotMatchWrapper(botRepository.getById(botId), getMatchesByBotId(botId));
    }

    public String getMatchHistory(UUID matchId){
        Match m = matchRepository.getById(matchId);
        V8Array parameters = new V8Array(runtime);
        parameters.push(botService.getCodeForBot(m.getBot1()));
        parameters.push(botService.getCodeForBot(m.getBot2()));
        return runtime.executeStringFunction("get_match_log", parameters);
    }

    public Match playMatch(UUID bot1Id, UUID bot2Id){
        V8Array parameters = new V8Array(runtime);
        Bot bot1 = botRepository.getById(bot1Id);
        Bot bot2 = botRepository.getById(bot2Id);
        Match match = new Match();
        match.setBot1(bot1);
        match.setBot2(bot2);
        parameters.push(botService.getCodeForBot(bot1));
        parameters.push(botService.getCodeForBot(bot2));
        int func = runtime.executeIntegerFunction("get_match_results", parameters);
        switch (func) {
            case 0 -> match.setWinnerBot(bot1);
            case 1 -> match.setWinnerBot(bot2);
        }
        matchRepository.save(match);
        return match;
    }

    public List<Match> getMatchesByBotId(UUID botId){
        return matchRepository.findByBot1_IdIsOrBot2_IdIs(botId, botId);
    }

    private String createEnvironment() {
        String uniteScript = readFileFromResources("unite.js");
        String botScript = readFileFromResources("bot.js");
        String fieldScript = readFileFromResources("field.js");
        String matchScript = readFileFromResources("match.js");
        return uniteScript + "\n" + botScript + "\n" + fieldScript + "\n" + matchScript;
    }

    private String readFileFromResources(String filename) {
        filename = "js/" + filename;
        InputStream is = this.getClass().getClassLoader().getResourceAsStream(filename);
        StringBuilder sb = new StringBuilder();
        try(BufferedReader br = new BufferedReader(new InputStreamReader(is, StandardCharsets.UTF_8))) {
            String line = br.readLine();
            while (line != null) {
                sb.append(line);
                sb.append(System.lineSeparator());
                line = br.readLine();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        return sb.toString();
    }

}
