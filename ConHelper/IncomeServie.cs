using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Helpers;
using BusinessERP.Models.IncomeSummaryViewModel;
using Microsoft.EntityFrameworkCore;

namespace BusinessERP.ConHelper
{
    public class IncomeServie : IIncomeServie
    {
        private readonly ApplicationDbContext _context;
        public IncomeServie(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<IncomeSummaryCRUDViewModel> GetAllIncomeSummary()
        {
            try
            {
                var result = (from _IncomeSummary in _context.IncomeSummary
                              join _IncomeType in _context.IncomeType on _IncomeSummary.TypeId equals _IncomeType.Id
                              into _IncomeType
                              from listIncomeType in _IncomeType.DefaultIfEmpty()
                              join _IncomeCategory in _context.IncomeCategory on _IncomeSummary.TypeId equals _IncomeCategory.Id
                               into _IncomeCategory
                              from listIncomeCategory in _IncomeCategory.DefaultIfEmpty()
                              where _IncomeSummary.Cancelled == false
                              select new IncomeSummaryCRUDViewModel
                              {
                                  Id = _IncomeSummary.Id,
                                  Title = _IncomeSummary.Title,
                                  TypeId = _IncomeSummary.TypeId,
                                  TypeDisplay = listIncomeType.Name,
                                  CategoryId = _IncomeSummary.CategoryId,
                                  CategoryDisplay = listIncomeCategory.Name,
                                  Amount = _IncomeSummary.Amount,
                                  Description = _IncomeSummary.Description,
                                  IncomeDate = _IncomeSummary.IncomeDate,
                                  CreatedDate = _IncomeSummary.CreatedDate,
                                  ModifiedDate = _IncomeSummary.ModifiedDate,
                                  CreatedBy = _IncomeSummary.CreatedBy,
                                  ModifiedBy = _IncomeSummary.ModifiedBy,
                                  Cancelled = _IncomeSummary.Cancelled
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<IncomeSummaryCRUDViewModel>> GetAllIncomeSummaryForGridItem(string _StartDate, string _EndDate, Int64 IncomeTypeId, Int64 IncomeCategoryId)
        {
            List<IncomeSummary> listIncomeSummary = new();
            List<IncomeSummaryCRUDViewModel> listIncomeSummaryCRUDViewModel = new();
            try
            {
                if (IncomeTypeId == 0 && IncomeCategoryId == 0)
                {
                    listIncomeSummary = await _context.IncomeSummary.Where(x => x.Cancelled == false).ToListAsync();
                }
                else if (IncomeTypeId == 0 && IncomeCategoryId != 0)
                {
                    listIncomeSummary = await _context.IncomeSummary.Where(x => x.Cancelled == false && x.CategoryId == IncomeCategoryId).ToListAsync();
                }
                else if (IncomeTypeId != 0 && IncomeCategoryId == 0)
                {
                    listIncomeSummary = await _context.IncomeSummary.Where(x => x.Cancelled == false && x.TypeId == IncomeTypeId).ToListAsync();
                }
                else if (IncomeTypeId != 0 && IncomeCategoryId != 0)
                {
                    listIncomeSummary = await _context.IncomeSummary.
                    Where(x => x.Cancelled == false && x.TypeId == IncomeTypeId && x.CategoryId == IncomeCategoryId).ToListAsync();
                }

                if (_StartDate != null && _EndDate != null && _StartDate != "" && _EndDate != "")
                {
                    DateTime StartDate = Convert.ToDateTime(_StartDate).StartOfDay();
                    DateTime EndDate = Convert.ToDateTime(_EndDate).EndOfDay();
                    listIncomeSummary = listIncomeSummary.Where(x => x.CreatedDate >= StartDate && x.CreatedDate <= EndDate).ToList();
                }

                if (listIncomeSummary.Count > 0)
                {
                    listIncomeSummaryCRUDViewModel = (from _IncomeSummary in listIncomeSummary
                                                      join _IncomeType in _context.IncomeType on _IncomeSummary.TypeId equals _IncomeType.Id
                                                      //into _IncomeType
                                                      //from listIncomeType in _IncomeType.DefaultIfEmpty()
                                                      join _IncomeCategory in _context.IncomeCategory on _IncomeSummary.TypeId equals _IncomeCategory.Id
                                                      //into _IncomeCategory
                                                      //from listIncomeCategory in _IncomeCategory.DefaultIfEmpty()
                                                      select new IncomeSummaryCRUDViewModel
                                                      {
                                                          Id = _IncomeSummary.Id,
                                                          Title = _IncomeSummary.Title,
                                                          TypeId = _IncomeSummary.TypeId,
                                                          TypeDisplay = _IncomeType.Name,
                                                          CategoryId = _IncomeSummary.CategoryId,
                                                          CategoryDisplay = _IncomeCategory.Name,
                                                          Amount = _IncomeSummary.Amount,
                                                          Description = _IncomeSummary.Description,
                                                          IncomeDate = _IncomeSummary.IncomeDate,
                                                          CreatedDate = _IncomeSummary.CreatedDate,
                                                          ModifiedDate = _IncomeSummary.ModifiedDate,
                                                          CreatedBy = _IncomeSummary.CreatedBy,
                                                          ModifiedBy = _IncomeSummary.ModifiedBy,
                                                          Cancelled = _IncomeSummary.Cancelled
                                                      }).OrderByDescending(x => x.Id).ToList();

                }

                return listIncomeSummaryCRUDViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
