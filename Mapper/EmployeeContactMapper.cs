using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public static class EmployeeContactMapper
    {
        public static EmployeeContactDto ToDto(EmployeeContact contact)
        {
            return new EmployeeContactDto
            {
                ContactId = contact.ContactId,
                EmployeeId = contact.EmployeeId,
                ContactType = contact.ContactType ?? string.Empty,
                ContactValue = contact.ContactValue ?? string.Empty,
                IsPrimary = contact.IsPrimary ?? false
            };
        }

        public static EmployeeContact ToEntity(CreateEmployeeContactDto dto)
        {
            return new EmployeeContact
            {
                EmployeeId = dto.EmployeeId,
                ContactType = dto.ContactType.ToString(),  // enum → string
                ContactValue = dto.ContactValue,
                IsPrimary = dto.IsPrimary
            };
        }
    }

}
