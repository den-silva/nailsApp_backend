using Microsoft.AspNetCore.Mvc;
using nailsApp_Backend.DTOs;
using nailsApp_Backend.Repositories;

namespace nailsApp_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _repository;
        private readonly ILogger<ClientesController> _logger;
        
        public ClientesController(IClienteRepository repository, ILogger<ClientesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        // GET: api/clientes
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClienteResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClienteResponseDTO>>> GetClientes()
        {
            try
            {
                var clientes = await _repository.ObterTodosAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter clientes");
                return StatusCode(500, "Erro interno ao processar a requisição");
            }
        }
        
        // GET: api/clientes/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClienteResponseDTO>> GetCliente(int id)
        {
            try
            {
                var cliente = await _repository.ObterPorIdAsync(id);
                
                if (cliente == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado");
                }
                
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter cliente com ID {id}");
                return StatusCode(500, "Erro interno ao processar a requisição");
            }
        }
        
        // POST: api/clientes
        [HttpPost]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClienteResponseDTO>> PostCliente([FromBody] ClienteCreateDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                // Verificar se CPF já existe
                var clienteExistenteCpf = await _repository.ObterPorCpfAsync(clienteDTO.CPF);
                if (clienteExistenteCpf != null)
                {
                    return Conflict("CPF já cadastrado");
                }
                
                // Verificar se Email já existe
                var clienteExistenteEmail = await _repository.ObterPorEmailAsync(clienteDTO.Email);
                if (clienteExistenteEmail != null)
                {
                    return Conflict("Email já cadastrado");
                }
                
                var clienteCriado = await _repository.CriarAsync(clienteDTO);
                
                return CreatedAtAction(nameof(GetCliente), 
                    new { id = clienteCriado.Id }, 
                    clienteCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente");
                return StatusCode(500, "Erro interno ao processar a requisição");
            }
        }
        
        // PUT: api/clientes/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteUpdateDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if (!await _repository.ClienteExisteAsync(id))
                {
                    return NotFound($"Cliente com ID {id} não encontrado");
                }
                
                // Verificar se CPF já existe em outro cliente
                var clienteCpf = await _repository.ObterPorCpfAsync(clienteDTO.CPF);
                if (clienteCpf != null && clienteCpf.Id != id)
                {
                    return Conflict("CPF já cadastrado para outro cliente");
                }
                
                // Verificar se Email já existe em outro cliente
                var clienteEmail = await _repository.ObterPorEmailAsync(clienteDTO.Email);
                if (clienteEmail != null && clienteEmail.Id != id)
                {
                    return Conflict("Email já cadastrado para outro cliente");
                }
                
                await _repository.AtualizarAsync(id, clienteDTO);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar cliente com ID {id}");
                return StatusCode(500, "Erro interno ao processar a requisição");
            }
        }
        
        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                if (!await _repository.ClienteExisteAsync(id))
                {
                    return NotFound($"Cliente com ID {id} não encontrado");
                }
                
                var excluido = await _repository.ExcluirAsync(id);
                
                if (!excluido)
                {
                    return StatusCode(500, "Erro ao excluir cliente");
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir cliente com ID {id}");
                return StatusCode(500, "Erro interno ao processar a requisição");
            }
        }
    }
}