using AuthTips.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Threading;
using System.Configuration;

namespace AuthTips
{
    class Program
    {
        static FileHelper fileHelper = new FileHelper();
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static bool IsSendEmail = false;
        private static DateTime SendTime = DateTime.Now;
        private static bool IsPrint = false;
        private static DateTime PrintTime = DateTime.Now;
        static void Main(string[] args)
        {
            Console.WriteLine("是否要启动文件夹检测程序？(Y/N)");
            var responseVal = Console.ReadLine();
            if (responseVal.ToUpper() == "Y")
            {
                Console.WriteLine("开始启动网站文件夹检测；");
                var filePathStr = ConfigurationManager.AppSettings["path"];
                var pathList = filePathStr.Split(',');
                foreach (var filePath in pathList)
                {
                    Console.WriteLine(string.Format("检测地址：{0}", filePath));
                }

                while (true)
                {
                    var printVals = new List<string>();
                    foreach (var path in pathList)
                    {
                        var result = fileHelper.IsCanModity(path);
                        if (result)
                        {

                            printVals.Add($"检测到文件夹【{path}】权限未关闭，已发送邮件！！！");
                            if (!IsSendEmail)
                            {
                                log.Info($"检测到文件夹【{path}】权限未关闭");
                                IsSendEmail = true;
                                IsPrint = false;
                                SendTime = DateTime.Now;
                            }
                        }
                        else
                        {
                            printVals.Add($"已检测文件夹【{path}】权限已关闭");
                        }

                    }
                    if (!IsPrint)
                    {
                        foreach (var item in printVals)
                        {
                            Console.WriteLine(item);
                        }
                        IsPrint = true;
                        PrintTime = DateTime.Now;
                        

                    }
                    Task.Factory.StartNew(() =>
                    {
                        CountDown();
                    });
                    Thread.Sleep(10000);

                }
            }


        }
        private static void CountDown()
        {
            if (SendTime.AddHours(1) <= DateTime.Now && IsSendEmail)
            {
                IsSendEmail = false;
            }
            if (PrintTime.AddMinutes(10) <= DateTime.Now)
            {
                IsPrint = false;
            }
        }


    }
}
