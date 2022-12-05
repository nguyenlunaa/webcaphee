using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();

        public ActionResult Index(string sortColumn ="",int page =1)
        {
            //View All
            List<WebCafe.Models.SanPham> SanPhams = db.SanPhams.Where(x => x.DaXoa == true).ToList();
            ViewBag.TitleAction = "Danh sách sản phẩm";
            List<LoaiSanPham> LoaiSanPhams = db.LoaiSanPhams.ToList();
            List<LoaiSanPham> rd3loai = (from l in LoaiSanPhams select l).OrderBy(x => Guid.NewGuid()).Take(3).ToList(); 

            List<LoaiVaSoLuong> listSL =
                (from l in LoaiSanPhams
                 let theCount = l.SanPhams.Count()
                 select new LoaiVaSoLuong
                 {
                     MaLoai = l.MaLoai,
                     TenLoai = l.TenLoai,
                     SoLuong = theCount
                 }).ToList();
            

            var shopModel = new ShopViewModel();
            shopModel.listSanPham = SanPhams;
            shopModel.listLoaiSP = LoaiSanPhams;
            shopModel.listLoaiSoLuong = listSL;
            shopModel.rd3loai = rd3loai;

            //Sort
            if (sortColumn == "AtoZ")
            {
                SanPhams = db.SanPhams.OrderBy(row => row.TenSP).ToList();
                shopModel.listSanPham = SanPhams;
            }
            else if(sortColumn == "ZtoA")
            {
                SanPhams = db.SanPhams.OrderByDescending(row => row.TenSP).ToList();
                shopModel.listSanPham = SanPhams;
            }else if(sortColumn == "LowToHigh")
            {
                SanPhams = db.SanPhams.OrderBy(row => row.DonGia).ToList();
                shopModel.listSanPham = SanPhams;
            }
            else if(sortColumn == "HighToLow")
            {
                SanPhams = db.SanPhams.OrderByDescending(row => row.DonGia).ToList();
                shopModel.listSanPham = SanPhams;
            }

            //Paging
            int NumOfRecordPerPage = 3;
            int NumOfPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(SanPhams.Count) / NumOfRecordPerPage));
            int NumOfRecordToSkip = (page - 1) * NumOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NumOfPage = NumOfPage;
            SanPhams = SanPhams.Skip(NumOfRecordToSkip).Take(NumOfRecordPerPage).ToList();
            shopModel.listSanPham = SanPhams;
            return View(shopModel);
        }

        public ActionResult ChiTiet(int id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSP == id);
            ViewBag.LoaiSanPham = db.LoaiSanPhams.ToList();

            return View(sp);
        }
    }
}