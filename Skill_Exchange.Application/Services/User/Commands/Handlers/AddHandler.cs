using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs.DTOInterfaces;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.User.Commands.Handlers
{
/*    public class AddHandler<T,TCreateDTO, TCreateResponseDTO> : IRequestHandler<Add<T,TCreateDTO, TCreateResponseDTO>, TCreateResponseDTO>
        where TCreateDTO : class, ICreate
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
        public Task<TCreateResponseDTO> Handle(Add<T,TCreateDTO, TCreateResponseDTO> request, CancellationToken cancellationToken)
        {
            //var engtity = _mapper.Map<TCreateDTO>(request.CreateDTO);
            //var entity = _unitOfWork.GetRepository<T>().GetByIdAsync(request.CreateDTO.Id);
            *//*if (entity != null)
            {
                throw new Exception("Entity with the same ID already exists.");
            }*//*

            var newEntity = _mapper.Map<T>(request.CreateDTO);
            var addedEntity = _unitOfWork.GetRepository<T>().AddAsync(newEntity);
            
            if(addedEntity.Result)
            {
                _unitOfWork.CompleteAsync();
                var ReponseDto = _mapper.Map<TCreateResponseDTO>(request.CreateDTO);
                return Task.FromResult(ReponseDto);
            }
            else
            {
                throw new Exception("Failed to add entity.");
            }


        }
    }
*/}
