package com.zina.BotegyBack.repository;

import com.zina.BotegyBack.entity.Player;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;
import java.util.UUID;

public interface PlayerRepository extends JpaRepository<Player, UUID> {
    Optional<Player> findByEmailIsAndPasswordIs(String email, String password);


}