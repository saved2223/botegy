package com.zina.BotegyBack.repository;

import com.zina.BotegyBack.entity.Bot;
import com.zina.BotegyBack.entity.Player;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

public interface BotRepository extends JpaRepository<Bot, UUID> {

    List<Bot> findByPlayerIs(Player player);

}