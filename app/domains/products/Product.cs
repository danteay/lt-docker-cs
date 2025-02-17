using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace app.domains.products
{
    [Index(nameof(Sku), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(20)]
        public required string Sku { get; set; }

        [Required]
        public required decimal Price { get; set; }
    }
}
