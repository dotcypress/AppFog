#region

using System.Collections.Generic;
using dotCypress.AppFog.Common.Models;

#endregion

namespace dotCypress.AppFog.Common
{
    public class Repository
    {
        public List<AppInfo> Apps { get; set; }

        public Info Info { get; set; }

        public List<Service> Services { get; set; }
    }
}