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
        public ActionResult Create(string magv, int idtkb)
        {
            var test1 = data.TKBs.Where(x => x.ID == idtkb).FirstOrDefault();
            if (test1 != null)
            {
                FORMLUUTRU frm = new FORMLUUTRU();
                frm.MAGIANGVIEN = magv;
                
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

        public ActionResult TeacherCheckIn(int id)
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                GIANGVIEN b = (GIANGVIEN)Session["Login"];
                ViewBag.name = b.TEN;
                if (b.ROLE == 2)
                {
                    var a = data.NHOMs;
                    ViewBag.a = id;
                    ViewBag.b = b.ID;
                    ViewBag.idmonhoc = null;
                    ViewBag.tenmonhoc = null;
                    return View(a);
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập form điểm danh" });
                }
            }
        }
        [HttpPost]
        public ActionResult TeacherCheckIn(int id, string idmonhoc, string tenmonhoc)
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                GIANGVIEN b = (GIANGVIEN)Session["Login"];

                if (b.ROLE == 2)
                {
                    var a = data.NHOMs;
                    ViewBag.a = id;
                    ViewBag.b = b.ID;
                    ViewBag.idmonhoc = idmonhoc;
                    ViewBag.tenmonhoc = tenmonhoc;
                    return View(a);
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập form điểm danh" });
                }
            }
        }


        public ActionResult ManageSchedule1(int id, string username, string idmonhoc, string tenmonhoc)
        {
            try
            {
                var result = data.TKBs;

                if (id == 0)
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

                    if (idmonhoc == "" && tenmonhoc == "")
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    else if (idmonhoc != "" && tenmonhoc == "")
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username) && x.MONHOC.IDMONHOC.Equals(idmonhoc));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    else if (idmonhoc == "" && tenmonhoc != "")
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username) && x.MONHOC.TENMONHOC.Equals(tenmonhoc) );
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    else {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username) && x.MONHOC.TENMONHOC.Equals(tenmonhoc) && x.MONHOC.IDMONHOC.Equals(tenmonhoc));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }                    
                }
                else
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

                    if (idmonhoc == "" && tenmonhoc == "")
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    else if (idmonhoc != "" && tenmonhoc == "")
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username) && x.MONHOC.IDMONHOC.Equals(idmonhoc));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    else if (idmonhoc == "" && tenmonhoc != "")
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username) && x.MONHOC.TENMONHOC.Equals(tenmonhoc));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var customerData = result.Where(x => x.MAGIANGVIEN.Equals(username) && x.MONHOC.TENMONHOC.Equals(tenmonhoc) && x.MONHOC.IDMONHOC.Equals(tenmonhoc));
                        //total number of rows count   
                        recordsTotal = customerData.ToList().Count();
                        //Paging   
                        var kq = customerData.ToList().Skip(skip).Take(pageSize);
                        var dssp = new List<ThoiKhoaBieu>();
                        foreach (var item in kq)
                        {
                            ThoiKhoaBieu sp = new ThoiKhoaBieu();
                            sp.id = item.ID.ToString();
                            sp.mamh = item.MAMH;
                            sp.tenmh = item.MONHOC.TENMONHOC;
                            sp.phong = item.PHONG;
                            sp.lop = item.TENLOP;
                            sp.ngaybatdau = item.NGAYBATDAU.ToString();
                            sp.ngayketthuc = item.NGAYKETHUC.ToString();
                            sp.cahoc = item.CA.ToString();
                            sp.thu = item.THU;
                            sp.nhom = item.NHOM.ToString();

                            dssp.Add(sp);
                        }
                        //Returning Json Data  
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dssp }, JsonRequestBehavior.AllowGet);
                    }
                    }

                }
            catch (Exception ex)
            {
                throw;
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

        [HttpGet]
        public ActionResult QrCodeGenarate(/*string txtQRCode,*/ int id)
        {
            Random r = new Random();
            int a = r.Next(100000, 1000000);
            var result = data.FORMLUUTRUs.Where(x => x.IDTKB == id).FirstOrDefault();
            if(result != null)
            {
                string txtQRCode = "https://localhost:44349/CheckIn/Form/" + result.ID;
                ViewBag.id = result.ID;
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
            else
            {
                var tkb = data.TKBs.Where(x => x.ID == id).FirstOrDefault();
                FORMLUUTRU fr = new FORMLUUTRU();
                fr.MAGIANGVIEN = tkb.MAGIANGVIEN;
                fr.IDTKB = tkb.ID;
                fr.CA = tkb.CA;
                data.FORMLUUTRUs.Add(fr);
                data.SaveChanges();

                return RedirectToAction("QrCodeGenarate", "CheckIn", new { id = id});
            }



        }

        public ActionResult Form(int id)
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
                return View();
            }
            
        }

      

        [HttpPost]
        public JsonResult DiemDanh(int id, int idtkb, string magv, string masv, string tensv, int code)
        {
            try
            {
                var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
                if (a.TRANGTHAI == 1)
                {
                    if (a.MAXACNHAN == code)
                    {
                        var b = data.SINHVIENs.Where(x => x.ID.Equals(masv)).FirstOrDefault();
                        DateTime now = DateTime.Now;
                        DIEMDANH d = new DIEMDANH();
                        d.MASINHVIEN = masv;
                        d.TENSINHVIEN = b.TEN;
                        d.MAGIANGVIEN = magv;
                        d.NGAYDIEMDANH = now;
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
            ViewBag.id = id;
            return View();
        }

        public ActionResult kiemtra1(int id)

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
                var customerData = data.DIEMDANHs.Where(x => x.IDTKB == id);

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => (m.TENSINHVIEN.Contains(searchValue)) || (m.MASINHVIEN.Contains(searchValue)) /*|| (m.SoLuong.Contains(searchValue)*/);
                }
                //total number of rows count   
                recordsTotal = customerData.ToList().Count();
                //Paging   
                var kq = customerData.ToList().Skip(skip).Take(pageSize);
                var dssp = new List<CheckIn>();
                foreach (var item in kq)
                {
                   
                    CheckIn sp = new CheckIn();
                    sp.MASINHVIEN = item.MASINHVIEN;
                    sp.TENSINHVIEN = item.TENSINHVIEN;
                    var kq1 = data.GIANGVIENs.Where(x => x.ID.Equals(item.MAGIANGVIEN)).FirstOrDefault();
                    sp.tengiangvien = kq1.TEN;
                    var kq2 = data.TKBs.Where(x => x.ID == item.IDTKB).FirstOrDefault();
                    sp.tenmonhoc = kq2.MONHOC.TENMONHOC;
                    int results = data.DIEMDANHs.GroupBy(p => p.MASINHVIEN.Equals(item.MASINHVIEN), p => p.NGAYDIEMDANH, (key, g) => new { MASINHVIEN = key, NGAYDIEMDANH = g.Count() }).Count();
                    sp.SoBuoiDiemDanh = results.ToString();
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
        [HttpGet]
        public ActionResult LoadData(int id)
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                GIANGVIEN b = (GIANGVIEN)Session["Login"];
                ViewBag.name = b.TEN;
                if (b.ROLE == 1 || b.ROLE == 2 )
                {
                    var ketqua = data.TKBs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                    ViewBag.tenmonhoc = ketqua.MONHOC.TENMONHOC;
                    ViewBag.id = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập" });
                }
            }
        }

        public ActionResult LoadData1(int id)
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
                var customerData = data.DIEMDANHs.GroupBy(p => new {p.MASINHVIEN, p.TENSINHVIEN, p.IDTKB }).Select(g => new { masv = g.Key.MASINHVIEN, name = g.Key.TENSINHVIEN, tkb = g.Key.IDTKB ,count = g.Count() }).Where(p => p.tkb == id); 
                //var customerData = data.DIEMDANHs.Where(x => x.IDTKB == id);

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    //customerData = customerData.Where(m => (m.TENSINHVIEN.Contains(searchValue)) || (m.MASINHVIEN.Contains(searchValue)) /*|| (m.SoLuong.Contains(searchValue)*/);
                }
                //total number of rows count   
                recordsTotal = customerData.ToList().Count();
                //Paging   
                var kq = customerData.ToList().Skip(skip).Take(pageSize);
                var dssp = new List<CheckIn>();
                foreach (var item in kq)
                {
                    CheckIn sp = new CheckIn();
                    sp.mathoikhoabieu = item.tkb.ToString();
                    //sp.tenmonhoc = item.TKB.MONHOC.TENMONHOC;
                    sp.MASINHVIEN = item.masv;
                    sp.SoBuoiDiemDanh = item.count.ToString();
                    sp.TENSINHVIEN = item.name;
                    //sp.DiemDiemDanh ()
                   // sp.tengiangvien = item.TKB.GIANGVIEN.TEN;
                    //var dk = data.DIEMDANHs.Where(x => x.IDTKB == id && x.MASINHVIEN.Equals(item.MASINHVIEN));
                    //foreach (var item1 in dk)
                    //{
                    //    dem++;
                    //}
                    //sp.SoBuoiDiemDanh = dem;
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

        public ActionResult XemThongtin(string id, int idtkb)
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                GIANGVIEN b = (GIANGVIEN)Session["Login"];
                ViewBag.name = b.TEN;

                if (b.ROLE == 1 || b.ROLE == 2)
                {
                    var a = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(id) && x.IDTKB == idtkb);
                    var b1 = data.DIEMDANHs.Where(x => x.MASINHVIEN.Equals(id) && x.IDTKB == idtkb).FirstOrDefault();
                    ViewBag.tensv = b1.SINHVIEN.TEN;
                    ViewBag.tenmh = b1.TKB.MONHOC.TENMONHOC;
                    return View(a);
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập" });
                }
            }
            

            
            
        }
        [HttpPost]
        public JsonResult Suathongtin(int id, int trangthai)
        {
            try
            {
                var a = data.FORMLUUTRUs.Where(x => x.ID == id).FirstOrDefault();
                a.TRANGTHAI = trangthai;
                data.SaveChanges();
                return Json(1);
            }
            catch
            { return Json(0); }


        }


    }
}