using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection1101.Models
{

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public DateTime CreationAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }



}
