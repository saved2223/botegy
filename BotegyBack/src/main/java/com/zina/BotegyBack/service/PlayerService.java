package com.zina.BotegyBack.service;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.repository.PlayerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class PlayerService {
    private final PlayerRepository playerRepository;

    @Autowired
    public PlayerService(PlayerRepository playerRepository){
        this.playerRepository = playerRepository;
    }

    public Optional<Player> getPlayerByEmailAndPass(String email, String pass){
        return playerRepository.findByEmailIsAndPasswordIs(email, pass);
    }

    public Player addPlayer(String nick, String email, String pass){
        Player p = new Player();
        p.setNickname(nick);
        p.setEmail(email);
        p.setPassword(pass);
        playerRepository.save(p);
        return p;
    }
}
