namespace human_resource_management.Util
{
    public enum EmployeeStatus
    {
        Active = 0,       // Đang làm việc
        Inactive = 1,     // Đã nghỉ việc hoặc bị khóa tài khoản
        OnLeave = 2,      // Đang nghỉ phép
        Probation = 3,    // Đang thử việc
        Suspended = 4     // Bị tạm đình chỉ
    }
}
