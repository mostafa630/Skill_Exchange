using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;
namespace Skill_Exchange.Application.Services.GlobalCommands.Handlers
{
    public class AddHandler<T, TCreateDTO, TCreateResponseDTO> : IRequestHandler<Add<T, TCreateDTO, TCreateResponseDTO>, Result<TCreateResponseDTO>>
        where TCreateDTO : class
        where T : class
        where TCreateResponseDTO : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<TCreateResponseDTO>> Handle(Add<T, TCreateDTO, TCreateResponseDTO> request, CancellationToken cancellationToken)
        {
            try
            {
                var newEntity = _mapper.Map<T>(request.CreateDTO);
                var IsAdded = await _unitOfWork.GetRepository<T>().AddAsync(newEntity);

                if (IsAdded)
                {
                    var ReponseDto = _mapper.Map<TCreateResponseDTO>(newEntity);
                    await _unitOfWork.CompleteAsync();
                    return Result<TCreateResponseDTO>.Ok(ReponseDto);
                }
                else
                {
                    return Result<TCreateResponseDTO>.Fail("Adding Operation Failed");
                }
            }
            catch
            {
                return Result<TCreateResponseDTO>.Fail("Adding Operation Failed");
            }
        }
    }
}
