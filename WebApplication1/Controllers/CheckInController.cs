using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
            if (test1 != null)
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
                    
                    if (test1 == null)
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
        public ActionResult EditTeacherCheckIn(int id, int maxacnhan)
        {
            var test1 = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
            test1.MAXACNHAN = maxacnhan;
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

        //public ActionResult StudentCheckIn()
        //{
        //    if (Session["Login"] == null)
        //        return RedirectToAction("Login", "Account");
        //    else
        //    {
        //        TAIKHOAN b = (TAIKHOAN)Session["Login"];
        //        if (b.ROLE1 == 3)
        //        {
        //            ViewBag.Masv = b.USERNAME;
        //            ViewBag.Tensv = b.Name;
        //            var test = data.FORMLUUTRUs.Where(x => x.TRANGTHAI == 1);
        //            return View(test);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Message", new { tenaction = "Không thể truy cập form điểm danh của sinh viên" });
        //        }
        //    }
        //}

        [HttpPost]
        public ActionResult QrCodeGenarate(string txtQRCode, int id)
        {
            Random r = new Random();
            int a = r.Next(100000, 1000000);
            var result = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.id = id;
            ViewBag.Tengv = result.GIANGVIEN.TEN;
            ViewBag.Tenmh = result.TKB.MONHOC.TENMONHOC;
            result.TRANGTHAI = 1;
            result.MAXACNHAN = a;
            ViewBag.Code = result.MAXACNHAN;
            data.SaveChanges();
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
                }
            }
            return View();
        }

        public ActionResult Form(int id, int b)
        {
            var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
            if(a.TRANGTHAI != 1)
            {
                return RedirectToAction("Message","Manage", new { tenaction = "Chưa mở điểm danh" });
            }
            else
            {
                ViewBag.id = id;

                var result = data.FORMLUUTRUs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                ViewBag.idtkb = result.IDTKB;
                ViewBag.idgiangvien = result.GIANGVIEN.ID;
                ViewBag.idmh = result.TKB.MAMH;
                ViewBag.a = result.GIANGVIEN.TEN;
                ViewBag.b = result.TKB.MONHOC.TENMONHOC;
                ViewBag.Code = result.MAXACNHAN;
                ViewBag.Buoidiemdanh = b;
                return View();
            }
            
        }

        //[HttpPost]
        //public ActionResult Form(int id, int idtkb, string idgv, string masv, string tensv, int maxn)
        //{
        //    var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
        //    if (a.TRANGTHAI == 1)
        //    {
        //        if (a.MAXACNHAN == maxn)
        //        {

        //            DateTime now = DateTime.Now;

        //            var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb && x.NGAYDIEMDANH == now).FirstOrDefault();
        //            if (test == null)
        //            {
        //                DIEMDANH d = new DIEMDANH();
        //                d.MASINHVIEN = masv;
        //                d.TENSINHVIEN = tensv;
        //                d.MAGIANGVIEN = idgv;
        //                d.NGAYDIEMDANH = now;
        //                d.IDTKB = idtkb;
        //                d.CA = a.CA;

        //                data.DIEMDANHs.Add(d);
        //                data.SaveChanges();
        //                ViewBag.Result = "Điểm danh thành công";
        //                return View();

        //                //return RedirectToAction("Message","Manage", new { tenaction = "Điểm danh thành công!!!" });
        //            }
        //            else
        //            {
        //                ViewBag.Result = "Bạn đã điểm danh";
        //                return View();

        //            }

        //        }
        //        else
        //        {
        //            ViewBag.Result = "Mã xác nhận sai !!! Hãy thử lại";
        //            return View();
        //        }

        //    }
        //    else
        //    {
        //        ViewBag.Result = "Quá hạn điểm danh !!!";
        //        return View();

        //    }

        //}

        [HttpPost]
        public JsonResult DiemDanh(int id, int idtkb, string magv, string masv, string tensv, int code, int buoidiemdanh)
        {
            try
            {
                var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
                if (a.TRANGTHAI == 1)
                {
                    if (a.MAXACNHAN == code)
                    {
                        if(buoidiemdanh == 1)
                        {
                            DateTime now = DateTime.Now;

                            var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb && x.NGAYDIEMDANH1 == now).FirstOrDefault();
                            if (test == null)
                            {
                                DIEMDANH d = new DIEMDANH();
                                d.MASINHVIEN = masv;
                                d.TENSINHVIEN = tensv;
                                d.MAGIANGVIEN = magv;
                                d.NGAYDIEMDANH1 = now;
                                d.IDTKB = idtkb;
                                d.CA = a.CA;

                                data.DIEMDANHs.Add(d);
                                data.SaveChanges();

                                return Json(1);
                            }
                            else
                            {

                                return Json(0);

                            }
                        }  
                        else if(buoidiemdanh == 2)
                        {
                            DateTime now = DateTime.Now;

                            var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb).FirstOrDefault();
                            if (test != null)
                            {
                                test.NGAYDIEMDANH2 = now;
                                data.SaveChanges();

                                return Json(1);
                            }
                            else
                            {
                                return Json(0);

                            }
                        }
                        else if (buoidiemdanh == 3)
                        {
                            DateTime now = DateTime.Now;

                            var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb).FirstOrDefault();
                            if (test != null)
                            {
                                test.NGAYDIEMDANH3 = now;
                                data.SaveChanges();

                                return Json(1);
                            }
                            else
                            {

                                return Json(0);

                            }
                        }
                        else if (buoidiemdanh == 4)
                        {
                            DateTime now = DateTime.Now;

                            var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb).FirstOrDefault();
                            if (test != null)
                            {
                                test.NGAYDIEMDANH4 = now;
                                data.SaveChanges();

                                return Json(1);
                            }
                            else
                            {

                                return Json(0);

                            }
                        }
                        else
                        {
                            DateTime now = DateTime.Now;

                            var test = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(masv) && x.IDTKB == idtkb).FirstOrDefault();
                            if (test != null)
                            {
                                test.NGAYDIEMDANH5 = now;
                                data.SaveChanges();

                                return Json(1);
                            }
                            else
                            {

                                return Json(0);

                            }
                        }



                    }
                        else
                        {

                            return Json(0);
                        }

                    }
                    else
                    {

                        return Json(0);
                    }
                
            }
            catch
            {
                return Json(0);
            }

        }

        [HttpPost]
        public JsonResult Suathongtin(int id, int trangthai)
        {
            try
            {
                var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
                if(a.NGAYHOCTHU == 5)
                {
                    a.NGAYHOCTHU = 1;
                }    
                else
                {
                    a.NGAYHOCTHU = a.NGAYHOCTHU + 1;
                    
                }    
                
                a.TRANGTHAI = trangthai;
                data.SaveChanges();
                return Json(1);
            }
            catch
            { return Json(0); }


        }

        [HttpPost]
        public JsonResult XoaFormLuuTru(int id)
        {
            try
            {
                var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
                data.FORMLUUTRUs.Remove(a);
                data.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }

        public ActionResult kiemtra(int id)
        {
            var a = data.DIEMDANHs.Where(x => x.IDTKB == id);
            return View(a);
        }
        [HttpGet]
        public ActionResult LoadData()
        {
            return View();
        }
        public ActionResult LoadData1()

        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skip number of Rows count  
                var start = Request.Form["start"];
                // Paging Length 10,20  
                var length = Request.Form["length"];
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"];
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"];
                //Paging Size(10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = from s in data.DIEMDANHs select s;
                ////Sorting
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{

                //    if (sortColumnDirection.CompareTo("asc") == 0)
                //    {
                //        switch (sortColumn)
                //        {
                //            case "id":
                //                customerData = from s in data.DIEMDANHs orderby s.ID ascending select s;
                //                break;
                //            case "ten":
                //                customerData = from s in data.DIEMDANHs orderby s.Ten ascending select s;
                //                break;
                //            case "soluong":
                //                customerData = from s in data.DIEMDANHs orderby s.SoLuong ascending select s;
                //                break;
                //            default:
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        switch (sortColumn)
                //        {
                //            case "id":
                //                customerData = from s in data.SanPhams orderby s.ID descending select s;
                //                break;
                //            case "ten":
                //                customerData = from s in data.SanPhams orderby s.Ten descending select s;
                //                break;
                //            case "soluong":
                //                customerData = from s in data.SanPhams orderby s.SoLuong descending select s;
                //                break;


                //            default:
                //                break;
                //        }
                //    }


                //}
                ////Search  
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    customerData = customerData.Where(m => /*(m.ID.Co(searchValue)) ||*/ (m.Ten.Contains(searchValue)) /*|| (m.SoLuong.Contains(searchValue)*/);
                //}
                //total number of rows count   
                recordsTotal = customerData.ToList().Count();
                //Paging   
                var kq = customerData.ToList().Skip(skip).Take(pageSize);
                var dssp = new List<CheckIn>();
                string[] listQuocGia = new string[customerData.Count()];
                foreach (var item in kq)
                {
                    CheckIn sp = new CheckIn();
                    sp.MASINHVIEN = item.MASINHVIEN;
                    sp.TENSINHVIEN = item.TENSINHVIEN;
                    sp.NGAYDIENDANH1 = item.NGAYDIEMDANH1.ToString();
                    sp.NGAYDIENDANH2 = item.NGAYDIEMDANH2.ToString();
                    sp.NGAYDIENDANH3 = item.NGAYDIEMDANH3.ToString();
                    sp.NGAYDIENDANH4 = item.NGAYDIEMDANH4.ToString();
                    sp.NGAYDIENDANH5 = item.NGAYDIEMDANH5.ToString();

                    dssp.Add(sp);
                }
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

    }
}