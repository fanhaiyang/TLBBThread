using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLBBThread.Helps
{
    public class LogHelper
    {
        private static readonly object consoleLock = new object();

        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        public void ConsoleLog(string msg,ConsoleColor color)
        {
            lock (consoleLock)
            {
                foreach (var item in msg.ToCharArray())
                {
                    Thread.Sleep(100);
                    Console.ForegroundColor = color;
                    Console.Write(item);
                }
                Console.WriteLine();
            }
        }
    }
}
