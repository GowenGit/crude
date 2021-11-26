using Crude.Core.Attributes;
using Crude.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

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

                if (property.GetCustomAttributes(typeof(DisplayAttribute)).FirstOrDefault(x => x is DisplayAttribute) is DisplayAttribute displayAttribute)
                {
                    var displayName = displayAttribute.GetName();

                    if (displayName != null)
                    {
                        name = displayName;
                    }
                }

                var disabled = attributes.FirstOrDefault(x => x is CrudeDisableAttribute) is CrudeDisableAttribute _;

                var password = attributes.FirstOrDefault(x => x is CrudePasswordAttribute) is CrudePasswordAttribute _;

                var htmlLabelAttribute = attributes.FirstOrDefault(x => x is CrudeHtmlLabelAttribute) as CrudeHtmlLabelAttribute;

                var htmlLabel = htmlLabelAttribute?.Html ?? string.Empty;

                var emptyPlaceholder = attributes.FirstOrDefault(x => x is CrudeEmptyPlaceholderAttribute) is CrudeEmptyPlaceholderAttribute _;

                CrudeEvent? onClick = null;

                foreach (var method in methods)
                {
                    if (method.Attributes.FirstOrDefault(x => x is CrudeOnClickAttribute) is CrudeOnClickAttribute onClickAttribute)
                    {
                        if (onClickAttribute.Property == property.Name)
                        {
                            onClick = new CrudeEvent(() => method.MethodInfo.Invoke(viewModel, Array.Empty<object>()));
                        }
                    }
                }

                items.Add(new CrudeProperty(
                    name,
                    order,
                    onClick,
                    property,
                    viewModel,
                    disabled,
                    password,
                    emptyPlaceholder,
                    htmlLabel));
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

        public bool Disabled { get; }

        public bool Password { get; }

        public bool EmptyPlaceholder { get; }

        public string HtmlLabel { get; }

        public CrudeProperty(
            string name,
            int order,
            CrudeEvent? onClick,
            PropertyInfo info,
            object viewModel,
            bool disabled,
            bool password,
            bool emptyPlaceholder,
            string htmlLabel)
        {
            Name = name;
            Order = order;
            OnClick = onClick;
            Info = info;
            ViewModel = viewModel;
            Disabled = disabled;
            Password = password;
            EmptyPlaceholder = emptyPlaceholder;
            HtmlLabel = htmlLabel;

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