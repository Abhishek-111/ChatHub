using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Helper
{
    public static class GenericConverter
    {
        public static TTarget Convert<TSource, TTarget>(TSource source)
        {
            if(source == null)
            {
                return default(TTarget);
            }

            var targetType = typeof(TTarget);
            var sourceType = typeof(TSource);

            var targetInstance = Activator.CreateInstance<TTarget>();

            var sourceProperties = sourceType.GetProperties();
            var targetProperties = targetType.GetProperties();

            foreach(var sourceProperty in sourceProperties)
            {
                var targetProperty = targetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);
                if(targetProperty != null && targetProperty.PropertyType == sourceProperty.PropertyType)
                {
                    var value = sourceProperty.GetValue(source);
                    targetProperty.SetValue(targetInstance, value);
                }
            }
            return targetInstance;
        }
    }
}
