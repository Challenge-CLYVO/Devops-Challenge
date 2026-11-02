namespace PetCare.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets
    public DbSet<Pet> Pets { get; set; }

    public DbSet<Tutor> Tutors { get; set; }

    public DbSet<Consulta> Consultas { get; set; }

    public DbSet<Clinica> Clinicas { get; set; }

    public DbSet<Vacina> Vacinas { get; set; }

    public DbSet<AplicacaoVacina> AplicacoesVacina { get; set; }

    public DbSet<HistoricoSaude> HistoricosSaude { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // =========================
        // TABELA PET
        // =========================
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.ToTable("Pet");

            entity.HasKey(e => e.IdPet);

            entity.Property(e => e.IdPet)
                .HasColumnName("id_pet")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Idade)
                .HasColumnName("idade");

            entity.Property(e => e.Especie)
                .HasColumnName("especie")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Raca)
                .HasColumnName("raca")
                .HasMaxLength(50);

            entity.Property(e => e.IdTutor)
                .HasColumnName("id_tutor");

            entity.HasOne(e => e.Tutor)
                .WithMany(t => t.Pets)
                .HasForeignKey(e => e.IdTutor)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // TABELA TUTOR
        // =========================
        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.ToTable("Tutor");

            entity.HasKey(e => e.IdTutor);

            entity.Property(e => e.IdTutor)
                .HasColumnName("id_tutor")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20);

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(100);
        });

        // =========================
        // TABELA CLINICA
        // =========================
        modelBuilder.Entity<Clinica>(entity =>
        {
            entity.ToTable("Clinica");

            entity.HasKey(e => e.IdClinica);

            entity.Property(e => e.IdClinica)
                .HasColumnName("id_clinica")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Endereco)
                .HasColumnName("endereco")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Telefone)
                .HasColumnName("telefone")
                .HasMaxLength(20);
        });

        // =========================
        // TABELA CONSULTA
        // =========================
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.ToTable("Consulta");

            entity.HasKey(e => e.IdConsulta);

            entity.Property(e => e.IdConsulta)
                .HasColumnName("id_consulta")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DataConsulta)
                .HasColumnName("data_consulta")
                .IsRequired();

            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(200);

            entity.Property(e => e.IdPet)
                .HasColumnName("id_pet");

            entity.Property(e => e.IdClinica)
                .HasColumnName("id_clinica");

            entity.HasOne(e => e.Pet)
                .WithMany(p => p.Consultas)
                .HasForeignKey(e => e.IdPet)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Clinica)
                .WithMany(c => c.Consultas)
                .HasForeignKey(e => e.IdClinica)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // TABELA VACINA
        // =========================
        modelBuilder.Entity<Vacina>(entity =>
        {
            entity.ToTable("Vacina");

            entity.HasKey(e => e.IdVacina);

            entity.Property(e => e.IdVacina)
                .HasColumnName("id_vacina")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(200);
        });

        // =========================
        // TABELA APLICACAO_VACINA
        // =========================
        modelBuilder.Entity<AplicacaoVacina>(entity =>
        {
            entity.ToTable("Aplicacao_vacina");

            entity.HasKey(e => e.IdAplicacao);

            entity.Property(e => e.IdAplicacao)
                .HasColumnName("id_aplicacao")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DataAplicacao)
                .HasColumnName("data_aplicacao")
                .IsRequired();

            entity.Property(e => e.IdVacina)
                .HasColumnName("id_vacina");

            entity.Property(e => e.IdPet)
                .HasColumnName("id_pet");

            entity.HasOne(e => e.Vacina)
                .WithMany(v => v.AplicacoesVacina)
                .HasForeignKey(e => e.IdVacina)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Pet)
                .WithMany(p => p.AplicacoesVacina)
                .HasForeignKey(e => e.IdPet)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // TABELA HISTORICO_SAUDE
        // =========================
        modelBuilder.Entity<HistoricoSaude>(entity =>
        {
            entity.ToTable("Historico_saude");

            entity.HasKey(e => e.IdHistorico);

            entity.Property(e => e.IdHistorico)
                .HasColumnName("id_historico")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.DataRegistro)
                .HasColumnName("data_registro")
                .IsRequired();

            entity.Property(e => e.IdPet)
                .HasColumnName("id_pet");

            entity.HasOne(e => e.Pet)
                .WithMany(p => p.HistoricosSaude)
                .HasForeignKey(e => e.IdPet)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}