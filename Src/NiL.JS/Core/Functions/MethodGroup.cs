// Decompiled with JetBrains decompiler
// Type: NiL.JS.Core.Functions.MethodGroup
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core.Interop;
using System;
using System.Reflection;

namespace NiL.JS.Core.Functions
{
  [Prototype(typeof (Function), true)]
  internal sealed class MethodGroup : Function
  {
    private const int PassesCount = 3;
    private readonly MethodProxy[] _methods;

    public override JSValue prototype
    {
      get => (JSValue) null;
      set
      {
      }
    }

    public override string name => this._methods[0].name;

    public MethodGroup(MethodProxy[] methods)
      : base(Context.CurrentContext ?? (Context) Context._DefaultGlobalContext)
    {
      this._methods = methods;
      if (methods == null)
        throw new ArgumentNullException();
      int val1 = 0;
      for (int index = 0; index < methods.Length; ++index)
        val1 = System.Math.Max(val1, methods[index]._parameters.Length);
      Number number = new Number(val1);
      number._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.ReadOnly;
      this._length = number;
    }

    protected internal override JSValue Invoke(
      bool construct,
      JSValue targetObject,
      Arguments arguments)
    {
      int iValue = arguments == null ? 0 : arguments._iValue;
      object[] parameters = (object[]) null;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < this._methods.Length; ++index2)
        {
          if (this._methods[index2]._parameters.Length == 1 && (object) this._methods[index2]._parameters[0].ParameterType == (object) typeof (Arguments))
            return this.Context.GlobalContext.ProxyValue((object) this._methods[index2].Call(targetObject, arguments));
          if (index1 == 2 || this._methods[index2]._parameters.Length == iValue)
          {
            if (iValue != 0)
            {
              parameters = this._methods[index2].ConvertArguments(arguments, (ConvertArgsOptions) ((index1 >= 1 ? 0 : 2) | (index1 >= 2 ? 4 : 0)));
              if (parameters != null)
              {
                int index3 = parameters.Length;
                while (index3-- > 0)
                {
                  if ((parameters[index3] != null ? (!TypeExtensions.IsAssignableFrom(this._methods[index2]._parameters[index3].ParameterType, parameters[index3].GetType()) ? 1 : 0) : (this._methods[index2]._parameters[index3].ParameterType.GetTypeInfo().IsValueType ? 1 : 0)) != 0)
                  {
                    index3 = 0;
                    parameters = (object[]) null;
                  }
                }
                if (parameters == null)
                  continue;
              }
              else
                continue;
            }
            object targetObject1 = this._methods[index2].GetTargetObject(targetObject, this._methods[index2]._hardTarget);
            object source;
            try
            {
              source = this._methods[index2]._method.Invoke(targetObject1, parameters);
              if (this._methods[index2]._returnConverter != null)
                source = this._methods[index2]._returnConverter.From(source);
            }
            catch (Exception ex)
            {
              Exception innerException = ex;
              while (innerException.InnerException != null)
                innerException = innerException.InnerException;
              if (innerException is JSException)
                throw innerException;
              ExceptionHelper.Throw((Error) new TypeError(innerException.Message), innerException);
              throw;
            }
            return this.Context.GlobalContext.ProxyValue(source);
          }
        }
      }
      ExceptionHelper.Throw((Error) new TypeError("Invalid arguments for function " + this._methods[0].name));
      return (JSValue) null;
    }
  }
}
