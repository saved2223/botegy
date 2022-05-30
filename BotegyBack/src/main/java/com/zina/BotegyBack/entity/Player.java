package com.zina.BotegyBack.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "player")
public class Player {

    public Player() {
        this.isModer = 0;
        this.isGoogle = 0;
    }

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(name = "id", nullable = false)
    private UUID id;

    @JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
    @Column(nullable = false, unique = true)
    private String nickname;

    @JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
    @Column(nullable = false, unique = true)
    private String email;

    @JsonIgnore
    private String password;

    @JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
    @Column(nullable = false)
    private int isModer; //0 - лох 1 - бох

    @JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
    @Column(nullable = false)
    private int isGoogle; //0 - лох 1 - бох

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

    public int getIsGoogle() {
        return isGoogle;
    }

    public void setIsGoogle(int isGoogle) {
        this.isGoogle = isGoogle;
    }
}