using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    public class BaseController : Controller
    {
        [HttpPost]
        public JsonResult GetDataTablesProcessData<T>(IQueryable<T> data, IQueryable<T> _ApplySearchOnData, string searchValue) where T : class
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = GetSortColumnName();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

                int pageSize = ParseInt(length);
                int skip = ParseInt(start);
                int resultTotal;

                var gridItems = data;

                // Sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnAscDesc))
                {
                    gridItems = ApplySorting(gridItems, sortColumn, sortColumnAscDesc);
                }

                // Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    gridItems = _ApplySearchOnData;
                }

                resultTotal = gridItems.Count();

                var result = gridItems.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortDirection))
            {
                return query;
            }

            return query.OrderBy($"{sortColumn} {sortDirection}");
        }
        private string GetSortColumnName()
        {
            var columnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            return Request.Form[$"columns[{columnIndex}][name]"].FirstOrDefault();
        }

        private int ParseInt(string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);
        }
    }
}
