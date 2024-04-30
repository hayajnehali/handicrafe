using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace handicrafe.Models
{
    public partial class Handicraft
    {
        public Handicraft()
        {
            Sales = new HashSet<Sale>();
        }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }


        public decimal IdHandicraft { get; set; }
        public string Name { get; set; }
        public DateTime Datee { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Id { get; set; }
        public string Image { get; set; }

        public virtual UserInfo IdNavigation { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
