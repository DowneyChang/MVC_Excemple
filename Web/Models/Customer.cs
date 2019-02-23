using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Customer : IValidatableObject
    {
        [Required]//代表此欄位為必要欄位
        [ScaffoldColumn(false)]//代表在以基架建置 View 頁面時將會忽略此欄位(不顯示在頁面上)
        public int Id { get; set; }
        [Required]
        [Display(Name = "顧客名稱")]
        [StringLength(30,MinimumLength = 2,ErrorMessage = "{0}必須在{2}到{1}個字元之間")]
        public string Name { get; set; }
        [Email]
        public string Email { get; set; }
        [Email]
        public string Email2 { get; set; }
        [Required]
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 針對個別 Model(Class) 進行一系列的驗證
        /// 此驗證將在送出 Model 後(Server端)並且驗證完 DataAnnotations 後才進行
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == null)
            {
                yield return new ValidationResult("名稱不得空白。", new[] { "Name" });
            }

            if (Email == null || Email2 == null)
            {
                yield return new ValidationResult("電子郵件不得空白。", new[] { "Email", "Email2" });
            }

            if (Email != Email2)
            {
                yield return new ValidationResult("電子郵件不相符。", new[] { "Email", "Email2" });
            }

            if (Birthday > DateTime.Now)
            {
                yield return new ValidationResult("時空旅人你確定?我要打電話給新聞台了喔~", new[] { "Birthday" });
            }

            if (Birthday < DateTime.Now.AddYears(-100))
            {
                yield return new ValidationResult("人瑞!!可以去看看您嗎?", new[] { "Birthday" });
            }
        }
    }
}