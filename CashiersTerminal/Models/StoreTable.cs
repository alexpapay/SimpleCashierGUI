using System.ComponentModel.DataAnnotations;

namespace CashiersTerminal.Models
{
    public class StoreTable
    {
        [Key]
        public int Id { get; set; }

        public string GoodName { get; set; }
        public double Cost { get; set; }
        public int Qty { get; set; }
        public int Code { get; set; }

        public StoreTable() { }
        
        public StoreTable(int id, string goodName, double cost, int qty, int code)
        {
            Id = id;
            GoodName = goodName;
            Cost = cost;
            Qty = qty;
            Code = code;
        }
    }
}
