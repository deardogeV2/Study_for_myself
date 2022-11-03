package com.example.LearnForDubbo.consumers.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.ImportResource;
import service.providerServerRPC;

@ImportResource("/providerBean.xml")
@Configuration
public class DubboConfig {
}
