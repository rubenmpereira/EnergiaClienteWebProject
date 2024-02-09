INSERT Utilizador(email,contacto,genero,nif,nomeCompleto,palavraPasse)
VALUES  ('brunoaveiro@gmail.com','917765665',0,'227883957','Bruno Gulherme Frenandes Agustinho','passesecreta'),
		('julianavita@gmail.com','936627884',1,'462789305','Juliana castro','passe123')


INSERT Habitacao(emailUtilizador,potencia,fase,nivelTensao,horario)
VALUES  ('brunoaveiro@gmail.com',6.9,'monofasico','baixo','simples'),
		('julianavita@gmail.com',6.9,'monofasico','baixo','simples')


INSERT Fatura(idHabitacao,numero,valor,pago,dataInicio,dataFim,dataLimite)
VALUES  (1,'HGYT45HJFG',45.80,1,'12-5-2023','1-5-2024','1-30-2024'),
		(2,'HGYT67QWE',86,1,'9-5-2023','10-5-2023','10-30-2023'),
		(2,'HGYT67ASD',92,0,'10-5-2023','11-5-2023','11-30-2023'),
		(2,'HGYT67ZCX',84,0,'11-5-2023','12-5-2023','12-30-2023'),
		(2,'HGYT67RTY',89,0,'12-5-2023','1-5-2024','1-30-2024')

INSERT Documento(numeroFatura,ficheiro)
VALUES  ('HGYT45HJFG',CAST('w' AS VARBINARY(1))),
		('HGYT67QWE',CAST('w' AS VARBINARY(1))),
		('HGYT67ASD',CAST('w' AS VARBINARY(1))),
		('HGYT67ZCX',CAST('w' AS VARBINARY(1))),
		('HGYT67RTY',CAST('w' AS VARBINARY(1)))

INSERT Leitura(idHabitacao,estimada,mes,ano,vazio,ponta,cheias,dataLeitura)
VALUES  (1,0,5,2023,18100,17500,7230,'6-6-2023'),
		(1,0,6,2023,18200,17800,7250,'7-6-2023'),
		(1,1,7,2023,18500,18200,7320,'8-10-2023'),
		(1,0,8,2023,18800,18430,7350,'9-6-2023'),
		(1,0,9,2023,19010,18610,7390,'10-6-2023'),
		(1,1,10,2023,19240,18660,7450,'11-10-2023'),
		(1,1,11,2023,19500,18750,7480,'12-10-2023'),
		(1,1,12,2023,20000,19300,7500,'1-10-2024'),
		(1,0,1,2024,21500,19500,7540,'2-6-2024'),
		(1,0,2,2024,23400,19700,7630,'3-6-2024'),
		(2,0,9,2023,127000,89000,56000,'10-6-2023'),
		(2,1,10,2023,135000,93000,59000,'11-10-2023'),
		(2,1,11,2023,146000,97000,67000,'12-10-2023'),
		(2,1,12,2023,174000,102000,69000,'1-10-2024')

INSERT Titular (idHabitacao,nomeCompleto,nif,contacto)
VALUES (1,'Bruno Aveiro',998465746,917726447),
(2,'Juliana Vita',917285948,926635447)