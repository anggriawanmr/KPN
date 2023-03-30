using KPN.Data;
using KPN.Models;
using KPN.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPN.Controllers
{
    public class CatsController : Controller
    {
        private readonly MVCDBContext mvcDbContext;

        public CatsController(MVCDBContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new AddCatViewModel();
            var cats = await mvcDbContext.Cats.ToListAsync();

            // Cat All
            viewModel.Cats = await mvcDbContext.Cats.ToListAsync();

            // Cat Gender Type
            viewModel.CatGenderTypeCounts = cats.GroupBy(c => new { c.Type, c.Gender })
                                                .Select(g => new CatGenderTypeCount
                                                {
                                                    Type = g.Key.Type,
                                                    Gender = g.Key.Gender,
                                                    Count = g.Count()
                                                })
                                                .ToList();

            // Cat Gender Type Pivot
            viewModel.PivotTable = cats.GroupBy(c => c.Gender)
                                        .Select(g => new CatPivotTableRow
                                        {
                                            Gender = g.Key,
                                            Persian = g.Count(c => c.Type == "Persian"),
                                            Mainecoon = g.Count(c => c.Type == "Mainecoon"),
                                            Sphynx = g.Count(c => c.Type == "Sphynx")
                                        })
                                        .ToList();

            // Cat Food Type 
            viewModel.CatFoodAmounts = cats.GroupBy(c => new { c.Type, c.Food })
                                            .Select(g => new CatFoodAmount
                                            {
                                                Type = g.Key.Type,
                                                Food = g.Key.Food,
                                                Amount = g.Sum(c => c.Amount)
                                            })
                                            .ToList();

            // Cat Food Type Pivot
            viewModel.CatFoodPivotTable = cats.GroupBy(c => c.Food)
                                                    .Select(g => new CatFoodPivotTableRow
                                                    {
                                                        Food = g.Key,
                                                        Persian = g.Where(c => c.Type == "Persian").Sum(c => c.Amount),
                                                        Mainecoon = g.Where(c => c.Type == "Mainecoon").Sum(c => c.Amount),
                                                        Sphynx = g.Where(c => c.Type == "Sphynx").Sum(c => c.Amount)
                                                    })
                                                    .ToList();

            viewModel.GuildId = 1;

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddCatViewModel addCatRequest) {
            var cat = new Cat()
            {
                Id = Guid.NewGuid(),
                Name = addCatRequest.Name,
                Gender = addCatRequest.Gender,
                Type = addCatRequest.Type,
                Colour = addCatRequest.Colour,
                Food = addCatRequest.Food,
                Amount = addCatRequest.Amount,
            };

            await mvcDbContext.Cats.AddAsync(cat);
            await mvcDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var cat = await mvcDbContext.Cats.FirstOrDefaultAsync(x => x.Id == id);

            if(cat !=null)
            {
                var viewModel = new UpdateCatViewModel()
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Gender = cat.Gender,
                    Type = cat.Type,
                    Colour = cat.Colour,
                    Food = cat.Food,
                    Amount = cat.Amount,
                };

                return await Task.Run(() => View("View", viewModel));

            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateCatViewModel model)
        {
            var cat = await mvcDbContext.Cats.FindAsync(model.Id);

            if(cat != null)
            {
                cat.Name = model.Name;
                cat.Gender = model.Gender;
                cat.Type = model.Type;
                cat.Colour = model.Colour;
                cat.Food = model.Food;
                cat.Amount = model.Amount;

                await mvcDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCatViewModel model)
        {
            var cat = await mvcDbContext.Cats.FindAsync(model.Id);

            if(cat != null)
            {
                mvcDbContext.Cats.Remove(cat);
                await mvcDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }



    }
}
