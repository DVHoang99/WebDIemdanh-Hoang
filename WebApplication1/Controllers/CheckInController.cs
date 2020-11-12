using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CheckInController : Controller
    {
        WEBATTENDANCEEntities data = new WEBATTENDANCEEntities();
        // GET: CheckIn
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 2)
                {
                    ViewBag.Magv = b.USERNAME;
                    return View();
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập tạo form điểm danh" });
                }
            }
        }
        [HttpPost]
        public ActionResult Create(string magv, int idtkb, int maxacnhan)
        {
            var test1 = data.TKBs.Where(x => x.ID == idtkb).FirstOrDefault();
            if(test1 != null)
            {
                FORMLUUTRU frm = new FORMLUUTRU();
                frm.MAGIANGVIEN = magv;
                frm.MAXACNHAN = maxacnhan;
                frm.IDTKB = idtkb;
                frm.CA = test1.CA;
                data.FORMLUUTRUs.Add(frm);
                data.SaveChanges();
                ViewBag.Mess = "Tạo điểm danh thành công";
                return View();
            }
            else
            {
                ViewBag.Mess = "Kiểm tra lại id thời khóa biểu";
                return View();
            }
            
        }

        public ActionResult TeacherCheckIn()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 2)
                {
                    var test1 = data.FORMLUUTRUs.Where(x => x.MAGIANGVIEN.Equals(b.USERNAME));
                    if(test1 == null)
                    {
                        return RedirectToAction("Create", "CheckIn");
                    }

                    else
                    {
                            return View(test1);
                    }
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập form điểm danh" });
                }
            }
        }
        [HttpPost]
        public ActionResult EditTeacherCheckIn(int id, int maxacnhan, int trangthai)
        {
            var test1 = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
            test1.MAXACNHAN = maxacnhan;
            test1.TRANGTHAI = trangthai;

            return RedirectToAction("TeacherCheckIn", "CheckIn");
        }
        //[HttpPost]
        //public ActionResult DeleteTeacherCheckIn(int id)
        //{
        //    var test1 = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
        //    data.FORMLUUTRUs.Remove(test1);

        //    return RedirectToAction("TeacherCheckIn", "CheckIn");
        //}

        public ActionResult StudentCheckIn()
        {
            if (Session["Login"] == null)
                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];
                if (b.ROLE1 == 3)
                {
                    ViewBag.Masv = b.USERNAME;
                    ViewBag.Tensv = b.Name;
                    var test = data.FORMLUUTRUs.Where(x => x.TRANGTHAI == 1);
                    return View(test);
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập form điểm danh của sinh viên" });
                }
            }
        }

        [HttpPost]
        public ActionResult StudentCheckIn(string magv, string masv, string tensv, string mamh, int maxacnhan, int id)
        {
            var test1 = data.FORMLUUTRUs.Where(x => x.ID == id && x.TRANGTHAI == 1).FirstOrDefault();
            if(test1 != null )
            {
                DateTime now = DateTime.Now;
                var test2 = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.MAMON.Equals(mamh) && x.NGAYDIENDANH == now).FirstOrDefault();
                if(test2 == null)
                {
                    DIEMDANH diemdanh = new DIEMDANH();
                    diemdanh.MASINHVIEN = masv;
                    diemdanh.TENSINHVIEN = tensv;
                    diemdanh.MAGIANGVIEN = magv;
                    diemdanh.MAMON = mamh;
                    diemdanh.NGAYDIENDANH = now;
                    data.DIEMDANHs.Add(diemdanh);
                    data.SaveChanges();
                    ViewBag.Mess = "Đã điểm danh thành công";
                    return RedirectToAction("StudentCheckIn", "CheckIn");
                }    
                else
                {
                    ViewBag.Mess = "Đã điểm";
                    return RedirectToAction("StudentCheckIn", "CheckIn");
                }    
                
            }
            else
            {
                ViewBag.Mess = "Form điểm danh đã đóng";
                return RedirectToAction("StudentCheckIn", "CheckIn");
            }
            
             
        }
    }
}