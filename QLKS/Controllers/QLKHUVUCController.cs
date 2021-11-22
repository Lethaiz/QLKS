using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;

namespace QLKS.Controllers
{
    public class QLKHUVUCController : Controller
    {
        dbQLKSDataContext data = new dbQLKSDataContext();
        public ActionResult KhuVuc(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                //return View(db.SACHes.ToList());
                return View(data.KHUVUCs.ToList().OrderBy(n => n.MAKV).ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nsx = from nxb in data.KHUVUCs where nxb.MAKV == id select nxb;
                return View(nsx.SingleOrDefault());
            }
        }
        //3. Thêm mới Nhà xuất bản
        [HttpGet]
        public ActionResult taomoi()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");

            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult taomoi(KHUVUC p, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
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
                    var path = Path.Combine(Server.MapPath("~/Content/IMG"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    p.ANH = fileName;
                    data.KHUVUCs.InsertOnSubmit(p);
                    data.SubmitChanges();
                }
                return RedirectToAction("KhuVuc");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nsx = from nxb in data.KHUVUCs where nxb.MAKV == id select nxb;
                return View(nsx.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                KHUVUC nsx = data.KHUVUCs.SingleOrDefault(n => n.MAKV == id);
                data.KHUVUCs.DeleteOnSubmit(nsx);
                data.SubmitChanges();

                return RedirectToAction("KhuVuc", "QLKHUVUC");
            }
        }
      
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nsx = from nxb in data.KHUVUCs where nxb.MAKV == id select nxb;
                return View(nsx.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                KHUVUC nsx = data.KHUVUCs.SingleOrDefault(n => n.MAKV == id);

                UpdateModel(nsx);
                data.SubmitChanges();
                return RedirectToAction("KhuVuc", "QLKHUVUC");
            }

        }
    }
}