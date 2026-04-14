USE NailsApp
GO

--------------------- DROP TABELAS (ordem correta: filhas primeiro) -----------------------
DROP TABLE IF EXISTS dbo.tb_agendamentos
DROP TABLE IF EXISTS dbo.tb_procedimentos
DROP TABLE IF EXISTS dbo.tb_statusAgendamento
DROP TABLE IF EXISTS dbo.tb_clientes
GO

--------------------- CLIENTES -----------------------
CREATE TABLE tb_clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    LoginCliente NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Senha NVARCHAR(100) NOT NULL,
    CPF NVARCHAR(11) UNIQUE NOT NULL,
    Ativo BIT NOT NULL,
    DataNascimento DATE NOT NULL,
    DataInclusao DATETIME NOT NULL DEFAULT GETDATE(),
    DataAtualizacao DATETIME NULL
);
GO

--------------------- STATUS AGENDAMENTO -----------------------
CREATE TABLE tb_statusAgendamento(
    Id INT NOT NULL,
    Descricao VARCHAR(MAX) NULL,
    Ativo BIT NOT NULL,
    DataInclusao DATETIME NOT NULL,
    DataAtualizacao DATETIME NULL,
    PRIMARY KEY CLUSTERED (Id ASC)
);
GO

--------------------- PROCEDIMENTOS -----------------------
CREATE TABLE tb_procedimentos(
    Id INT NOT NULL,
    Descricao VARCHAR(MAX) NULL,
    TempoDuracao TIME(7) NULL,
    Valor DECIMAL(6,2) NULL,
    Ativo BIT NOT NULL,
    DataInclusao DATETIME NULL DEFAULT GETDATE(),
    DataAtualizacao DATETIME NULL,
    PRIMARY KEY CLUSTERED (Id ASC)
);
GO

--------------------- AGENDAMENTOS -----------------------
CREATE TABLE tb_agendamentos(
    Id INT NOT NULL,
    IdCliente INT NOT NULL,
    DataAgendamento DATE NOT NULL,
    HorarioAgendamento DATETIME NULL,
    IdProcedimento INT NOT NULL,
    TempoDuracao TIME(7) NOT NULL,
    Valor DECIMAL(10,2) NOT NULL,
    IdStatusAgendamento INT NOT NULL,  -- Corrigido: INT, não BIT
    DataInclusao DATETIME NOT NULL,
    DataAtualizacao DATETIME NULL,
    PRIMARY KEY CLUSTERED (Id ASC)
);
GO

--------------------- CHAVES ESTRANGEIRAS (ordem correta) -----------------------
ALTER TABLE tb_agendamentos ADD CONSTRAINT FK_agendamentos_clientes 
    FOREIGN KEY (IdCliente) REFERENCES tb_clientes(Id);

ALTER TABLE tb_agendamentos ADD CONSTRAINT FK_agendamentos_procedimentos 
    FOREIGN KEY (IdProcedimento) REFERENCES tb_procedimentos(Id);

ALTER TABLE tb_agendamentos ADD CONSTRAINT FK_agendamentos_status 
    FOREIGN KEY (IdStatusAgendamento) REFERENCES tb_statusAgendamento(Id);
GO

