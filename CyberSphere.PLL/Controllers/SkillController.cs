using CyberSphere.BLL.DTO.SkillDTO;

using CyberSphere.BLL.Services.Implementation;
using CyberSphere.BLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSkillsController : ControllerBase
    {
        private readonly ISkillService skillService;

        public StudentSkillsController(ISkillService skillService)
        {
            this.skillService = skillService;
        }
        [HttpPost]
        public async Task<IActionResult> AddSkill(AddSkillDTO skillDTO)
        {
            if(ModelState.IsValid)
            {
                var skilled = await skillService.AddSkill(skillDTO);
                return Ok(skilled);
            }
            return BadRequest("can not add skills to students");
        }
        [HttpGet("get-skill-by-id")]
        public async Task<IActionResult> GetSkillById(int id)
        {
            if (ModelState.IsValid)
            {
                var skilld = await skillService.GetSkill(id);
                return Ok(skilld);
            }
            return BadRequest("can not show skill");
        }
        [HttpGet("get-skills-by-student-id")]
        public async Task<IActionResult > GetStudentSkills(int StudentId)
        {
            if (ModelState.IsValid)
            {
                var skills = await skillService.GetAllSkillsByStudentId(StudentId);
                return Ok(skills);
            }
            return BadRequest("can not show your skills");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id,UpdateSkillDTO skillDTO)
        {
            if(ModelState.IsValid)
            {
                var updatedskill = await skillService.UpdateSkill(id, skillDTO);
                return Ok(updatedskill);
            }
            return BadRequest("can not update skill");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            if (ModelState.IsValid)
            {
              await  skillService.DeleteSkill(id);
                return Ok("skill deleted successfully");
            }
            return BadRequest(" can not able to delete");
        }
    }
}
