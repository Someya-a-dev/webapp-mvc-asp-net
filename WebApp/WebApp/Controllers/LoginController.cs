using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Login;
using WebApp.Services;
using System.Diagnostics;
using System.EnterpriseServices;
using Microsoft.Identity.Client;
using WebApp.Models;
using System.Web.UI.WebControls;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly MST_USERService _userService = new MST_USERService();

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            string userIp = Request.UserHostAddress; // ユーザーのIPアドレスを取得
            ViewBag.UserIP = userIp; // ビューにIPを渡す

            return View();
        }

        // POST : Login
        [HttpPost]
        public ActionResult Index(FormLoginModel model)
        {
            if(ModelState.IsValid)
            {
                //formから入力内容を取得
                string LoginID = model.LoginID; //USER_CD
                string LoginPassword = model.Password;
                string UserIPAddress = model.IpAddress;

                //DB確認
                //_userService.MST_USERCheck();

                // ログイン処理
                bool isUserCheck = _userService.UserCheck(LoginID);

                if (isUserCheck)
                {
                    bool isPassword = _userService.PasswordCheck(LoginID, LoginPassword);

                    if (isPassword)
                    {
                        _userService.UpdateLastLoginTimeAndIp(LoginID, UserIPAddress);

                        //ユーザCDをセッションに保存
                        Session["USER_CD"] = LoginID;

                        // ログイン成功時の処理（顧客一覧）
                        TempData["From"] = "LOGIN"; //どこから遷移してきたのか情報を渡す
                        return RedirectToAction("Index", "Customer/Search");
                    }
                }
                string ErrorMessage = "ログインIDまたはパスワードが間違っています。";
                ViewBag.ErrorMessage = ErrorMessage;
                return View();
            }
            else
            {
                //異常あり
                return View(model);
            }
        }
    }
}