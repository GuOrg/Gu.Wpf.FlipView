using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;

[assembly: CLSCompliant(false)]
[assembly: InternalsVisibleTo("Gu.Wpf.FlipView.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ed009756dd50ca13f2a484be8db257502eb0814c28710bfe4c7de6a7719ddc96bc7823e07e03cd6c4c45287721a76256466fd5079fc28612bb93dfdfcd26b75ed39ce16f8239c40c64a248f8e6d94e7d435fb0a2b97f6b77712f4bbf14ada327a71c80915529b2797104fa36f6a6cb3ccd1a1ad12b5cc9dd056f46b60c43a5b8", AllInternalsVisible = true)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: XmlnsDefinition("https://github.com/JohanLarsson/Gu.Wpf.FlipView", "Gu.Wpf.FlipView")]
[assembly: XmlnsDefinition("https://github.com/JohanLarsson/Gu.Wpf.FlipView", "Gu.Wpf.FlipView.AttachedProperties")]
[assembly: XmlnsDefinition("https://github.com/JohanLarsson/Gu.Wpf.FlipView", "Gu.Wpf.FlipView.Gestures")]
[assembly: XmlnsPrefix("https://github.com/JohanLarsson/Gu.Wpf.FlipView", "flipView")]

[assembly: XmlnsDefinition("https://github.com/GuOrg/Gu.Wpf.FlipView", "Gu.Wpf.FlipView")]
[assembly: XmlnsDefinition("https://github.com/GuOrg/Gu.Wpf.FlipView", "Gu.Wpf.FlipView.AttachedProperties")]
[assembly: XmlnsDefinition("https://github.com/GuOrg/Gu.Wpf.FlipView", "Gu.Wpf.FlipView.Gestures")]
[assembly: XmlnsPrefix("https://github.com/GuOrg/Gu.Wpf.FlipView", "gu")]

#if NET48
#pragma warning disable SA1402, SA1502, SA1600, SA1649, GU0073
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        public MaybeNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

        public bool ReturnValue { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

        public bool ReturnValue { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName) => this.ParameterName = parameterName;

        public string ParameterName { get; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) => this.ParameterValue = parameterValue;

        public bool ParameterValue { get; }
    }
}
#endif
