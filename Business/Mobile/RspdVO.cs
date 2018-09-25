using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.VO;


namespace Business.Mobile
{
    /// <summary>
    /// 终端提交的调查结果
    /// </summary>
    public class RspdVO
    {
        public List<AnswerVO> answers { get; set; }
        public BaseInfoVO baseinfo { get; set; }
        public List<FamilyMemberVO> family { get; set; }

        public List<TimeVO> timelist { get; set; }
    }
}
