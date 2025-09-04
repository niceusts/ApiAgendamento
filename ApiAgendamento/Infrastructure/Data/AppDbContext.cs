using ApiAgendamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<HorarioDisponivel> HorarioDisponiveis { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medico>().Property(m => m.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Paciente>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Agendamento>().Property(a => a.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<HorarioDisponivel>().Property(h => h.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Usuario>().Property(u => u.Id).ValueGeneratedOnAdd();
    }
}