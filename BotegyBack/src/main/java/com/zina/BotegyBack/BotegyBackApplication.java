package com.zina.BotegyBack;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.servlet.FilterRegistrationBean;
import org.springframework.context.annotation.Bean;
import org.springframework.core.Ordered;
import org.springframework.core.annotation.Order;
import org.springframework.web.filter.CharacterEncodingFilter;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;


@SpringBootApplication
@EnableWebMvc
public class BotegyBackApplication {

    public static void main(String[] args) {
        SpringApplication.run(BotegyBackApplication.class, args);
    }


    @Bean
    @Order(Ordered.HIGHEST_PRECEDENCE)
    public FilterRegistrationBean<CharacterEncodingFilter> characterEncodingFilterRegistration() {
        CharacterEncodingFilter filter = new CharacterEncodingFilter();
        filter.setEncoding("UTF-8"); // use your preferred encoding
        filter.setForceEncoding(true); // force the encoding

        FilterRegistrationBean<CharacterEncodingFilter> registrationBean =
                new FilterRegistrationBean<>(filter); // register the filter
        registrationBean.addUrlPatterns("/*"); // set preferred url
        return registrationBean;
    }

}
