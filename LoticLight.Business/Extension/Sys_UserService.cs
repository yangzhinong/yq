using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business
{
    public partial class Sys_UserService
    {
        public static bool VerifyLogin(string username, string password, out string msg)
        {
            msg = "OK";
            if (username.IsEmpty() || password.IsEmpty())
            {
                msg = "提示：用户名、密码不能为空";
                return false;

            }
            if (Instance.LoadEntitiesNoTracking(o => o.LoginName == username.Trim()).Count() == 0)
            {
                msg = "提示：用户不存在";
                return false;

            }
            //加密密码
            password = Utility.DEncrypt.Encrypt(password);
            var user = Instance.LoadEntitiesNoTracking(o => o.LoginName == username.Trim() && o.Password == password.Trim()).FirstOrDefault();
            if (user == null)
            {
                msg = "提示：用户名与密码不匹配";
                return false;
            }
            if (LoticLight.Session.WebSession.Current != null)
            {
                LoticLight.Session.WebSession.RemoveSession();
            }
            Model.AccountInfo account = new Model.AccountInfo();
            account.User = user;
            var menus = Business.Sys_MenuService.GetmenusByUserId(account.User.Id);
            account.initdata = new Model.initData
            {
                menus = menus,
                menusActions = Business.Sys_ActionService.GetMenuActin(account.User.Id, menus),
                menusViews = Business.Sys_GridViewService.GetMenuView(account.User.Id, menus)
            };
            LoticLight.Session.WebSession.SetCurrentAccount(account);


            return true;
        }
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SaveUser(Model.Sys_User obj)
        {
            var password = Utility.DEncrypt.Encrypt(obj.Password);
            var count = Business.Sys_UserService.Instance.LoadEntities(u => u.LoginName == obj.LoginName.Trim());
            var data = Business.Sys_UserService.Instance.FindEntities(obj.Id);
            if (string.IsNullOrEmpty(obj.Id))
            {
                if (count.ToList().Count > 0)
                {
                    return false;
                }
                obj.Password = password;
                Business.Sys_UserService.Instance.AddEntities(obj);
            }
            else
            {
                if (count.ToList().Count > 0 && obj.LoginName != data.LoginName)
                {
                    return false;
                }
                data.Id = obj.Id;
                data.LoginName = obj.LoginName;
                data.Password = password;
                data.Status = obj.Status;
                Business.Sys_UserService.Instance.UpdateEntities(data);
            }
            return true;
        }
    }
}
