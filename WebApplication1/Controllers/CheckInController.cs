using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
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
            data.SaveChanges();

            return RedirectToAction("TeacherCheckIn", "CheckIn");
        }
        [HttpGet]
        public ActionResult Test(int id, string xyz, int abc)
        {
            ViewBag.id = id;
            ViewBag.gv = xyz;
            ViewBag.maxacnhan = abc;
            return View();
        }

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

        //[HttpPost]
        //public ActionResult StudentCheckIn(string magv, string masv, string tensv, string mamh, int maxacnhan, int id)
        //{
        //    var test1 = data.FORMLUUTRUs.Where(x => x.ID == id && x.TRANGTHAI == 1).FirstOrDefault();
        //    if(test1 != null )
        //    {
        //        DateTime now = DateTime.Now;
        //        var test2 = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.MAMON.Equals(mamh) && x.NGAYDIENDANH == now).FirstOrDefault();
        //        if(test2 == null)
        //        {
        //            DIEMDANH diemdanh = new DIEMDANH();
        //            diemdanh.MASINHVIEN = masv;
        //            diemdanh.TENSINHVIEN = tensv;
        //            diemdanh.MAGIANGVIEN = magv;
        //            diemdanh.MAMON = mamh;
        //            diemdanh.NGAYDIENDANH = now;
        //            data.DIEMDANHs.Add(diemdanh);
        //            data.SaveChanges();
        //            ViewBag.Mess = "Đã điểm danh thành công";
        //            return RedirectToAction("StudentCheckIn", "CheckIn");
        //        }    
        //        else
        //        {
        //            ViewBag.Mess = "Đã điểm";
        //            return RedirectToAction("StudentCheckIn", "CheckIn");
        //        }    

        //    }
        //    else
        //    {
        //        ViewBag.Mess = "Form điểm danh đã đóng";
        //        return RedirectToAction("StudentCheckIn", "CheckIn");
        //    }


        //}
        [HttpPost]
        public ActionResult QrCodeGenarate(string txtQRCode)
        {
            ViewBag.txtQRCode = txtQRCode;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(txtQRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ViewBag.imageBytes = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(int id)
        {
            ViewBag.id = id;

            var result = data.FORMLUUTRUs.Where(x => x.ID.Equals(id)).FirstOrDefault();
            ViewBag.idtkb = result.IDTKB;
            ViewBag.idgiangvien = result.GIANGVIEN.ID;
            ViewBag.idmh = result.TKB.MAMH;
            ViewBag.a = result.GIANGVIEN.TEN;
            ViewBag.b = result.TKB.MONHOC.TENMONHOC; 
            return View();
        }


        [HttpPost]
        public ActionResult Form(int id, int idtkb, string idgv, string masv, string tensv, int maxn)
        {
            var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
            if(a.TRANGTHAI == 1)
            {
                if(a.MAXACNHAN == maxn)
                {
                    
                    DateTime now = DateTime.Now;

                    var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb && x.NGAYDIEMDANH == now).FirstOrDefault();
                    if(test == null)
                    {
                        DIEMDANH d = new DIEMDANH();
                        d.MASINHVIEN = masv;
                        d.TENSINHVIEN = tensv;
                        d.MAGIANGVIEN = idgv;
                        d.NGAYDIEMDANH = now;
                        d.IDTKB = idtkb;
                        d.CA = a.CA;

                        data.DIEMDANHs.Add(d);
                        data.SaveChanges();
                        return RedirectToAction("Message","Manage", new { tenaction = "Điểm danh thành công!!!" });
                    }   
                    else
                    {
                        return RedirectToAction("Message","Manage", new { tenaction = "Bạn đã điểm danh" });
                    }    
                    
                }
                else
                {
                    return RedirectToAction("Message","Manage", new { tenaction = "Mã xác nhận sai !!! Hãy thử lại" });
                }
                
            }
            else
            {
                return RedirectToAction("Message","Manage", new { tenaction = "Quá hạn điểm danh !!!" });
            }
            
        }


    }
}