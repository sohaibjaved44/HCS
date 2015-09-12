using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCS.Enums
{
    public  enum ActivityType
    {
        [StringValue("00000")]
        [Description("none")]
        None = 1,
        [StringValue("00001")]
        [Description("sale")]
        Sale = 2,
        [StringValue("00002")]
        [Description("purchase")]
        Purchase = 3
    }
}
