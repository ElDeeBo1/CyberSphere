using CyberSphere.BLL.DTO.LevelDTO;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService levelService;

        public LevelController(ILevelService levelService)
        {
            this.levelService = levelService;
        }
        [HttpGet("get-all-levels")]
        public IActionResult GetAllLevels()
        {
            if(ModelState.IsValid)
            {
                var levels = levelService.GetAllLevels();
                return Ok(levels);
            }
            return BadRequest("can not shows");
        }
        [HttpGet("get-level-by-id")]
        public IActionResult GetLevelById(int id)
        {
            if(ModelState.IsValid)
            {
                var level = levelService.GetLevelById(id);
                return Ok(level);
            }
            return BadRequest("can not show");
        }
        [HttpPost]
        public IActionResult CreateLevel(CreateLevelDTO createLevelDTO)
        {
            if(ModelState.IsValid)
            {
                var level = levelService.CreateLevel(createLevelDTO);
                return Ok(level);
            }
            return BadRequest("can not be add level");
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateLevel(int id,[FromForm] UpdateLevelDTO updateLevelDTO)
        {
            if(ModelState.IsValid)
            {
                var level = levelService.GetLevelById(id);
                if (level == null)
                    return BadRequest("the level not exists");
              var updated=  levelService.UpdateLevel(id,updateLevelDTO);
                return Ok(updated);
            }
            return BadRequest("bad error");
        }
        [HttpDelete]
        public IActionResult DeleteLevel(int id)
        {
            if(ModelState.IsValid)
            {
                levelService.DeleteLevel(id);
                return Ok("the level deleted successfully");
            }
            return BadRequest("can not be delete");
        }

    }
}
