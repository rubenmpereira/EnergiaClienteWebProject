USE EnergiaClienteDados
GO

CREATE PROCEDURE ListaIdHabitacao
AS
SELECT H.id FROM Habitacao H
GO

EXEC ListaIdHabitacao;
