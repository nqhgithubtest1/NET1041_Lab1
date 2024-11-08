using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET1041_Lab1.Models;

namespace NET1041_Lab1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Net1041Bai3Context _context;

        public ProductsController(Net1041Bai3Context context)
        {
            _context = context;
        }

        // GET: Products
        public ActionResult Index(ProductFilterViewModel model)
        {
            ViewBag.SortByOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "ProductName", Text = "Product Name" },
                new SelectListItem { Value = "Price", Text = "Price" },
                new SelectListItem { Value = "Quantity", Text = "Quantity" },
                new SelectListItem { Value = "Brand", Text = "Brand" },
                new SelectListItem { Value = "Model", Text = "Model" },
                new SelectListItem { Value = "ManufactureDate", Text = "Manufacture Date" },
                new SelectListItem { Value = "ExpirationDate", Text = "Expiration Date" },
                new SelectListItem { Value = "Rating", Text = "Rating" },
                new SelectListItem { Value = "IsAvailable", Text = "Availability" },
                new SelectListItem { Value = "CreatedDate", Text = "Created Date" },
                new SelectListItem { Value = "UpdatedDate", Text = "Updated Date" }
            };

            ViewBag.SortOrderOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "asc", Text = "Ascending" },
                new SelectListItem { Value = "desc", Text = "Descending" }
            };

            var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(model.Keyword))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(model.Keyword));
            }

            if (model.PriceFrom.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= model.PriceFrom.Value);
            }
            if (model.PriceTo.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= model.PriceTo.Value);
            }

            if (!string.IsNullOrEmpty(model.SortBy))
            {
                // solution 1:
                //productsQuery = model.SortOrder == "desc"
                //    ? productsQuery.OrderByDescending(e => EF.Property<object>(e, model.SortBy))
                //    : productsQuery.OrderBy(e => EF.Property<object>(e, model.SortBy));

                // solution 2:
                switch (model.SortBy)
                {
                    case "ProductName":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.ProductName)
                            : productsQuery.OrderBy(p => p.ProductName);
                        break;
                    case "Price":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.Price)
                            : productsQuery.OrderBy(p => p.Price);
                        break;
                    case "Quantity":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.Quantity)
                            : productsQuery.OrderBy(p => p.Quantity);
                        break;
                    case "Brand":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.Brand)
                            : productsQuery.OrderBy(p => p.Brand);
                        break;
                    case "Model":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.Model)
                            : productsQuery.OrderBy(p => p.Model);
                        break;
                    case "ManufactureDate":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.ManufactureDate)
                            : productsQuery.OrderBy(p => p.ManufactureDate);
                        break;
                    case "ExpirationDate":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.ExpirationDate)
                            : productsQuery.OrderBy(p => p.ExpirationDate);
                        break;
                    case "Rating":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.Rating)
                            : productsQuery.OrderBy(p => p.Rating);
                        break;
                    case "IsAvailable":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.IsAvailable)
                            : productsQuery.OrderBy(p => p.IsAvailable);
                        break;
                    case "CreatedDate":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.CreatedDate)
                            : productsQuery.OrderBy(p => p.CreatedDate);
                        break;
                    case "UpdatedDate":
                        productsQuery = model.SortOrder == "desc"
                            ? productsQuery.OrderByDescending(p => p.UpdatedDate)
                            : productsQuery.OrderBy(p => p.UpdatedDate);
                        break;
                    default:
                        break;
                }
            }

            model.Products = productsQuery
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)productsQuery.Count() / model.PageSize);
            ViewBag.CurrentPage = model.Page;

            return View(model);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Price,Quantity,CategoryId,Brand,Model,ManufactureDate,ExpirationDate,Rating,IsAvailable,CreatedDate,UpdatedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Price,Quantity,CategoryId,Brand,Model,ManufactureDate,ExpirationDate,Rating,IsAvailable,CreatedDate,UpdatedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.UpdatedDate = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
