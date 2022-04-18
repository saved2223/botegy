package com.zina.BotegyBack.entity;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "match")
public class Match {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(name = "id", nullable = false)
    private UUID id;

    @ManyToOne(cascade = CascadeType.ALL, optional = false)
    @JoinColumn(name = "bot_1_id", nullable = false)
    private Bot bot1;

    @ManyToOne(cascade = CascadeType.ALL, optional = false)
    @JoinColumn(name = "bot_2_id", nullable = false)
    private Bot bot2;

    @ManyToOne(cascade = CascadeType.ALL)
    @JoinColumn(name = "winner_bot_id")
    private Bot winnerBot;

    public Bot getWinnerBot() {
        return winnerBot;
    }

    public void setWinnerBot(Bot winnerBot) {
        this.winnerBot = winnerBot;
    }

    public Bot getBot2() {
        return bot2;
    }

    public void setBot2(Bot bot2) {
        this.bot2 = bot2;
    }

    public Bot getBot1() {
        return bot1;
    }

    public void setBot1(Bot bot1) {
        this.bot1 = bot1;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }
}