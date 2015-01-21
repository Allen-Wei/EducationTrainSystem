using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationTrainSystem.Library
{
    public class Utils
    {
        public class RichMessage
        {
            public bool success { get; set; }
            public string brief { get; set; }
            public string message { get; set; }
            public object result { get; set; }
            public RichMessage() { }

            public RichMessage(bool s, string m)
            {
                this.success = s;
                this.message = m;
            }
        }
    }
}