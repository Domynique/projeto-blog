# Feedback do Instrutor

#### 23/10/24 - Revisão Inicial - Eduardo Pires

## Pontos Positivos:

- Separação de responsabilidades
- Demonstrou conhecimento em Identity e JWT.
- Mostrou entendimento do ecossistema de desenvolvimento em .NET

## Pontos Negativos:

- O projeto não está completo / totalmente funcional
- Já que a camada business é anemica e depende de identity não seria necessário separá-la, daria para unificar Data e Business em uma única camada "Core", pois a complexidade do projeto não pede algo além disto.
- A maneira de criar o autor/user não está errada, mas poderia ser bem melhor, a entidade Autor pode ser 100% independente de user mas ambos serem tratados como uma coisa só pela aplicação, assim evita acoplamento do Identity no domain.
- A maneira de se obter o usuário e comparar com o dono do post está muito verbosa e complexa (vai ser ruim de manter)

## Sugestões:

- Unificar a criação do user + autor no mesmo processo. Utilize o ID do registro do Identity como o ID da PK do Autor, assim você mantém um link lógico entre os elementos.
- Simplificar a arquitetura 3 camadas (web, api, core) resolveriam tudo devido a baixa complexidade.

## Problemas:

- Não consegui executar a aplicação MVC, está dando 404, verifique sua configuração de rotas e etc.
