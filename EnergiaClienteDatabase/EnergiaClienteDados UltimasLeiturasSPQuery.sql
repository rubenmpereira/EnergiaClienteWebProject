USE EnergiaClienteDados
GO

CREATE PROCEDURE UltimasLeituras
@habitacao INT,
@quantidade INT
AS
SELECT TOP(@quantidade) * FROM Leitura L WHERE L.idHabitacao=@habitacao ORDER BY L.dataLeitura DESC
GO

EXEC UltimasLeituras @habitacao = 2,@quantidade=1;