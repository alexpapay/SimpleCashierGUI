using System;
using System.ComponentModel.DataAnnotations;

namespace CashiersTerminal.Models
{
    public class SalesTable
    {
        [Key]
        public int Id { get; set; }

        public string GoodName { get; set; }
        public double Cost { get; set; }
        public int Qty { get; set; }
        public double CostSum { get; set; }
        public int Code { get; set; }
        public int DocNum { get; set; }
        public DateTime SaleDate { get; set; }

        public SalesTable() { }

        public SalesTable(string goodName, double cost, int qty, double costSum, int code, int docNum)
        {
            GoodName = goodName;
            Cost = cost;
            Qty = qty;
            CostSum = costSum;
            DocNum = docNum;
            Code = code;
            SaleDate = DateTime.Now;
        }
    }
}
