using AutoMapper;
using CyberSphere.BLL.DTO.SkillDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepo skillRepo;
        private readonly IMapper mapper;

        public SkillService(ISkillRepo skillRepo , IMapper mapper)
        {
            this.skillRepo = skillRepo;
            this.mapper = mapper;
        }
        public async Task<AddSkillDTO> AddSkill(AddSkillDTO skillDTO)
        {
            try
            {
                var skilled = mapper.Map<Skill>(skillDTO);
                var entity = await skillRepo.AddSkill(skilled);
                var showed = skillRepo.GetSkill(skilled.Id);
               var created= mapper.Map<AddSkillDTO>(entity);
                return created;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteSkill(int id)
        {
            try
            {
                var existedskill = await skillRepo.GetSkill(id);
                if (existedskill != null)
                {
                    {

                        var created = mapper.Map<Skill>(existedskill);
                        await skillRepo.DeleteSkill(created);
                    }
                    return false;

                }
                return false;;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<GetSkillDTO>> GetAllSkillsByStudentId(int studentid)
        {
            try
            {
                var studentskills= await skillRepo.GetAllSkillsByStudentId(studentid);
                return mapper.Map<List<GetSkillDTO>>(studentskills);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GetSkillDTO> GetSkill(int id)
        {
            try
            {
                var studentskill = await skillRepo.GetSkill(id);
                return mapper.Map<GetSkillDTO>(studentskill);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UpdateSkillDTO> UpdateSkill(int id, UpdateSkillDTO skillDTO)
        {
            try
            {
                var existedskill = await skillRepo.GetSkill(id);
                if(!string.IsNullOrEmpty(skillDTO.Name))
                    existedskill.Name = skillDTO.Name;
                var updatedskill = await skillRepo.UpdateSkill(id,existedskill);
                return mapper.Map<UpdateSkillDTO>(updatedskill);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
