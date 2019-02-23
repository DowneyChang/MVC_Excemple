using Resources;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Web.Models
{
    [MetadataType(typeof(ProductMD))]
    [CustomValidation(typeof(StringValidator), "Invalid")]//在此類別上套用自定義驗證屬性 StringValidator 中的 Invalid 驗證方法
    public partial class Product//partial 關鍵字表示此類別為部份類別，意為此類別須和在其他 .cs 檔案中的同名部份類別合併後，才算一個完整類別
    {
        public class ProductMD
        {
            [ScaffoldColumn(false)]
            public int ProductID { get; set; }
            [Required(ErrorMessageResourceType = typeof(ModelResource),ErrorMessageResourceName = "ProductName")]
            [Display(Name = "產品名稱")]
            [StringLength(50,MinimumLength = 3,ErrorMessage = "{0}必須介於{2}到{1}之間，")]
            [Remote("ProductName", "Validations")]//設定該欄位在以 Ajax 傳遞時使用自定義屬性進行驗證
            public string ProductName { get; set; }
            [Required]
            public string QuantityPerUnit { get; set; }
            [Required]
            public bool Discontinued { get; set; }
            [ScaffoldColumn(false)]
            public int? SupplierID { get; set; }
            [ScaffoldColumn(false)]
            public int? CategoryID { get; set; }
            [Display(Name = "單價")]
            [Price(MinPrice = 10)]//自定義的擴充 ValidationAttribute (驗證方法)
            public decimal? UnitPrice { get; set; }
            [Display(Name = "庫存")]
            [Range(0,32767,ErrorMessage = "{0}必須介於{1}到{2}之間。")]
            public short? UnitsInStock { get; set; }
            [ScaffoldColumn(false)]
            public short? UnitsOnOrder { get; set; }
            [ScaffoldColumn(false)]
            public short? ReorderLevel { get; set; }
        }
    }

    /// <summary>
    /// 自訂驗證屬性
    /// </summary>
    public class StringValidator
    {
        /// <summary>
        /// 自訂驗證屬性的方法
        /// </summary>
        /// <param name="product">被驗證的物件來源</param>
        /// <param name="validationContext">選擇性參數</param>
        /// <returns>ValidationResult.Success 或 ValidationResult(錯誤訊息)</returns>
        public static ValidationResult Invalid(Product product, ValidationContext validationContext)
        {
            string re = string.Empty; //宣告要回傳的錯誤訊息
            Regex regex = new Regex(@"[^\w\.-]", RegexOptions.IgnoreCase);
            re = (product.ProductName != null && regex.Match(product.ProductName).Length > 0) //判斷 ProductName 格式
                ? "產品名稱只允許包含英數字元，句號(.)，連字號(-)。"//格式錯誤則設置錯誤訊息
                : re;//格式正確則原自串不動
            //以下其他驗證省略...

            if (re == string.Empty)//沒有錯誤訊息，則表示驗證通過
            {
                return ValidationResult.Success;
            }
            else//有錯誤訊息則驗證失敗，回傳錯誤訊息
            {
                return new ValidationResult(re);
            }
        }
    }
}