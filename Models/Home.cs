using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace handicrafe.Models
{
    public partial class Home
    {
        public decimal Id { get; set; }
        public string Imag { get; set; }
        public string Hometext { get; set; }
        public string Text2 { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile_home { get; set; }

    }

}
