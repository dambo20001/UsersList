using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class HomeController : Controller
    {
        UserManagerEntities db = new UserManagerEntities();
        [HttpGet]
        public ActionResult JsTreeDemo()
        {
            return View();
        }
        public ActionResult Nodes()
        {
            var nodes = new List<JsTreeModel>();
            foreach (Group items in db.Groups)
            {
                if (items.GroupId == 1)
                {
                    nodes.Add(new JsTreeModel() { id = items.GroupId.ToString(), parent = "#", text = items.Name.ToString() });
                }
                else
                    nodes.Add(new JsTreeModel() { id = items.GroupId.ToString(), parent = items.Parent_Id.ToString(), text = items.Name.ToString() });
            }
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult TaoNhom(string tenNhom, string NhomCha, string maNhom)
        {
            Group ob = new Group();
            ob.Name = tenNhom;
            ob.Code = maNhom;
            ob.Parent_Id = Int32.Parse(NhomCha);
            db.Groups.Add(ob);
            db.SaveChanges();
            return RedirectToAction("JsTreeDemo");
        }
        public ActionResult HienThi(int groupId)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            var users = db.Users.Where(u => u.GroupId == groupId).ToList();
            var jsonUsers = users.Select(u => new
            {
                id = u.Id,
                fullname = u.FullName,
                username = u.UserName,
                date =u.Date.ToString(),
                gender = u.Gender,
                number = u.Number,
                email = u.Email,
                groupid = u.GroupId,
            });
            return Json(jsonUsers, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SuaNhom(int GroupId)
        {     // Lấy thông tin nhóm con từ cơ sở dữ liệu theo groupId
            var editgr = db.Groups.Where(g => g.GroupId == GroupId);
            var jsongr = editgr.Select(u => new
            {
                id = u.GroupId,
                name = u.Name,
                code = u.Code,
            });
            return Json(jsongr, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CapNhatNhom(string tenNhom,string IdNhom,int Id)
        {
            var editgr = db.Groups.FirstOrDefault(g => g.GroupId == Id);

            editgr.Name = tenNhom;
            editgr.Code = IdNhom;
            db.SaveChanges();
            return RedirectToAction("JsTreeDemo");
        }
        [HttpGet]
        public ActionResult XoaNhom(int Id)
        {
            var item = db.Groups.FirstOrDefault(g => g.GroupId == Id);
            db.Groups.Remove(item);
            db.SaveChanges();
            return RedirectToAction("JsTreeDemo");
        }
        [HttpPost]
        public ActionResult TaoNguoiDungMoi(string FullName, string UserName, DateTime? Date, string Gender,string Number,string Email, int GroupId)
        {
            //DateTime enteredDate = DateTime.Parse(Date);
                // Tạo một đối tượng người dùng mới
                User nguoiDungMoi = new User
                { 
                    FullName = FullName,
                    UserName = UserName,
                    Date = Date,
                    Gender = Gender,
                    Number = Number,
                    Email = Email,
                    GroupId = GroupId,
                };

                // Lưu đối tượng người dùng mới vào cơ sở dữ liệu
                db.Users.Add(nguoiDungMoi);
                db.SaveChanges();
            return RedirectToAction("JsTreeDemo");
        }
        [HttpGet]
        public ActionResult ChinhSuaNguoiDung(string UserName, string FullName, DateTime? Date, string Gender, string Number, string Email, int Id)
        {
            var editgr = db.Users.FirstOrDefault(g => g.Id == Id);

            editgr.FullName = FullName;
            editgr.UserName = UserName;
            //editgr.Date = Date;
            editgr.Gender = Gender;
            editgr.Number = Number;
            editgr.Email = Email;
            db.SaveChanges();
            return RedirectToAction("JsTreeDemo");
        }
        [HttpGet]
        public ActionResult XoaNguoiDung(int Id)
        {
            var item = db.Users.FirstOrDefault(g => g.Id == Id);
            db.Users.Remove(item);
            db.SaveChanges();
            return RedirectToAction("JsTreeDemo");
        }
    }
}