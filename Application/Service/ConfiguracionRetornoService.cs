using Application.Dtos.ConfiguracionRetorno;
using Persistence.Context;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Service
{
    public class ConfiguracionRetornoService
    {
        private readonly ConfiguracionRetornoRepository _repository;

        public ConfiguracionRetornoService(FutureVestContext context)
        {
            _repository = new ConfiguracionRetornoRepository(context);
        }

        public async Task<ConfiguracionRetornoDto?> GetAsync()
        {
            var entity = await _repository.GetAllAsync()
                .ContinueWith(t => t.Result.FirstOrDefault());

            if (entity == null) return null;

            return new ConfiguracionRetornoDto
            {
                Id = entity.Id,
                TasaMinima = entity.TasaMinima,
                TasaMaxima = entity.TasaMaxima
            };
        }

        public async Task<bool> CrearAsync(decimal tasaMinima, decimal tasaMaxima)
        {
            var entity = new ConfiguracionRetorno
            {
                TasaMinima = tasaMinima,
                TasaMaxima = tasaMaxima
            };

            await _repository.CreateAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(ConfiguracionRetornoDto dto)
        {
            if (dto.TasaMinima >= dto.TasaMaxima)
                return false;

            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            entity.TasaMinima = dto.TasaMinima;
            entity.TasaMaxima = dto.TasaMaxima;

            await _repository.UpdateAsync(entity);
            return true;
        }




    }
}
