using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiEnrolement.Models;

public partial class BdEnrollementContext : DbContext
{
    public BdEnrollementContext()
    {
    }

    public BdEnrollementContext(DbContextOptions<BdEnrollementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TEmpreinte> TEmpreintes { get; set; }

    public virtual DbSet<TInfoPersonne> TInfoPersonnes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=172.10.10.103;port=5434;Database=Bd_enrollement;Username=vitbank;Password=vitbank");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TEmpreinte>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("primarykey_constraint");

            entity.ToTable("t_empreinte", "sc_enrollement");

            entity.Property(e => e.RId)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, null, 9999999999999L, null, null)
                .HasColumnName("r_id");
            entity.Property(e => e.RCreatedBy)
                .HasMaxLength(50)
                .HasColumnName("r_created_by");
            entity.Property(e => e.RCreatedOn)
                .HasMaxLength(50)
                .HasColumnName("r_created_on");
            entity.Property(e => e.RIdPersonneFk)
                .HasMaxLength(50)
                .HasColumnName("r_id_personne_fk");
            entity.Property(e => e.RLien)
                .HasMaxLength(500)
                .HasColumnName("r_lien");
            entity.Property(e => e.RType)
                .HasMaxLength(15)
                .HasColumnName("r_type");
            entity.Property(e => e.RValeur).HasColumnName("r_valeur");

            entity.HasOne(d => d.RIdPersonneFkNavigation).WithMany(p => p.TEmpreintes)
                .HasForeignKey(d => d.RIdPersonneFk)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_key_id_personne");
        });

        modelBuilder.Entity<TInfoPersonne>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("t_Info_personne_pkey");

            entity.ToTable("t_info_personne", "sc_enrollement");

            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .HasColumnName("r_id");
            entity.Property(e => e.RCreatedBy)
                .HasMaxLength(50)
                .HasColumnName("r_created_by");
            entity.Property(e => e.RCreatedOn).HasColumnName("r_created_on");
            entity.Property(e => e.RDateDExpiration).HasColumnName("r_date_d'expiration");
            entity.Property(e => e.RDateEmission).HasColumnName("r_date_emission");
            entity.Property(e => e.RDateNaissance).HasColumnName("r_date_naissance");
            entity.Property(e => e.RDescriptionLock)
                .HasMaxLength(200)
                .HasColumnName("r_description_lock");
            entity.Property(e => e.RIsLock).HasColumnName("r_is_lock");
            entity.Property(e => e.RLieuDeNaissance)
                .HasMaxLength(50)
                .HasColumnName("r_lieu de naissance ");
            entity.Property(e => e.RLieuEmission)
                .HasMaxLength(50)
                .HasColumnName("r_lieu_emission");
            entity.Property(e => e.RNationnalite)
                .HasMaxLength(20)
                .HasColumnName("r_nationnalite");
            entity.Property(e => e.RNni)
                .HasMaxLength(13)
                .HasColumnName("r_NNI");
            entity.Property(e => e.RNom)
                .HasMaxLength(16)
                .HasColumnName("r_nom");
            entity.Property(e => e.RNumCni)
                .HasMaxLength(13)
                .HasColumnName("r_num_cni");
            entity.Property(e => e.RNumUnique)
                .HasMaxLength(50)
                .HasColumnName("r_num_unique");
            entity.Property(e => e.RPrenom)
                .HasMaxLength(80)
                .HasColumnName("r_prenom");
            entity.Property(e => e.RProfession)
                .HasMaxLength(25)
                .HasColumnName("r_profession");
            entity.Property(e => e.RSexe)
                .HasMaxLength(1)
                .HasColumnName("r_sexe");
            entity.Property(e => e.RTaille).HasColumnName("r_taille");
            entity.Property(e => e.RUpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("r_updated_by");
            entity.Property(e => e.RUpdatedOn).HasColumnName("r_updated_on");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
