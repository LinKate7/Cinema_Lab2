using Microsoft.AspNetCore.Mvc;
using Lab2OOP.Services;
using Lab2OOP.DTO;

namespace Lab2OOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesCotroller : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchasesCotroller(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        // GET: api/PurchasesCotroller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetPurchases()
        {
            var films = await _purchaseService.GetPurchasesAsync();
            return Ok(films);
        }

        // GET: api/PurchasesCotroller/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> GetPurchase(Guid id)
        {
            var purchase = await _purchaseService.GetPurchaseAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/PurchasesCotroller/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(Guid id, PurchaseDto purchaseDto)
        {
            var success = await _purchaseService.UpdatePurchaseAsync(id, purchaseDto);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/PurchasesCotroller
        [HttpPost]
        public async Task<ActionResult<PurchaseDto>> PostPurchase(PurchaseDto purchaseDto)
        {
            var createdPurchase = await _purchaseService.CreatePurchaseAsync(purchaseDto);
            var createdPurchaseDto = _purchaseService.MapToPurchaseDtoAsync(createdPurchase);

            return CreatedAtAction("GetPurchase", new { id = createdPurchase.Id }, createdPurchaseDto);
        }

        // DELETE: api/PurchasesCotroller/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(Guid id)
        {
            var success = await _purchaseService.DeletePurchaseAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
