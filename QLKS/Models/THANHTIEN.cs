using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKS.Models
{
    public class THANHTIEN
    {



        dbQLKSDataContext data = new dbQLKSDataContext();
              public int iMaPhong { set; get; }
             public string iSoPhong{ set; get; }
            public string sANH{ set; get; }
            public Double dDONGIA { set; get; }
              public int iSODEM { set; get; }
             public Double dTHANHTIEN
            {
                get { return iSODEM * dDONGIA; }

            }

            public THANHTIEN(int MaPhong)
            {
                iMaPhong = MaPhong;
                Phong sp = data.Phongs.Single(n => n.MaPhong == MaPhong);
                iSoPhong = sp.SoPhong;
                  sANH = sp.ANH;
                dDONGIA = double.Parse(sp.GiaPhong.ToString());
            iSODEM = 1;
            }


        }
    }
