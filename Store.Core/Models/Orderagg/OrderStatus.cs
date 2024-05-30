using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Models.Orderagg
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending, //lma el order yt3ml
        [EnumMember(Value = "PaymentRecieved")]
        PaymentRecieved, // ya etdf3 ya failed
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
