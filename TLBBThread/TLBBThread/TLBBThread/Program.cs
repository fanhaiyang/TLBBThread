using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLBBThread.Helps;
using TLBBThread.Models;

namespace TLBBThread
{
    public class Program
    {
        private static readonly object OpenLock = new object();

        static void Main(string[] args)
        {
            LogHelper logHelper = new LogHelper();
            bool isOpen = false;

            // 获取配置信息
            List<PersonModel> personList = JsonHelper.JsonFileToObj<List<PersonModel>>("JsonConfig.json");

            TaskFactory taskFactory = new TaskFactory();
            foreach (var item in personList)
            {
                taskFactory.StartNew(() =>
                {
                    for (int i = 0; i < item.Experience.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (!isOpen)
                            {
                                lock (OpenLock)
                                {
                                    if (!isOpen)
                                    {
                                        logHelper.ConsoleLog(item.Experience[i], item.Color);
                                        logHelper.ConsoleLog("天龙八部就此拉开序幕。。。。", ConsoleColor.White);
                                        isOpen = true;
                                    }
                                    else
                                    {
                                        logHelper.ConsoleLog(item.Experience[i], item.Color);
                                    }
                                }
                            }
                            else
                            {
                                logHelper.ConsoleLog(item.Experience[i], item.Color);
                            }
                        }
                        else
                        {
                            logHelper.ConsoleLog(item.Experience[i], item.Color);
                        }
                    }
                });
            }

            Console.ReadKey();
        }
    }
}
