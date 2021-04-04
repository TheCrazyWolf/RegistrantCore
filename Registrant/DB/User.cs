using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class User
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LevelAccess { get; set; }
        public string Name { get; set; }
    }
}
