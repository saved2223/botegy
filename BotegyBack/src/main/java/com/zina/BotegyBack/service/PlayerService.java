package com.zina.BotegyBack.service;

import com.zina.BotegyBack.entity.Player;
import com.zina.BotegyBack.repository.BotRepository;
import com.zina.BotegyBack.repository.MatchRepository;
import com.zina.BotegyBack.repository.PlayerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;
import java.util.UUID;

@Service
public class PlayerService {
    private final PlayerRepository playerRepository;
    private final BotRepository botRepository;
    private final MatchRepository matchRepository;

    @Autowired
    public PlayerService(PlayerRepository playerRepository, BotRepository botRepository, MatchRepository matchRepository) {
        this.playerRepository = playerRepository;
        this.botRepository = botRepository;
        this.matchRepository = matchRepository;
    }

    public Player authPlayerByGoogle(String email, String nick) {
        Optional<Player> p = getPlayerByEmail(email);
        if (p.isPresent()) {
            return p.get();
        } else {
            Player player = new Player();
            player.setIsGoogle(1);
            player.setNickname(nick);
            player.setEmail(email);
            playerRepository.save(player);
            return player;
        }
    }

    public Optional<Player> getPlayerByEmail(String email) {
        return playerRepository.findByEmailAndIsGoogle(email, 1);
    }

    public Optional<Player> getPlayerByEmailAndPass(String email, String pass) {
        return playerRepository.findByEmailIsAndPasswordIs(email, pass);
    }

    public Player updateNick(UUID id, String nick) {
        Player p = getPlayer(id);
        p.setNickname(nick);
        playerRepository.save(p);
        return p;
    }

    public Optional<Player> updatePass(UUID id, String pass, String oldPass) {
        Optional<Player> p = Optional.ofNullable(getPlayer(id));
        if (p.get().getPassword().equals(oldPass)) {
            p.get().setPassword(pass);
            playerRepository.save(p.get());
        } else p = Optional.empty();
        return p;
    }

    public Player getPlayer(UUID id) {
        return playerRepository.findById(id).get();
    }

    public Player addPlayer(String nick, String email, String pass) {
        Player p = new Player();
        p.setNickname(nick);
        p.setEmail(email);
        p.setPassword(pass);
        playerRepository.save(p);
        return p;
    }

    public Player unDoModer(UUID playerId) {
        Player p = getPlayer(playerId);
        p.setIsModer(0);
        playerRepository.save(p);
        return p;
    }

    public Player doModer(UUID playerId) {
        Player p = playerRepository.findById(playerId).get();
        p.setIsModer(1);
        playerRepository.save(p);
        return p;
    }

    public void ban(UUID playerId) {
        Player p = getPlayer(playerId);
        matchRepository.findByBot1_Player_IdAndBot2_Player_Id(playerId, playerId).forEach(match -> {
            match.setWinnerBot(null);
            match.setBot2(null);
            match.setBot1(null);
            matchRepository.save(match);
            matchRepository.delete(match);
        });
        matchRepository.findByBot1_Player_IdOrBot2_Player_Id(playerId, playerId).forEach(match -> {
            match.setWinnerBot(null);
            match.setBot2(null);
            match.setBot1(null);
            matchRepository.save(match);
            matchRepository.delete(match);
        });
        botRepository.findByPlayer_Id(p.getId()).forEach(b -> {
            b.setPlayer(null);
            botRepository.save(b);
            botRepository.delete(b);
        });

        playerRepository.delete(getPlayer(playerId));

    }
}
