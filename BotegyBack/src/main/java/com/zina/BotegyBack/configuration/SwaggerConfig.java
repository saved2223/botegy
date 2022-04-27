package com.zina.BotegyBack.configuration;

import org.springdoc.core.GroupedOpenApi;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;


@Configuration
public class SwaggerConfig  {

    @Bean
    public GroupedOpenApi publicApi(){
        return GroupedOpenApi.builder().group("Botegy")
                .packagesToScan("com.zina.BotegyBack.controller")
                .pathsToMatch("/**")
                .build();
    }


}
