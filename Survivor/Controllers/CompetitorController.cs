using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data;
using Survivor.Entities;

//CompetitorController:
// 
// GET /api/competitors - Tüm yarışmacıları listele.
// 
// GET /api/competitors/{id} - Belirli bir yarışmacıyı getir.
// 
// GET /api/competitors/categories/{CategoryId} - Kategori Id'ye göre yarışmacıları getir.
// 
// POST /api/competitors - Yeni bir yarışmacı oluştur.
// 
// PUT /api/competitors/{id} - Belirli bir yarışmacıyı güncelle.
// 
// DELETE /api/competitors/{id} - Belirli bir yarışmacıyı sil.
namespace Survivor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorController : ControllerBase
    {
        private readonly SurvivorContext _context;

        public CompetitorController(SurvivorContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<CompetitorEntity>> GetAll()
        {
            var competitor = await _context.Competitors.ToListAsync();

            if (competitor is null) 
                return NotFound();

            return Ok(competitor);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetitorEntity>> GetById(int id)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(c => c.Id == id);

            if (competitor is null) return NotFound();

            return Ok(competitor);
        }
        
        [HttpGet("categories/{CategoryId}")]
        public async Task<ActionResult<CompetitorEntity>> GetByCategoryId(int id)
        {
            var competitor = await _context.Competitors.Where(c => c.CategoryId == id).ToListAsync();

            if (competitor is null) return NotFound();
            
            return Ok(competitor);
        }
        
        [HttpPost]
        public async Task<ActionResult<CompetitorEntity>> Create(CompetitorEntity competitor)
        {
            _context.Competitors.Add(competitor);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = competitor.Id }, competitor);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompetitor(int id, CompetitorEntity updatedCompetitor)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(p => p.Id == id);
            
            if (competitor == null) return NotFound($"{id} idli yarışmacı bulunamadı.");
            
            competitor.FirstName = updatedCompetitor.FirstName;
            competitor.LastName = updatedCompetitor.LastName;
            competitor.ModifiedDate = DateTimeOffset.Now;
            competitor.Category = updatedCompetitor.Category;
            
            try
            {
                await _context.SaveChangesAsync();
                
                return Ok(new
                {
                    Message = "Yarışmacı güncellendi.",
                    updatedName = competitor.FirstName + " " + competitor.LastName,
                    ModifiedDate = DateTimeOffset.Now,
                    updatedCompetitor = competitor.Category
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Yarışmacı güncellenirken hata oluştu.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<CompetitorEntity>> DeleteCompetitor(int id)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(c => c.Id == id);

            if (competitor is null) return NotFound($"{id} idli yarışmacı bulunamadı");
            
            competitor.IsDeleted = true;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

    }
}
