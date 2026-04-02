using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessERP.Data;
using BusinessERP.Models.DamageItemDeatilsViewModel;
using Microsoft.AspNetCore.Authorization;

namespace BusinessERP.Controllers
{
    [Authorize]
    public class DamageItemReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DamageItemReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DamageItemReport/Index
        public async Task<IActionResult> Index()
        {
            ViewBag.Suppliers = await GetSupplierList();
            ViewBag.Items = await GetItemList();

            var model = new DamageItemReportViewModel
            {
                FromDate = DateTime.Now.AddMonths(-1),
                ToDate = DateTime.Now,
                ReportType = "Detail"
            };

            return View(model);
        }

        // POST: DamageItemReport/GenerateReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReport(DamageItemReportViewModel model)
        {
            try
            {
                // Build base query
                var query = from damage in _context.DamageItemDeatils
                            join item in _context.Items on damage.ItemId equals item.Id
                            join supplier in _context.Supplier on item.SupplierId equals supplier.Id
                            where damage.Cancelled == false
                            select new
                            {
                                Damage = damage,
                                Item = item,
                                Supplier = supplier
                            };

                // Apply date filters
                if (model.FromDate.HasValue)
                {
                    query = query.Where(x => x.Damage.CreatedDate >= model.FromDate.Value);
                }

                if (model.ToDate.HasValue)
                {
                    var toDate = model.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.Damage.CreatedDate <= toDate);
                }

                // Apply supplier filter
                if (model.SupplierId.HasValue && model.SupplierId.Value > 0)
                {
                    query = query.Where(x => x.Supplier.Id == model.SupplierId.Value);
                }

                // Apply item filter
                if (model.ItemId.HasValue && model.ItemId.Value > 0)
                {
                    query = query.Where(x => x.Item.Id == model.ItemId.Value);
                }

                var data = await query.OrderByDescending(x => x.Damage.CreatedDate).ToListAsync();

                if (!data.Any())
                {
                    TempData["info"] = "No data found for the selected criteria.";
                    ViewBag.Suppliers = await GetSupplierList();
                    ViewBag.Items = await GetItemList();
                    return View("Index", model);
                }

                // Get adjustments for all damage items
                var damageIds = data.Select(x => x.Damage.Id).ToList();
                var adjustments = await _context.DamageItemAdjustments
                    .Where(x => damageIds.Contains(x.DamageItemId) && x.Cancelled == false)
                    .ToListAsync();

                // Build report data
                var reportData = data.Select(x =>
                {
                    var itemAdjustments = adjustments.Where(a => a.DamageItemId == x.Damage.Id).ToList();
                    var totalAdjusted = itemAdjustments.Sum(a => a.AdjustmentQuantity);
                    var remaining = x.Damage.TotalDamageItem - totalAdjusted;
                    var percentage = x.Damage.TotalDamageItem > 0
                        ? Math.Round((decimal)totalAdjusted / x.Damage.TotalDamageItem * 100, 1)
                        : 0;

                    return new DamageItemReportDetailViewModel
                    {
                        DamageItemId = x.Damage.Id,
                        DamageDate = x.Damage.CreatedDate,
                        ItemName = x.Item.Name,
                        ItemCode = x.Item.Code,
                        SupplierName = x.Supplier.Name,
                        TotalDamaged = x.Damage.TotalDamageItem,
                        TotalAdjusted = totalAdjusted,
                        Remaining = remaining,
                        AdjustmentPercentage = percentage,
                        Status = totalAdjusted == 0 ? "Not Adjusted"
                            : (totalAdjusted >= x.Damage.TotalDamageItem ? "Fully Adjusted" : "Partially Adjusted"),
                        ReasonOfDamage = x.Damage.ReasonOfDamage,
                        CreatedBy = x.Damage.CreatedBy,
                        Adjustments = itemAdjustments.Select(a => new AdjustmentDetailViewModel
                        {
                            AdjustmentId = a.Id,
                            AdjustmentDate = a.AdjustmentDate,
                            Quantity = a.AdjustmentQuantity,
                            Reason = a.AdjustmentReason,
                            CreatedBy = a.CreatedBy
                        }).OrderByDescending(a => a.AdjustmentDate).ToList()
                    };
                }).ToList();

                // Calculate summary
                var summary = CalculateSummary(reportData);

                model.ReportData = reportData;
                model.Summary = summary;

                ViewBag.Suppliers = await GetSupplierList();
                ViewBag.Items = await GetItemList();

                TempData["success"] = $"Report generated successfully! Found {reportData.Count} records.";
                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error generating report: " + ex.Message;
                ViewBag.Suppliers = await GetSupplierList();
                ViewBag.Items = await GetItemList();
                return View("Index", model);
            }
        }

        // GET: DamageItemReport/DetailReport
        public async Task<IActionResult> DetailReport(long id)
        {
            try
            {
                var damageItem = await (from damage in _context.DamageItemDeatils
                                       join item in _context.Items on damage.ItemId equals item.Id
                                       join supplier in _context.Supplier on item.SupplierId equals supplier.Id
                                       where damage.Id == id && damage.Cancelled == false
                                       select new
                                       {
                                           Damage = damage,
                                           Item = item,
                                           Supplier = supplier
                                       }).FirstOrDefaultAsync();

                if (damageItem == null)
                {
                    return NotFound();
                }

                var adjustments = await _context.DamageItemAdjustments
                    .Where(x => x.DamageItemId == id && x.Cancelled == false)
                    .OrderByDescending(x => x.AdjustmentDate)
                    .ToListAsync();

                var totalAdjusted = adjustments.Sum(a => a.AdjustmentQuantity);
                var remaining = damageItem.Damage.TotalDamageItem - totalAdjusted;
                var percentage = damageItem.Damage.TotalDamageItem > 0
                    ? Math.Round((decimal)totalAdjusted / damageItem.Damage.TotalDamageItem * 100, 1)
                    : 0;

                var model = new DamageItemReportDetailViewModel
                {
                    DamageItemId = damageItem.Damage.Id,
                    DamageDate = damageItem.Damage.CreatedDate,
                    ItemName = damageItem.Item.Name,
                    ItemCode = damageItem.Item.Code,
                    SupplierName = damageItem.Supplier.Name,
                    TotalDamaged = damageItem.Damage.TotalDamageItem,
                    TotalAdjusted = totalAdjusted,
                    Remaining = remaining,
                    AdjustmentPercentage = percentage,
                    Status = totalAdjusted == 0 ? "Not Adjusted"
                        : (totalAdjusted >= damageItem.Damage.TotalDamageItem ? "Fully Adjusted" : "Partially Adjusted"),
                    ReasonOfDamage = damageItem.Damage.ReasonOfDamage,
                    CreatedBy = damageItem.Damage.CreatedBy,
                    Adjustments = adjustments.Select(a => new AdjustmentDetailViewModel
                    {
                        AdjustmentId = a.Id,
                        AdjustmentDate = a.AdjustmentDate,
                        Quantity = a.AdjustmentQuantity,
                        Reason = a.AdjustmentReason,
                        CreatedBy = a.CreatedBy
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error loading detail report: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DamageItemReport/SummaryReport
        public async Task<IActionResult> SummaryReport(DateTime? fromDate, DateTime? toDate, int? supplierId, int? itemId)
        {
            try
            {
                var query = from damage in _context.DamageItemDeatils
                            join item in _context.Items on damage.ItemId equals item.Id
                            join supplier in _context.Supplier on item.SupplierId equals supplier.Id
                            where damage.Cancelled == false
                            select new
                            {
                                Damage = damage,
                                Item = item,
                                Supplier = supplier
                            };

                if (fromDate.HasValue)
                {
                    query = query.Where(x => x.Damage.CreatedDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    var toDateEnd = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.Damage.CreatedDate <= toDateEnd);
                }

                if (supplierId.HasValue && supplierId.Value > 0)
                {
                    query = query.Where(x => x.Supplier.Id == supplierId.Value);
                }

                if (itemId.HasValue && itemId.Value > 0)
                {
                    query = query.Where(x => x.Item.Id == itemId.Value);
                }

                var data = await query.ToListAsync();
                var damageIds = data.Select(x => x.Damage.Id).ToList();
                var adjustments = await _context.DamageItemAdjustments
                    .Where(x => damageIds.Contains(x.DamageItemId) && x.Cancelled == false)
                    .ToListAsync();

                var reportData = data.Select(x =>
                {
                    var totalAdjusted = adjustments.Where(a => a.DamageItemId == x.Damage.Id).Sum(a => a.AdjustmentQuantity);
                    var remaining = x.Damage.TotalDamageItem - totalAdjusted;
                    var percentage = x.Damage.TotalDamageItem > 0
                        ? Math.Round((decimal)totalAdjusted / x.Damage.TotalDamageItem * 100, 1)
                        : 0;

                    return new DamageItemReportDetailViewModel
                    {
                        DamageItemId = x.Damage.Id,
                        DamageDate = x.Damage.CreatedDate,
                        ItemName = x.Item.Name,
                        SupplierName = x.Supplier.Name,
                        TotalDamaged = x.Damage.TotalDamageItem,
                        TotalAdjusted = totalAdjusted,
                        Remaining = remaining,
                        AdjustmentPercentage = percentage,
                        Status = totalAdjusted == 0 ? "Not Adjusted"
                            : (totalAdjusted >= x.Damage.TotalDamageItem ? "Fully Adjusted" : "Partially Adjusted")
                    };
                }).ToList();

                var summary = CalculateSummary(reportData);

                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.SupplierId = supplierId;
                ViewBag.ItemId = itemId;

                return View(summary);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error generating summary report: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DamageItemReport/ExportToExcel
        [HttpPost]
        public async Task<IActionResult> ExportToExcel(DamageItemReportViewModel model)
        {
            try
            {
                // Generate report data
                var reportData = await GenerateReportDataForExport(model);

                if (reportData == null || !reportData.Any())
                {
                    TempData["warning"] = "No data available to export.";
                    return RedirectToAction(nameof(Index));
                }

                // Create CSV content (simple implementation)
                var csv = new System.Text.StringBuilder();
                
                // Add headers
                csv.AppendLine("Damage Item Report");
                csv.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                csv.AppendLine($"Period: {model.FromDate?.ToString("yyyy-MM-dd")} to {model.ToDate?.ToString("yyyy-MM-dd")}");
                csv.AppendLine();
                
                // Add column headers
                csv.AppendLine("ID,Date,Item,Item Code,Supplier,Total Damaged,Total Adjusted,Remaining,Adjustment %,Status,Reason,Created By");
                
                // Add data rows
                foreach (var item in reportData)
                {
                    csv.AppendLine($"{item.DamageItemId}," +
                                  $"{item.DamageDate:yyyy-MM-dd}," +
                                  $"\"{item.ItemName}\"," +
                                  $"\"{item.ItemCode}\"," +
                                  $"\"{item.SupplierName}\"," +
                                  $"{item.TotalDamaged}," +
                                  $"{item.TotalAdjusted}," +
                                  $"{item.Remaining}," +
                                  $"{item.AdjustmentPercentage}," +
                                  $"\"{item.Status}\"," +
                                  $"\"{item.ReasonOfDamage?.Replace("\"", "\"\"")}\"," +
                                  $"\"{item.CreatedBy}\"");
                }

                var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
                var fileName = $"DamageItemReport_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                return File(bytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error exporting report: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DamageItemReport/PrintReport
        [HttpPost]
        public async Task<IActionResult> PrintReport(DamageItemReportViewModel model)
        {
            try
            {
                var reportData = await GenerateReportDataForExport(model);
                
                if (reportData == null || !reportData.Any())
                {
                    TempData["warning"] = "No data available to print.";
                    return RedirectToAction(nameof(Index));
                }

                model.ReportData = reportData;
                model.Summary = CalculateSummary(reportData);

                return View("PrintReport", model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error preparing print report: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        #region Helper Methods

        private async Task<SelectList> GetSupplierList()
        {
            var suppliers = await _context.Supplier
                .Where(x => x.Cancelled == false)
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            return new SelectList(suppliers, "Id", "Name");
        }

        private async Task<SelectList> GetItemList()
        {
            var items = await _context.Items
                .Where(x => x.Cancelled == false)
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            return new SelectList(items, "Id", "Name");
        }

        private DamageItemReportSummaryViewModel CalculateSummary(List<DamageItemReportDetailViewModel> reportData)
        {
            var summary = new DamageItemReportSummaryViewModel
            {
                TotalDamageRecords = reportData.Count,
                TotalItemsDamaged = reportData.Sum(x => x.TotalDamaged),
                TotalItemsAdjusted = reportData.Sum(x => x.TotalAdjusted),
                TotalItemsRemaining = reportData.Sum(x => x.Remaining),
                FullyAdjustedCount = reportData.Count(x => x.Status == "Fully Adjusted"),
                PartiallyAdjustedCount = reportData.Count(x => x.Status == "Partially Adjusted"),
                NotAdjustedCount = reportData.Count(x => x.Status == "Not Adjusted"),

                // Supplier summaries
                SupplierSummaries = reportData
                    .GroupBy(x => x.SupplierName)
                    .Select(g => new SupplierSummaryViewModel
                    {
                        SupplierName = g.Key,
                        TotalDamaged = g.Sum(x => x.TotalDamaged),
                        TotalAdjusted = g.Sum(x => x.TotalAdjusted),
                        Remaining = g.Sum(x => x.Remaining)
                    })
                    .OrderByDescending(x => x.TotalDamaged)
                    .ToList(),

                // Product summaries
                ProductSummaries = reportData
                    .GroupBy(x => x.ItemName)
                    .Select(g => new ProductSummaryViewModel
                    {
                        ItemName = g.Key,
                        TotalDamaged = g.Sum(x => x.TotalDamaged),
                        TotalAdjusted = g.Sum(x => x.TotalAdjusted),
                        Remaining = g.Sum(x => x.Remaining)
                    })
                    .OrderByDescending(x => x.TotalDamaged)
                    .ToList()
            };

            if (summary.TotalItemsDamaged > 0)
            {
                summary.TotalAdjustmentPercentage = Math.Round(
                    (decimal)summary.TotalItemsAdjusted / summary.TotalItemsDamaged * 100, 1);
            }

            return summary;
        }

        private async Task<List<DamageItemReportDetailViewModel>> GenerateReportDataForExport(DamageItemReportViewModel model)
        {
            var query = from damage in _context.DamageItemDeatils
                        join item in _context.Items on damage.ItemId equals item.Id
                        join supplier in _context.Supplier on item.SupplierId equals supplier.Id
                        where damage.Cancelled == false
                        select new
                        {
                            Damage = damage,
                            Item = item,
                            Supplier = supplier
                        };

            if (model.FromDate.HasValue)
            {
                query = query.Where(x => x.Damage.CreatedDate >= model.FromDate.Value);
            }

            if (model.ToDate.HasValue)
            {
                var toDate = model.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.Damage.CreatedDate <= toDate);
            }

            if (model.SupplierId.HasValue && model.SupplierId.Value > 0)
            {
                query = query.Where(x => x.Supplier.Id == model.SupplierId.Value);
            }

            if (model.ItemId.HasValue && model.ItemId.Value > 0)
            {
                query = query.Where(x => x.Item.Id == model.ItemId.Value);
            }

            var data = await query.OrderByDescending(x => x.Damage.CreatedDate).ToListAsync();

            if (!data.Any())
            {
                return new List<DamageItemReportDetailViewModel>();
            }

            var damageIds = data.Select(x => x.Damage.Id).ToList();
            var adjustments = await _context.DamageItemAdjustments
                .Where(x => damageIds.Contains(x.DamageItemId) && x.Cancelled == false)
                .ToListAsync();

            var reportData = data.Select(x =>
            {
                var totalAdjusted = adjustments.Where(a => a.DamageItemId == x.Damage.Id).Sum(a => a.AdjustmentQuantity);
                var remaining = x.Damage.TotalDamageItem - totalAdjusted;
                var percentage = x.Damage.TotalDamageItem > 0
                    ? Math.Round((decimal)totalAdjusted / x.Damage.TotalDamageItem * 100, 1)
                    : 0;

                return new DamageItemReportDetailViewModel
                {
                    DamageItemId = x.Damage.Id,
                    DamageDate = x.Damage.CreatedDate,
                    ItemName = x.Item.Name,
                    ItemCode = x.Item.Code,
                    SupplierName = x.Supplier.Name,
                    TotalDamaged = x.Damage.TotalDamageItem,
                    TotalAdjusted = totalAdjusted,
                    Remaining = remaining,
                    AdjustmentPercentage = percentage,
                    Status = totalAdjusted == 0 ? "Not Adjusted"
                        : (totalAdjusted >= x.Damage.TotalDamageItem ? "Fully Adjusted" : "Partially Adjusted"),
                    ReasonOfDamage = x.Damage.ReasonOfDamage,
                    CreatedBy = x.Damage.CreatedBy
                };
            }).ToList();

            return reportData;
        }

        #endregion
    }
}
