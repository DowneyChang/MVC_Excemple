using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

//自定義 Attribute 類別
namespace Web.Models
{
    /// <summary>
    /// 以繼承特性實作 Email 驗證屬性(Pattern)
    /// </summary>
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"^[a-zA-Z0-9.!#$%&'*+/=?^'{|}~-]+@[a-zA-Z0-p](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")// Email 格式 Pattern
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("你的 {0} 看起來怪怪的，我不能接受！！", name);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PriceAttribute : ValidationAttribute,IClientValidatable
    {
        public double MinPrice { get; set; }

        public override bool IsValid(object value)
        {
            if(value == null)
                return true;

            if (Convert.ToDouble(value) < MinPrice)
                return false;

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("minprice", MinPrice);
            rule.ValidationType = "price";
            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} 太低了！安捏毋湯～",name);
        }
    }
}