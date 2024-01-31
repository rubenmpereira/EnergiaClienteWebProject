USE EnergiaClienteDados
GO

CREATE PROCEDURE ReceberLeitura
@habitacao INT,
@mes		INT,
@ano		INT
AS
SELECT * FROM Leitura L WHERE L.idHabitacao= @habitacao AND L.mes=@mes AND L.ano=@ano ORDER BY L.id DESC
GO

EXEC ReceberLeitura @habitacao = 1,@mes=12,@ano=2023;
