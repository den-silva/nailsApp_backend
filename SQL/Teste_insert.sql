 declare @Nome varchar(100) = 'Denilson Murilo Silva'
 declare @LoginCliente varchar(100) = 'DENILSON.SILVA'
 declare @Email varchar(100) = 'dmurilosilva@hotmail.com'
 declare @Senha varchar(100) = '123456'
 declare @CPF NVARCHAR(11) = '37555844842'
 declare @DataNascimento Date = '1990-03-23'

 exec sp_inserirCliente

@Nome,
@LoginCliente,
@Email,
@Senha,
@CPF,  
@DataNascimento  

