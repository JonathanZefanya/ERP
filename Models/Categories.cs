using System;

namespace BusinessERP.Models
{
    public class Categories : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
