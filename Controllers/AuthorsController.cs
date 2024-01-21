using LazyLoadingInAspNetCoreWebApI.AppDbContext;
using LazyLoadingInAspNetCoreWebApI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LazyLoadingInAspNetCoreWebApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            // Eager loading using Include method
            var authors = await _context.Authors.Include(a => a.Books).ToListAsync();

            return authors;
        }

        // GET: api/authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            // Eager loading for a specific author by ID
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id);

            if(author == null)
            {
                return NotFound();
            }

            return author;
        }

        // POST: api/authors
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        // PUT: api/authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            if(id != author.AuthorId)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!_context.Authors.Any(a => a.AuthorId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if(author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}