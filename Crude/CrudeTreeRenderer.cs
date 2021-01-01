﻿using Crude.Models;
using Crude.Models.LayoutFragments;
using Microsoft.AspNetCore.Components;
using System;

namespace Crude
{
    internal class CrudeTreeRenderer
    {
        private readonly object _viewModel;

        public CrudeTreeRenderer(object viewModel)
        {
            _viewModel = viewModel;
        }

        public RenderFragment Render(CrudeOptions options, Action notifyStateChange) => builder =>
        {
            var items = CrudePropertyFactory.Create(_viewModel);

            var context = new RenderContext(notifyStateChange, options);

            var seq = 0;

            foreach (var item in items)
            {
                if (item.Type == CrudePropertyType.Field)
                {
                    var fragment = CreateFieldFragment(item);

                    builder.AddContent(seq++, fragment.Render(context));
                }

                if (item.Type == CrudePropertyType.Table)
                {
                    var fragment = CreateTableFragment(item);

                    if (fragment == null)
                    {
                        continue;
                    }

                    builder.AddContent(seq++, fragment.Render(context));
                }
            }
        };

        private static FieldGroupFragment CreateFieldFragment(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Field)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            return new FieldGroupFragment(property.Name, CrudeFragmentFactory.Create(property));
        }

        private static ICrudeLayoutFragment? CreateTableFragment(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Table)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            var viewModelType = property.Value.GetType().BaseType?.GetGenericArguments()[0];

            if (viewModelType == null)
            {
                throw new ArgumentException($"Could not resolve view model type for property {property.Name}");
            }

            var type = typeof(TableFragment<>).MakeGenericType(viewModelType);

            var fragment = Activator.CreateInstance(type, property.Value);

            return (ICrudeLayoutFragment?)fragment;
        }
    }
}