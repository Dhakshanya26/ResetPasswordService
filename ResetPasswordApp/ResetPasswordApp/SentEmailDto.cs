using System;
using System.Collections.Generic;
using System.Text;

namespace ResetPasswordApp
{
    public class SentEmailDto
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public DateTime SentOn { get; set; }
        public string EmailXml { get; set; }
        public bool IsSentSuccessful { get; set; }
        public string FailureReason { get; set; }
    }
}
