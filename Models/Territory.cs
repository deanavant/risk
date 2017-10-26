using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace risk.Models
{
    public class Territory
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(1)]
        public string name { get; set; }

        [ForeignKey("owner_id")]
        public Player owner { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        [NotMapped]
        public int topLeftX { get; set; }

        [NotMapped]
        public int topLeftY { get; set; }

        [NotMapped]
        public int bottomRightX { get; set; }

        [NotMapped]
        public int bottomRightY { get; set; }

        [NotMapped]
        public List<Territory> neighbors { get; set; }

    }
}
