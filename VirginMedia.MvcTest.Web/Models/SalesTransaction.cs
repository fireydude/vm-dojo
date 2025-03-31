namespace VirginMedia.MvcTest.Web.Models;

public class SalesTransaction
{
  public required string Segment { get; init; } 
  public required string Country { get; init; } 
  public required string Product { get; init; } 
  public required string DiscountBand { get; init; }
  public required decimal UnitsSold { get; init; }
  public required decimal ManufacturingPrice { get; init; }
  public required decimal SalePrice { get; init; } 
  public required string Date { get; init; }  
}

public class SalesTransactionPage
{
    private string _sortOrder;
    private IOrderedEnumerable<SalesTransaction> _salesTransactions;
    private int _pageNumber;
    private int _pageSize;

    public SalesTransactionPage(IOrderedEnumerable<SalesTransaction> salesTransactions, string sortOrder, int pageNumber, int pageSize)    
    {
        _sortOrder = sortOrder;
        _salesTransactions = salesTransactions;
        _pageNumber = pageNumber;
        _pageSize = pageSize;
    }

    public IEnumerable<SalesTransaction> SalesTransactions => _salesTransactions.Skip(_pageNumber * _pageSize).Take(_pageSize);
    public string SortOrder => _sortOrder;
    public int PageNumber => _pageNumber;
    public int PageSize => _pageSize;
    public int PageCount => _salesTransactions.Count() / _pageSize == 0 ? 
      _salesTransactions.Count() / _pageSize - 1: 
      (_salesTransactions.Count() / _pageSize);
}