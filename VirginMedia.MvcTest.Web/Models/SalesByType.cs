namespace VirginMedia.MvcTest.Web.Models;

public class SaleByType
{
  public string Title { get; set; }
  public Func<SalesTransaction, decimal> GetSum { get; set; } = null;
  public Func<SalesTransaction, string> GetName { get; set; } = null;
  public IEnumerable<SalesTransaction> SalesTransactions { get; set; } = Enumerable.Empty<SalesTransaction>();
}