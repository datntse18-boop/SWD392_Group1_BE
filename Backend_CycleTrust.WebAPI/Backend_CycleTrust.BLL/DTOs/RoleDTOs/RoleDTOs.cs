namespace Backend_CycleTrust.BLL.DTOs.RoleDTOs
{
    public class CreateRoleDto
    {
        public string RoleName { get; set; } = null!;
    }

    public class UpdateRoleDto
    {
        public string RoleName { get; set; } = null!;
    }

    public class RoleResponseDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
