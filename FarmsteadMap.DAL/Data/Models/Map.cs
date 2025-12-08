using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmsteadMap.DAL.Data.Models
{
    [Table("maps", Schema = "public")]
    public class Map
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        // Important: Map this property to the jsonb column
        [Required]
        [Column("map", TypeName = "jsonb")]
        public string MapJson { get; set; } = "{}";

        [Column("is_Private")]
        public bool IsPrivate { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}