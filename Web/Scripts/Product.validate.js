//引入 jquery.validate 以使用 VS 的 IntelliSence(自動完成輸入) 功能
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

//使用 JQuery Validation 在頁面前端做在伺服器端設置的 ValidationAttribute 自定義驗證屬性
//第一個參數必須是伺服器端設置的 ValidationType，第二個是 ValidationParameters.Add() 的值
$.validator.unobtrusive.adapters.addSingleVal("price", "minprice");

$.validator.addMethod("price", function (value, element, param) {
    if (value) {
        var inputValue = parseInt(value, 10);
        var validateValue = parseInt(param, 10);
        if (inputValue < validateValue)
            return false;
    }
    return true;
});