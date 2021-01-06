using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crude.Core.Attributes;
using Crude.Core.Models;

namespace Crude.Core.Parsers
{
    internal static class ViewModelParser
    {
        public static IEnumerable<CrudeProperty> ParseProperties(object viewModel)
        {
            var items = new List<CrudeProperty>();

            var properties = viewModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var methods = ParseMethods(viewModel);

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

                CrudeEvent? onClick = null;

                foreach (var method in methods)
                {
                    if (method.Attributes.FirstOrDefault(x => x is CrudeOnClickAttribute) is CrudeOnClickAttribute onClickAttribute)
                    {
                        if (onClickAttribute.Property == property.Name)
                        {
                            onClick = new CrudeEvent(() => method.MethodInfo.Invoke(viewModel, new object[] { }));
                        }
                    }
                }

                items.Add(new CrudeProperty(name, order, onClick, property, viewModel));
            }

            return items.OrderBy(x => x.Order);
        }

        public static List<CrudeMethod> ParseMethods(object viewModel)
        {
            var items = new List<CrudeMethod>();

            var type = viewModel.GetType();

            const BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var methods = type.GetMethods(bindingAttr);

            foreach (var memberInfo in methods)
            {
                var attributes = memberInfo.GetCustomAttributes(typeof(CrudeMethodAttribute)).ToList();

                if (attributes.Any())
                {
                    var methodInfo = type.GetMethod(memberInfo.Name, bindingAttr);

                    if (methodInfo == null)
                    {
                        continue;
                    }

                    items.Add(new CrudeMethod(memberInfo, methodInfo, attributes.Cast<CrudeMethodAttribute>().ToList()));
                }
            }

            return items;
        }
    }

    internal class CrudeMethod
    {
        public MemberInfo MemberInfo { get; }

        public MethodInfo MethodInfo { get; }

        public List<CrudeMethodAttribute> Attributes { get; }

        public CrudeMethod(MemberInfo memberInfo, MethodInfo methodInfo, List<CrudeMethodAttribute> attributes)
        {
            MemberInfo = memberInfo;
            MethodInfo = methodInfo;
            Attributes = attributes;
        }
    }

    internal class CrudeProperty
    {
        public string Name { get; }

        public int Order { get; }

        public CrudePropertyType Type { get; }

        public CrudeEvent? OnClick { get; }

        public PropertyInfo Info { get; }

        public object ViewModel { get; }

        public CrudeProperty(
            string name,
            int order,
            CrudeEvent? onClick,
            PropertyInfo info,
            object viewModel)
        {
            Name = name;
            Order = order;
            OnClick = onClick;
            Info = info;
            ViewModel = viewModel;

            if (Info.PropertyType.IsGenericBaseType(typeof(CrudeTable<>)))
            {
                Type = CrudePropertyType.Table;
            }
            else
            {
                Type = CrudePropertyType.Field;
            }
        }

        public object? GetValue()
        {
            return Info.GetValue(ViewModel);
        }

        public void SetValue(object? value)
        {
            Info.SetValue(ViewModel, value);
        }
    }

    internal enum CrudePropertyType
    {
        Field = 0,
        Table = 1
    }
}