using eSya.ProductSetup.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ProductSetup.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessStructureController : ControllerBase
    {
        private readonly IBusinessStructureRepository _BusinessStructureRepository;
        public BusinessStructureController(IBusinessStructureRepository businessStructureRepository)
        {
            _BusinessStructureRepository = businessStructureRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessEntities()
        {
            var b_entities = await _BusinessStructureRepository.GetBusinessEntities();
            return Ok(b_entities);

        }
    }
}
