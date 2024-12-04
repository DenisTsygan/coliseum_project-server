using System.ComponentModel.DataAnnotations;

public record RefreshTokenRequest(
    [Required] string FingerPrint
);
