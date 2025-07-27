using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? HireDate { get; set; }

    public decimal? Salary { get; set; }

    public string? Status { get; set; }

    public int? PositionId { get; set; }

    public int? DepartmentId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public int? LeadEmployeeId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<EmployeeContact> EmployeeContacts { get; set; } = new List<EmployeeContact>();

    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public virtual ICollection<Employee> InverseLeadEmployee { get; set; } = new List<Employee>();

    public virtual Employee? LeadEmployee { get; set; }

    public virtual ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<PromotionHistory> PromotionHistories { get; set; } = new List<PromotionHistory>();

    public virtual ICollection<PromotionRequest> PromotionRequestApprovedByNavigations { get; set; } = new List<PromotionRequest>();

    public virtual ICollection<PromotionRequest> PromotionRequestAssignedToNavigations { get; set; } = new List<PromotionRequest>();

    public virtual ICollection<PromotionRequest> PromotionRequestEmployees { get; set; } = new List<PromotionRequest>();

    public virtual ICollection<PromotionRequest> PromotionRequestRequestedByNavigations { get; set; } = new List<PromotionRequest>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<RequestForm> RequestFormEmployees { get; set; } = new List<RequestForm>();

    public virtual ICollection<RequestForm> RequestFormReviewedByNavigations { get; set; } = new List<RequestForm>();

    public virtual Role? Role { get; set; }
}
