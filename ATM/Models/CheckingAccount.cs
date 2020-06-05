using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class CheckingAccount
    {
        private ApplicationDbContext context;

        
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        [Column(TypeName="varchar")]
        [RegularExpression(@"\d{6,10}",ErrorMessage ="Account # must be between 6 and 10 digit")]
        [Display(Name="Account #" )]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        public virtual ApplicationUser User { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        internal void CheckingAccounts(string v1, string v2, string id, int v3)
        {
            throw new NotImplementedException();
        }
    }
}