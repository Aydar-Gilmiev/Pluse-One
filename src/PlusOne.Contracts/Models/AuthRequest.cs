using System.ComponentModel.DataAnnotations;

namespace PlusOne.Contracts.Models;

public sealed class AuthRequest
{
    [Required(ErrorMessage = "Укажите логин.")]
    [StringLength(50, ErrorMessage = "Логин должен содержать не более 50 символов.")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите пароль.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен содержать от 4 до 100 символов.")]
    public string Password { get; set; } = string.Empty;
}
