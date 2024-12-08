using System.ComponentModel.DataAnnotations;

public record RegisterUserRequest(
    [Required] string Email,
    [Required] string Password,
    [Required] string UserName,
    [Required] int RoleId
);