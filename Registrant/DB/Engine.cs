using System;
using System.Collections.Generic;

#nullable disable

namespace Registrant.DB
{
    public partial class Engine
    {
        public int Id { get; set; }
        public string ActualAppVer { get; set; }
        public string ActualBdVer { get; set; }
    }
}
