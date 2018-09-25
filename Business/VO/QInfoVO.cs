using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.VO
{
    public class QInfoVO
    {
        public string qid { get; set; }
        public string qno { get; set; }
        public string question { get; set; }
       
        public string answer { get; set; }

        /// <summary>
        /// Muti  -1=日期选择 0=单选 1=多选
        /// </summary>
        public int muti { get; set; }

        /// <summary>
        /// iput   1=手输
        /// </summary>
        public int iput { get; set; }
        public string jump { get; set; }

           

    }
     
}
