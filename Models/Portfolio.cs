using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; } = string.Empty;
        public int StockId { get; set; }

        // Navigation Properties
        public AppUser AppUser { get; set; } = null!;
        public Stock Stock { get; set; } = null!;

        // Optionally add a constructor to ensure proper initialization
        public Portfolio()
        {
            AppUserId = string.Empty;
        }
    }
}
