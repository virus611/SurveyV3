using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyV3.Areas.Web.Models
{
    public class AnswerFileVO
    {
       public  string start { get; set; }
       public string end { get; set; }
       public string qcode { get; set; }
       public string acode { get; set; }

       public string rid { get; set; }
    }
}