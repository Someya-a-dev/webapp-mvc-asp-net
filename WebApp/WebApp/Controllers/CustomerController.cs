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
        private int vbSortCustId;
        private int vbSortCustNm;
        private int vbSortCustType;

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
            vbSortCustId = 1;
            vbSortCustNm = 0;
            vbSortCustType = 0;
            vbKeyWord = "";
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

            return View("CustomerList");
        }

        // POST: Customer
        [HttpPost]
        public ActionResult Search(FormCustomerSearchModel form)
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
                from = TempData["From"].ToString();
                TempData["From"] = from; //再度登録してリロードできるように
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            //入力チェック
            //CondCustomerIdFromとCondCustomerIdToが数字がチェック

            return RedirectToAction("Index", "Customer/List");
        }

    }
}