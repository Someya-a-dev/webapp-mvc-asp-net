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
        public ActionResult Search(FormCustomerSearchModel formModel = null)
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
            vbSearchDataTo = 4;
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
            List<DBCustomerSearchModel> SearchResult = new List<DBCustomerSearchModel>();
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
                    SearchResult = _customerSearchService.GetCustomerFirstToTen();
                    break;
                case "SORT":
                    vbCondCustomerIdFrom = Session["CondcustomerIdFrom"].ToString();
                    vbCondCustomerIdTo = Session["CondCustomerIdTo"].ToString(); 
                    vbCustomerType0 = (bool)Session["CustomerType0"];
                    vbCustomerType1 = (bool)Session["CustomerType1"];
                    vbCustomerType2 = (bool)Session["CustomerType2"];
                    vbSortCustId = Session["SortCustId"].ToString();
                    vbSortCustNm = Session["SortCustNm"].ToString();
                    vbSortCustType = Session["SortCustType"].ToString();
                    vbKeyWord = Session["KeyWord"].ToString();
                    vbSearchDataFrom = (int)Session["SearchDataFrom"];
                    vbSearchDataTo = (int)Session["SearchDataTo"];

                    if(formModel != null)
                    {
                        SearchResult = _customerSearchService.Getcustomer(formModel);
                    }
                    break;
                case "INPUT":
                case "CONFIRM":
                    break;
                case "DELETE":
                    break;
                default:
                    break;
            }

            //Form初期表示
            ViewBag.SearchResult = SearchResult; //検索結果
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
                from = "SORT";
                TempData["From"] = from; //再度登録してリロードできるように
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            Session["Title"] = "顧客一覧";
            Session["Name"] = _userService.GetName(Session["USER_CD"].ToString());
            Session["CondCustomerIdFrom"] = form.CondCustomerIdFromString ?? "";
            Session["CondCustomerIdTo"] = form.CondCustomerIdToString ?? "";
            Session["CustomerType0"] = form.chkCustomerType0;
            Session["CustomerType1"] = form.chkCustomerType1;
            Session["CustomerType2"] = form.chkCustomerType2;
            Session["SortCustId"] = form.SortCustId ?? "";
            Session["SortCustNm"] = form.SortCustNm ?? "";
            Session["SortCustType"] = form.SortCustType ?? "";
            Session["KeyWord"] = form.CondKeyword ?? "";
            Session["SearchDataFrom"] = form.SearchDataFrom;
            Session["SearchDataTo"] = form.SearchDataTo;

            return RedirectToAction("Search","Customer" ,form);
        }

    }
}