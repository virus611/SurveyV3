using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mobile
{
    /// <summary>
    /// 终端提交上来的时间点
    /// </summary>
    public class TimeVO
    {
        public long stime { get; set; }
        /// <summary>
        /// 1=开始，0=结束
        /// </summary>
        public int isstart { get; set; }
    }
}
