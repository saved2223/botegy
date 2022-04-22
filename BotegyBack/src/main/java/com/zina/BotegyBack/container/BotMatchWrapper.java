package com.zina.BotegyBack.container;

import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Match;

import java.util.List;

public class BotMatchWrapper {
    private Bot bot;
    private List<Match> matches;
    private String code; ///возможно подправить на формат кода

    public BotMatchWrapper(Bot bot, List<Match> matches){
        this.bot = bot;
        this.matches = matches;
        //todo: сделать доставание кода из файла и определить тут
    }
}
