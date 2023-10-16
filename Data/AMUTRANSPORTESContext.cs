using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AMUTRANSAPP.Models.AMUTRANSPORTES;

namespace AMUTRANSAPP.Data
{
    public partial class AMUTRANSPORTESContext : DbContext
    {
        public AMUTRANSPORTESContext()
        {
        }

        public AMUTRANSPORTESContext(DbContextOptions<AMUTRANSPORTESContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal>()
              .HasOne(i => i.Paragem)
              .WithMany(i => i.Fiscals)
              .HasForeignKey(i => i.ParagemId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador>()
              .HasOne(i => i.EstacaoServico)
              .WithMany(i => i.Lavadors)
              .HasForeignKey(i => i.EstacaoServicoId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja>()
              .HasOne(i => i.Empresa)
              .WithMany(i => i.LicencaLojas)
              .HasForeignKey(i => i.EmpresaId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja>()
              .Property(p => p.Situacao)
              .HasDefaultValueSql(@"(N'Em Espera')");

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa>()
              .Property(p => p.DataCadastro)
              .HasColumnType("datetime");

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja>()
              .Property(p => p.DataCadastro)
              .HasColumnType("datetime");

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento>()
              .Property(p => p.DataEntrada)
              .HasColumnType("datetime");

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento>()
              .Property(p => p.DataResposta)
              .HasColumnType("datetime");

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.CarroPreso>()
              .Property(p => p.DataEntrada)
              .HasColumnType("datetime");

            builder.Entity<AMUTRANSAPP.Models.AMUTRANSPORTES.CarroPreso>()
              .Property(p => p.DataSaida)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> Empresas { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> EstacaoServicos { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> Fiscals { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> Lavadors { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> LicencaLojas { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> Movimentos { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> Paragems { get; set; }

        public DbSet<AMUTRANSAPP.Models.AMUTRANSPORTES.CarroPreso> CarroPresos { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}