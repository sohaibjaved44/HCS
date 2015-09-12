using System.ComponentModel;

namespace HCS
{
    public enum ProductType
    {
        [StringValue("00000")]
        [Description("none")]
        None = 1,
        [StringValue("00001")]
        [Description("goods")]
        Goods = 2,
        [StringValue("00002")]
        [Description("cash")]
        Cash = 3,
        [StringValue("00003")]
        [Description("cheque")]
        cheque = 4
    }
}
