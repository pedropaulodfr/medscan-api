using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartaoControle> CartaoControles { get; set; }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Medicamento> Medicamentos { get; set; }

    public virtual DbSet<Notificaco> Notificacoes { get; set; }

    public virtual DbSet<NotificacoesDetalhe> NotificacoesDetalhes { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Receituario> Receituarios { get; set; }

    public virtual DbSet<Setup> Setups { get; set; }

    public virtual DbSet<Solicitaco> Solicitacoes { get; set; }

    public virtual DbSet<TipoMedicamento> TipoMedicamentos { get; set; }

    public virtual DbSet<Unidade> Unidades { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<CartaoControle>(entity =>
        {
            entity.ToTable("CartaoControle");

            entity.Property(e => e.Data).HasColumnType("date");
            entity.Property(e => e.DataRetorno).HasColumnType("date");
            entity.Property(e => e.MedicamentoId).HasColumnName("Medicamento_Id");
            entity.Property(e => e.PacienteId).HasColumnName("Paciente_Id");
            entity.Property(e => e.Profissional)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Medicamento).WithMany(p => p.CartaoControles)
                .HasForeignKey(d => d.MedicamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartaoControle_Medicamentos");

            entity.HasOne(d => d.Paciente).WithMany(p => p.CartaoControles)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartaoControle_Pacientes");
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.Property(e => e.Corpo).HasColumnType("text");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Identificacao)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Perfil)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Logs", "Logs");

            entity.Property(e => e.Acao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.JsonAntigo).HasColumnType("text");
            entity.Property(e => e.JsonNovo).HasColumnType("text");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_Id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Logs_Usuarios");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.Property(e => e.Concentracao)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Identificacao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TipoMedicamentoId).HasColumnName("TipoMedicamento_Id");
            entity.Property(e => e.UnidadeId).HasColumnName("Unidade_Id");

            entity.HasOne(d => d.TipoMedicamento).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.TipoMedicamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicamentos_TipoMedicamento");

            entity.HasOne(d => d.Unidade).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.UnidadeId)
                .HasConstraintName("FK_Medicamentos_Unidades");
        });

        modelBuilder.Entity<Notificaco>(entity =>
        {
            entity.Property(e => e.CartaoControleId).HasColumnName("CartaoControle_Id");
            entity.Property(e => e.Data).HasColumnType("datetime");
            entity.Property(e => e.EmailId).HasColumnName("Email_Id");
            entity.Property(e => e.PacienteId).HasColumnName("Paciente_Id");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_Id");

            entity.HasOne(d => d.CartaoControle).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.CartaoControleId)
                .HasConstraintName("FK_Notificacoes_CartaoControle");

            entity.HasOne(d => d.Email).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacoes_Emails");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.PacienteId)
                .HasConstraintName("FK_Notificacoes_Pacientes1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Notificacoes_Usuarios1");
        });

        modelBuilder.Entity<NotificacoesDetalhe>(entity =>
        {
            entity.Property(e => e.AssuntoEnviado).HasColumnType("text");
            entity.Property(e => e.DataHoraEnvio).HasColumnType("datetime");
            entity.Property(e => e.EmailId).HasColumnName("Email_Id");
            entity.Property(e => e.EnderecosEnviados)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NotificacoesId).HasColumnName("Notificacoes_Id");
            entity.Property(e => e.TituloEnviado)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Email).WithMany(p => p.NotificacoesDetalhes)
                .HasForeignKey(d => d.EmailId)
                .HasConstraintName("FK_NotificacoesDetalhes_Emails1");

            entity.HasOne(d => d.Notificacoes).WithMany(p => p.NotificacoesDetalhes)
                .HasForeignKey(d => d.NotificacoesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotificacoesDetalhes_Notificacoes1");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC0725924F7D");

            entity.Property(e => e.Bairro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cep)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("CEP");
            entity.Property(e => e.Cidade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cns)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CNS");
            entity.Property(e => e.Complemento)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cpf)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.DataNascimento).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Logradouro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NomeCompleto)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PlanoSaude)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("UF");
            entity.Property(e => e.UsuariosId).HasColumnName("Usuarios_Id");

            entity.HasOne(d => d.Usuarios).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.UsuariosId)
                .HasConstraintName("FK_Pacientes_Usuario");
        });

        modelBuilder.Entity<Receituario>(entity =>
        {
            entity.ToTable("Receituario");

            entity.Property(e => e.MedicamentoId).HasColumnName("Medicamento_Id");
            entity.Property(e => e.PacienteId).HasColumnName("Paciente_Id");
            entity.Property(e => e.Periodo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("1 - Manhã; 2 - Tarde; 3 - Noite");
            entity.Property(e => e.Tempo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoMedicamentoId).HasColumnName("TipoMedicamento_Id");

            entity.HasOne(d => d.Medicamento).WithMany(p => p.Receituarios)
                .HasForeignKey(d => d.MedicamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receituario_Medicamentos");

            entity.HasOne(d => d.TipoMedicamento).WithMany(p => p.Receituarios)
                .HasForeignKey(d => d.TipoMedicamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receituario_TipoMedicamento");
        });

        modelBuilder.Entity<Setup>(entity =>
        {
            entity.ToTable("Setup");

            entity.Property(e => e.CaminhoArquivos).HasColumnType("text");
            entity.Property(e => e.SmtpHost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("smtpHost");
            entity.Property(e => e.SmtpPassword)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("smtpPassword");
            entity.Property(e => e.SmtpPort)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("smtpPort");
            entity.Property(e => e.SmtpUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("smtpUser");
            entity.Property(e => e.Urlapi)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("URLApi");
            entity.Property(e => e.Urlweb)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("URLWeb");
        });

        modelBuilder.Entity<Solicitaco>(entity =>
        {
            entity.Property(e => e.Concentracao)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DataHoraAnalise).HasColumnType("datetime");
            entity.Property(e => e.DataHoraSolicitacao).HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Identificacao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PacienteId).HasColumnName("Paciente_Id");
            entity.Property(e => e.TipoMedicamentoId).HasColumnName("TipoMedicamento_Id");
            entity.Property(e => e.UnidadeId).HasColumnName("Unidade_Id");
            entity.Property(e => e.UsuarioAnaliseId).HasColumnName("UsuarioAnalise_Id");
            entity.Property(e => e.UsuarioSolicitacaoId).HasColumnName("UsuarioSolicitacao_Id");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicitacos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitacoes_Pacientes");

            entity.HasOne(d => d.TipoMedicamento).WithMany(p => p.Solicitacos)
                .HasForeignKey(d => d.TipoMedicamentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitacoes_TipoMedicamento1");

            entity.HasOne(d => d.Unidade).WithMany(p => p.Solicitacos)
                .HasForeignKey(d => d.UnidadeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitacoes_Unidades1");

            entity.HasOne(d => d.UsuarioAnalise).WithMany(p => p.SolicitacoUsuarioAnalises)
                .HasForeignKey(d => d.UsuarioAnaliseId)
                .HasConstraintName("FK_Solicitacoes_Usuarios1");

            entity.HasOne(d => d.UsuarioSolicitacao).WithMany(p => p.SolicitacoUsuarioSolicitacaos)
                .HasForeignKey(d => d.UsuarioSolicitacaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitacoes_Usuarios");
        });

        modelBuilder.Entity<TipoMedicamento>(entity =>
        {
            entity.ToTable("TipoMedicamento");

            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Identificacao)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Unidade>(entity =>
        {
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Identificacao)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.CodigoCadastro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImagemPerfil).HasColumnType("text");
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Perfil)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
