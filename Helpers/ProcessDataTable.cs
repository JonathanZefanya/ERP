using BusinessERP.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusinessERP.Helpers
{
    public class ProcessDataTable<T> : Controller where T : class
    {
        private readonly ApplicationDbContext _context;
        public ProcessDataTable(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult GetDataTabelData(T entity)
        {
            try
            {
                return null;

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
