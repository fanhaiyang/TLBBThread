using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLBBThread.Models
{
    public class PersonModel
    {
        public string Name { get; set; }

        /// <summary>
        /// 代表颜色
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// 人生经历
        /// </summary>
        public List<string> Experience { get; set; }
    }
}
