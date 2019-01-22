namespace PersonalFinances.Models.ViewModels
{
    public class DashboardViewModel
    {
        public double TotalBalance { get; set; }
        public double BalanceOnMonth { get; set; }
        public int Movements { get; set; }
        public int Accounts { get; set; }
    }
}