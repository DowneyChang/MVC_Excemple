using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //加上 String-->Decimal 的轉換函數
            ModelBinders.Binders.Add(
                typeof(decimal),
                new FlexModelBinder(s => Convert.ToDecimal(s, CultureInfo.CurrentCulture))
            );

            //調整 ViewEngines 順序，以降低檔案搜尋成本，提高效能
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());

            // 將 CookieValueProviderFactory 註冊進 ValueProviderFactories 中
            //ValueProviderFactories.Factories.Add(new CookieValueProviderFactory());
        }

        /// <summary>
        /// 自訂 FlexModelBinder 擴充類別
        /// </summary>
        public class FlexModelBinder : IModelBinder
        {
            // 將轉換核心抽出變成 Func<string, object> 當成初始化參數
            // 傳入不同轉換函數，就可以變成不同型別的 ModelBinder
            Func<string, object> _convFn = null;

            /// <summary>
            /// 初始化核心參數 _convFn
            /// </summary>
            /// <param name="convFunc"></param>
            public FlexModelBinder(Func<string, object> convFunc)
            {
                _convFn = convFunc;
            }

            /// <summary>
            /// 實作 IModelBinder 介面
            /// </summary>
            /// <param name="controllerContext"></param>
            /// <param name="bindingContext"></param>
            /// <returns></returns>
            public object BindModel(ControllerContext controllerContext,ModelBindingContext bindingContext)
            {
                // ValueProviderResult
                // 表示將值(例如從表單張貼或查詢字串)繫結至動作方法引述屬性或繫結至引述本身的結果
                ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                ModelState modelState = new ModelState { Value = valueResult };
                object actualValue = null;
                try
                {
                    // valueResult：取得或設定轉換為顯示字串之未經處理的值。
                    // _convFn委派：進行轉換
                    actualValue = _convFn(valueResult.AttemptedValue);
                }
                catch(Exception e)
                {
                    modelState.Errors.Add(e);
                }

                bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
                return actualValue;
            }
        }

        /// <summary>
        /// 自訂 CookieValueProviderFactory 擴充類別
        /// </summary>
        public class CookieValueProviderFactory : ValueProviderFactory
        {
            /// <summary>
            /// 將某個 Cookie 的值加入 Model Binding 中
            /// </summary>
            /// <param name="controllerContext"></param>
            /// <returns></returns>
            public override IValueProvider GetValueProvider(ControllerContext controllerContext)
            {
                var source = controllerContext.RequestContext.HttpContext.Request.Cookies["Datas"].Values;

                // 因為 Cookie 也是 NameValue 格式，因此使用內建的 Provider
                return new NameValueCollectionValueProvider(source, CultureInfo.CurrentCulture);
            }
        }
    }
}
