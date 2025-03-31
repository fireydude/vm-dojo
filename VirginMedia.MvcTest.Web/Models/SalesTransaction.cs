namespace VirginMedia.MvcTest.Web.Models;

public class SalesTransaction
{
  public string Segment { get; set; } 
  public string Country { get; set; } 
  public string Product { get; set; } 
  public string DiscountBand { get; set; }
  public string UnitsSold { get; set; }
  public string ManufacturingPrice { get; set; }
  public string SalePrice { get; set; } 
  public string Date { get; set; }  
}