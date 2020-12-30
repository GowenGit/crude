using System;

namespace Crude.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class CrudePropertyAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class CrudeViewModelAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class CrudeMethodAttribute : Attribute { }
}
