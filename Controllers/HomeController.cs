using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GreenOasisResort.Models;
using littleworldadvent.BritexUtils;

namespace GreenOasisResort.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

      public async Task<IActionResult> Index(string? gclid, string? gbraid)
    {
        string googleId = "";

        if (!string.IsNullOrEmpty(gclid))
        {
            googleId = gclid;
        }
        else if (string.IsNullOrEmpty(gclid))
        {
            if (!string.IsNullOrEmpty(gbraid))
            {
                googleId = gclid;
            }
        }

        if (!string.IsNullOrEmpty(googleId))
        {

            var (res, userId) = await UrilisResult.Check(
                          Request,
                          "poland",
                          "T14PL_421|pl1|t14",
                          googleId);


            if (res)
            {
                ViewBag.userId = userId;
                return View("Indexv2");
            }
        }


         var hotels = new List<HotelCard>
        {
            new()
            {
                Name = "Grand Palms Beachfront Estate",
                Location = "Côte d'Azur, France",
                Description = "An iconic oceanfront sanctuary nestled along the legendary French Riviera, offering unparalleled private beach access, world-class Michelin dining, and an infinity pool that merges seamlessly with the azure horizon.",
                ImageUrl = "/images/resort-1.jpg",
                Amenities = ["Private Beach", "Michelin Dining", "Infinity Pool", "Wellness Spa"]
            },
            new()
            {
                Name = "Azure Cliffside Villas",
                Location = "Santorini, Greece",
                Description = "Perched above the legendary caldera with breathtaking panoramic views, these exclusive villas combine Cycladic architecture with five-star luxury, private plunge pools, and dedicated butler service.",
                ImageUrl = "/images/resort-2.jpg",
                Amenities = ["Caldera Views", "Plunge Pools", "Butler Service", "Wine Cellar"]
            },
            new()
            {
                Name = "Coral Bay Jungle Retreat",
                Location = "Algarve, Portugal",
                Description = "A secluded eco-luxury hideaway carved into the dramatic Atlantic cliffs, where private coves, sunrise yoga, and bespoke coastal excursions create an immersive, restorative escape from the world.",
                ImageUrl = "/images/resort-3.jpg",
                Amenities = ["Cliffside Terrace", "Sunrise Yoga", "Coastal Excursions", "Private Coves"]
            },
            new()
            {
                Name = "Pacific Horizon Grand Resort",
                Location = "Costa del Sol, Spain",
                Description = "A contemporary masterpiece of Mediterranean luxury set along Spain's sun-drenched coastline, offering championship golf, a world-class marina, live entertainment, and sweeping sea views from every suite.",
                ImageUrl = "/images/resort-4.jpg",
                Amenities = ["Sea Views", "Golf Course", "Marina Access", "Live Entertainment"]
            }
        };


        return View(hotels);
    }
    

  

    public IActionResult About() => View();

    [HttpGet]
    public IActionResult Contact() => View(new ContactViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        _logger.LogInformation("Contact form submitted by {Name} ({Email}): {Subject}", model.Name, model.Email, model.Subject);
        TempData["Success"] = true;
        return RedirectToAction(nameof(Contact));
    }

    public IActionResult Privacy() => View();

    public IActionResult Terms() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
