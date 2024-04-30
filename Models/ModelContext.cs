using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace handicrafe.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<AboutU> AboutUs { get; set; }
        public virtual DbSet<FeedbackHandicraft> FeedbackHandicrafts { get; set; }
        public virtual DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public virtual DbSet<Handicraft> Handicrafts { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<VisaCard> VisaCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("USER ID=JOR16_User15;PASSWORD=Test321;DATA SOURCE=94.56.229.181:3488/traindb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("JOR16_USER15")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<About>(entity =>
            {
                entity.ToTable("ABOUT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Image)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.Text)
                    .HasMaxLength(1500)
                    .IsUnicode(false)
                    .HasColumnName("TEXT");
            });

            modelBuilder.Entity<AboutU>(entity =>
            {
                entity.ToTable("ABOUT_US");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL")
                    .IsFixedLength(true);

                entity.Property(e => e.Facebock)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FACEBOCK");

                entity.Property(e => e.Instagram)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INSTAGRAM");

                entity.Property(e => e.LocationL)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION_L");

                entity.Property(e => e.LocationY)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION_Y");

                entity.Property(e => e.Locations)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LOCATIONS")
                    .IsFixedLength(true);

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PHONE")
                    .IsFixedLength(true);

                entity.Property(e => e.Twitter)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TWITTER");
            });

            modelBuilder.Entity<FeedbackHandicraft>(entity =>
            {
                entity.HasKey(e => e.IdFeedback)
                    .HasName("SYS_C00276847");

                entity.ToTable("FEEDBACK_HANDICRAFT");

                entity.Property(e => e.IdFeedback)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_FEEDBACK");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE")
                    .IsFixedLength(true);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.FeedbackHandicrafts)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_FEEDBACK_HANDICRAFT_USER_INFO");
            });

            modelBuilder.Entity<FinancialAccount>(entity =>
            {
                entity.ToTable("FINANCIAL__ACCOUNT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Costs)
                    .HasColumnType("FLOAT")
                    .HasColumnName("COSTS");

                entity.Property(e => e.FinancialReturn)
                    .HasColumnType("FLOAT")
                    .HasColumnName("FINANCIAL_RETURN");
            });

            modelBuilder.Entity<Handicraft>(entity =>
            {
                entity.HasKey(e => e.IdHandicraft)
                    .HasName("SYS_C00276824");

                entity.ToTable("HANDICRAFT");

                entity.Property(e => e.IdHandicraft)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_HANDICRAFT");

                entity.Property(e => e.Datee)
                    .HasColumnType("DATE")
                    .HasColumnName("DATEE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Price)
                    .HasColumnType("FLOAT")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("QUANTITY");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Handicrafts)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_HANDICRAFT_USER_INFO");
            });

            modelBuilder.Entity<Home>(entity =>
            {
                entity.ToTable("HOME");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Hometext)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOMETEXT");

                entity.Property(e => e.Imag)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IMAG");

                entity.Property(e => e.Text2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TEXT2");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.IdSales)
                    .HasName("SYS_C00276836");

                entity.ToTable("SALES");

                entity.Property(e => e.IdSales)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_SALES");

                entity.Property(e => e.DateSale)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_SALE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID");

                entity.Property(e => e.IdHandicraft)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID_HANDICRAFT");

                entity.Property(e => e.PdfEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PDF_EMAIL");

                entity.Property(e => e.Quantity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("QUANTITY");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_SALES_USER");

                entity.HasOne(d => d.IdHandicraftNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.IdHandicraft)
                    .HasConstraintName("FK_SALES_HANDICRAFT");
            });

            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.HasKey(e => e.IdTestimonial)
                    .HasName("SYS_C00281539");

                entity.ToTable("TESTIMONIAL");

                entity.Property(e => e.IdTestimonial)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_TESTIMONIAL");

                entity.Property(e => e.Acceptt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACCEPTT")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID");

                entity.Property(e => e.Text)
                    .HasMaxLength(1500)
                    .IsUnicode(false)
                    .HasColumnName("TEXT");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Testimonials)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TESTIMONIAL_USER");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("USER_INFO");

                entity.HasIndex(e => e.UserName, "SYS_C00276813")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("F_NAME");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.LName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("L_NAME");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.RegisteringDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REGISTERING_DATE");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ROLE_NAME");

                entity.Property(e => e.Testimonial)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TESTIMONIAL")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME");
            });

            modelBuilder.Entity<VisaCard>(entity =>
            {
                entity.HasKey(e => e.IdVisa)
                    .HasName("SYS_C00276829");

                entity.ToTable("VISA_CARD");

                entity.Property(e => e.IdVisa)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_VISA");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID");

                entity.Property(e => e.Total)
                    .HasColumnType("FLOAT")
                    .HasColumnName("TOTAL");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.VisaCards)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_VISA_CARD_USER_INFO");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
