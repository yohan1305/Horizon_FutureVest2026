using Application.Dtos.Macroindicador;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Service
{
    public class MacroindicadorService
    {

        private readonly MacroindicadorRepository _macroindicadorRepository;

        public MacroindicadorService(Persistence.Context.FutureVestContext context)
        {
            _macroindicadorRepository = new MacroindicadorRepository(context);
        }

        public async Task<bool> AddAsync(MacroindicadorDto dto)
        {
            var existente = await _macroindicadorRepository.GetAllAsync();
            var SumaActual = existente.Sum(m => m.Peso);

            if (SumaActual + dto.Peso > 1)
            {
                return false;
            }

            Macroindicador entity = new()
            {
                Id = 0,
                Name = dto.Name,
                Peso = dto.Peso,
                EsMejorMasAlto = dto.EsMejorMasAlto
            };

            await _macroindicadorRepository.AddAsync(entity);

            return true;
        }

        public async Task<bool> UpdateAsync(MacroindicadorDto dto)
        {
            var existente = await _macroindicadorRepository.GetAllAsync();
            var SumaActual = existente.Where(m => m.Id != dto.Id).Sum(m => m.Peso);
            if (SumaActual + dto.Peso > 1)
            {
                return false;
            }
            Macroindicador entity = new()
            {
                Id = dto.Id,
                Name = dto.Name,
                Peso = dto.Peso,
                EsMejorMasAlto = dto.EsMejorMasAlto
            };
            await _macroindicadorRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _macroindicadorRepository.DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<MacroindicadorDto?> GetByIdAsync(int id)
        {
            var entity = await _macroindicadorRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return new MacroindicadorDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Peso = entity.Peso,
                EsMejorMasAlto = entity.EsMejorMasAlto
            };
        }


        public async Task<List<MacroindicadorDto>> GetAllAsync()
        {
            var entities = await _macroindicadorRepository.GetAllAsync();

            return entities.Select(entity => new MacroindicadorDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Peso = entity.Peso,
                EsMejorMasAlto = entity.EsMejorMasAlto
            }).ToList();

        }   

    }
}
