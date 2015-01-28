using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

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

    public static class Extensions
    {
        public static void SetValuesExclude(this object entity, object target, params string[] excludes)
        {
            entity.GetType().GetProperties().Where(prop => !excludes.Contains(prop.Name)).All(prop =>
            {
                prop.SetValue(entity, target.GetValue(prop.Name), null);
                return true;
            });
        }

        public static void SetValuesInclude(this object entity, object target, params string[] includes)
        {
            entity.GetType().GetProperties().Where(prop => includes.Contains(prop.Name)).All(prop =>
            {
                prop.SetValue(entity, target.GetValue(prop.Name), null);
                return true;
            });
        }
        public static object GetValue(this object entity, string propertyName)
        {
            var property = entity.GetType().GetProperties().FirstOrDefault(prop => prop.Name == propertyName);
            if (property == null) return null;
            return property.GetValue(entity, null);
        }
    }
}