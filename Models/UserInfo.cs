using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace handicrafe.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            FeedbackHandicrafts = new HashSet<FeedbackHandicraft>();
            Handicrafts = new HashSet<Handicraft>();
            Sales = new HashSet<Sale>();
            Testimonials = new HashSet<Testimonial>();
            VisaCards = new HashSet<VisaCard>();
        }


        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }


        public decimal Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public DateTime RegisteringDate { get; set; }
        public string Image { get; set; }
        public string Testimonial { get; set; }

        public virtual ICollection<FeedbackHandicraft> FeedbackHandicrafts { get; set; }
        public virtual ICollection<Handicraft> Handicrafts { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
        public virtual ICollection<VisaCard> VisaCards { get; set; }
    }
}
