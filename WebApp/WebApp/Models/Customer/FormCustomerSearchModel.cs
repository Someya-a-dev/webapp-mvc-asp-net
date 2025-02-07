using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Customer
{
    public class FormCustomerSearchModel


    {
        // フォームから受け取る文字列
        public string CondCustomerIdFromString { get; set; }
        public string CondCustomerIdToString { get; set; }

        [Range(0, 99999999, ErrorMessage = "8桁以内の数値を入力してください。")]
        public int? CondCustomerIdFrom { get; set; }

        [Range(0, 99999999, ErrorMessage = "8桁以内の数値を入力してください。")]
        public int? CondCustomerIdTo { get; set; }

        public bool chkCustomerType0 { get; set; }

        public bool chkCustomerType1 { get; set; }

        public bool chkCustomerType2 { get; set; }
        public string CondKeyword { get; set; }

        public int? SortCustId  { get; set; }

        public int? SortCustNm { get; set; }

        public int? SortCustType { get; set; }
    }
}