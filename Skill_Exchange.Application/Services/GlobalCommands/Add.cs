using MediatR;
namespace Skill_Exchange.Application.Services.GlobalCommands
{
    public record Add<T, TCreateDTO, TCreateResponseDTO>(TCreateDTO CreateDTO) : IRequest<TCreateResponseDTO>
        where TCreateDTO : class
        where T : class
        where TCreateResponseDTO : class;
}
