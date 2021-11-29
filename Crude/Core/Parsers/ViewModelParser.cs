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

                var placeholder = attributes.FirstOrDefault(x => x is CrudePlaceholderAttribute) is CrudePlaceholderAttribute _;

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

                items.Add(new CrudeProperty
                {
                    Name = name,
                    Order = order,
                    OnClick = onClick,
                    Info = property,
                    ViewModel = viewModel,
                    Disabled = disabled,
                    Password = password,
                    Placeholder = placeholder,
                    HtmlLabel = htmlLabel
                });
            }

            var seqNo = 0;

            foreach (var item in items.OrderBy(x => x.Order))
            {
                yield return item with
                {
                    SequenceNo = ++seqNo
                };
            }
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

                    items.Add(new CrudeMethod
                    {
                        MemberInfo = memberInfo,
                        MethodInfo = methodInfo,
                        Attributes = attributes.Cast<CrudeMethodAttribute>().ToList()
                    });
                }
            }

            return items;
        }
    }

    internal record CrudeMethod
    {
        public MemberInfo MemberInfo { get; init; } = default!;

        public MethodInfo MethodInfo { get; init; } = default!;

        public List<CrudeMethodAttribute> Attributes { get; init; } = default!;
    }

    internal record CrudeProperty
    {
        public string Name { get; init; } = string.Empty;

        public int Order { get; init; }

        public CrudePropertyType Type => Info.PropertyType.IsGenericBaseType(typeof(CrudeTable<>))
            ? CrudePropertyType.Table
            : CrudePropertyType.Field;

        public CrudeEvent? OnClick { get; init; }

        public PropertyInfo Info { get; init; } = default!;

        public object ViewModel { get; init; } = default!;

        public bool Disabled { get; init; }

        public bool Password { get; init; }

        public bool Placeholder { get; init; }

        public string HtmlLabel { get; init; } = string.Empty;

        public int SequenceNo { get; init; }

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