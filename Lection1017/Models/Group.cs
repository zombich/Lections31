using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lection1017.Models
{
    [Table("StudentsGroups")]
    public partial class Group
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"[а-я]{2,}\-\d{2}")]
        [Column("GroupTitle")]
        public required string Title { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    }
}
