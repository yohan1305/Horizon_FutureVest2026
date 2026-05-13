using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class FutureVestContext : DbContext
    {
        public FutureVestContext(DbContextOptions<FutureVestContext> options) : base(options)
        {

        }

        #region DbSets

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Macroindicador> Macroindicadores { get; set; }

        public DbSet<IndicadorPorPais> IndicadoresPorPais { get; set; }

        public DbSet<SimulacionMacroindicador> SimulacionMacroindicadores { get; set; }

        public DbSet<ConfiguracionRetorno> ConfiguracionRetorno { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
