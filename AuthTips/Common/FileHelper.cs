using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AuthTips.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 获取目录权限列表
        /// </summary>
        /// <param name="path">目录的路径。</param>
        /// <returns>指示目录的权限列表</returns>
        public IList<FileSystemRights> GetDirectoryPermission(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    return null;

                IList<FileSystemRights> result = new List<FileSystemRights>();
                var dSecurity = Directory.GetAccessControl(new DirectoryInfo(path).FullName);
                foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                    result.Add(rule.FileSystemRights);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        /// <summary>
        /// 判断目录是否包含修改权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsCanModity(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var fileName = Path.Combine(path, "test01.txt");

                //创建文件
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
                using (StreamWriter sw = new StreamWriter(fs)) // 创建写入流
                {
                    sw.WriteLine($"检测时间：{DateTime.Now}");
                }
                return true;
                //var dSecurity = Directory.GetAccessControl(new DirectoryInfo(path).FullName);
                //foreach (FileSystemAccessRule rule in dSecurity.GetAccessRules(true, true, typeof(NTAccount)))
                //{
                //    var right = rule.FileSystemRights;

                //    if (right == FileSystemRights.FullControl  ///所有权限
                //        || right.ToString().IndexOf("Modify") >= 0  //修改权限
                //        || right.ToString().IndexOf("Create") >= 0  //创建权限
                //        || right.ToString().IndexOf("Change") >= 0 //修改文件夹权限
                //        || right.ToString()=="262144")  //修改文件夹权限
                //    {
                //        return true;
                //    }

                //}

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
