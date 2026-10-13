using System.ComponentModel.DataAnnotations;

namespace PlusOne.Contracts.Models;

public sealed class RegRequest
{
    [Required(ErrorMessage = "Укажите имя.")]
    [StringLength(80, ErrorMessage = "Имя должно содержать не более 80 символов.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите логин.")]
    [StringLength(50, ErrorMessage = "Логин должен содержать не более 50 символов.")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите пароль.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен содержать от 4 до 100 символов.")]
    public string Password { get; set; } = string.Empty;
}
