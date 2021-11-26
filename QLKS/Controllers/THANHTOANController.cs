using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers
{
    public class THANHTOANController : Controller
    {
        dbQLKSDataContext data = new dbQLKSDataContext();
        public List<THANHTIEN> LayTHANHTIEN()
        {
            List<THANHTIEN> lstTHANHTIEN = Session["THANHTIEN"] as List<THANHTIEN>;
            if (lstTHANHTIEN == null)
            {

                lstTHANHTIEN = new List<THANHTIEN>();
                Session["THANHTIEN"] = lstTHANHTIEN;
            }
            return lstTHANHTIEN;
        }

        public ActionResult ThemTHANHTIEN(int iMASP, string strURL)
        {

            List<THANHTIEN> lstGiohang = LayTHANHTIEN();

            THANHTIEN sanpham = lstGiohang.Find(n => n.iMaPhong == iMASP);
            if (sanpham == null)
            {
                sanpham = new THANHTIEN(iMASP);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSODEM++;
                return Redirect(strURL);
            }
        }



        public ActionResult THANHTOAN()
        {
            List<THANHTIEN> lstTHANHTIEN = LayTHANHTIEN();
            if (lstTHANHTIEN.Count == 0)
            {
                return RedirectToAction("Index", "HomePage");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstTHANHTIEN);
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<THANHTIEN> lstTHANHTIEN = Session["THANHTIEN"] as List<THANHTIEN>;
            if (lstTHANHTIEN != null)
            {
                iTongSoLuong = lstTHANHTIEN.Sum(n => n.iSODEM);
            }
            return iTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<THANHTIEN> lstTHANHTIEN = Session["THANHTIEN"] as List<THANHTIEN>;
            if (lstTHANHTIEN != null)
            {
                iTongTien = lstTHANHTIEN.Sum(n => n.dTHANHTIEN);
            }
            return iTongTien;
        }
        //Tao Partial view de hien thi thong tin gio hang
        public ActionResult THANHTIENPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        //Cap nhat Giỏ hàng
        public ActionResult CapnhatTHANHTIEN(int iMaPhong, FormCollection f)
        {

            //Lay gio hang tu Session
            List<THANHTIEN> lstTHANHTIEN = LayTHANHTIEN();
            //Kiem tra sach da co trong Session["THANHTIEN"]
            THANHTIEN sanpham = lstTHANHTIEN.SingleOrDefault(n => n.iMaPhong == iMaPhong);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSODEM = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("THANHTIEN");
        }
        //Xoa THANHTIEN
        public ActionResult XoaTHANHTIEN(int iMaPhong)
        {
            //Lay gio hang tu Session
            List<THANHTIEN> lstTHANHTIEN = LayTHANHTIEN();
            //Kiem tra sach da co trong Session["THANHTIEN"]
            THANHTIEN sanpham = lstTHANHTIEN.SingleOrDefault(n => n.iMaPhong == iMaPhong);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                lstTHANHTIEN.RemoveAll(n => n.iMaPhong == iMaPhong);
                return RedirectToAction("THANHTIEN");

            }
            if (lstTHANHTIEN.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("THANHTIEN");
        }
        //Xoa tat ca thong tin trong Gio hang
        public ActionResult XoaTatcaTHANHTIEN()
        {
            //Lay gio hang tu Session
            List<THANHTIEN> lstTHANHTIEN = LayTHANHTIEN();
            lstTHANHTIEN.Clear();
            return RedirectToAction("Index", "HomePage");
        }

        ////Hien thi View DatHang de cap nhat cac thong tin cho Don hang
        [HttpGet]
        public ActionResult Donhang()
        {
            //Kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "User");
            }
            if (Session["THANHTIEN"] == null)
            {
                return RedirectToAction("Index", "HomePage");
            }

            //Lay gio hang tu Session
            List<THANHTIEN> lstTHANHTIEN = LayTHANHTIEN();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstTHANHTIEN);
        }
        //Xay dung chuc nang Dathang
        [HttpPost]
        public ActionResult Donhang(FormCollection collection)
        {
            //Them Don hang
            HoaDon ddh = new HoaDon();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<THANHTIEN> gh = LayTHANHTIEN();
            ddh.MaKH = kh.MaKH;
            ddh.NGAYDAT = DateTime.Now;
            var n = String.Format("{0:MM/dd/yyyy}", collection["NGAYNHAN"]);
            ddh.NGAYNHAN = DateTime.Parse(n);
            //ddh.TINHTRANGGIAOHANG = false;
            //ddh.DATHANHTOAN = false;
            data.HoaDons.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Them chi tiet don hang            
            foreach (var item in gh)
            {
                ChiTietHoaDon ctdh = new ChiTietHoaDon();
                ctdh.MaHD = ddh.MaHD;
                ctdh.MaPhong = item.iMaPhong;
                ctdh.SoDem = item.iSODEM;
                ctdh.ThanhTien = (decimal)item.dDONGIA;
                data.ChiTietHoaDons.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["THANHTIEN"] = null;
            return RedirectToAction("Xacnhandonhang", "THANHTIEN");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}