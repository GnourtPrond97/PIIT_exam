using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIIT_exam.Models
{
    public class VerificationCode
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public string Code { get; set; }
    }
}