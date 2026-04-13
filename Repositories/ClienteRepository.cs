using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using nailsApp_Backend.DTOs;

namespace nailsApp_Backend.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;
        
        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string não encontrada");
        }
        
        public async Task<IEnumerable<ClienteResponseDTO>> ObterTodosAsync()
        {
            var clientes = new List<ClienteResponseDTO>();
            
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("sp_Cliente_ListarTodos", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                clientes.Add(MapToClienteResponseDTO(reader));
            }
            
            return clientes;
        }
        
        public async Task<ClienteResponseDTO> ObterPorIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("sp_Cliente_ObterPorId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            command.Parameters.AddWithValue("@Id", id);
            
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToClienteResponseDTO(reader);
            }
            
            return null;
        }
        
        public async Task<ClienteResponseDTO> ObterPorCpfAsync(string cpf)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("SELECT * FROM Clientes WHERE CPF = @CPF", connection);
            command.Parameters.AddWithValue("@CPF", cpf);
            
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToClienteResponseDTO(reader);
            }
            
            return null;
        }
        
        public async Task<ClienteResponseDTO> ObterPorEmailAsync(string email)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("SELECT * FROM Clientes WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);
            
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToClienteResponseDTO(reader);
            }
            
            return null;
        }
        
        public async Task<ClienteResponseDTO> CriarAsync(ClienteCreateDTO cliente)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("sp_Cliente_Inserir", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@CPF", cliente.CPF);
            command.Parameters.AddWithValue("@Status", cliente.Status);
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            
            var id = Convert.ToInt32(await command.ExecuteScalarAsync());
            
            // Retornar o cliente criado
            return await ObterPorIdAsync(id);
        }
        
        public async Task<ClienteResponseDTO> AtualizarAsync(int id, ClienteUpdateDTO cliente)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("sp_Cliente_Atualizar", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@CPF", cliente.CPF);
            command.Parameters.AddWithValue("@Status", cliente.Status);
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            
            await command.ExecuteNonQueryAsync();
            
            // Retornar o cliente atualizado
            return await ObterPorIdAsync(id);
        }
        
        public async Task<bool> ExcluirAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("sp_Cliente_Excluir", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            command.Parameters.AddWithValue("@Id", id);
            
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        
        public async Task<bool> ClienteExisteAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new SqlCommand("SELECT COUNT(1) FROM Clientes WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        
        private ClienteResponseDTO MapToClienteResponseDTO(SqlDataReader reader)
        {
            return new ClienteResponseDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                CPF = reader.GetString(reader.GetOrdinal("CPF")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                DataNascimento = reader.GetDateTime(reader.GetOrdinal("DataNascimento")),
                DataInclusao = reader.GetDateTime(reader.GetOrdinal("DataInclusao")),
                DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"))
            };
        }
    }
}