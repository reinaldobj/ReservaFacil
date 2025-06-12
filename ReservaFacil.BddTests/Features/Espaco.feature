# language: pt
Funcionalidade: Gerenciamento de Espaços
  Como cliente da API
  Quero consultar e manipular registros de espaços

  @sucesso
  Cenário: Buscar espaço existente por ID
    Dado que existe um espaço com ID 42 e nome "Sala A"
    Quando eu enviar um GET para "/api/Espaco/42"
    Então o status da resposta deve ser 200
    E o corpo deve conter:
      """
      { "id": 42, "nome": "Sala A" }
      """

  @erro
  Cenário: Buscar espaço inexistente
    Quando eu enviar um GET para "/api/Espaco/9999"
    Então o status da resposta deve ser 404
    E o corpo deve conter:
      """
      { "error": "Espaço não encontrado" }
      """

  @sucesso
  Cenário: Criar novo espaço com dados válidos
    Quando eu enviar um POST para "/api/Espaco" com o body:
      """
      {
        "nome": "Sala B",
        "descricao": "Sala de reunião",
        "capacidade": 10,
        "tipoEspaco": "SalaDeReuniao"
      }
      """
    Então o status da resposta deve ser 201
    E o header "Location" deve apontar para "/api/Espaco/{id}"

  @erro
  Cenário: Criar espaço sem nome
    Quando eu enviar um POST para "/api/Espaco" com o body:
      """
      {
        "descricao": "Sala",
        "capacidade": 10,
        "tipoEspaco": "SalaDeReuniao"
      }
      """
    Então o status da resposta deve ser 400
    E o corpo deve conter:
      """
      { "errors": { "Nome": "O campo Nome é obrigatório" } }
      """
