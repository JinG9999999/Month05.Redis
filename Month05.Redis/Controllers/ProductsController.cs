using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Month05.Redis.Models;
using ServiceStack.Redis;
using log4netDemo;

namespace Month05.Redis.Controllers
{
    public class ProductsController : Controller
    {
        //显示
        public ActionResult Show(int currentpage = 1)
        {
            //先打开Redis服务
            var processes = Process.GetProcessesByName("redis-server");
            if (processes.Length == 0)
            {
                Process.Start(@"C:\Users\吕静\Desktop\KaoShi\Redis所需文件\redis服务\redis-server.exe");
            }
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                var products = redisClient.GetTypedClient<ProductsModel>();
                List<ProductsModel> list = products.GetAll().ToList();

                if (list.Count() % 2 == 0)
                {
                    ViewBag.totalpage = list.Count() / 2;
                }
                else
                {
                    ViewBag.totalpage = list.Count() / 2 + 1;
                }
                ViewBag.currentpage = currentpage;
                return View(list.Skip((currentpage - 1) * 2).Take(2).ToList());
            }
        }
        [HttpPost]
        //分页
        public ActionResult Show(string time1 = "", string time2 = "", string name = "", int currentpage = 1)
        {
            if (currentpage < 1)
            {
                currentpage = 1;
            }
            var processes = Process.GetProcessesByName("redis-server");
            if (processes.Length == 0)
            {
                Process.Start(@"C:\Users\吕静\Desktop\KaoShi\Redis所需文件\redis服务\redis-server.exe");
            }
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                var products = redisClient.GetTypedClient<ProductsModel>();
                List<ProductsModel> list = products.GetAll().ToList();
                //查询
                if (time1 != "")
                {
                    list = list.Where(m => m.UpdTime >= Convert.ToDateTime(time1)).ToList();
                }
                if (time2 != "")
                {
                    list = list.Where(m => m.UpdTime <= Convert.ToDateTime(time2)).ToList();
                }
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(m => m.Pname.Contains(name)).ToList();
                }
            
                if (list.Count() % 2 == 0)
                {
                    ViewBag.totalpage = list.Count() / 2;
                }
                else
                {
                    ViewBag.totalpage = list.Count() / 2 + 1;
                }
               
                if (currentpage> ViewBag.totalpage)
                {
                    return Show("","","", ViewBag.totalpage);
                }
                ViewBag.currentpage = currentpage;
                return View(list.OrderByDescending(m => m.UpdTime).Skip((currentpage - 1) * 2).Take(2).ToList());
            }
        }
        //添加
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(ProductsModel m)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var products = redisClient.GetTypedClient<ProductsModel>();
                    //添加必备
                    m.ID = Convert.ToInt32(products.GetNextSequence());
                    products.Store(m);
                    ViewBag.login = m.Pname;
                    //打印日志
                    LogHelper.Default.WriteInfo("添加完毕");
                }
                return Content("<script>alert('添加成功');location.href='/Products/Show/';</script>");
            }
            catch
            {
                return Content("<script>alert('添加失败');location.href='/Products/Show/';</script>");
            }
        }
        //修改
        public ActionResult Upd(int id)
        {
            using (IRedisClient redisClient = RedisManager.GetClient())
            {
                var products = redisClient.GetTypedClient<ProductsModel>();
                return View(products.GetById(id));
            }
          
        }

        [HttpPost]
        public ActionResult Upd(ProductsModel m)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var products = redisClient.GetTypedClient<ProductsModel>();
                    products.Store(m);
                    //打印日志
                    LogHelper.Default.WriteInfo("修改完毕");
                }
                return Content("<script>alert('修改成功');location.href='/Products/Show/';</script>");
            }
            catch
            {
                return Content("<script>alert('修改失败');</script>");

            }
        }
        //删除

        public ActionResult Del(string id)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var products = redisClient.GetTypedClient<ProductsModel>();
                    products.DeleteByIds(id);
                    //打印日志
                    LogHelper.Default.WriteInfo("删除完毕");
                }
                return Content("<script>alert('删除成功');location.href='/Products/Show/';</script>");
            }
            catch
            {
                return Content("<script>alert('删除失败');</script>");
            }
        }
    }
}
