using AutoMapper;
using MediatR;
using Skill_Exchange.Domain.Interfaces;
namespace Skill_Exchange.Application.Services.GlobalCommands.Handlers
{
    public class AddHandler<T, TCreateDTO, TCreateResponseDTO> : IRequestHandler<Add<T, TCreateDTO, TCreateResponseDTO>, TCreateResponseDTO>
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
        public async Task<TCreateResponseDTO> Handle(Add<T, TCreateDTO, TCreateResponseDTO> request, CancellationToken cancellationToken)
        {
            //var engtity = _mapper.Map<TCreateDTO>(request.CreateDTO);
            //var entity = _unitOfWork.GetRepository<T>().GetByIdAsync(request.CreateDTO.Id);
            /*if (entity != null)
            {
                throw new Exception("Entity with the same ID already exists.");
            }*/

            var newEntity = _mapper.Map<T>(request.CreateDTO);
            var IsAdded = await _unitOfWork.GetRepository<T>().AddAsync(newEntity);

            if (IsAdded)
            {
                await _unitOfWork.CompleteAsync();
                var ReponseDto = _mapper.Map<TCreateResponseDTO>(newEntity);
                return ReponseDto;
            }
            else
            {
                throw new Exception("Failed to add entity.");
            }


        }
    }
}
