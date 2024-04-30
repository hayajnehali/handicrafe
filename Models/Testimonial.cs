using System;
using System.Collections.Generic;

#nullable disable

namespace handicrafe.Models
{
    public partial class Testimonial
    {
        public decimal IdTestimonial { get; set; }
        public string Text { get; set; }
        public decimal? Id { get; set; }
        public string Acceptt { get; set; }

        public virtual UserInfo IdNavigation { get; set; }
    }
}
