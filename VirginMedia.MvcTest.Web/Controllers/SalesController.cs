using System.Diagnostics;
using System.Reflection;
using System.Text;
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

    public async Task<IActionResult> Index(string sortOrder, int? page)
    {
        var assembly = typeof(SalesController).GetTypeInfo().Assembly;
        var stream = assembly.GetManifestResourceStream("VirginMedia.MvcTest.Web.Data.csv");
        var reader = new StreamReader(stream!, Encoding.ASCII);
        string csvData = await reader.ReadToEndAsync();

        ViewBag.SegmentSortParm = string.IsNullOrEmpty(sortOrder) ? "seg_desc" : "";
        ViewBag.CountrySortParm = sortOrder == "c" ? "c_desc" : "c";
        ViewBag.ProductSortParm = sortOrder == "p" ? "p_desc" : "p";
        ViewBag.DiscountBandSortParm = sortOrder == "dc" ? "dc_desc" : "dc";
        ViewBag.UnitsSoldSortParm = sortOrder == "us" ? "us_desc" : "us";
        ViewBag.ManufacturingPriceSortParm = sortOrder == "mp" ? "mp_desc" : "mp";
        ViewBag.SalePriceSortParm = sortOrder == "sp" ? "sp_desc" : "sp";
        ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

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
        foreach (var line in lines.Skip(1).Where(x => !string.IsNullOrEmpty(x)))
        {
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
                    UnitsSold = decimal.Parse(columns[4].Replace(" ", "")),
                    ManufacturingPrice = string.IsNullOrEmpty(columns[5]) ? 0 : decimal.Parse(columns[5].Substring(1)),
                    SalePrice = string.IsNullOrEmpty(columns[6]) ? 0 : decimal.Parse(columns[6].Substring(1)),
                    Date = columns[7],
                };
                data.Add(transaction);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message + "<" + line + ">";
                break;
            }
        }

        var ordered = sortOrder switch
        {
            "seg_desc" => data.OrderByDescending(s => s.Segment),
            "c" => data.OrderBy(s => s.Country),
            "c_desc" => data.OrderByDescending(s => s.Country),
            "p" => data.OrderBy(s => s.Product),
            "p_desc" => data.OrderByDescending(s => s.Product),
            "dc" => data.OrderBy(s => s.DiscountBand),
            "dc_desc" => data.OrderByDescending(s => s.DiscountBand),
            "us" => data.OrderBy(s => s.UnitsSold),
            "us_desc" => data.OrderByDescending(s => s.UnitsSold),
            "mp" => data.OrderBy(s => s.ManufacturingPrice),
            "mp_desc" => data.OrderByDescending(s => s.ManufacturingPrice),
            "sp" => data.OrderBy(s => s.SalePrice),
            "sp_desc" => data.OrderByDescending(s => s.SalePrice),
            "Date" => data.OrderBy(s => s.Date),
            "date_desc" => data.OrderByDescending(s => s.Date),
            _ => data.OrderBy(s => s.Segment)
        };

        int pageSize = 10;
        int pageNumber = (page ?? 1);
        return View(new SalesTransactionPage(ordered, sortOrder, pageNumber, pageSize));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
