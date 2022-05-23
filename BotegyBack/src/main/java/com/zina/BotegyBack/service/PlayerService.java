package com.zina.BotegyBack.service;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.repository.PlayerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;
import java.util.UUID;

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

    public Player updateNick(UUID id, String nick){
        Player p = getPlayer(id);
        p.setNickname(nick);
        playerRepository.save(p);
        return p;
    }

    public void updatePass(UUID id, String pass){
        getPlayer(id).setPassword(pass);
    }

    public Player getPlayer(UUID id){
        return playerRepository.getById(id);
    }

    public Player addPlayer(String nick, String email, String pass){
        Player p = new Player();
        p.setNickname(nick);
        p.setEmail(email);
        p.setPassword(pass);
        playerRepository.save(p);
        return p;
    }

    public Player unDoModer(UUID playerId){
       Player p = playerRepository.getById(playerId);
       p.setIsModer(0);
       playerRepository.save(p);
       return p;
    }

    public Player doModer(UUID playerId){
        Player p = playerRepository.getById(playerId);
        p.setIsModer(1);
        playerRepository.save(p);
        return p;
    }

    public void ban(UUID playerId){
        playerRepository.delete(playerRepository.getById(playerId));
    }
}
