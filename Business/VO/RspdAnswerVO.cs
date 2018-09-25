using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.VO
{
    /// <summary>
    /// 调查结果答案列表项
    /// </summary>
    public class RspdAnswerVO : IComparable<RspdAnswerVO>
    {
        public string sid {get;set;}
        public string qno {get;set;}
        public string answer {get;set;}

        public int CompareTo(RspdAnswerVO obj)
        {
            return this.qno.CompareTo(obj.qno) ; 
        }
    }
}
