USE EnergiaClienteDados
GO

CREATE PROCEDURE AuthenticarUtilizador
@email NVARCHAR(320),
@palavrapasse NVARCHAR(250)
AS
SELECT Case when COUNT(U.email) = 0 Then 'false' Else 'true' end as 'success' FROM Utilizador U WHERE U.email=@email AND U.palavraPasse=@palavrapasse
Go

EXEC AuthenticarUtilizador @email='julianavita@gmail.com',@palavrapasse='passe123';
