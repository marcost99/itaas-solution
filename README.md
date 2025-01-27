"# itaas-solution" 

1- Bibliotecas e recursos utilizados neste projeto: 
- Linguagem: C# utilizando a plataforma Net Core 2.1.
- Banco de dados: SQL Server; migrations
- ORM: Entity Framework
- Validações: biblioteca FluentValidation
- testes unitários: Xunit utilizando a biblioteca FluentAssertions

2- Padrões e boas práticas de codificação implementados:
- POO
- SOLID
- DDD
- Injeção de dependência
- Filtro de exceções

3- Estruturação do Projeto:
3.1- Pastas da raiz
- repository: contém os arquivos de log salvos pela aplicação. Foi criado apenas para fins de demonstração da funcionalidade pois em um ambiente produtivo pode ser mais indicado salvar esses arquivos em um outro local
- src: contém os arquivos com os códigos da aplicação
- tests: contém os arquivos do projeto de teste das aplicação da pasta src

3.2- Pastas do projeto ItaasSolution.Api dentro da pasta src:
- Api: contém os arquivos relacionados a Web API. Como por exemplo os arquivos de controllers
- Application: contém os arquivos relacionados as regras de negócio. Como por exemplo os casos de uso e as validações
- Communication: contém as classes com os Payloads e Requests das requisições
- Domain: contém os domínios da aplicação. Como por exemplo as entidades e os contratos das ações que devem ser feitas (salvar, consultar logs e etc)
- Exception: contém as classes de exceções customizadas e o arquivo de recursos com as mensagens de erros
- Infraestructure: implementa os contratos das ações descritas na pasta de Domain. As classes dessas pastas possuem funcionalidades que manipulam recursos como por exemplo arquivos e banco de dados

3.2- Pastas do projeto ItaasSolution.Test dentro da pasta tests:
- contém as funções de testes das funcionalidades desenvolvidas no projeto ItaasSolution.Api