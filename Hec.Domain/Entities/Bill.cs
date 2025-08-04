using System;
using Hec.Integrations;
using Newtonsoft.Json;

namespace Hec.Entities
{
    public class Bill : Entity
    {
        public Guid ContractAccountId { get; set; }
        [JsonIgnore] public virtual ContractAccount ContractAccount { get; set; }
        public string PrintDocument { get; set; }
        public DateTime PrintDate { get; set; }
        public DateTime PostDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Consumption { get; set; }
        public int MonthYear { get; set; }

        public void CopyBrcmInfo(BillInfo billInfo)
        {
            this.PrintDocument = billInfo.PrintDocument;
            this.PrintDate = billInfo.PrintDate;
            this.PostDate = billInfo.PostDate;
            this.Amount = billInfo.Amount;
            this.Consumption = billInfo.Consumption;
            this.MonthYear = BuildMonthYear(billInfo.PrintDate);
        }

        public static int BuildMonthYear(DateTime dt)
        {
            return dt.Year * 100 + dt.Month;    
        }
    }
}