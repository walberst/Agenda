using Microsoft.AspNetCore.Mvc;
using MediatR;
using Agenda.Application.DTOs;
using Agenda.Application.Users.Commands;
using Agenda.Application.Users.Queries;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Agenda.Api.Controllers;

/// <summary>
/// Controlador responsável pelas operações de autenticação e gerenciamento de usuários.
/// </summary>
/// <remarks>
/// Este controlador lida com o registro, autenticação (login) e recuperação de informações
/// de usuários. Ele utiliza o padrão CQRS com o MediatR para delegar o processamento
/// das requisições.
/// </remarks>
[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private int GetLoggedUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim != null ? int.Parse(claim.Value) : 0;
    }

    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="UsersController"/>.
    /// </summary>
    /// <param name="mediator">Instância do <see cref="IMediator"/> usada para enviar comandos e consultas.</param>
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    /// <param name="request">Objeto contendo os dados do usuário e a senha.</param>
    /// <returns>
    /// Retorna o usuário criado e a URL para consulta do recurso.
    /// </returns>
    /// <response code="201">Usuário registrado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos na requisição.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var userDto = new UserDto
        {
            Name = dto.Name,
            Email = dto.Email
        };

        var command = new RegisterUserCommand(userDto, dto.Password);
        var createdUser = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// Autentica um usuário e retorna um token JWT.
    /// </summary>
    /// <param name="request">Objeto contendo o e-mail e a senha do usuário.</param>
    /// <returns>
    /// Retorna um objeto JSON com o token JWT caso as credenciais sejam válidas.
    /// </returns>
    /// <response code="200">Usuário autenticado com sucesso.</response>
    /// <response code="401">Credenciais inválidas.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        var command = new AuthenticateUserCommand(request.Email, request.Password);
        var token = await _mediator.Send(command);

        if (string.IsNullOrEmpty(token))
            return Unauthorized(new { message = "Email ou senha inválidos." });

        return Ok(new { token });
    }

    /// <summary>
    /// Retorna as informações de um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário que será consultado.</param>
    /// <returns>
    /// Retorna os dados do usuário correspondente ao ID informado.
    /// </returns>
    /// <response code="200">Usuário encontrado com sucesso.</response>
    /// <response code="401">Usuário não autenticado.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetById()
    {
        // Ignora o ID passado na rota e pega o usuário logado
        var loggedUserId = GetLoggedUserId();

        if (loggedUserId == 0)
            return Unauthorized(new { message = "Usuário não autenticado." });

        var query = new GetUserByIdQuery(loggedUserId);
        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound(new { message = "Usuário não encontrado." });

        return Ok(user);
    }

    /// <summary>
    /// Objeto usado para registrar um novo usuário.
    /// </summary>
    /// <param name="User">Dados do usuário (nome, e-mail, etc.).</param>
    /// <param name="Password">Senha do usuário.</param>
    public record RegisterUserRequest(UserDto User, string Password);

    /// <summary>
    /// Objeto usado para autenticação de usuário.
    /// </summary>
    /// <param name="Email">E-mail do usuário.</param>
    /// <param name="Password">Senha do usuário.</param>
    public record AuthenticateUserRequest(string Email, string Password);
}
