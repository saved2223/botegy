package com.zina.BotegyBack.repository;

import com.zina.BotegyBack.entity.Match;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface MatchRepository extends JpaRepository<Match, UUID> {
}