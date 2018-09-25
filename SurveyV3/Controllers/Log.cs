using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace SurveyV3.Controllers
{
    public class Log
    {
        public static void i(string tag,string content)
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "logs";
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);

            //向日志文件写入内容
            string write_content = time + " " + tag + ": " + content; 
            mySw.WriteLine(write_content);

            //关闭日志文件
            mySw.Close();
        }
    }
}