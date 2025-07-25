namespace human_resource_management.Dto
{
    public class DepartmentBudgetDTO
    {
        public int BudgetId { get; set; }
        public string DepartmentName { get; set; }
        public decimal? AllocatedBudget { get; set; }
        public decimal? UsedBudget { get; set; }
        public int Year { get; set; }
    }

}
