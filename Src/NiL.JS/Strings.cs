// Decompiled with JetBrains decompiler
// Type: NiL.JS.Strings
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace NiL.JS
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Strings
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Strings.resourceMan == null)
          Strings.resourceMan = new ResourceManager("NiL.JS" + ".Strings", typeof (Strings).GetTypeInfo().Assembly);
        return Strings.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Strings.resourceCulture;
      set => Strings.resourceCulture = value;
    }

    internal static string CannotAssignReadOnly => Strings.ResourceManager.GetString(nameof (CannotAssignReadOnly), Strings.resourceCulture);

    internal static string ConstructorCannotBeStatic => Strings.ResourceManager.GetString(nameof (ConstructorCannotBeStatic), Strings.resourceCulture);

    internal static string DoNotDeclareFunctionInNestedBlocks => Strings.ResourceManager.GetString(nameof (DoNotDeclareFunctionInNestedBlocks), Strings.resourceCulture);

    internal static string FunctionInLoop => Strings.ResourceManager.GetString(nameof (FunctionInLoop), Strings.resourceCulture);

    internal static string IdentifierAlreadyDeclared => Strings.ResourceManager.GetString(nameof (IdentifierAlreadyDeclared), Strings.resourceCulture);

    internal static string IncrementPropertyWOSetter => Strings.ResourceManager.GetString(nameof (IncrementPropertyWOSetter), Strings.resourceCulture);

    internal static string IncrementReadonly => Strings.ResourceManager.GetString(nameof (IncrementReadonly), Strings.resourceCulture);

    internal static string InvalidLefthandSideInAssignment => Strings.ResourceManager.GetString(nameof (InvalidLefthandSideInAssignment), Strings.resourceCulture);

    internal static string InvalidPropertyName => Strings.ResourceManager.GetString(nameof (InvalidPropertyName), Strings.resourceCulture);

    internal static string InvalidRegExp => Strings.ResourceManager.GetString(nameof (InvalidRegExp), Strings.resourceCulture);

    internal static string InvalidTryToCallWithNew => Strings.ResourceManager.GetString(nameof (InvalidTryToCallWithNew), Strings.resourceCulture);

    internal static string InvalidTryToCallWithoutNew => Strings.ResourceManager.GetString(nameof (InvalidTryToCallWithoutNew), Strings.resourceCulture);

    internal static string InvalidTryToCreateWithNew => Strings.ResourceManager.GetString(nameof (InvalidTryToCreateWithNew), Strings.resourceCulture);

    internal static string InvalidTryToCreateWithoutNew => Strings.ResourceManager.GetString(nameof (InvalidTryToCreateWithoutNew), Strings.resourceCulture);

    internal static string TooManyArgumentsForFunction => Strings.ResourceManager.GetString(nameof (TooManyArgumentsForFunction), Strings.resourceCulture);

    internal static string TryingToGetProperty => Strings.ResourceManager.GetString(nameof (TryingToGetProperty), Strings.resourceCulture);

    internal static string TryingToSetProperty => Strings.ResourceManager.GetString(nameof (TryingToSetProperty), Strings.resourceCulture);

    internal static string UnexpectedEndOfSource => Strings.ResourceManager.GetString(nameof (UnexpectedEndOfSource), Strings.resourceCulture);

    internal static string UnexpectedToken => Strings.ResourceManager.GetString(nameof (UnexpectedToken), Strings.resourceCulture);

    internal static string UnknowIdentifier => Strings.ResourceManager.GetString(nameof (UnknowIdentifier), Strings.resourceCulture);

    internal static string VariableNotDefined => Strings.ResourceManager.GetString(nameof (VariableNotDefined), Strings.resourceCulture);

    internal static string LogicalNullishCoalescing => Strings.ResourceManager.GetString(nameof (LogicalNullishCoalescing), Strings.resourceCulture);
  }
}
