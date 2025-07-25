namespace human_resource_management.Dto
{
    public class DepartmentBudget
    {
        public string DepartmentName { get; set; }
        public int Year { get; set; }
        public decimal AllocatedBudget { get; set; }
        public decimal UsedBudget { get; set; }
    }
}
