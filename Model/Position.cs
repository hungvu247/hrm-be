using System;
using System.Collections.Generic;

namespace human_resource_management.Model;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<PromotionHistory> PromotionHistoryNewPositionNavigations { get; set; } = new List<PromotionHistory>();

    public virtual ICollection<PromotionHistory> PromotionHistoryOldPositionNavigations { get; set; } = new List<PromotionHistory>();

    public virtual ICollection<PromotionRequest> PromotionRequestCurrentPositions { get; set; } = new List<PromotionRequest>();

    public virtual ICollection<PromotionRequest> PromotionRequestSuggestedPositions { get; set; } = new List<PromotionRequest>();
}
