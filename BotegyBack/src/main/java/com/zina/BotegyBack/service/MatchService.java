package com.zina.BotegyBack.service;

import com.eclipsesource.v8.V8;
import com.eclipsesource.v8.V8Array;
import com.eclipsesource.v8.V8ScriptExecutionException;
import com.zina.BotegyBack.container.BotMatchWrapper;
import com.zina.BotegyBack.container.History;
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


    public MatchService(MatchRepository matchRepository, BotRepository botRepository) {
        this.matchRepository = matchRepository;
        this.botRepository = botRepository;
    }

    public BotMatchWrapper getBotAndMatches(UUID botId) {
        BotMatchWrapper bmw = new BotMatchWrapper();
        bmw.setMatches(getMatchesByBotId(botId));
        bmw.setBot(botRepository.findById(botId).get());
        return bmw;
    }

    public History getMatchHistory(UUID matchId) {
        V8 runtime = V8.createV8Runtime();
        String defaultScript = createEnvironment();
        runtime.executeVoidScript(defaultScript);
        Match m = matchRepository.getById(matchId);
        V8Array parameters = new V8Array(runtime);
        parameters.push(m.getBot1().getCode());
        parameters.push(m.getBot2().getCode());

        InputStream s = new ByteArrayInputStream(
                (runtime.executeStringFunction("get_match_log", parameters))
                        .getBytes(StandardCharsets.UTF_8));
        String res = "";
        try {
            res = new String(s.readAllBytes());
        } catch (IOException e) {
            e.printStackTrace();
        }
        History history = new History();
        history.setHistory(res);
        return history;
    }

    public Match playMatch(UUID bot1Id, UUID bot2Id) {
        V8 runtime = V8.createV8Runtime();
        String defaultScript = createEnvironment();
        runtime.executeVoidScript(defaultScript);
        V8Array parameters = new V8Array(runtime);
        Bot bot1 = botRepository.findById(bot1Id).get();
        Bot bot2 = botRepository.findById(bot2Id).get();
        Match match = new Match();
        match.setBot1(bot1);
        match.setBot2(bot2);
        parameters.push(match.getBot1().getCode());
        parameters.push(match.getBot2().getCode());
        int func = -1;
        try {
            func = runtime.executeIntegerFunction("get_match_results", parameters);
        } catch (V8ScriptExecutionException e) {
            e.printStackTrace();
        }

        if (func != -1) {
            switch (func) {
                case 0 -> match.setWinnerBot(bot1);
                case 1 -> match.setWinnerBot(bot2);
            }
            matchRepository.save(match);
            return match;
        } else return null;

    }

    public List<Match> getMatchesByBotId(UUID botId) {
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
        try (BufferedReader br = new BufferedReader(new InputStreamReader(is, StandardCharsets.UTF_8))) {
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
