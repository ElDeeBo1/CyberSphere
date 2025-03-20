using AutoMapper;
using CyberSphere.BLL.DTO.LevelDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Implementation;
using CyberSphere.DAL.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSphere.BLL.Services.Implenentation
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepo levelRepo;
        private readonly IMapper mapper;

        public LevelService(ILevelRepo levelRepo,IMapper mapper)
        {
            this.levelRepo = levelRepo;
            this.mapper = mapper;
        }
        public CreateLevelDTO CreateLevel(CreateLevelDTO levelDTO)
        {
            try
            {
                var entity = mapper.Map<Level>(levelDTO);  
                var created = levelRepo.Addlevel(entity);
                var showed = levelRepo.GetLevel(created.Id);
                return mapper.Map<CreateLevelDTO>(created );
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteLevel(int id)
        {
            try
            {
                var entity = levelRepo.GetLevel(id);
                if (entity == null)
                {
                    Console.WriteLine(  "the level not esists");
                    return false;
                }
                levelRepo.DeleteLevel(entity);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GetAllLevelsDTO> GetAllLevels()
        {
            var levels = levelRepo.GetLevels().ToList();
            return mapper.Map<List<GetAllLevelsDTO>>(levels);   
         
        }

        public GetLevelByIdDTO GetLevelById(int id)
        {
            var level = levelRepo.GetLevel(id);
            return mapper.Map<GetLevelByIdDTO>(level);
        }

        public UpdateLevelDTO UpdateLevel(int id,UpdateLevelDTO levelDTO)
        {
            var existedlevel = levelRepo.GetLevel(id);
            if (existedlevel == null)
                throw new Exception("not found");
            if(!string.IsNullOrEmpty(levelDTO.Title))
               existedlevel.Title = levelDTO.Title; 
            if(!string.IsNullOrEmpty(levelDTO.Description))
                existedlevel.Description = levelDTO.Description;
            if(levelDTO.ParentLevelId.HasValue)
                existedlevel.ParentLevelId = levelDTO.ParentLevelId.Value;
            var updated = levelRepo.Updatelevel(id, existedlevel);

            return mapper.Map<UpdateLevelDTO>(updated);
        }
    }
}
