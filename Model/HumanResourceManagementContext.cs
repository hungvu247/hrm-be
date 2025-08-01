﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace human_resource_management.Model;

public partial class HumanResourceManagementContext : DbContext
{
    public HumanResourceManagementContext()
    {
    }

    public HumanResourceManagementContext(DbContextOptions<HumanResourceManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentBudget> DepartmentBudgets { get; set; }

    public virtual DbSet<EligibleForPromotion> EligibleForPromotions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeContact> EmployeeContacts { get; set; }

    public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; }

    public virtual DbSet<PerformanceReview> PerformanceReviews { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; }

    public virtual DbSet<PromotionHistory> PromotionHistories { get; set; }

    public virtual DbSet<PromotionRequest> PromotionRequests { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RequestForm> RequestForms { get; set; }

    public virtual DbSet<RequestType> RequestTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SkillCertificate> SkillCertificates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("MyCnn")); }
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__departme__C223242272DE7928");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(255)
                .HasColumnName("department_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LeadEmployeeId).HasColumnName("lead_employee_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.LeadEmployee).WithMany(p => p.Departments)
                .HasForeignKey(d => d.LeadEmployeeId)
                .HasConstraintName("FK_Department_LeadEmployee");
        });

        modelBuilder.Entity<DepartmentBudget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PK__departme__3A655C1493EE8890");

            entity.ToTable("department_budgets");

            entity.Property(e => e.BudgetId).HasColumnName("budget_id");
            entity.Property(e => e.AllocatedBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("allocated_budget");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.UsedBudget)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("used_budget");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.Department).WithMany(p => p.DepartmentBudgets)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__departmen__depar__5CD6CB2B");
        });

        modelBuilder.Entity<EligibleForPromotion>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("eligible_for_promotion");

            entity.Property(e => e.AvgKpi).HasColumnName("avg_kpi");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.FirstReview).HasColumnName("first_review");
            entity.Property(e => e.FullName)
                .HasMaxLength(511)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.LatestReview).HasColumnName("latest_review");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.ReviewCount).HasColumnName("review_count");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA8A32C9F12");

            entity.ToTable("employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.LeadEmployeeId).HasColumnName("lead_employee_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("salary");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__employees__depar__32E0915F");

            entity.HasOne(d => d.LeadEmployee).WithMany(p => p.InverseLeadEmployee)
                .HasForeignKey(d => d.LeadEmployeeId)
                .HasConstraintName("FK_Employee_LeadEmployee");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__employees__posit__31EC6D26");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_role_id");
        });

        modelBuilder.Entity<EmployeeContact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__employee__024E7A86EB70A027");

            entity.ToTable("employee_contacts");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.ContactType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact_type");
            entity.Property(e => e.ContactValue)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contact_value");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.IsPrimary).HasColumnName("is_primary");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeContacts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employee___emplo__6C190EBB");
        });

        modelBuilder.Entity<EmployeeProject>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeId, e.ProjectId }).HasName("PK__employee__2EE9924914E4C169");

            entity.ToTable("employee_projects");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.RoleInProject)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("role_in_project");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeProjects)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employee___emplo__6D0D32F4");

            entity.HasOne(d => d.Project).WithMany(p => p.EmployeeProjects)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__employee___proje__6E01572D");
        });

        modelBuilder.Entity<PerformanceReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__performa__60883D90DAC09431");

            entity.ToTable("performance_reviews");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comments)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.ReviewDate).HasColumnName("review_date");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Employee).WithMany(p => p.PerformanceReviews)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__performan__emplo__71D1E811");

            entity.HasOne(d => d.Project).WithMany(p => p.PerformanceReviews)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__performan__proje__72C60C4A");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__permissi__E5331AFA779D0CC2");

            entity.ToTable("permissions");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__position__99A0E7A49B639C57");

            entity.ToTable("positions");

            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .HasColumnName("position_name");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__projects__BC799E1FF9722ADC");

            entity.ToTable("projects");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("project_name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<ProjectDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__project___9666E8ACE20BFF3E");

            entity.ToTable("project_documents");

            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_name");
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("file_path");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.UploadDate).HasColumnName("upload_date");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectDocuments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__project_d__proje__73BA3083");
        });

        modelBuilder.Entity<PromotionHistory>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__promotio__2CB9556BA6F017F5");

            entity.ToTable("promotion_history");

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.NewPosition).HasColumnName("new_position");
            entity.Property(e => e.OldPosition).HasColumnName("old_position");
            entity.Property(e => e.PromotionDate).HasColumnName("promotion_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.PromotionHistories)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__emplo__74AE54BC");

            entity.HasOne(d => d.NewPositionNavigation).WithMany(p => p.PromotionHistoryNewPositionNavigations)
                .HasForeignKey(d => d.NewPosition)
                .HasConstraintName("fk_new_position");

            entity.HasOne(d => d.OldPositionNavigation).WithMany(p => p.PromotionHistoryOldPositionNavigations)
                .HasForeignKey(d => d.OldPosition)
                .HasConstraintName("fk_old_position");
        });

        modelBuilder.Entity<PromotionRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__promotio__18D3B90FA7D2BE4F");

            entity.ToTable("promotion_request", tb => tb.HasTrigger("trg_AutoInsertPromotion"));

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.ApproveDate).HasColumnName("approve_date");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.AssignedTo).HasColumnName("assigned_to");
            entity.Property(e => e.CurrentPositionId).HasColumnName("current_position_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("request_date");
            entity.Property(e => e.RequestedBy).HasColumnName("requested_by");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.SuggestedPositionId).HasColumnName("suggested_position_id");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.PromotionRequestApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__promotion__appro__7B5B524B");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.PromotionRequestAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_promotion_request_assigned_to");

            entity.HasOne(d => d.CurrentPosition).WithMany(p => p.PromotionRequestCurrentPositions)
                .HasForeignKey(d => d.CurrentPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__curre__787EE5A0");

            entity.HasOne(d => d.Employee).WithMany(p => p.PromotionRequestEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__emplo__778AC167");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.PromotionRequestRequestedByNavigations)
                .HasForeignKey(d => d.RequestedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__reque__7A672E12");

            entity.HasOne(d => d.SuggestedPosition).WithMany(p => p.PromotionRequestSuggestedPositions)
                .HasForeignKey(d => d.SuggestedPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__sugge__797309D9");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07903F7EDB");

            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_id");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.JwtId).HasMaxLength(200);
            entity.Property(e => e.Token).HasMaxLength(200);

            entity.HasOne(d => d.Employee).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RefreshTokens_AspNetUsers");
        });

        modelBuilder.Entity<RequestForm>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__RequestF__33A8517A8653D0A2");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ReviewedAt).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Employee).WithMany(p => p.RequestFormEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestFo__Emplo__1BC821DD");

            entity.HasOne(d => d.RequestType).WithMany(p => p.RequestForms)
                .HasForeignKey(d => d.RequestTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestFo__Reque__1CBC4616");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.RequestFormReviewedByNavigations)
                .HasForeignKey(d => d.ReviewedBy)
                .HasConstraintName("FK__RequestFo__Revie__1DB06A4F");
        });

        modelBuilder.Entity<RequestType>(entity =>
        {
            entity.HasKey(e => e.RequestTypeId).HasName("PK__RequestT__4D328B830AE40E8D");

            entity.Property(e => e.RequestTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC48926C41");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("role_name");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_role_permissions_permissions"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_role_permissions_roles"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId");
                        j.ToTable("role_permissions");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("permission_id");
                    });
        });

        modelBuilder.Entity<SkillCertificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("PK__skill_ce__E2256D3159F2D432");

            entity.ToTable("skill_certificates");

            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity.Property(e => e.CertificateName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("certificate_name");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.IssuedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("issued_by");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
