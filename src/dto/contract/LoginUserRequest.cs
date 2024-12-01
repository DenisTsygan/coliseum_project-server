using System.ComponentModel.DataAnnotations;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password
);
