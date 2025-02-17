using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace app.domains.users
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(100)]
        public required string Email { get; set; }
    }
}
