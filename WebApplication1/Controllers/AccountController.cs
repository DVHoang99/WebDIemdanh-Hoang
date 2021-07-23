using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
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
        public JsonResult login1(string username, string password) {

            return Json(1);
        }
        [HttpPost]
        public JsonResult Register(string taikhoan, string matkhau, string nhaplaimatkhau, int chucvu, string email ,string ten)
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

                            t.PASSWORD = Encodemd5(matkhau);
                            t.Name = ten;
                            t.ROLE1 = chucvu;
                            t.EMAIL = email;
                            data.TAIKHOANs.Add(t);
                            data.SaveChanges();
                            ViewBag.Message = "Đăng kí thành công !!!";
                            return Json(1);

                        }
                        else
                        {
                            ViewBag.Message = "Mật khẩu và nhập lại mật khẩu không đúng !!!";
                            return Json(0);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Nhập sai mã giảng viên !!!";
                        return Json(0);
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
                            string output = "Đăng kí thành công !!!";
                            return Json(output, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            string output = "Mật khẩu và nhập lại mật khẩu không đúng !!!";
                            return Json(output, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string output = "Nhập sai mã sinh viên !!!";
                        return Json(output, JsonRequestBehavior.AllowGet);

                    }
                }

            }
            else
            {
                string output = "Tên tài khoản đã tồn tại !!!";
                return Json(output, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult forget(string idfg)
        {

            var test1 = data.TAIKHOANs.Where(x => x.USERNAME.Equals(idfg)).FirstOrDefault();

            if (test1 != null)
            {
                if (test1.EMAIL != null) 
                {
                    sendEmail(test1.EMAIL, test1.PASSWORD, test1.Name);
                    string output = "Bạn hãy kiểm tra email của mình để lấy lại mật khẩu !!!";
                    return Json(output, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string output = "Tài khoản của bạn chưa có email hãy liên hệ văn phòng khoa để có thể lấy lại tài khoản !!!";
                    return Json(output, JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                string output = "Tài khoản của bạn không tồn tại !!!";
                return Json(output, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult sendEmail(string toemail, string emailbody, string name)
        {

            bool result = false;

            result = sendEmailkq(toemail, "Tìm mật khẩu","<p>Xin chào  " +name + "</p>" + " </br> <p>Nhấn vào link để đổi mật khẩu https://localhost:44349/Account/FindAccount/" + emailbody + "</p>");


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool sendEmailkq(string toemail, string subject, string emailbody)
        {
            try
            {

                string senderEmail = "hoang080699@gmail.com";
                string passw = "Hoang_99_99";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, passw);

                MailMessage mailMessage = new MailMessage(senderEmail, toemail, subject, emailbody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);

                return true;
            }
            catch (Exception e) { return false; }
        }

        public ActionResult FindAccount(string id) 
        {
            var datadb = data.GIANGVIENs.Where(x => x.PASSWORD.Equals(id)).FirstOrDefault();
            ViewBag.nameacc = datadb.TEN;
            ViewBag.id = datadb.ID;
            return View();
        }

        public JsonResult FindAccountjson (string id, string newpass)
        {
            var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(id)).FirstOrDefault();
            string c = Encodemd5(newpass);
            result.PASSWORD = c;
            data.SaveChanges();
            string output = "Đổi mật khẩu thành công";
            return Json (output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account a)
        {
            // chưa có chức năng đăng nhập sinh viên
            var check1 = data.GIANGVIENs.Where(x => x.STATUS == 0 && x.ID.Equals(a.UserName)).FirstOrDefault();
            if (check1 != null)
            {
                string c = Encodemd5(a.PassWord);

                var test1 = data.GIANGVIENs.Where(x => x.ID.Equals(a.UserName) && x.PASSWORD.Equals(c)).FirstOrDefault();
                if (test1 != null)
                {
                    if (test1.ROLE == 1)
                    {
                        Session["Login"] = test1;
                        return RedirectToAction("Index", "Manage", new { n = test1.TEN });
                    }
                    else if (test1.ROLE == 2)
                    {
                        Session["Login"] = test1;
                        return RedirectToAction("ManageTeacherIndex", "Manage", new { n = test1.TEN });
                    }
                    else
                    {
                        ViewBag.a = test1.TEN;
                        Session["Login"] = test1;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "error";
                    ViewBag.a = "Sai tên tài khoản hoặc mật khẩu";
                }
            }
            else {
                var checkid = data.GIANGVIENs.Where(x => x.ID == a.UserName).FirstOrDefault();
                if (checkid != null && a.PassWord == a.UserName)
                {
                    if (checkid.ROLE == 1)
                    {
                        Session["Login"] = checkid;
                        return RedirectToAction("Index", "Manage", new { n = checkid.TEN });
                    }
                    // dang nhap lan dau role == 2
                    else
                    {
                        Session["Login"] = checkid;
                        return RedirectToAction("ManageTeacherIndex", "Manage", new { n = checkid.TEN });
                    }
                }
                else {
                    ViewBag.error = "error";
                    ViewBag.a = "Sai tên tài khoản hoặc mật khẩu";
                }

            }
            return View();


        }

        public ActionResult ProfileAcount()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                GIANGVIEN b = (GIANGVIEN)Session["Login"];
                ViewBag.name = b.TEN;

                var result = data.GIANGVIENs.Where(x => x.TEN.Equals(b.TEN)).FirstOrDefault();
                
                ViewBag.id = result.ID;
                ViewBag.ten = result.TEN;
                ViewBag.pw = result.PASSWORD;
                ViewBag.chucvu = result.CHUCVU;
                ViewBag.email = result.Email;
                return View();

            }

        }
        [HttpPost]
        public ActionResult ProfileAcount(string id, string ten, string chucvu, string email)
        {
            try
            {
                var result = data.GIANGVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                result.TEN = ten;
                result.Email = email;
                data.SaveChanges();
                var result1 = data.GIANGVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                ViewBag.id = result1.ID;
                ViewBag.ten = result1.TEN;
                ViewBag.pw = result1.PASSWORD;
                ViewBag.chucvu = result1.CHUCVU;
                ViewBag.email = result1.Email;
                ViewBag.thanhcong = 1;
                return View(result1);
            }
            catch {
                var result1 = data.GIANGVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                ViewBag.id = result1.ID;
                ViewBag.ten = result1.TEN;
                ViewBag.pw = result1.PASSWORD;
                ViewBag.chucvu = result1.CHUCVU;
                ViewBag.email = result1.Email;
                ViewBag.thanhcong = 0;
                return View(result1);
            }
            
        }

        public ActionResult ChangePw()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                GIANGVIEN b = (GIANGVIEN)Session["Login"];
                ViewBag.name = b.TEN;

                var result = data.GIANGVIENs.Where(x => x.TEN.Equals(b.TEN)).FirstOrDefault();
                ViewBag.id = result.ID;
                return View();

            }

        }
        [HttpPost]
        public ActionResult ChangePw(string id, string pw, string repw)
        {
            var result = data.GIANGVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
            if (pw.Equals(repw))
            {
                string pwNew = Encodemd5(pw);
                result.PASSWORD = pwNew;
                result.STATUS = 0;
                ViewBag.thanhcong = "thanhcong";
                data.SaveChanges();
            }
            else {
                ViewBag.thatbai = "thatbai";
            }
            
            return View(result);
        }



        public string Encodemd5(string a)
        {
            string key = "hutech";
            var data = Encoding.UTF8.GetBytes(a);

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keys = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                using (var tripDes = new TripleDESCryptoServiceProvider { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    var transform = tripDes.CreateEncryptor();
                    var results = transform.TransformFinalBlock(data, 0, data.Length);
                    string q = Convert.ToBase64String(results, 0, results.Length);
                    return q;
                }
            }
        }

        public string Decriptmd5(string a)
        {
            string key = "hutech";
            byte[] data = UTF8Encoding.UTF8.GetBytes(a);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }


      

    }
}