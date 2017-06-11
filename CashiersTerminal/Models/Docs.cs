using System;
using System.ComponentModel.DataAnnotations;

namespace CashiersTerminal.Models
{
    public class Docs
    {
        [Key]
        public int Id { get; set; }

        public int DocNum { get; set; }
        public int DocQty { get; set; }
        public double DocSum { get; set; }
        public DateTime SaleDate { get; set; }

        public Docs() { }

        public Docs(int docNum, int docQty, double docSum)
        {
            DocNum = docNum;
            DocQty = docQty;
            DocSum = docSum;
            SaleDate = DateTime.Now;
        }
    }
}
