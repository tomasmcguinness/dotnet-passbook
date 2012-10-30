using Passbook.Generator;
using Passbook.Generator.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passbook.Sample.Web.SampleRequests
{
    public class CouponPassGeneratorRequest : PassGeneratorRequest
    {
        public CouponPassGeneratorRequest()
        {
            this.Style = PassStyle.Coupon;
        }

        public override void PopulateFields()
        {
            this.AddPrimaryField(new NumberField("discount","Discount",0.01m, FieldNumberStyle.PKNumberStylePercent));
        }
    }
}