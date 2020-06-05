using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class TransferViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount   { get; set; }

        [Required]        
        public int CheckingAccountId { get; set; }
        public string ReturnUrl { get; set; }

        [Required]
        [Display(Name = " To Account #")]
        public string DestinationAccount { get; set; }

    }
}