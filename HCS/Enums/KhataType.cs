using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCS.Enums
{
    public enum KhataType
    {
        [StringValue("00000")]
        [Description("none")]
        None =0,
        [StringValue("00001")]
        [Description("labour")]
        Labour = 1,
        [StringValue("00002")]
        [Description("fee")]
        Fee = 2,
        [StringValue("00003")]
        [Description("commission")]
        Commission = 3,
        [StringValue("00004")]
        [Description("bardana")]
        Bardana = 4,
        [StringValue("00005")]
        [Description("sootli")]
        Sootli = 5,
        [StringValue("00006")]
        [Description("munshiyana")]
        Munshiyana = 6
    }
}
