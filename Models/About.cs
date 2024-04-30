using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace handicrafe.Models
{
    public partial class About
    {
        public decimal Id { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }


    }
}
