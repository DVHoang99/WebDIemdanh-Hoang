using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ManageController : Controller
    {
        WEBATTENDANCEEntities data = new WEBATTENDANCEEntities();
        // GET: Manage
        //role = 1 (admin) || role = 2 (teacher) || role = 3 (student)
        public ActionResult Redirect()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                ViewBag.Logout = "";
                TAIKHOAN b = (TAIKHOAN)Session["Login"];
                if (b.ROLE1 == 1)
                {
                    return RedirectToAction("Index", "Manage");
                }
                else /*if (b.ROLE1 == 2)*/
                {
                    return RedirectToAction("ManageTeacherIndex", "Manage");
                }
                //else 
                //{
                //    return View("StudentIndex", "Manage" );
                //}

            }
            
        }
        public ActionResult Index()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 1)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập" });
                }
            }
            
        }
        //=======================         Account        =======================//
        public ActionResult ManageAccount()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 1)
                {
                    var v = data.TAIKHOANs;
                    return View(v);
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý tài khoản" });
                }
            }

        }

        public ActionResult Details(string id)
        {
            if (id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(id)).FirstOrDefault();
                        return View(result);
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý tài khoản" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn tài khoản muốn sửa thông tin" });
            }

           
        }

        public ActionResult Delete(string id)
        {
            if (id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(id)).FirstOrDefault();
                        data.TAIKHOANs.Remove(result);
                        data.SaveChanges();
                        return RedirectToAction("ManageAccount", "Manage");
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý tài khoản" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn tài khoản muốn xóa thông tin" });
            }
            
        }

        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(id)).FirstOrDefault();
                        return View(result);
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý tài khoản" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn tài khoản muốn sửa thông tin" });
            }
            
        }

        [HttpPost]
        public ActionResult Edit(Account a)
        {
            var result = data.TAIKHOANs.Where(x => x.USERNAME.Equals(a.UserName)).FirstOrDefault();
            data.TAIKHOANs.Remove(result);
            data.SaveChanges();

            TAIKHOAN t = new TAIKHOAN();
            t.USERNAME = a.UserName;
            t.PASSWORD = a.PassWord;
            t.Name = a.Name;
            t.ROLE1 = a.Role;
            data.TAIKHOANs.Add(t);
            data.SaveChanges();
            var result1 = data.TAIKHOANs.Where(x => x.USERNAME.Equals(a.UserName)).FirstOrDefault();
            ViewBag.a = "Sửa thông tin thành công !!!";
           return View(result1);

        }

        //=======================         Student        =======================//
        
        public ActionResult AddListStudent()
        {

            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 1)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập vào đây" });
                }
            }
        }

        [HttpPost]
        public ActionResult AddListStudent(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;

                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }

                    conString = @"Data Source=DESKTOP-TB2RUF7;Initial Catalog=WEBATTENDANCE;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.SINHVIEN";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("ID", "ID");
                            sqlBulkCopy.ColumnMappings.Add("TEN", "TEN");
                            sqlBulkCopy.ColumnMappings.Add("TENLOP", "TENLOP");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    //if the code reach here means everthing goes fine and excel data is imported into database
                    ViewBag.Mess = "Thêm thành công";

                    return View();
                }
                else
                {
                    ViewBag.Mess = "Bạn phải chọn file!!!";
                    return View();
                }    
                
            }
            catch (Exception e)
            {

                ViewBag.Mess = e.Message;
                return View();
            }
            
        }

        public ActionResult ManageStudent()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 1)
                {
                    var v = data.SINHVIENs;
                    return View(v);
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý sinh viên" });
                }
            }
        }

        public ActionResult ManageStudent1()
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
                var customerData = from s in data.SINHVIENs select s;

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => (m.ID.Contains(searchValue)) || (m.TEN.Contains(searchValue)) || (m.TENLOP.Contains(searchValue)));
                }
                //total number of rows count   
                recordsTotal = customerData.ToList().Count();
                //Paging   
                var kq = customerData.ToList().Skip(skip).Take(pageSize);
                var dssp = new List<CheckIn>();
                foreach (var item in kq)
                {
                    CheckIn sp = new CheckIn();
                    sp.MASINHVIEN = item.ID;
                    sp.TENSINHVIEN = item.TEN;
                    sp.Lop = item.TENLOP;
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
        public ActionResult EditStudent(string id)
        {
            if(id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var a = data.SINHVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                        return View(a);
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý sinh viên" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn sinh viên muốn sửa thông tin" });
            }
  
        }
        [HttpPost]
        public ActionResult EditStudent(Student std)
        {
            var result = data.SINHVIENs.Where(x => x.ID.Equals(std.Id)).FirstOrDefault();
            data.SINHVIENs.Remove(result);
            data.SaveChanges();

            SINHVIEN t = new SINHVIEN();
            t.ID = std.Id;
            t.TEN = std.TEN;
            t.TENLOP = std.TENLOP;
            data.SINHVIENs.Add(t);
            data.SaveChanges();
            var result1 = data.SINHVIENs.Where(x => x.ID.Equals(std.Id)).FirstOrDefault();
            ViewBag.a = "Sửa thông tin thành công !!!";
            return View(result1);
        }

        public ActionResult DetailsStudent(string id)
        {
            if (id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var a = data.SINHVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                        return View(a);
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý sinh viên" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn sinh viên muốn xem thông tin" });
            }
            
        }

        //public ActionResult DeleteStudent(Student std)
        //{
        //    var result = data.SINHVIENs.Where(x => x.USERNAME.Equals(id)).FirstOrDefault();
        //    data.TAIKHOANs.Remove(result);
        //    data.SaveChanges();
        //    return RedirectToAction("ManageAccount", "Manage");
        //}


        [HttpPost]
        public JsonResult suasinhvien(string id, string name, string lop)
        {
            try
            {
                var a = data.SINHVIENs.Where(x => x.ID == id).FirstOrDefault();
                if (a != null)
                {
                    a.TEN = name;
                    a.TENLOP = lop;
                    data.SaveChanges();

                    return Json(1);

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





        //=======================         Teacher        =======================//
        public ActionResult AddListTeacher()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddListTeacher(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;

                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }

                    conString = @"Data Source=DESKTOP-TB2RUF7;Initial Catalog=WEBATTENDANCE;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.GIANGVIEN";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("ID", "ID");
                            sqlBulkCopy.ColumnMappings.Add("TEN", "TEN");
                            sqlBulkCopy.ColumnMappings.Add("CHUCVU", "CHUCVU");
                            sqlBulkCopy.ColumnMappings.Add("BANGCAP", "BANGCAP");
                            sqlBulkCopy.ColumnMappings.Add("MADONVI", "MADONVI");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    //if the code reach here means everthing goes fine and excel data is imported into database
                    ViewBag.Mess = "Thêm thành công";

                    return View();
                }
                else
                {
                    ViewBag.Mess = "Bạn phải chọn file!!!";
                    return View();
                }

            }
            catch (Exception e)
            {

                ViewBag.Mess = e.Message;
                return View();
            }
           
        }

        [HttpPost]
        public JsonResult suagiangvien(string id, string name, string chucvu, string bangcap)
        {
            try
            {
                var a = data.GIANGVIENs.Where(x => x.ID == id).FirstOrDefault();
                if (a != null)
                {
                    a.TEN = name;
                    a.CHUCVU = chucvu;
                    a.BANGCAP = bangcap;
                    data.SaveChanges();

                    return Json(1);

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

        public ActionResult ManageTeacher()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 1)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập quản lý giảng viên" });
                }
            }
        }
       
        public ActionResult ManageTeacher1()
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
                var customerData = from s in data.GIANGVIENs select s;

                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => (m.ID.Contains(searchValue)) || (m.TEN.Contains(searchValue)) || (m.CHUCVU.Contains(searchValue))|| (m.BANGCAP.Contains(searchValue))|| (m.DONVI.TENDONVI.Contains(searchValue)));
                }
                //total number of rows count   
                recordsTotal = customerData.ToList().Count();
                //Paging   
                var kq = customerData.ToList().Skip(skip).Take(pageSize);
                var dssp = new List<Teacher>();
                foreach (var item in kq)
                {
                    Teacher sp = new Teacher();
                    sp.ID = item.ID;
                    sp.TEN = item.TEN;
                    sp.CHUCVU = item.CHUCVU;
                    sp.BANGCAP = item.BANGCAP;
                    sp.DONVI = item.DONVI.TENDONVI;

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



        public ActionResult EditTeacher(string id)
        {
            if (id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var a = data.GIANGVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                        return View(a);
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý giảng viên" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn giảng viên muốn sửa thông tin" });
            }

        }
        [HttpPost]
        public ActionResult EditTeacher(Teacher tch)
        {
            var result = data.GIANGVIENs.Where(x => x.ID.Equals(tch.ID)).FirstOrDefault();
            data.GIANGVIENs.Remove(result);
            data.SaveChanges();

            GIANGVIEN t = new GIANGVIEN();
            t.ID = tch.ID;
            t.TEN = tch.TEN;
            t.BANGCAP = tch.BANGCAP;
            t.CHUCVU = tch.CHUCVU;

            data.GIANGVIENs.Add(t);
            data.SaveChanges();
            var result1 = data.GIANGVIENs.Where(x => x.ID.Equals(tch.ID)).FirstOrDefault();
            ViewBag.a = "Sửa thông tin thành công !!!";
            return View(result1);
        }

       
        public ActionResult DeleteTeacher(string id)
        {
            if (id != null)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var a = data.GIANGVIENs.Where(x => x.ID.Equals(id)).FirstOrDefault();
                        data.GIANGVIENs.Remove(a);
                        return RedirectToAction("ManageTeacher", "Manage");
                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Tài khoản của bạn không có quyền truy cập quản lý giảng viên" });
                    }
                }
            }
            else
            {
                return RedirectToAction("Message", new { tenaction = "Bạn phải chọn giảng viên muốn sửa thông tin" });
            }
        }
        //=======================         schedule        =======================//

        public ActionResult AddSchedule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSchedule(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;

                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }

                    conString = @"Data Source=DESKTOP-TB2RUF7;Initial Catalog=WEBATTENDANCE;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.TKB";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("ID", "ID");
                            sqlBulkCopy.ColumnMappings.Add("NHOM", "NHOM");
                            sqlBulkCopy.ColumnMappings.Add("MAMH", "MAMH");
                            sqlBulkCopy.ColumnMappings.Add("MAGIANGVIEN", "MAGIANGVIEN");
                            sqlBulkCopy.ColumnMappings.Add("PHONG", "PHONG");
                            sqlBulkCopy.ColumnMappings.Add("TENLOP", "TENLOP");
                            sqlBulkCopy.ColumnMappings.Add("NGAYBATDAU", "NGAYBATDAU");
                            sqlBulkCopy.ColumnMappings.Add("NGAYKETHUC", "NGAYKETHUC");
                            sqlBulkCopy.ColumnMappings.Add("CA", "CA");
                            sqlBulkCopy.ColumnMappings.Add("THU", "THU");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    //if the code reach here means everthing goes fine and excel data is imported into database
                    ViewBag.Mess = "Thêm thành công";

                    return View();
                }
                else
                {
                    ViewBag.Mess = "Bạn phải chọn file!!!";
                    return View();
                }

            }
            catch (Exception e)
            {

                ViewBag.Mess = e.Message;
                return View();
            }
        }

        public ActionResult ManageSchedule(int id)
        {
            if (id != 0)
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var a = data.NHOMs;
                        ViewBag.a = id;
                        return View(a);

                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Không thể truy cập quản lý thời khóa biểu" });
                    }
                }
            }
            else
            {
                if (Session["Login"] == null)

                    return RedirectToAction("Login", "Account");
                else
                {
                    TAIKHOAN b = (TAIKHOAN)Session["Login"];

                    if (b.ROLE1 == 1)
                    {
                        var a = data.NHOMs;
                        ViewBag.a = id;
                        return View(a);

                    }
                    else
                    {
                        return RedirectToAction("Message", new { tenaction = "Không thể truy cập quản lý thời khóa biểu" });
                    }
                }
            }
            
        }

        public ActionResult ManageSchedule1(int id)
        {
            try
            {
                if(id == 0)
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
                    var customerData = from s in data.TKBs select s;

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => (m.GIANGVIEN.TEN.Contains(searchValue)) /*|| (m.CA.Contains(searchValue)) */);
                    }
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
                        sp.magv = item.MAGIANGVIEN;
                        sp.tengv = item.GIANGVIEN.TEN;
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
                else{
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
                    var customerData = data.TKBs.Where(x => x.NHOM == id);

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => (m.GIANGVIEN.TEN.Contains(searchValue)) /*|| (m.CA.Contains(searchValue)) */);
                    }
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
                        sp.magv = item.MAGIANGVIEN;
                        sp.tengv = item.GIANGVIEN.TEN;
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
            catch (Exception ex)
            {
                throw;
            }
        }


        //=======================         AddInfoSubject        =======================//

        public ActionResult AddInfoSubject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddInfoSubject(HttpPostedFileBase postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;

                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }

                    conString = @"Data Source=DESKTOP-TB2RUF7;Initial Catalog=WEBATTENDANCE;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.CHITIETMONHOC";

                            // Map the Excel columns with that of the database table, this is optional but good if you do
                            // 
                            sqlBulkCopy.ColumnMappings.Add("IDTKB", "IDTKB");
                            sqlBulkCopy.ColumnMappings.Add("MASV", "MASV");
                            sqlBulkCopy.ColumnMappings.Add("CA", "CA");

                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    //if the code reach here means everthing goes fine and excel data is imported into database
                    ViewBag.Mess = "Thêm thành công";

                    return View();
                }
                else
                {
                    ViewBag.Mess = "Bạn phải chọn file!!!";
                    return View();
                }

            }
            catch (Exception e)
            {

                ViewBag.Mess = e.Message;
                return View();
            }


        }

        public ActionResult LoadDataInfoSub(int id)
        {
            ViewBag.id = id;
            return View();
        }
        
        public ActionResult LoadDataInfoSub1(int id)
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
                var customerData = data.CHITIETMONHOCs.Where(x => x.IDTKB == id);

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
                //////Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => (m.MASV.Contains(searchValue)) || (m.SINHVIEN.TEN.Contains(searchValue)) /*|| (m.SoLuong.Contains(searchValue)*/);
                }
                //total number of rows count   
                recordsTotal = customerData.ToList().Count();
                //Paging   
                var kq = customerData.ToList().Skip(skip).Take(pageSize);
                var dssp = new List<InfoSub>();
                foreach (var item in kq)
                {
                    int dem = 0;
                    InfoSub sp = new InfoSub();
                    sp.MaSv = item.MASV;
                    sp.TenSv = item.SINHVIEN.TEN;
                    
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


        //=======================         Message        =======================//
        public ActionResult Message(string tenaction)
        {
            ViewBag.tenaction = tenaction;
            return View();
        }

        //=======================         ManageTeacherIndex        =======================//

        public ActionResult ManageTeacherIndex()
        {
            if (Session["Login"] == null)

                return RedirectToAction("Login", "Account");
            else
            {
                TAIKHOAN b = (TAIKHOAN)Session["Login"];

                if (b.ROLE1 == 2)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Message", new { tenaction = "Không thể truy cập" });
                }
            }
            
        }
        //[HttpPost]
        //public ActionResult EditProfileAccount(string id, string password, string name)
        //{
        //    var test = data.TAIKHOANs.Where(x => x.USERNAME.Equals(id)).FirstOrDefault();
        //    TAIKHOAN tk = new TAIKHOAN();
        //    tk.PASSWORD = password;
        //    tk.Name = name;
        //    data.SaveChanges();
        //    return View();
        //}

        public ActionResult LogOut()
        {
            Session["Login"] = null;
            return RedirectToAction("Login", "Account");
        }


    }

}