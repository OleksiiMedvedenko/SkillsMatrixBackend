using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class Permission
    {
        public short? PermissionId { get; private set; }
        public string? Name { get; private set; }

        public Permission()
        {
                
        }

        public Permission(short? id, string? name)
        {
            PermissionId = id;
            Name = name;
        }
    }
}
