using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SistemaEmpleados.Models;

namespace SistemaEmpleados.Data
{
    public class EmpleadoContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }

        public EmpleadoContext()
        {
        }

        public EmpleadoContext(DbContextOptions<EmpleadoContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-7VTD5RN;Database=SistemaEmpleados;Integrated Security=true;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Departamento
            modelBuilder.Entity<Departamento>()
                .HasKey(d => d.DepartamentoID);
            
            modelBuilder.Entity<Departamento>()
                .Property(d => d.DepartamentoID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Departamento>()
                .Property(d => d.FechaCreacion)
                .HasDefaultValueSql("GETUTCDATE()");

            // Configuración de Cargo
            modelBuilder.Entity<Cargo>()
                .HasKey(c => c.CargoID);
            
            modelBuilder.Entity<Cargo>()
                .Property(c => c.CargoID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Cargo>()
                .Property(c => c.FechaCreacion)
                .HasDefaultValueSql("GETUTCDATE()");

            // Configuración de Empleado
            modelBuilder.Entity<Empleado>()
                .HasKey(e => e.EmpleadoID);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.EmpleadoID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Departamento)
                .WithMany(d => d.Empleados)
                .HasForeignKey(e => e.DepartamentoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Cargo)
                .WithMany(c => c.Empleados)
                .HasForeignKey(e => e.CargoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Empleado>()
                .Property(e => e.Salario)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Empleado>()
                .Property(e => e.AFP)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Empleado>()
                .Property(e => e.ARS)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Empleado>()
                .Property(e => e.ISR)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Empleado>()
                .Property(e => e.SalarioNeto)
                .HasColumnType("decimal(10,2)");
        }
    }
}
