using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data;
using Survivor.Entities;

//CategoryController:
// 
// GET /api/categories - Tüm kategorileri listele.
// 
// GET /api/categories/{id} - Belirli bir kategoriyi getir.
// 
// POST /api/categories - Yeni bir kategori oluştur.
// 
// PUT /api/categories/{id} - Belirli bir kategoriyi güncelle.
// 
// DELETE /api/categories/{id} - Belirli bir kategoriyi sil.
namespace Survivor.Controllers
{
    [Route("api/categories/")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly SurvivorContext _context;

        public CategoryController(SurvivorContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<CategoryEntity>> GetAll()
        {
            var category = await _context.Categories.ToListAsync();

            if (category is null) 
                return NotFound();

            return Ok(category);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryEntity>> GetById(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            return Ok(category);
        }
        
        [HttpPost]
        public async Task<ActionResult<CategoryEntity>> Create(CategoryEntity category)
        {
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryEntity updatedCategory)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            
            if (category == null) return NotFound($"{id} idli kategori bulunamadı.");
            
            category.Name = updatedCategory.Name;
            category.ModifiedDate = DateTimeOffset.Now;
            category.Competitors = updatedCategory.Competitors;
            
            try
            {
                await _context.SaveChangesAsync();
                
                return Ok(new
                {
                    Message = "Kategori güncellendi.",
                    updatedName = category.Name,
                    ModifiedDate = DateTimeOffset.Now,
                    updatedCompetitors = category.Competitors
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Kategori güncellenirken hata oluştu.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryEntity>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound($"{id} idli kategori bulunamadı");
            
            category.IsDeleted = true;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

    }
}
