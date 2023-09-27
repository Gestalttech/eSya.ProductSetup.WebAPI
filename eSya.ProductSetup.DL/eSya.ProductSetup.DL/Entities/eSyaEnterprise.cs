﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eSya.ProductSetup.DL.Entities
{
    public partial class eSyaEnterprise : DbContext
    {
        public static string _connString = "";
        public eSyaEnterprise()
        {
        }

        public eSyaEnterprise(DbContextOptions<eSyaEnterprise> options)
            : base(options)
        {
        }

        public virtual DbSet<GtAddrct> GtAddrcts { get; set; } = null!;
        public virtual DbSet<GtAddrst> GtAddrsts { get; set; } = null!;
        public virtual DbSet<GtDccnpa> GtDccnpas { get; set; } = null!;
        public virtual DbSet<GtDccnst> GtDccnsts { get; set; } = null!;
        public virtual DbSet<GtDncnfd> GtDncnfds { get; set; } = null!;
        public virtual DbSet<GtEbecnt> GtEbecnts { get; set; } = null!;
        public virtual DbSet<GtEcapcd> GtEcapcds { get; set; } = null!;
        public virtual DbSet<GtEcapct> GtEcapcts { get; set; } = null!;
        public virtual DbSet<GtEcaprb> GtEcaprbs { get; set; } = null!;
        public virtual DbSet<GtEcaprl> GtEcaprls { get; set; } = null!;
        public virtual DbSet<GtEcbsen> GtEcbsens { get; set; } = null!;
        public virtual DbSet<GtEcbsln> GtEcbslns { get; set; } = null!;
        public virtual DbSet<GtEcbssc> GtEcbsscs { get; set; } = null!;
        public virtual DbSet<GtEcbssu> GtEcbssus { get; set; } = null!;
        public virtual DbSet<GtEcclco> GtEcclcos { get; set; } = null!;
        public virtual DbSet<GtEccldt> GtEccldts { get; set; } = null!;
        public virtual DbSet<GtEccncd> GtEccncds { get; set; } = null!;
        public virtual DbSet<GtEccnpi> GtEccnpis { get; set; } = null!;
        public virtual DbSet<GtEccnsd> GtEccnsds { get; set; } = null!;
        public virtual DbSet<GtEccntc> GtEccntcs { get; set; } = null!;
        public virtual DbSet<GtEccnti> GtEccntis { get; set; } = null!;
        public virtual DbSet<GtEccuco> GtEccucos { get; set; } = null!;
        public virtual DbSet<GtEccudn> GtEccudns { get; set; } = null!;
        public virtual DbSet<GtEcfmac> GtEcfmacs { get; set; } = null!;
        public virtual DbSet<GtEcfmal> GtEcfmals { get; set; } = null!;
        public virtual DbSet<GtEcfmfd> GtEcfmfds { get; set; } = null!;
        public virtual DbSet<GtEcfmnm> GtEcfmnms { get; set; } = null!;
        public virtual DbSet<GtEcfmp> GtEcfmps { get; set; } = null!;
        public virtual DbSet<GtEcfmpa> GtEcfmpas { get; set; } = null!;
        public virtual DbSet<GtEcmamn> GtEcmamns { get; set; } = null!;
        public virtual DbSet<GtEcmnfl> GtEcmnfls { get; set; } = null!;
        public virtual DbSet<GtEcparh> GtEcparhs { get; set; } = null!;
        public virtual DbSet<GtEcparm> GtEcparms { get; set; } = null!;
        public virtual DbSet<GtEcprrl> GtEcprrls { get; set; } = null!;
        public virtual DbSet<GtEcsbmn> GtEcsbmns { get; set; } = null!;
        public virtual DbSet<GtEcsupa> GtEcsupas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GtAddrct>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.StateCode, e.CityCode });

                entity.ToTable("GT_ADDRCT");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.CityDesc).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtAddrst>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.StateCode });

                entity.ToTable("GT_ADDRST");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.StateDesc).HasMaxLength(50);
            });

            modelBuilder.Entity<GtDccnpa>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.ParameterId });

                entity.ToTable("GT_DCCNPA");

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtDccnst>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.ToTable("GT_DCCNST");

                entity.Property(e => e.DocumentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.DocumentDesc).HasMaxLength(50);

                entity.Property(e => e.DocumentType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.SchemaId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SchemaID")
                    .IsFixedLength();

                entity.Property(e => e.ShortDesc)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GtDncnfd>(entity =>
            {
                entity.HasKey(e => new { e.FormId, e.DocumentId });

                entity.ToTable("GT_DNCNFD");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEbecnt>(entity =>
            {
                entity.ToTable("GT_EBECNT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Area).HasMaxLength(50);

                entity.Property(e => e.Controller).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcapcd>(entity =>
            {
                entity.HasKey(e => e.ApplicationCode)
                    .HasName("PK_GT_ECAPCD_1");

                entity.ToTable("GT_ECAPCD");

                entity.Property(e => e.ApplicationCode).ValueGeneratedNever();

                entity.Property(e => e.CodeDesc).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ShortCode).HasMaxLength(15);

                entity.HasOne(d => d.CodeTypeNavigation)
                    .WithMany(p => p.GtEcapcds)
                    .HasForeignKey(d => d.CodeType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECAPCD_GT_ECAPCT");
            });

            modelBuilder.Entity<GtEcapct>(entity =>
            {
                entity.HasKey(e => e.CodeType);

                entity.ToTable("GT_ECAPCT");

                entity.Property(e => e.CodeType).ValueGeneratedNever();

                entity.Property(e => e.CodeTyepDesc).HasMaxLength(50);

                entity.Property(e => e.CodeTypeControl)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcaprb>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.RuleId, e.ProcessId });

                entity.ToTable("GT_ECAPRB");

                entity.Property(e => e.RuleId).HasColumnName("RuleID");

                entity.Property(e => e.ProcessId).HasColumnName("ProcessID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcaprl>(entity =>
            {
                entity.HasKey(e => new { e.RuleId, e.ProcessId });

                entity.ToTable("GT_ECAPRL");

                entity.Property(e => e.RuleId).HasColumnName("RuleID");

                entity.Property(e => e.ProcessId).HasColumnName("ProcessID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.Property(e => e.RuleDesc).HasMaxLength(500);

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.GtEcaprls)
                    .HasForeignKey(d => d.ProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECAPRL_GT_ECPRRL");
            });

            modelBuilder.Entity<GtEcbsen>(entity =>
            {
                entity.HasKey(e => e.BusinessId);

                entity.ToTable("GT_ECBSEN");

                entity.Property(e => e.BusinessId)
                    .ValueGeneratedNever()
                    .HasColumnName("BusinessID");

                entity.Property(e => e.BusinessDesc).HasMaxLength(75);

                entity.Property(e => e.BusinessUnitType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('S')")
                    .IsFixedLength();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcbsln>(entity =>
            {
                entity.HasKey(e => new { e.BusinessId, e.LocationId });

                entity.ToTable("GT_ECBSLN");

                entity.HasIndex(e => e.BusinessKey, "IX_GT_ECBSLN")
                    .IsUnique();

                entity.Property(e => e.BusinessId).HasColumnName("BusinessID");

                entity.Property(e => e.BusinessName).HasMaxLength(100);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.CurrencyCode).HasMaxLength(4);

                entity.Property(e => e.EActiveUsers).HasColumnName("eActiveUsers");

                entity.Property(e => e.EBusinessKey).HasColumnName("eBusinessKey");

                entity.Property(e => e.ENoOfBeds).HasColumnName("eNoOfBeds");

                entity.Property(e => e.ESyaLicenseType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("eSyaLicenseType")
                    .IsFixedLength();

                entity.Property(e => e.EUserLicenses).HasColumnName("eUserLicenses");

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.LocationDescription).HasMaxLength(150);

                entity.Property(e => e.LocnDateFormat)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.TocurrConversion).HasColumnName("TOCurrConversion");

                entity.Property(e => e.TolocalCurrency)
                    .IsRequired()
                    .HasColumnName("TOLocalCurrency")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TorealCurrency).HasColumnName("TORealCurrency");
            });

            modelBuilder.Entity<GtEcbssc>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.TocurrencyCode })
                    .HasName("PK_GT_ECBSSC_1");

                entity.ToTable("GT_ECBSSC");

                entity.Property(e => e.TocurrencyCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TOCurrencyCode");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcbssu>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.SubscribedFrom })
                    .HasName("PK_GT_ECBSSU_1");

                entity.ToTable("GT_ECBSSU");

                entity.Property(e => e.SubscribedFrom).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.SubscribedTill).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessKeyNavigation)
                    .WithMany(p => p.GtEcbssus)
                    .HasPrincipalKey(p => p.BusinessKey)
                    .HasForeignKey(d => d.BusinessKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECBSSU_GT_ECBSLN");
            });

            modelBuilder.Entity<GtEcclco>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.FinancialYear });

                entity.ToTable("GT_ECCLCO");

                entity.Property(e => e.FinancialYear).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.TillDate).HasColumnType("datetime");

                entity.HasOne(d => d.BusinessKeyNavigation)
                    .WithMany(p => p.GtEcclcos)
                    .HasPrincipalKey(p => p.BusinessKey)
                    .HasForeignKey(d => d.BusinessKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECCLCO_GT_ECBSLN");
            });

            modelBuilder.Entity<GtEccldt>(entity =>
            {
                entity.HasKey(e => new { e.BusinessKey, e.FinancialYear, e.MonthId });

                entity.ToTable("GT_ECCLDT");

                entity.Property(e => e.FinancialYear).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.MonthId).HasColumnName("MonthID");

                entity.Property(e => e.BudgetMonth).HasMaxLength(4);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.MonthFreezeHis).HasColumnName("MonthFreezeHIS");

                entity.Property(e => e.MonthFreezeHr).HasColumnName("MonthFreezeHR");

                entity.Property(e => e.PatientIdgen).HasColumnName("PatientIDGen");

                entity.Property(e => e.PatientIdserial)
                    .HasMaxLength(1)
                    .HasColumnName("PatientIDSerial")
                    .IsFixedLength();

                entity.HasOne(d => d.GtEcclco)
                    .WithMany(p => p.GtEccldts)
                    .HasForeignKey(d => new { d.BusinessKey, d.FinancialYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECCLDT_GT_ECCLCO");
            });

            modelBuilder.Entity<GtEccncd>(entity =>
            {
                entity.HasKey(e => e.Isdcode);

                entity.ToTable("GT_ECCNCD");

                entity.Property(e => e.Isdcode)
                    .ValueGeneratedNever()
                    .HasColumnName("ISDCode");

                entity.Property(e => e.CountryCode).HasMaxLength(4);

                entity.Property(e => e.CountryFlag).HasMaxLength(150);

                entity.Property(e => e.CountryName).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.CurrencyCode).HasMaxLength(4);

                entity.Property(e => e.DateFormat)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.IsPinapplicable).HasColumnName("IsPINApplicable");

                entity.Property(e => e.IsPoboxApplicable).HasColumnName("IsPOBoxApplicable");

                entity.Property(e => e.MobileNumberPattern)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.PincodePattern)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("PINcodePattern");

                entity.Property(e => e.PoboxPattern)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("POBoxPattern");

                entity.Property(e => e.ShortDateFormat)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GtEccnpi>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.Uidlabel });

                entity.ToTable("GT_ECCNPI");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.Uidlabel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UIDLabel");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.Uidpattern)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UIDPattern");
            });

            modelBuilder.Entity<GtEccnsd>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.StatutoryCode });

                entity.ToTable("GT_ECCNSD");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.StatPattern).HasMaxLength(25);

                entity.Property(e => e.StatShortCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StatutoryDescription).HasMaxLength(50);

                entity.HasOne(d => d.IsdcodeNavigation)
                    .WithMany(p => p.GtEccnsds)
                    .HasForeignKey(d => d.Isdcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECCNSD_GT_ECCNCD");
            });

            modelBuilder.Entity<GtEccntc>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.TaxCode });

                entity.ToTable("GT_ECCNTC");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.SlabOrPerc)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TaxDescription).HasMaxLength(50);

                entity.Property(e => e.TaxShortCode).HasMaxLength(10);

                entity.HasOne(d => d.IsdcodeNavigation)
                    .WithMany(p => p.GtEccntcs)
                    .HasForeignKey(d => d.Isdcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECCNTC_GT_ECCNCD");
            });

            modelBuilder.Entity<GtEccnti>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.TaxIdentificationId })
                    .HasName("PK_GT_ECCNTI_1");

                entity.ToTable("GT_ECCNTI");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.TaxIdentificationId).HasColumnName("TaxIdentificationID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.StateCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TaxIdentificationDesc).HasMaxLength(25);
            });

            modelBuilder.Entity<GtEccuco>(entity =>
            {
                entity.HasKey(e => e.CurrencyCode);

                entity.ToTable("GT_ECCUCO");

                entity.Property(e => e.CurrencyCode).HasMaxLength(4);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.CurrencyName).HasMaxLength(25);

                entity.Property(e => e.DecimalPlaces).HasColumnType("decimal(2, 0)");

                entity.Property(e => e.DecimalPortionWord).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.Symbol).HasMaxLength(10);
            });

            modelBuilder.Entity<GtEccudn>(entity =>
            {
                entity.HasKey(e => new { e.CurrencyCode, e.BnorCnId, e.DenomId });

                entity.ToTable("GT_ECCUDN");

                entity.Property(e => e.CurrencyCode).HasMaxLength(4);

                entity.Property(e => e.BnorCnId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BNorCN_ID")
                    .IsFixedLength();

                entity.Property(e => e.DenomId)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("DenomID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.DenomConversion).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.DenomDesc)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.HasOne(d => d.CurrencyCodeNavigation)
                    .WithMany(p => p.GtEccudns)
                    .HasForeignKey(d => d.CurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECCUDN_GT_ECCUCO");
            });

            modelBuilder.Entity<GtEcfmac>(entity =>
            {
                entity.HasKey(e => e.ActionId);

                entity.ToTable("GT_ECFMAC");

                entity.Property(e => e.ActionId)
                    .ValueGeneratedNever()
                    .HasColumnName("ActionID");

                entity.Property(e => e.ActionDesc).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcfmal>(entity =>
            {
                entity.HasKey(e => new { e.FormId, e.ActionId });

                entity.ToTable("GT_ECFMAL");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.ActionId).HasColumnName("ActionID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.GtEcfmals)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECFMAL_GT_ECFMAC");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.GtEcfmals)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECFMAL_GT_ECFMFD");
            });

            modelBuilder.Entity<GtEcfmfd>(entity =>
            {
                entity.HasKey(e => e.FormId);

                entity.ToTable("GT_ECFMFD");

                entity.Property(e => e.FormId)
                    .ValueGeneratedNever()
                    .HasColumnName("FormID");

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FormName).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ToolTip).HasMaxLength(250);
            });

            modelBuilder.Entity<GtEcfmnm>(entity =>
            {
                entity.HasKey(e => new { e.FormId, e.FormIntId });

                entity.ToTable("GT_ECFMNM");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.FormIntId)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("FormIntID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormDescription).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.NavigateUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NavigateURL");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.GtEcfmnms)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECFMNM_GT_ECFMFD");
            });

            modelBuilder.Entity<GtEcfmp>(entity =>
            {
                entity.HasKey(e => new { e.FormId, e.ParameterId, e.SubParameterId });

                entity.ToTable("GT_ECFMPS");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcfmpa>(entity =>
            {
                entity.HasKey(e => new { e.FormId, e.ParameterId });

                entity.ToTable("GT_ECFMPA");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.GtEcfmpas)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECFMPA_GT_ECFMFD");
            });

            modelBuilder.Entity<GtEcmamn>(entity =>
            {
                entity.HasKey(e => e.MainMenuId);

                entity.ToTable("GT_ECMAMN");

                entity.Property(e => e.MainMenuId)
                    .ValueGeneratedNever()
                    .HasColumnName("MainMenuID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.MainMenu).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcmnfl>(entity =>
            {
                entity.HasKey(e => new { e.FormId, e.MainMenuId, e.MenuItemId });

                entity.ToTable("GT_ECMNFL");

                entity.Property(e => e.FormId).HasColumnName("FormID");

                entity.Property(e => e.MainMenuId).HasColumnName("MainMenuID");

                entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormNameClient).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.GtEcmnfls)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECMNFL_GT_ECFMFD");

                entity.HasOne(d => d.MainMenu)
                    .WithMany(p => p.GtEcmnfls)
                    .HasForeignKey(d => d.MainMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECMNFL_GT_ECMAMN");
            });

            modelBuilder.Entity<GtEcparh>(entity =>
            {
                entity.HasKey(e => e.ParameterType);

                entity.ToTable("GT_ECPARH");

                entity.Property(e => e.ParameterType).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ParameterHeaderDesc).HasMaxLength(50);
            });

            modelBuilder.Entity<GtEcparm>(entity =>
            {
                entity.HasKey(e => new { e.ParameterType, e.ParameterId });

                entity.ToTable("GT_ECPARM");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ParameterDesc).HasMaxLength(50);

                entity.Property(e => e.ParameterValueType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<GtEcprrl>(entity =>
            {
                entity.HasKey(e => e.ProcessId);

                entity.ToTable("GT_ECPRRL");

                entity.Property(e => e.ProcessId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProcessID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.FormId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FormID");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ProcessControl)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProcessDesc).HasMaxLength(500);
            });

            modelBuilder.Entity<GtEcsbmn>(entity =>
            {
                entity.HasKey(e => new { e.MenuItemId, e.MainMenuId });

                entity.ToTable("GT_ECSBMN");

                entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");

                entity.Property(e => e.MainMenuId).HasColumnName("MainMenuID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.MenuItemName).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.HasOne(d => d.MainMenu)
                    .WithMany(p => p.GtEcsbmns)
                    .HasForeignKey(d => d.MainMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GT_ECSBMN_GT_ECMAMN");
            });

            modelBuilder.Entity<GtEcsupa>(entity =>
            {
                entity.HasKey(e => new { e.Isdcode, e.StatutoryCode, e.ParameterId });

                entity.ToTable("GT_ECSUPA");

                entity.Property(e => e.Isdcode).HasColumnName("ISDCode");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedTerminal).HasMaxLength(50);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedTerminal).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
