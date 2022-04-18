package com.zina.BotegyBack.entity;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "player")
public class Player {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(name = "id", nullable = false)
    private UUID id;

    @Column(nullable = false, unique = true)
    private String nickname;

    @Column(nullable = false, unique = true)
    private String email;
    private String password;

    @Column(nullable = false)
    private int isModer; //0 - лох 1 - бох

    public String getNickname() {
        return nickname;
    }

    public void setNickname(String nickname) {
        this.nickname = nickname;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public int getIsModer() {
        return isModer;
    }

    public void setIsModer(int isModer) {
        this.isModer = isModer;
    }

    public UUID getId() {
        return id;
    }
}