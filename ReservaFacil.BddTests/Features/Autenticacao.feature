# language: pt
Funcionalidade: Autenticação
  Como usuário da API
  Quero efetuar login com credenciais válidas
  Para receber um token JWT

  Contexto:
    Dado que existe um usuário cadastrado com email "user@test.com" e senha "P@ssw0rd"

  @sucesso
  Cenário: Login com credenciais válidas
    Quando eu enviar um POST para "/api/Auth/login" com o body:
      """
      {
        "email": "user@test.com",
        "senha": "P@ssw0rd"
      }
      """
    Então o status da resposta deve ser 200
    E o corpo da resposta deve conter um campo "token" não vazio

  @erro
  Cenário: Login com credenciais inválidas
    Quando eu enviar um POST para "/api/Auth/login" com o body:
      """
      {
        "email": "user@test.com",
        "senha": "senhaErrada"
      }
      """
    Então o status da resposta deve ser 401
    E o corpo da resposta deve conter um campo "mensagem" com o valor "Falha no login: credenciais inválidas."