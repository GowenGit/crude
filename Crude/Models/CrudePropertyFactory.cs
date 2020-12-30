using Crude.Models.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Crude.Models
{
    internal static class CrudePropertyFactory
    {
        public static IEnumerable<CrudeProperty> Create(object viewModel)
        {
            var items = new List<CrudeProperty>();

            var properties = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(CrudePropertyAttribute)).ToList();

                if (attributes.Any(x => x is CrudeIgnoreAttribute))
                {
                    continue;
                }

                var order = int.MaxValue;

                if (attributes.FirstOrDefault(x => x is CrudeOrderAttribute) is CrudeOrderAttribute orderAttribute)
                {
                    order = orderAttribute.Order;
                }

                var name = property.Name;

                if (attributes.FirstOrDefault(x => x is CrudeNameAttribute) is CrudeNameAttribute nameAttribute)
                {
                    name = nameAttribute.Name;
                }

                var value = property.GetValue(viewModel);

                if (value == null)
                {
                    value = new EmptyValue();
                }

                items.Add(new CrudeProperty(name, order, value));
            }

            return items.OrderBy(x => x.Order);
        }
    }

    internal class CrudeProperty
    {
        public string Name { get; }

        public int Order { get; }

        public object Value { get; }

        public CrudePropertyType Type { get; }

        public CrudeProperty(string name, int order, object value)
        {
            Name = name;
            Order = order;
            Value = value;

            if (value.IsGenericBaseType(typeof(CrudeTable<>)))
            {
                Type = CrudePropertyType.Table;
            }
            else
            {
                Type = CrudePropertyType.Field;
            }
        }
    }

    internal enum CrudePropertyType
    {
        Field = 0,
        Table = 1
    }

    internal class EmptyValue { }
}