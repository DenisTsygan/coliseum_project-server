using System.ComponentModel.DataAnnotations;

public record LogoutByRSIdRequest(
    [Required] string Rsid
);
