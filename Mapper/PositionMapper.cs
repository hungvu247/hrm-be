using human_resource_management.Dto;
using human_resource_management.Model;

namespace human_resource_management.Mapper
{
    public static class PositionMapper
    {
        public static PositionDto ToDto(Position position)
        {
            return new PositionDto
            {
                PositionId = position.PositionId,
                PositionName = position.PositionName,
            };
        }
    }
}
