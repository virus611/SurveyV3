using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
  public class Tools
    {

        public static string createID()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <returns></returns>
        public static string GetECConnStr()
        { 
            return @"Data Source=119.23.213.213;Initial Catalog=SurveyV3;User ID=survey;Pwd=cqfy#hm0717re";
        }
    }
}
