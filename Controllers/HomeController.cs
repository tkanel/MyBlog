using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        //Field for paging size for Posts in Home Page
        public static int pageSize = 10;
        private readonly ILogger<PostsController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager, ILogger<PostsController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            
            var db = _context.Posts.Include(p => p.Blog).Include(p => p.Category).OrderByDescending(p => p.CreatedOn);
            //return View(await postsData.ToListAsync());

            return View(await PaginatedList<Post>.CreateAsync(db.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
