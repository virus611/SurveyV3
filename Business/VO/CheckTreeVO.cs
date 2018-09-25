using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Business.VO
{
    public class CheckTreeVO
    {
        public string id {get;set;}
         public string text {get;set;}

         [JsonProperty("checked")]
         public bool select {get;set;}
         public List<CheckTreeVO> children {get;set;}
         
    }
}
