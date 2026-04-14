USE [NailsApp]
GO
/****** Object:  StoredProcedure [dbo].[sp_inserirCliente]    Script Date: 13/04/2026 23:40:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[sp_atualizarCliente] 
 @Id int,	
 @Nome NVARCHAR(100),
 @LoginCliente NVARCHAR(100),
 @Email NVARCHAR(100),
 @Senha NVARCHAR(100),
 @CPF NVARCHAR(11),    
 @Ativo bit,
 @DataNascimento Date 

as 

begin
	-- verificar se o e-mail e/ou CPF já existe no banco de dados
	update tb_clientes 
	set
		 Nome = @Nome
		,LoginCliente = @LoginCliente
		,Email = @Email
		,Senha = @Senha
		,CPF = @CPF
		,Ativo = @Ativo
		,DataNascimento = @DataNascimento
		,DataAtualizacao = GETDATE()
	where 
		Id = @Id	
	select * from tb_clientes with (nolock) where id = @Id
end