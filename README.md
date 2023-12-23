# Blog Web API

[![NPM](https://img.shields.io/github/license/jehveiga/Blog-api)](https://github.com/jehveiga/Blog-api/blob/master/LICENSE)

# Sobre o projeto

Desenvolvimento de uma aplicação Web API REST com ASP.NET Core. Neste projeto foi desenvolver por completo uma aplicação ASP.NET MVC Web API.

Este projeto suporta todas as versões do ASP.NET Core (até a 7.0) e foi utilizado o Visual Studio 2022 para a construção da aplicação final.

O projeto está disponível na versão 7.0 Aplicação com gerenciamento de Categoria, Post e Role de usuários, Tag e usuários a aplicação contem as end-points com métodos CRUD(Create, Read, Update e Delete) de seus cadastros respectivos.
Tem todas validações de models de preenchimento tanto no back-end, as validações mostram alertas personalizados no Response para verificar as impossilidades de cada umas das ações que não foram possíveis realizar ou de sucesso ao realizar ou problemas no servidor. Controles de Cadastros e também com upload de imagens, utilizados as validações dos modelos usando as regras de negócios disponíveis na aplicação.

Usado nos envios/retorno se necessário envio de parâmetro, foi utilizado informações para o consumo dos end-points as ViewModels para transitar dados para as classes modelos para persistencias de dados no banco, utilizados Serialização e Deserialização JSON a quem estará consumindo o serviço da Web API, usado as Models, Controller, Rotas no projeto como padrão de arquitetura do projeto, utilizado SQL Server como banco de dados alvo e com suporte de mudança se necessário usando príncipios de Injeção de Depência e Inversão de controle do contexto que representa o banco de dados na aplicação, nomeado os end-points conforme o dados que transita na aplicação, usado tratamentos padronização de tratamento de erros personalizados para usabilidade a quem está consumindo o serviço da Web API, utilizado os métodos Assíncronos para melhor velocidade de resposta e melhor processamentos de informações, usado versionamento da API.

Utilizado autenticação e autorização usando Token JWT criando o token de usuário ao registro para consumo da API, utilizado as claims do usuário cadastrado nos bancos conforme autorizado nos end-points, usado criptação de senhas para cadastro no banco e comparação ao usuário ao logar na API, configurado o envio de e-mail usando o serviço de SMTP na configuração da aplicação, registrado end-point de upload de imagem para cadastro com vinculo ao usuário.

Utilizado paginação de dados nos end-points que retorna listagem no result da requisições, utilizado serviço de cache no consumo dos end-points, forçado o uso de HTTPS para segurança da aplicação

Mensagem de negócios e afins capturadas para informar o usuário sobre ações dentro da aplicação. Utilizando a arquitetura limpa e principios SOLID na prática.

# Apresentação - Categoria

## Métodos Get, GetById, Post, Put e Delete
![Apresentacao GerarSenha](https://github.com/jehveiga/Blog-api/blob/master/assets/apresentacao-end-point.gif)

# Apresentação - Gerar Senha

![Apresentacao GerarSenha](https://github.com/jehveiga/Blog-api/blob/master/assets/gerar-password.gif)

# Imagem apresentação - Gerar Token JWT

![Apresentacao GerarToken](https://github.com/jehveiga/Blog-api/blob/master/assets/gerando_token.png)

# Imagem apresentação - Listar Categorias

![Apresentacao Listar Categorias](https://github.com/jehveiga/Blog-api/blob/master/assets/lista_categorias.png)

# Imagem apresentação - Obter Categoria Por Id

![Apresentacao Categoria por Id](https://github.com/jehveiga/Blog-api/blob/master/assets/obter-categoria-porId.png)

# Tecnologias Utilizadas

## Back end

- C#

## Outras Tecnologias

- Swagger
- Asp.Net Core
- Asp.Net Web API
- Authentication JwtBearer
- AspNetCore OpenApi
- Secure Identity

## Banco de Dados

- SQL Server

# Autor 

Jefferson Veiga

https://www.linkedin.com/in/jefferson-veiga-dev/
