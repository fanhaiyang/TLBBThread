using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            bool isMonitor = true;
            try
            {
                // 获取配置信息
                List<PersonModel> personList = JsonHelper.JsonFileToObj<List<PersonModel>>("JsonConfig.json");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // 线程取消信号
                CancellationTokenSource cts = new CancellationTokenSource();

                #region 监控线程
                Task.Run(() =>
                {
                    int randomNum = 0;
                    int thisYear = DateTime.Now.Year;
                    while (isMonitor && randomNum != thisYear)
                    {
                        randomNum = new Random(randomNum).Next(2015, 2030);
                        Thread.Sleep(100);
                    }
                    if (isMonitor && randomNum == thisYear)
                    {
                        logHelper.ConsoleLog("天降雷霆灭世，天龙八部的故事就此结束....", ConsoleColor.Red);
                        cts.Cancel();
                    }
                    else
                    {
                        logHelper.ConsoleLog("监控结束", ConsoleColor.Magenta);
                    }
                });

                #endregion

                bool isOpen = false;
                TaskFactory taskFactory = new TaskFactory();
                List<Task> taskList = new List<Task>();

                foreach (var item in personList)
                {
                    taskList.Add(taskFactory.StartNew(s =>
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
                                            // 线程是否已经取消
                                            if (cts.IsCancellationRequested)
                                            {
                                                break;
                                            }
                                            logHelper.ConsoleLog(item.Experience[i], item.Color);
                                            logHelper.ConsoleLog("天龙八部就此拉开序幕。。。。", ConsoleColor.White);
                                            isOpen = true;
                                        }
                                        else
                                        {
                                            if (cts.IsCancellationRequested)
                                            {
                                                break;
                                            }
                                            logHelper.ConsoleLog(item.Experience[i], item.Color);
                                        }
                                    }
                                }
                                else
                                {
                                    if (cts.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    logHelper.ConsoleLog(item.Experience[i], item.Color);
                                }
                            }
                            else
                            {
                                if (cts.IsCancellationRequested)
                                {
                                    break;
                                }
                                logHelper.ConsoleLog(item.Experience[i], item.Color);
                            }
                        }
                    }, item.Name, cts.Token));
                }

                // 任何一个人物事件结束后执行
                taskList.Add(taskFactory.ContinueWhenAny(taskList.ToArray(), s =>
                {
                    if (cts.IsCancellationRequested)
                    {
                        return;
                    }
                    logHelper.ConsoleLog($"{s.AsyncState}已经做好准备啦。。。。", ConsoleColor.White);
                }));

                // 所有人物事件结束后执行
                taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), s =>
                {
                    if (cts.IsCancellationRequested)
                    {
                        return;
                    }
                    logHelper.ConsoleLog("中原群雄大战辽兵，忠义两难一死谢天", ConsoleColor.White);
                }));

                // 等待 taskList 任务集合全部结束
                Task.WaitAll(taskList.ToArray());
                stopwatch.Stop();

                if (!cts.IsCancellationRequested)
                {
                    logHelper.ConsoleLog($"整个天龙八部的故事花了{stopwatch.ElapsedMilliseconds}ms", ConsoleColor.White);
                }
            }

            catch (Exception ex)
            {
                logHelper.ConsoleLog(ex.Message, ConsoleColor.Red);
            }
            finally
            {
                isMonitor = false;
            }

            Console.ReadKey();
        }
    }
}
