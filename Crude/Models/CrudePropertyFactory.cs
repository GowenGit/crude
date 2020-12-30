using System;
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

            var methods = GetMethods(viewModel);

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

                var value = property.GetValue(viewModel) ?? new EmptyValue();

                Action? onClick = null;

                foreach (var method in methods)
                {
                    if (method.Attributes.FirstOrDefault(x => x is CrudeOnClickAttribute) is CrudeOnClickAttribute onClickAttribute)
                    {
                        if (onClickAttribute.Property == property.Name)
                        {
                            onClick = () =>
                            {
                                method.MethodInfo.Invoke(viewModel, new object[] { });
                            };
                        }
                    }
                }

                items.Add(new CrudeProperty(name, order, value, onClick));
            }

            return items.OrderBy(x => x.Order);
        }

        private static List<CrudeMethod> GetMethods(object viewModel)
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

        public object Value { get; }

        public CrudePropertyType Type { get; }

        public Action? OnClick { get; }

        public CrudeProperty(string name, int order, object value, Action? onClick)
        {
            Name = name;
            Order = order;
            Value = value;
            OnClick = onClick;

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