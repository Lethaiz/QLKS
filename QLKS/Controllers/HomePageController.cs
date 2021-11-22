using PagedList;
using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers
{
    public class HomePageController : Controller
    {
        // GET: HomePage
        dbQLKSDataContext data = new dbQLKSDataContext();
        private List<Phong> layhangmoi(int count)
        {
            return data.Phongs.OrderByDescending(a => a.SoPhong).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);

            var spmoi = layhangmoi(12);
            return View(spmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
           
                var nsx = from nxb in data.Phongs where nxb.MaPhong == id select nxb;
                return View(nsx.SingleOrDefault());
            
        }
        public ActionResult Loai()
        {
            var loai = from l in data.LoaiPhongs select l;
            return PartialView(loai);

        }


        public ActionResult Sptheoloai(int id)
        {
            var sp = from h in data.Phongs where h.MaLP == id select h;
            return View(sp);
        }

        public ActionResult Khuvuc()
        {
            var KV = from k in data.KHUVUCs select k;
            return View(KV);
        }

        public ActionResult SPTheoKV(int id)
        {
            var KV = from s in data.KHUVUCs where s.MAKV == id select s;
            return View(KV);
        }
        public ActionResult About()
        {
            return View();
        }

    }
}