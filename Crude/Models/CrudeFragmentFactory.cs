using System;
using Crude.Models.Fragments;

namespace Crude.Models
{
    internal static class CrudeFragmentFactory
    {
        public static IFieldFragment Create(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Field)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            IFieldFragment? fragment;

            switch (property.Value)
            {
                case double doubleValue:
                    fragment = new NumberFragment<double>(doubleValue);
                    break;
                case int intValue:
                    fragment = new NumberFragment<int>(intValue);
                    break;
                case string stringValue:
                    fragment = new StringFragment(stringValue);
                    break;
                case bool boolValue:
                    fragment = new BooleanFragment(boolValue);
                    break;
                case Enum enumValue:
                    fragment = new EnumFragment(enumValue);
                    break;
                case DateTime dateTimeValue:
                    fragment = new DateFragment(dateTimeValue);
                    break;
                case DateTimeOffset dateTimeOffsetValue:
                    fragment = new DateFragment(dateTimeOffsetValue.Date);
                    break;
                case EmptyValue:
                    fragment = new EmptyFragment();
                    break;
                default:
                    fragment = new NotRenderedFragment();
                    break;
            }

            if (property.OnClick != null)
            {
                fragment = new ActionFieldDecorator(fragment, property.OnClick);
            }

            return fragment;
        }
    }
}
