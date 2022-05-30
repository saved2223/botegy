package com.zina.BotegyBack.container;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Match;

import java.util.List;


@JsonIgnoreProperties(value = {"hibernateLazyInitializer", "handler"})
public class BotMatchWrapper {

    public BotMatchWrapper() {

    }

    public BotMatchWrapper(Bot bot, List<Match> matches) {
        this.bot = bot;
        this.matches = matches;
    }

    private Bot bot;
    private List<Match> matches;

    @JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
    public Bot getBot() {
        return bot;
    }

    public void setBot(Bot bot) {
        this.bot = bot;
    }

    public List<Match> getMatches() {
        return matches;
    }

    public void setMatches(List<Match> matches) {
        this.matches = matches;
    }
}
