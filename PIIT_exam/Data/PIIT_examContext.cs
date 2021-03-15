using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PIIT_exam.Data
{
    public class PIIT_examContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PIIT_examContext() : base("name=PIIT_examContext")
        {
        }

        public System.Data.Entity.DbSet<PIIT_exam.Models.Account> Accounts { get; set; }
        public System.Data.Entity.DbSet<PIIT_exam.Models.VerificationCode> VerificationCodes { get; set; }
    }
}
