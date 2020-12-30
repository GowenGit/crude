﻿using Microsoft.AspNetCore.Components;

namespace Crude.Models.Fragments
{
    internal interface ICrudeFragment
    {
        RenderFragment Render(RenderOptions options);
    }
}