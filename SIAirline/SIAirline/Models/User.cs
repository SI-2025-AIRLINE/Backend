using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public enum UserRole
{
    Admin,
    Employee,
    Customer
}

public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

    [Required]
    public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime? LastLogin { get; set; }

        [Required]
        public bool IsVerified { get; set; }

        [Required]
        [MaxLength(100)]
        public string VerificationToken { get; set; }

        [Required]
        [MaxLength(100)]
        public string ResetToken { get; set; }
    }
