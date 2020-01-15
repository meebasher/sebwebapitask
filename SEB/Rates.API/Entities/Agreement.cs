using Rates.API.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rates.API.Entities
{
    public class Agreement
    {
        [Key]       
        public ulong Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public string BaseRateCode { get; set; }
        public string NewBaseRateCode { get; set; }

        [Required]
        [Editable(false)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Margin { get; set; }

        [Required]
        public int Duration { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        public ulong UserId { get; set; }
    }
}
