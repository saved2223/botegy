package com.zina.BotegyBack.container;

import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Match;
import com.zina.BotegyBack.service.BotService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.Resource;
import org.springframework.core.io.ResourceLoader;

import java.io.IOException;
import java.io.InputStream;
import java.nio.charset.StandardCharsets;
import java.util.List;

public class BotMatchWrapper {
    private Bot bot;
    private List<Match> matches;
    private String code; ///возможно подправить на формат кода

    @Autowired
    BotService botService;

    public BotMatchWrapper(Bot bot, List<Match> matches){
        this.bot = bot;
        this.matches = matches;
        this.code = botService.getCodeForBot(bot);
    }
}
