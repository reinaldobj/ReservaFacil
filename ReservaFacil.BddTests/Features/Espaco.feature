# language: pt
Funcionalidade: Gerenciamento de Espaços
  Como cliente da API
  Quero consultar e manipular registros de espaços

  Contexto:
    Dado que estou autenticado como Admin com token válido

  @sucesso
  Cenário: Buscar espaço existente por ID
    Dado que existe um espaço com ID 42 e nome "Sala A"
    Quando eu enviar um GET para "/api/Espaco/00000000-0000-0000-0000-000000000042"
    Então o status da resposta deve ser 200
    E o corpo da resposta deve conter um campo "nome" dentro de dados com o valor "Sala A"
    E o corpo da resposta deve conter um campo "id" não vazio

  @erro
  Cenário: Buscar espaço inexistente
    Quando eu enviar um GET para "/api/Espaco/00000000-0000-0000-0000-000000009999"
    Então o status da resposta deve ser 404
    E o corpo da resposta deve conter um campo "mensagem" com o valor "Espaço com ID 00000000-0000-0000-0000-000000009999 não encontrado."

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
    E o corpo da resposta deve conter "The Nome field is required."
