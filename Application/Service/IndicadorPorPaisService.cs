using Persistence.Context;
using Persistence.Repositories;
using Application.Dtos.IndicadorPorPais;
using Persistence.Entities;

namespace Application.Service
{
    public class IndicadorPorPaisService
    {

        public readonly IndicadorPorPaisRepository _repository;
        public readonly PaisRepository _paisRepository;
        public readonly MacroindicadorRepository _macroRepository;

        public IndicadorPorPaisService(FutureVestContext context)
        {
            _repository = new IndicadorPorPaisRepository(context);
            _paisRepository = new PaisRepository(context);
            _macroRepository = new MacroindicadorRepository(context);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }


        public async Task<bool> AddAsync(IndicadorPorPaisDto dto)
        {
            var existe = await _repository.GetAllAsync().ContinueWith(t => t.Result.Any(i =>
                i.PaisId == dto.PaisId &&
                i.MacroindicadorId == dto.MacroindicadorId &&
                i.Anio == dto.Anio
                ));

            if (existe) return false;

            IndicadorPorPais entity = new()
            {
                Id = 0,
                PaisId = dto.PaisId,
                MacroindicadorId = dto.MacroindicadorId,
                Valor = dto.Valor,
                Anio = dto.Anio
            };

            await _repository.AddAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(IndicadorPorPaisDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            entity.Valor = dto.Valor;
            await _repository.UpdateAsync(entity);
            return true;
        }

        public async Task<IndicadorPorPaisDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var pais = await _paisRepository.GetByIdAsync(entity.PaisId);
            var macro = await _macroRepository.GetByIdAsync(entity.MacroindicadorId);

            return new IndicadorPorPaisDto
            {
                Id = entity.Id,
                PaisId = entity.PaisId,
                MacroindicadorId = entity.MacroindicadorId,
                Valor = entity.Valor,
                Anio = entity.Anio,
                NombrePais = pais?.Name,
                NombreMacroindicador = macro?.Name
            };
        }

        public async Task<List<IndicadorPorPaisDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var paises = await _paisRepository.GetAllAsync();
            var macros = await _macroRepository.GetAllAsync();

            return entities.Select(i => new IndicadorPorPaisDto
            {
                Id = i.Id,
                PaisId = i.PaisId,
                MacroindicadorId = i.MacroindicadorId,
                Valor = i.Valor,
                Anio = i.Anio,
                NombrePais = paises.FirstOrDefault(p => p.Id == i.PaisId)?.Name,
                NombreMacroindicador = macros.FirstOrDefault(m => m.Id == i.MacroindicadorId)?.Name
            }).ToList();
        }

        public async Task<List<IndicadorPorPaisDto>> FiltrarAsync(int paisId, int? anio)
        {
            var todos = await GetAllAsync();
            var filtrados = todos.Where(i => i.PaisId == paisId);

            if (anio.HasValue)
                filtrados = filtrados.Where(i => i.Anio == anio.Value);

            return filtrados.ToList();
        }

        public async Task<List<int>> GetAñosDisponiblesAsync()
        {
            var todos = await _repository.GetAllAsync();
            return todos.Select(i => i.Anio).Distinct().OrderByDescending(a => a).ToList();
        }

    }
}
