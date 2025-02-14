using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using WebApp.Models.Customer;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MST_USERService _userService = new MST_USERService();
        private readonly CustomerSearchService _customerSearchService = new CustomerSearchService();

        //vb宣言
        private string vbCondCustomerIdFrom;
        private string vbCondCustomerIdTo;
        private bool vbCustomerType0;
        private bool vbCustomerType1;
        private bool vbCustomerType2;
        private string vbKeyWord;
        private string from;
        private string vbSortCustId;
        private string vbSortCustNm;
        private string vbSortCustType;
        private int vbSearchDataFrom;
        private int vbSearchDataTo;

        // GET: Customer
        [HttpGet]
        public ActionResult List()
        {
            //初期値設定
            vbCondCustomerIdFrom = "";
            vbCondCustomerIdTo = "";
            vbCustomerType0 = false;
            vbCustomerType1 = false;
            vbCustomerType2 = false;
            vbSortCustId = "▲";
            vbSortCustNm = "△";
            vbSortCustType = "△";
            vbKeyWord = "";
            vbSearchDataFrom = 0;
            vbSearchDataTo = 10;
            //後で削除

            // ※２．ログイン済みのチェック
            if (Session["USER_CD"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //遷移の確認
            if (TempData["From"] != null)
            {
                from = TempData["From"].ToString();
                TempData["From"] = from; //再度登録してリロードできるように
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            //モード別の処理
            switch (from)
            {
                case "LOGIN":
                    vbCondCustomerIdFrom = "";
                    vbCondCustomerIdTo = "";
                    vbCustomerType0 = false;
                    vbCustomerType1 = false;
                    vbCustomerType2 = false;
                    vbSortCustId = "▲";
                    vbSortCustNm = "△";
                    vbSortCustType = "△";
                    vbKeyWord = "";
                    break;
                case "INPUT":
                case "CONFIRM":
                    break;
                case "DELETE":
                    break;
                default:
                    break;
            }

            try
            {
                //検索
                List <DBCustomerSearchModel>  SearchResult = _customerSearchService.GetCustomerFirstToTen();
                ViewBag.SearchResult = SearchResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            //Form初期表示
            ViewBag.Title = "顧客一覧";
            ViewBag.Name = _userService.GetName(Session["USER_CD"].ToString());
            ViewBag.CondCustomerIdFrom = vbCondCustomerIdFrom;
            ViewBag.CondCustomerIdTo = vbCondCustomerIdTo;
            ViewBag.CustomerType0 = vbCustomerType0;
            ViewBag.CustomerType1 = vbCustomerType1;
            ViewBag.CustomerType2= vbCustomerType2;
            ViewBag.SortCustId = vbSortCustId;
            ViewBag.SortCustNm = vbSortCustNm;
            ViewBag.SortCustType = vbSortCustType;
            ViewBag.KeyWord = vbKeyWord;
            ViewBag.SearchDataFrom = vbSearchDataFrom;
            ViewBag.SearchDataTo = vbSearchDataTo;

            return View("CustomerList");
        }

        // POST: Customer
        [HttpPost]
        public ActionResult List(FormCustomerSearchModel form)
        {
            string from;
            // ※２．ログイン済みのチェック
            if (Session["USER_CD"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //遷移の確認
            if (TempData["From"] != null)
            {
                from = "Search";
                TempData["From"] = from; //再度登録してリロードできるように
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            //入力チェック
            //CondCustomerIdFromとCondCustomerIdToが数字がチェック
            // Debug用のConsole出力
            Debug.WriteLine("==== Customer Search Form Data ====");
            Debug.WriteLine($"CondCustomerIdFromString: {form.CondCustomerIdFromString}");
            Debug.WriteLine($"CondCustomerIdToString: {form.CondCustomerIdToString}");
            Debug.WriteLine($"CondCustomerIdFrom: {form.CondCustomerIdFrom}");
            Debug.WriteLine($"CondCustomerIdTo: {form.CondCustomerIdTo}");
            Debug.WriteLine($"chkCustomerType0: {form.chkCustomerType0}");
            Debug.WriteLine($"chkCustomerType1: {form.chkCustomerType1}");
            Debug.WriteLine($"chkCustomerType2: {form.chkCustomerType2}");
            Debug.WriteLine($"CondKeyword: {form.CondKeyword}");
            Debug.WriteLine($"SortCustId: {form.SortCustId}");
            Debug.WriteLine($"SortCustNm: {form.SortCustNm}");
            Debug.WriteLine($"SortCustType: {form.SortCustType}");

            ViewBag.Title = "顧客一覧";
            ViewBag.Name = _userService.GetName(Session["USER_CD"].ToString());
            ViewBag.CondCustomerIdFrom = form.CondCustomerIdFromString;
            ViewBag.CondCustomerIdTo = form.CondCustomerIdToString;
            ViewBag.CustomerType0 = form.chkCustomerType0;
            ViewBag.CustomerType1 = form.chkCustomerType1;
            ViewBag.CustomerType2 = form.chkCustomerType2;
            ViewBag.SortCustId = form.SortCustId;
            ViewBag.SortCustNm = form.SortCustNm;
            ViewBag.SortCustType = form.SortCustType;
            ViewBag.KeyWord = form.CondKeyword;
            ViewBag.SearchDataFrom = form.SearchDataFrom;
            ViewBag.SearchDataTo = form.SearchDataTo;

            return View("CustomerList");
        }

    }
}