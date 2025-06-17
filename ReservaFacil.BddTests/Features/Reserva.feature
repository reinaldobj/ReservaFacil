# language: pt
Funcionalidade: Reserva de Espaços
  Como usuário autenticado
  Quero criar reservas sem conflito de horário

  Contexto:
    Dado que existe um espaço com ID 77 e nome "Sala B"
    Dado que existe um usuário cadastrado com o id 58

  @sucesso
  Cenário: Criar reserva em horário livre
    Dado que estou autenticado com token válido
    Quando eu enviar um POST para "/api/Reserva" com o body:
      """
      {
        "espacoId": "00000000-0000-0000-0000-000000000077",
        "usuarioId": "00000000-0000-0000-0000-000000000058",
        "dataInicio": "2025-07-01T10:00:00",
        "dataFim": "2025-07-01T11:00:00"
      }
      """
    Então o status da resposta deve ser 201
    E o corpo da resposta deve conter um campo "id" não vazio

  
  @erro
  Cenário: Criar reserva em horário já reservado
    Dado que já existe uma reserva no espaço 77 entre "2025-07-01T10:00:00" e "2025-07-01T11:00:00"
    Dado que estou autenticado com token válido
    Quando eu enviar um POST para "/api/Reserva" com o body:
      """
      {
        "espacoId": "00000000-0000-0000-0000-000000000077",
        "usuarioId": "00000000-0000-0000-0000-000000000058",
        "dataInicio": "2025-07-01T10:30:00",
        "dataFim": "2025-07-01T11:30:00"
      }
      """
    Então o status da resposta deve ser 400
    E o corpo da resposta deve conter "Conflito de hor\u00E1rio com outra reserva."