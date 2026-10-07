using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookDbContext _context;

        public BooksController(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .Include(b => b.Images)
                .ToListAsync();

            return View(books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, List<IFormFile> imageFiles)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    string folderPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "books"
                    );

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    foreach (var imageFile in imageFiles)
                    {
                        if (imageFile == null || imageFile.Length == 0)
                        {
                            continue;
                        }

                        string extension = Path.GetExtension(imageFile.FileName)
                            .ToLowerInvariant();

                        if (extension != ".jpg" &&
                            extension != ".jpeg" &&
                            extension != ".png")
                        {
                            ModelState.AddModelError(
                                "",
                                "Chỉ được upload file JPG, JPEG hoặc PNG."
                            );

                            return View(book);
                        }

                        string fileName = Guid.NewGuid().ToString() + extension;

                        string filePath = Path.Combine(
                            folderPath,
                            fileName
                        );

                        using (var stream = new FileStream(
                            filePath,
                            FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        BookImage bookImage = new BookImage
                        {
                            BookId = book.Id,
                            ImagePath = "/images/books/" + fileName
                        };

                        _context.BookImages.Add(bookImage);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Có lỗi xảy ra khi tải ảnh: " + ex.Message
                );

                return View(book);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            return View("ConfirmEdit", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEdit(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View("ConfirmEdit", book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}