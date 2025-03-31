using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VirginMedia.MvcTest.Web.Models;

namespace VirginMedia.MvcTest.Web.Controllers;

public class SalesController : Controller
{
    private readonly ILogger<SalesController> _logger;

    public SalesController(ILogger<SalesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var csvData = System.IO.File.ReadAllText(@"/Users/brian/Projects/virgin-test/Data.csv");
        if (string.IsNullOrEmpty(csvData))
        {
            ViewBag.Error = "The input file appears to be empty";
            return View();
        }
        var lines = csvData.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.TrimEntries).Skip(1);
        if (lines.Count() < 3)
        {
            ViewBag.Error = "The input file does not have the correct data";
            return View();
        }
        var data = new List<SalesTransaction>();
        foreach (var line in lines.Skip(1))
        {
            ViewBag.File = line;
            try
            {
                var columns = line.Split(new char[] { ',' }, StringSplitOptions.TrimEntries);
                if (columns.Length == 0)
                { 
                    continue; 
                }
                var transaction = new SalesTransaction
                {
                    Segment = columns[0],
                    Country = columns[1],
                    Product = columns[2],
                    DiscountBand = columns[3],
                    UnitsSold = columns[4],
                    ManufacturingPrice = columns[5],
                    SalePrice = columns[6],
                    Date = columns[7],
                };
                data.Add(transaction);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                break;
            }
        }

        IEnumerable<SalesTransaction> x = data;

        return View(data);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
