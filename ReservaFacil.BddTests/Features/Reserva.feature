# language: pt
Funcionalidade: Reserva de Espaços
  Como usuário autenticado
  Quero criar reservas sem conflito de horário

  Contexto:
    Dado que existe um espaço com ID 42

  @sucesso
  Cenário: Criar reserva em horário livre
    Dado que estou autenticado com token válido
    Quando eu enviar um POST para "/api/reservas" com o body:
      """
      {
        "espacoId": 42,
        "usuarioId": 1,
        "dataHoraInicio": "2025-07-01T10:00:00",
        "dataHoraFim":    "2025-07-01T11:00:00"
      }
      """
    Então o status da resposta deve ser 201
    E o corpo deve conter algo como:
      """
      { "id": 100, "espacoId": 42, "usuarioId": 1 }
      """

  @erro
  Cenário: Criar reserva em horário já reservado
    Dado que já existe uma reserva no espaço 42 entre "2025-07-01T10:00:00" e "2025-07-01T11:00:00"
    Dado que estou autenticado com token válido
    Quando eu enviar um POST para "/api/reservas" com o body:
      """
      {
        "espacoId": 42,
        "usuarioId": 2,
        "dataHoraInicio": "2025-07-01T10:30:00",
        "dataHoraFim":    "2025-07-01T11:30:00"
      }
      """
    Então o status da resposta deve ser 409
    E o corpo deve conter:
      """
      { "error": "Horário já reservado" }
      """
