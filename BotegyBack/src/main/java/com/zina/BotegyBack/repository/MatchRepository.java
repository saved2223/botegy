package com.zina.BotegyBack.repository;

import com.zina.BotegyBack.entity.Match;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.UUID;

public interface MatchRepository extends JpaRepository<Match, UUID> {
    List<Match> findByBot1_IdIsOrBot2_IdIs(UUID id, UUID id1);

    List<Match> findByBot1_IdIsAndBot2_IdIs(UUID id, UUID id1);

    List<Match> findByBot1_Player_IdOrBot2_Player_Id(UUID id, UUID id1);

    List<Match> findByBot1_Player_IdAndBot2_Player_Id(UUID id, UUID id1);


}