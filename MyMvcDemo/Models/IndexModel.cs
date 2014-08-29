using System.Collections.Generic;

namespace MyMvcDemo.Models
{
    public class IndexModel
    {
        public AdministratorDTO Admin { get; set; }

        public IEnumerable<ModuleDTO> Modules { get; set; }
    }

    public class ModuleDTO
    {
        public ModuleDTO()
        {
            Children = new List<ModuleDTO>();
        }

        public string Name { get; set; }
        public string VName { get; set; }
        public string CSS { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public int Sort { get; set; }
        public bool IsSuper { get; set; }

        public ModuleDTO Parent { get; set; }

        public List<ModuleDTO> Children { get; set; }
    }

    public class AdministratorDTO
    {
        public AdministratorDTO()
        {
        }
        public string  Name { get; set; }
    }
}