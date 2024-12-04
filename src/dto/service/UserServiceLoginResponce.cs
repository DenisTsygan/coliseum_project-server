using System.ComponentModel.DataAnnotations;

public record UserServiceLoginResponce(//TODO replace TokensResponce
    [Required] string AccessToken,
    [Required] string RefreshToken
);