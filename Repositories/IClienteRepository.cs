using nailsApp_Backend.DTOs;

namespace nailsApp_Backend.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<ClienteResponseDTO>> ObterTodosAsync();
        Task<ClienteResponseDTO> ObterPorIdAsync(int id);
        Task<ClienteResponseDTO> ObterPorCpfAsync(string cpf);
        Task<ClienteResponseDTO> ObterPorEmailAsync(string email);
        Task<ClienteResponseDTO> CriarAsync(ClienteCreateDTO cliente);
        Task<ClienteResponseDTO> AtualizarAsync(int id, ClienteUpdateDTO cliente);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ClienteExisteAsync(int id);
    }
}