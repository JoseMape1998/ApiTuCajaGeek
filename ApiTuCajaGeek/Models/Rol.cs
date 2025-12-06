using System;
using System.Collections.Generic;

namespace ApiTuCajaGeek.Models
{
    public class Rol
    {
        public Guid Rol_id { get; set; }
        public string Name_Rol { get; set; } = string.Empty;
        public ICollection<Users>? Users { get; set; }
    }
}
