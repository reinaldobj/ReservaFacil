# language: pt
Funcionalidade: Usuário
  Como administrador
  Quero cadastrar e consultar usuários

  @sucesso
  Cenário: Cadastrar usuário válido
    Quando eu enviar um POST para "/api/usuarios" com o body:
      """
      {
        "nome": "João",
        "email": "joao@test.com",
        "senha": "P@ss1234"
      }
      """
    Então o status da resposta deve ser 201
    E o corpo deve conter algo como:
      """
      { "id": 1, "nome": "João", "email": "joao@test.com" }
      """

  @erro
  Cenário: Cadastrar usuário com email já existente
    Dado que já existe um usuário com email "joao@test.com"
    Quando eu enviar um POST para "/api/usuarios" com o body:
      """
      {
        "nome": "João2",
        "email": "joao@test.com",
        "senha": "OutraSenha1"
      }
      """
    Então o status da resposta deve ser 409
    E o corpo deve conter:
      """
      { "error": "Email já cadastrado" }
      """
