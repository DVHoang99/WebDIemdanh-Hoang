using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        WEBATTENDANCEEntities data = new WEBATTENDANCEEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Register()
        //{
        //    return View();
        //}
        [HttpPost]
        public JsonResult Register(string taikhoan, string matkhau, string nhaplaimatkhau, int chucvu, string ten)
        {
            
                int test1 = data.TAIKHOANs.Where(x => x.USERNAME.Equals(taikhoan)).Count();

                if (test1 == 0)
                {
                    TAIKHOAN t = new TAIKHOAN();
                    if (chucvu == 2)
                    {
                        int test2 = data.GIANGVIENs.Where(w => w.ID.Equals(taikhoan)).Count();
                        if (test2 == 1)
                        {
                            t.USERNAME = taikhoan;
                            if (matkhau == nhaplaimatkhau)
                            {

                                t.PASSWORD = matkhau;
                                t.Name = ten;
                                t.ROLE1 = chucvu;
                                data.TAIKHOANs.Add(t);
                                data.SaveChanges();
                                ViewBag.a = "Đăng kí thành công !!!";
                                return Json(1);

                            }
                            else
                            {
                                ViewBag.a = "Mật khẩu và nhập lại mật khẩu không đúng !!!";
                                return Json(1);
                            }
                        }
                        else
                        {
                            ViewBag.a = "Nhập sai mã giảng viên !!!";
                            return Json(1);
                        }
                    }
                    else
                    {
                        int test2 = data.SINHVIENs.Where(w => w.ID.Equals(taikhoan)).Count();
                        if (test2 == 1)
                        {
                            t.USERNAME = taikhoan;
                            if (matkhau == nhaplaimatkhau)
                            {

                                t.PASSWORD = matkhau;
                                t.Name = ten;
                                t.ROLE1 = chucvu;
                                data.TAIKHOANs.Add(t);
                                data.SaveChanges();
                                ViewBag.a = "Đăng kí thành công !!!";
                                return Json(1);

                            }
                            else
                            {
                                ViewBag.a = "Mật khẩu và nhập lại mật khẩu không đúng !!!";
                                return Json(1);
                            }
                        }
                        else
                        {
                            ViewBag.a = "Nhập sai mã sinh viên !!!";
                            return Json(1);
                        }
                    }

                }
                else
                {
                    ViewBag.a = "Tên tài khoản đã tồn tại !!!";
                    return Json(1);
                }
            
        }

        //[HttpPost]
        //public ActionResult Register(Account a, string RePassWord)
        //{
        //    int test1 = data.TAIKHOANs.Where(x => x.USERNAME.Equals(a.UserName)).Count();

        //    if (test1 == 0)
        //    {
        //        TAIKHOAN t = new TAIKHOAN();
        //        if(a.Role == 2 )
        //        {
        //            int test2 = data.GIANGVIENs.Where(w => w.ID.Equals(a.UserName)).Count();
        //            if (test2 == 1)
        //            {
        //                t.USERNAME = a.UserName;
        //                if (a.PassWord == RePassWord)
        //                {

        //                    t.PASSWORD = a.PassWord;
        //                    t.Name = a.Name;
        //                    t.ROLE1 = a.Role;
        //                    data.TAIKHOANs.Add(t);
        //                    data.SaveChanges();
        //                    ViewBag.a = "Đăng kí thành công !!!";
        //                    return View();

        //                }
        //                else
        //                {
        //                    ViewBag.a = "Mật khẩu và nhập lại mật khẩu không đúng !!!";
        //                    return View();
        //                }
        //            }
        //            else
        //            {
        //                ViewBag.a = "Nhập sai mã giảng viên !!!";
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            int test2 = data.SINHVIENs.Where(w => w.ID.Equals(a.UserName)).Count();
        //            if (test2 == 1)
        //            {
        //                t.USERNAME = a.UserName;
        //                if (a.PassWord == RePassWord)
        //                {

        //                    t.PASSWORD = a.PassWord;
        //                    t.Name = a.Name;
        //                    t.ROLE1 = a.Role;
        //                    data.TAIKHOANs.Add(t);
        //                    data.SaveChanges();
        //                    ViewBag.a = "Đăng kí thành công !!!";
        //                    return View();

        //                }
        //                else
        //                {
        //                    ViewBag.a = "Mật khẩu và nhập lại mật khẩu không đúng !!!";
        //                    return View();
        //                }
        //            }
        //            else
        //            {
        //                ViewBag.a = "Nhập sai mã sinh viên !!!";
        //                return View();
        //            }
        //        }




        //    }
        //    else
        //    {
        //        ViewBag.a = "Tên tài khoản đã tồn tại !!!";
        //        return View();
        //    }
        //}

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account a)
        {
            var test1 = data.TAIKHOANs.Where(x => x.USERNAME.Equals(a.UserName) && x.PASSWORD.Equals(a.PassWord)).FirstOrDefault();
            if (test1 != null)
            {
                if (test1.ROLE1 == 1)
                {
                    Session["Login"] = test1;
                    return RedirectToAction("Index", "Manage");
                }
                else if (test1.ROLE1 == 2)
                {
                    Session["Login"] = test1;
                    return RedirectToAction("ManageTeacherIndex", "Manage");
                }
                else
                {
                    Session["Login"] = test1;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.a = "Sai tên tài khoản hoặc mật khẩu";
            }
            return View();
        }

        public ActionResult ProfileAcount()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(b.USERNAME)).FirstOrDefault();
                    return View(result);
            
            }

        }
        [HttpPost]
        public ActionResult ProfileAcount(string USERNAME, string password, string name)
        {

            var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(USERNAME)).FirstOrDefault();
            result.PASSWORD = password;
            result.Name = name;
            data.SaveChanges();

            return View(result);
        }

    }
}