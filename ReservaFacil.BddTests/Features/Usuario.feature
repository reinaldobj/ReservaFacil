# language: pt
Funcionalidade: Usuário
  Como administrador
  Quero cadastrar e consultar usuários

  @sucesso
  Cenário: Cadastrar usuário válido
    Quando eu enviar um POST para "/api/Usuario" com o body:
      """
      {
        "nome": "João",
        "email": "joao@test.com",
        "senha": "P@ss1234",
        "tipoUsuario": "UsuarioComum"
      }
      """
    Então o status da resposta deve ser 201
    E o corpo da resposta deve conter um campo "nome" dentro de dados com o valor "João"
    E o corpo da resposta deve conter um campo "email" dentro de dados com o valor "joao@test.com"
    E o corpo da resposta deve conter um campo "id" não vazio

  @erro
  Cenário: Cadastrar usuário com email já existente
    Dado que já existe um usuário com email "joaotest111@test.com"
    Quando eu enviar um POST para "/api/Usuario" com o body:
      """
      {
        "nome": "João2",
        "email": "joaotest111@test.com",
        "senha": "OutraSenha1",
        "tipoUsuario": "UsuarioComum"
      }
      """
    Então o status da resposta deve ser 400
    E o corpo da resposta deve conter um campo "mensagem" com o valor "O email joaotest111@test.com já está em uso."