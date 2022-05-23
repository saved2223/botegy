package com.zina.BotegyBack.container;

import com.zina.BotegyBack.entity.Bot;
import lombok.Getter;
import lombok.Setter;

public class BotCodeWrapper {

    @Getter
    @Setter
    private Bot bot;

    @Getter
    @Setter
    private String code;
}
