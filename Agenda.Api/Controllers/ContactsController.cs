using Microsoft.AspNetCore.Mvc;
using MediatR;
using Agenda.Application.DTOs;
using Agenda.Application.Contacts.Commands;
using Agenda.Application.Contacts.Queries;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Agenda.Api.Controllers;

/// <summary>
/// Controlador responsável por gerenciar as operações de contatos de um usuário.
/// </summary>
/// <remarks>
/// Este controlador utiliza o padrão CQRS com o MediatR para processar comandos e consultas
/// relacionados aos contatos. Todos os endpoints exigem autenticação JWT.
/// </remarks>
[ApiController]
[Route("api/v1/contacts")]
[Authorize] // Protege os endpoints com autenticação JWT
public class ContactsController : ControllerBase
{
    private int GetLoggedUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim != null ? int.Parse(claim.Value) : 0;
    }

    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="ContactsController"/>.
    /// </summary>
    /// <param name="mediator">Instância do <see cref="IMediator"/> usada para enviar comandos e consultas.</param>
    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retorna todos os contatos de um usuário específico.
    /// </summary>
    /// <param name="userId">ID do usuário cujos contatos serão retornados.</param>
    /// <returns>
    /// Retorna uma lista de contatos vinculados ao usuário informado.
    /// </returns>
    /// <response code="200">Lista de contatos retornada com sucesso.</response>
    /// <response code="401">Usuário não autorizado.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetLoggedUserId();

        if (userId == 0)
            return Unauthorized(new { message = "Usuário não identificado no token." });

        var query = new GetAllContactsQuery(userId);
        var contacts = await _mediator.Send(query);

        return Ok(contacts);
    }

    /// <summary>
    /// Retorna um contato específico pelo ID.
    /// </summary>
    /// <param name="id">ID do contato.</param>
    /// <returns>
    /// Retorna o contato correspondente ao ID informado.
    /// </returns>
    /// <response code="200">Contato encontrado com sucesso.</response>
    /// <response code="404">Contato não encontrado.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetContactByIdQuery(id);
        var contact = await _mediator.Send(query);

        if (contact == null)
            return NotFound(new { message = "Contato não encontrado." });

        return Ok(contact);
    }

    /// <summary>
    /// Cria um novo contato.
    /// </summary>
    /// <param name="dto">Objeto contendo os dados do novo contato.</param>
    /// <returns>
    /// Retorna o contato criado e a URL de acesso ao novo recurso.
    /// </returns>
    /// <response code="201">Contato criado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactDto dto)
    {
        var userId = GetLoggedUserId();
        if (userId == 0)
            return Unauthorized(new { message = "Usuário não identificado no token." });

        dto.UserId = userId;

        var command = new CreateContactCommand(dto);
        var createdContact = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = createdContact.Id }, createdContact);
    }

    /// <summary>
    /// Atualiza um contato existente.
    /// </summary>
    /// <param name="id">ID do contato a ser atualizado.</param>
    /// <param name="dto">Objeto com os novos dados do contato.</param>
    /// <returns>
    /// Retorna o contato atualizado.
    /// </returns>
    /// <response code="200">Contato atualizado com sucesso.</response>
    /// <response code="400">IDs inconsistentes entre rota e corpo da requisição.</response>
    /// <response code="404">Contato não encontrado.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContactDto dto)
    {
        var userId = GetLoggedUserId();
        if (userId == 0)
            return Unauthorized(new { message = "Usuário não identificado no token." });

        // Verifica se o contato pertence ao usuário logado
        var existingContact = await _mediator.Send(new GetContactByIdQuery(id));
        if (existingContact == null || existingContact.UserId != userId)
            return NotFound(new { message = "Contato não encontrado ou não pertence ao usuário." });

        // Atualiza os dados (não precisa confiar no dto.Id nem dto.UserId do frontend)
        dto.Id = id;
        dto.UserId = userId;

        var updatedContact = await _mediator.Send(new UpdateContactCommand(id, dto));

        return Ok(updatedContact);
    }

    /// <summary>
    /// Remove um contato pelo ID.
    /// </summary>
    /// <param name="id">ID do contato a ser removido.</param>
    /// <returns>
    /// Retorna status 204 caso a exclusão seja bem-sucedida.
    /// </returns>
    /// <response code="204">Contato removido com sucesso.</response>
    /// <response code="404">Contato não encontrado.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteContactCommand(id);
        var success = await _mediator.Send(command);

        if (!success)
            return NotFound(new { message = "Contato não encontrado para exclusão." });

        return NoContent();
    }
}
