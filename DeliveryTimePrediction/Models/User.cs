using System.ComponentModel.DataAnnotations;
namespace DeliveryTimePrediction.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";


        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = "";

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Created is required")]
        public string CreatedBy { get; set; } = "";

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

    }
}
