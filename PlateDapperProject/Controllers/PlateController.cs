using Microsoft.AspNetCore.Mvc;
using PlateDapperProject.PlateRepositories;
using X.PagedList.Extensions;

namespace PlateDapperProject.Controllers
{
    public class PlateController : Controller
    {
        private readonly IPlateRepository _plateRepository;

        public PlateController(IPlateRepository plateRepository)
        {
            _plateRepository = plateRepository;
        }

        public async Task<ActionResult> PlateList(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1; // Eğer page parametresi gelmezse, 1. sayfa açılacak.

            var allPlates = await _plateRepository.GetAllPlateAsync();

            if (!string.IsNullOrEmpty(searchTerm)) //Eğer searchTerm (arama terimi) boş değilse, Where(...) filtresi uygulanır.
            {
                allPlates = allPlates
                    .Where(p => p.Plate.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || //Kullanıcının girdiği searchTerm ifadesi plaka içinde geçiyor mu?
                                (p.Title != null && p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                                (p.Brand != null && p.Brand.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            var pagedPlates = allPlates.ToPagedList(pageNumber, pageSize);

            ViewBag.SearchTerm = searchTerm;
            return View(pagedPlates);
        }
    }
}
