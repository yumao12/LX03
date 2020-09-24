using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Get() {
            return Json(DAL.UserInfo.Instance.GetCount());
        }
        [HttpPut]
        public ActionResult getUser(string username) {
            var result = DAL.UserInfo.Instance.GetModel(username);
            if (result != null)
                return Json(Result.Ok(result));
            else
                return Json(Result.Err("用户名不存在"));
        }
        [HttpPost]
        public ActionResult Post([FromBody] Model.UserInfo users) {
            try {
                int n = DAL.UserInfo.Instance.Add(users);
                return Json(Result.Ok("添加成功"));
            }
            catch (Exception ex) {
                if (ex.Message.ToLower().Contains("primary"))
                    return Json(Result.Err("用户名已存在"));
                else if (ex.Message.ToLower().Contains("null"))
                    return Json(Result.Err("用户名、密码、身份不能为空"));
                else
                    return Json(Result.Err(ex.Message));
            }
        
        
        }
        [HttpPost("check")]
        public ActionResult UserCheck([FromBody] Model.UserInfo users)
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)

                    return Json(Result.Err("用户名错误"));
                else if (result.passWord == users.passWord)
                {
                    if (result.type == "管理员")
                    {
                        result.passWord = "********";
                        return Json(Result.Ok("管理员登录成功", result));
                    }
                    else
                        return Json(Result.Err("只有管理员哪个进入后台管理"));
                }
                else
                    return Json(Result.Err("密码错误"));
            }
            catch (Exception ex)
            {
               
                
                    return Json(Result.Err(ex.Message));
            }


        }
    }
}
