using System.ComponentModel.DataAnnotations;

public record RenameClientRequest(
    [Required] string NewName,
    [Required] string EcmId
);
