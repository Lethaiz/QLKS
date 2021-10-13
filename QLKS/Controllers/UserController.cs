using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers
{
    public class UserController : Controller
    {
        dbQLKSDataContext data = new dbQLKSDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tenDN = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var dt = collection["Dienthoai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            var tuoi = collection["Tuoi"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = " Nhập Họ tên khách hàng bạn ơi không bỏ trống được";
            }
            else if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi2"] = " Nhập Tên bạn ơi cho dễ đẹp";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = " Nhập Mật khẩu đi chứ không nhập làm sao bảo mật được";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = " Nhập lại Mật khẩu kìa bạn";
            }
            else if (String.IsNullOrEmpty(dt))
            {
                ViewData["Loi5"] = " Nhập Số Điện Thoại đi chứ ngại gì";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = " Nhập Địa chỉ bạn ơi không bỏ trống được";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi7"] = " Nhập Email bạn ơi không bỏ trống được";
            }
            else
            {
                kh.HOTEN = hoten;
                kh.TAIKHOAN = tenDN;
                kh.MATKHAU = matkhau;
                kh.EMAIL = email;
                kh.DIACHI = diachi;
                kh.DT = dt;
                kh.NGAYSINH = DateTime.Parse(ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tenDN = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi1"] = "Nhập Tên đăng nhập để Đăng nhập BRO";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật Khẩu Đâu BRO";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TAIKHOAN == tenDN && n.MATKHAU == matkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng Nhập Thành Công nha Bro";
                    Session["TaiKhoan"] = kh;
                    Session["TaiKhoan2"] = kh.HOTEN;
                    return RedirectToAction("Index", "Shop");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu sai nha xem lại đi!!";
            }
            return View();

        }
    }
}