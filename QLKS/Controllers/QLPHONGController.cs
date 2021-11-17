using PagedList;
using QLKS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers
{
    public class QLPHONGController : Controller
    {
        // GET: QLPHONG
        dbQLKSDataContext data = new dbQLKSDataContext();
        public ActionResult QLPhong(int? page)
        {
    
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                //return View(db.SACHes.ToList());
                return View(data.Phongs.ToList().OrderBy(n => n.MaPhong).ToPagedList(pageNumber, pageSize));
            }

        }
        public ActionResult ThemmoiSanpham()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                ViewBag.MALOAI = new SelectList(data.LoaiPhongs.ToList().OrderBy(n => n.TenLP), "MALOAI", "TENLOAI");
                ViewBag.MAKHVUC = new SelectList(data.KHUVUCs.ToList().OrderBy(n => n.TENKV), "MAKHVUC", "TENKV");
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSanpham(Phong p, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MALOAI = new SelectList(data.LoaiPhongs.ToList().OrderBy(n => n.TenLP), "MALOAI", "TENLOAI");
            ViewBag.MAKHVUC = new SelectList(data.KHUVUCs.ToList().OrderBy(n => n.TENKV), "MAKHVUC", "TENKV");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    p.ANH= fileName;
                    data.Phongs.InsertOnSubmit(p);
                    data.SubmitChanges();
                }
                return RedirectToAction("QLPhong");
            }
        }

        //Hiển thị sản phẩm
        public ActionResult ChitietSanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                //Lay ra doi tuong sach theo ma
                Phong sp = data.Phongs.SingleOrDefault(n => n.MaPhong == id);
                ViewBag.Masach = sp.MaPhong;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sp);
            }
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult XoaSanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                Phong sp = data.Phongs.SingleOrDefault(n => n.MaPhong == id);
                ViewBag.Masach = sp.MaPhong;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sp);
            }
        }


        [HttpPost, ActionName("XoaSanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                Phong sp = data.Phongs.SingleOrDefault(n => n.MaPhong == id);
                ViewBag.MASP = sp.MaPhong;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.Phongs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("QLPhong");
            }
        }
        //Chinh sửa sản phẩm
        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                //Lay ra doi tuong sach theo ma
                Phong sp = data.Phongs.SingleOrDefault(n => n.MaPhong == id);
                ViewBag.MASP = sp.MaPhong;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                ViewBag.MALOAI = new SelectList(data.LoaiPhongs.ToList().OrderBy(n => n.TenLP), "MALOAI", "TENLOAI");
                ViewBag.MAKHVUC = new SelectList(data.KHUVUCs.ToList().OrderBy(n => n.TENKV), "MAKHVUC", "TENKV");
                return View(sp);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                //Dua du lieu vao dropdownload
                ViewBag.MALOAI = new SelectList(data.LoaiPhongs.ToList().OrderBy(n => n.TenLP), "MALOAI", "TENLOAI");
                ViewBag.MAKHVUC = new SelectList(data.KHUVUCs.ToList().OrderBy(n => n.TENKV), "MAKHVUC", "TENKV");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    Phong sp = data.Phongs.SingleOrDefault(n => n.MaPhong == id);
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        sp.ANH = fileName;
                        //Luu vao CSDL   
                        UpdateModel(sp);
                        data.SubmitChanges();

                    }
                    return RedirectToAction("QLPhong");
                }
            }
        }
    }
}