USE EnergiaClienteDados
GO

CREATE PROCEDURE AutorizarHabitacao
@habitacao INT,
@email NVARCHAR(320)
AS
SELECT Case when COUNT(H.id) = 0 Then 'false' Else 'true' end as 'success' FROM Habitacao H WHERE H.emailUtilizador=@email AND H.id=@habitacao
Go

EXEC AutorizarHabitacao @habitacao = 1,@email='julianavita@gmail.com';

