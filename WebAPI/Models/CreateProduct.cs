using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CreateProduct
    {
        public int ProductId { get; set; }

        public int UserId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string IssueStatus { get; set; }
    }
}
