using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.ViewModels;

namespace MyBlog.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager, ILogger<PostsController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {



            var applicationDbContext = _context.Posts.Include(p => p.Blog).Include(p => p.Category).OrderByDescending(p => p.CreatedOn);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            ViewData["CreatedOn"] = DateTime.Now;

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.MyUser = user.UserName;


            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel post)
        {
            if (ModelState.IsValid)
            {
                string uniqueFeaturePhotoName = null;
                string uniqueFileName = null;


                //Attachement

                if (post.AttachmentName != null)
                {
                    //copy file to Attachments folder
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Attachments");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + post.AttachmentName.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    post.AttachmentName.CopyTo(new FileStream(filePath, FileMode.Create));

                }
                else
                {

                    uniqueFileName = "no attachement";
                }

                //Feature photo

                if (post.FeauturePhotoName != null)
                {
                    //copy feature photo to Media folder
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Media");
                    uniqueFeaturePhotoName = Guid.NewGuid().ToString() + "_" + post.FeauturePhotoName.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFeaturePhotoName);
                    post.FeauturePhotoName.CopyTo(new FileStream(filePath, FileMode.Create));

                }
                else
                {

                    uniqueFeaturePhotoName = "no feature photo";
                }



                Post newPost = new Post()
                {

                    Title = post.Title,
                    Body = post.Body,
                    CreatedOn = post.CreatedOn,
                    UnPublishOn = post.UnPublishOn,
                    Author = post.Author,
                    AttachmentName = uniqueFileName,
                    FeauturePhotoName = uniqueFeaturePhotoName,
                    CategoryId = post.CategoryId,
                    BlogId = post.BlogId



                };


                _context.Add(newPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "Name", post.BlogId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var viewModelPost = new PostCreateViewModel()
            {
                PostId = post.PostId,
                Title = post.Title,
                Body = post.Body,
                CreatedOn = post.CreatedOn,
                UnPublishOn = post.UnPublishOn,
                Author = post.Author,
                CurrentAttachmentName = post.AttachmentName,
                CurrentFeaturePhotoName = post.FeauturePhotoName,
                BlogId = post.BlogId,
                Blog = post.Blog,
                CategoryId=post.CategoryId,
                Category=post.Category



            };


            ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "Name", post.BlogId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", post.CategoryId);


            return View(viewModelPost);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Body,CreatedOn,UnPublishOn,Author,AttachmentName,FeauturePhotoName,CategoryId,BlogId,AttachmentName")] PostCreateViewModel post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string uniqueFeaturePhotoName = null;
                    string uniqueFileName = null;


                    //Attachement

                    if (post.AttachmentName != null)
                    {
                        //copy file to Attachments folder
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Attachments");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + post.AttachmentName.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        post.AttachmentName.CopyTo(new FileStream(filePath, FileMode.Create));

                    }
                    else
                    {

                        uniqueFileName = "no attachement";
                    }

                    //Feature photo

                    if (post.FeauturePhotoName != null)
                    {
                        //copy feature photo to Media folder
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "Media");
                        uniqueFeaturePhotoName = Guid.NewGuid().ToString() + "_" + post.FeauturePhotoName.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFeaturePhotoName);
                        post.FeauturePhotoName.CopyTo(new FileStream(filePath, FileMode.Create));

                    }
                    else
                    {

                        uniqueFeaturePhotoName = "no feature photo";
                    }



                    Post newPost = new Post()
                    {

                        Title = post.Title,
                        Body = post.Body,
                        CreatedOn = post.CreatedOn,
                        UnPublishOn = post.UnPublishOn,
                        Author = post.Author,
                        AttachmentName = uniqueFileName,
                        FeauturePhotoName = uniqueFeaturePhotoName,
                        CategoryId = post.CategoryId,
                        BlogId = post.BlogId



                    };


                    _context.Update(post);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return View(post);
                    }

                    
                }
                
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "Name", post.BlogId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", post.CategoryId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
