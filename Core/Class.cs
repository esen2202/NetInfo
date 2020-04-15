using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Class
    { 
            public static void CopyObjectPropertiesValue<TObject>(TObject source, TObject target)
            {
                typeof(TObject).GetProperties().ToList().ForEach(property =>
                {
                    property.SetValue(target, property.GetValue(source));
                });
            }
      
    }
}
