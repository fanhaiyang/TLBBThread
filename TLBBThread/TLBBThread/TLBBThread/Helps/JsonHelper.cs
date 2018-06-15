using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLBBThread.Helps
{
    public class JsonHelper
    {
        /// <summary>
        /// Json文件转换对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T JsonFileToObj<T>(string filename)
        {
            string fullName = Path.Combine(StaticConfig.JsonPath, filename);
            if (File.Exists(fullName))
            {
                string result = File.ReadAllText(fullName);
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception($"json文件 {filename} 不存在");
            }
        }
    }
}
