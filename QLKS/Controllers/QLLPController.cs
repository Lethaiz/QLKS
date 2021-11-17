using PagedList;
using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers
{
    public class QLLPController : Controller
    {
        // GET: QLLP
        dbQLKSDataContext data = new dbQLKSDataContext();


        public ActionResult Loaiphong(int? page)
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
                return View(data.LoaiPhongs.ToList().OrderBy(n => n.MaLP).ToPagedList(pageNumber, pageSize));
            }

        }
        public ActionResult ChiTietLP(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var l = from n in data.LoaiPhongs where n.MaLP == id select n;
                return View(l.SingleOrDefault());
            }
        }
        //3. Thêm mới Nhà xuất bản
        [HttpGet]
        public ActionResult themloai()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult themloai(LoaiPhong l)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.LoaiPhongs.InsertOnSubmit(l);
                data.SubmitChanges();

                return RedirectToAction("Loaiphong", "Loaiphong");
            }
        }
        //4. Xóa 1 Nhà xuất bản gồm 2 trang: xác nhận xóa và xử lý xóa
        [HttpGet]
        public ActionResult xoaloai(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var l = from nxb in data.LoaiPhongs where nxb.MaLP == id select nxb;
                return View(l.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("xoaloai")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                LoaiPhong l = data.LoaiPhongs.SingleOrDefault(n => n.MaLP == id);
                data.LoaiPhongs.DeleteOnSubmit(l);
                data.SubmitChanges();

                return RedirectToAction("Loaiphong", "Loaiphong");
            }
        }
        //5. Điều chỉnh thông tin 1  Nhà xuất bản gồm 2 trang: Xem và điều chỉnh và cập nhật lưu lại
        [HttpGet]
        public ActionResult Sualoai(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var l = from nxb in data.LoaiPhongs where nxb.MaLP == id select nxb;
                return View(l.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Sualoai")]
        public ActionResult Capnhat(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                LoaiPhong l = data.LoaiPhongs.SingleOrDefault(n => n.MaLP == id);

                UpdateModel(l);
                data.SubmitChanges();
                return RedirectToAction("Loaiphong", "Loaiphong");
            }
        }
    }
}