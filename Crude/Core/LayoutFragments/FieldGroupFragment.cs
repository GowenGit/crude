using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crude.Core.FieldFragments;
using Crude.Core.Fragments;
using Crude.Core.Models;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;

namespace Crude.Core.LayoutFragments
{
    internal class FieldGroupFragment : IFragment
    {
        private readonly CrudeProperty _property;

        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(ushort),
            typeof(uint),
            typeof(float)
        };

        private static readonly HashSet<Type> DateTypes = new HashSet<Type>
        {
            typeof(DateTime),
            typeof(DateTimeOffset)
        };

        public FieldGroupFragment(CrudeProperty property)
        {
            _property = property;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var fragment = Create(_property);

            var seq = 0;

            builder.OpenElement(seq++, "crude-field-fragment");

            if (!string.IsNullOrWhiteSpace(fragment.FragmentType))
            {
                builder.AddAttribute(seq++, "class", $"cft-{fragment.FragmentType}");
            }

            builder.OpenElement(seq++, "label");
            builder.AddAttribute(seq++, "for", fragment.Identifier);

            if (string.IsNullOrWhiteSpace(_property.HtmlLabel))
            {
                builder.AddContent(seq++, _property.Name.ToString(context.Formatter));
            }
            else
            {
                builder.AddMarkupContent(seq++, _property.HtmlLabel);
            }

            builder.CloseElement();

            builder.AddContent(seq++, fragment.Render(context));

            builder.CloseElement();
        };

        private static FieldFragment Create(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Field)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            var type = property.Info.PropertyType;

            var unwrappedType = type.UnwrapNullable();

            FieldFragment? fragment = null;

            if (IsNumeric(unwrappedType))
            {
                fragment = CreateGeneric(typeof(NumberFragment<>), type, property);
            }

            if (IsDate(unwrappedType))
            {
                fragment = CreateGeneric(typeof(DateFragment<>), type, property);
            }

            if (unwrappedType == typeof(bool))
            {
                fragment = new BooleanFragment(property);
            }

            if (unwrappedType == typeof(string))
            {
                fragment = new StringFragment(property);
            }

            if (unwrappedType.IsEnum)
            {
                fragment = new EnumFragment(property);
            }

            if (unwrappedType.IsSubclassOf(typeof(CrudeDropdown)))
            {
                fragment = new CrudeDropdownFragment(property);
            }

            return fragment ??= new NotRenderedFragment(property);
        }

        private static bool IsNumeric(Type type)
        {
            return NumericTypes.Contains(type);
        }

        private static bool IsDate(Type type)
        {
            return DateTypes.Contains(type);
        }

        private static FieldFragment CreateGeneric(Type outerType, Type innerType, CrudeProperty property)
        {
            var type = outerType.MakeGenericType(innerType);

            var ctor = type
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First();

            return (FieldFragment)ctor.Invoke(new object?[] { property });
        }
    }
}