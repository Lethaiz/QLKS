using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Create(KHUVUC kv)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.KHUVUCs.InsertOnSubmit(kv);
                data.SubmitChanges();

                return RedirectToAction("KhuVuc", "QLKHUVUC");
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