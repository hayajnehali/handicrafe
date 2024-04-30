using System;
using System.Collections.Generic;

#nullable disable

namespace handicrafe.Models
{
    public partial class FeedbackHandicraft
    {
        public decimal IdFeedback { get; set; }
        public string Message { get; set; }
        public decimal Id { get; set; }

        public virtual UserInfo IdNavigation { get; set; }
    }
}
