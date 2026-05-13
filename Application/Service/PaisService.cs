using Application.Dtos.Pais;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class PaisService
    {
        private readonly PaisRepository _paisRepository;

        public PaisService(FutureVestContext context)
        {
            _paisRepository = new PaisRepository(context);
        }

        public async Task<bool> AddAsync(PaisDto dto)
        {
            try
            {
                Pais entity = new()
                {
                    Id = 0,
                    Name = dto.Name,
                    CodigoIso = dto.CodigoIso
                };

                await _paisRepository.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(PaisDto dto)
        {
            try
            {
                Pais entity = new()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    CodigoIso = dto.CodigoIso
                };

                await _paisRepository.UpdateAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _paisRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<PaisDto?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _paisRepository.GetByIdAsync(id);
                if (entity == null) return null;

                return new PaisDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CodigoIso = entity.CodigoIso
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<PaisDto>> GetAllAsync()
        {
            try
            {
                var entities = await _paisRepository.GetAllAsync();
                return entities.Select(p => new PaisDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CodigoIso = p.CodigoIso
                }).ToList();
            }
            catch
            {
                return [];
            }
        }

    }
}
