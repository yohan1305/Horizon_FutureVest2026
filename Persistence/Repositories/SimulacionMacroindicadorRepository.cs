using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Entities;
using Persistence.Repositories.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class SimulacionMacroindicadorRepository : IRepositorio<SimulacionMacroindicador>
    {
        private readonly FutureVestContext _context;

        public SimulacionMacroindicadorRepository(FutureVestContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SimulacionMacroindicador entity)
        {
            await _context.Set<SimulacionMacroindicador>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<SimulacionMacroindicador>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<SimulacionMacroindicador>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SimulacionMacroindicador>> GetAllAsync()
        {
            return await _context.Set<SimulacionMacroindicador>().ToListAsync();
        }

        public async Task<SimulacionMacroindicador?> GetByIdAsync(int id)
        {
            return await _context.Set<SimulacionMacroindicador>().FindAsync(id);
        }

        public async Task UpdateAsync(SimulacionMacroindicador entity)
        {
            var entry = await _context.Set<SimulacionMacroindicador>().FindAsync(entity.Id);
            if (entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetSumaPesosAsync()
        {
            return await _context.SimulacionMacroindicadores
                .SumAsync(s => s.PesoSimulacion);
        }

        public async Task<decimal> GetSumaPesosExcluyendoAsync(int id)
        {
            return await _context.SimulacionMacroindicadores
                .Where(s => s.Id != id)
                .SumAsync(s => s.PesoSimulacion);
        }

        public async Task<bool> ExistsAsync(int macroindicadorId)
        {
            return await _context.SimulacionMacroindicadores
                .AnyAsync(s => s.MacroindicadorId == macroindicadorId);
        }




    }
}
