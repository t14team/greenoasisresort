using System.ComponentModel.DataAnnotations;

namespace GreenOasisResort.Models;

public class ContactViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Subject is required")]
    [StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required")]
    [StringLength(2000, MinimumLength = 10)]
    public string Message { get; set; } = string.Empty;
}
