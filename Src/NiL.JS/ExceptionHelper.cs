// Decompiled with JetBrains decompiler
// Type: NiL.JS.ExceptionHelper
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NiL.JS
{
  internal static class ExceptionHelper
  {
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Throw(Error error) => throw new JSException(error);

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Throw(Error error, CodeNode exceptionMaker, string code) => throw new JSException(error, exceptionMaker, code);

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Throw(JSValue error) => throw new JSException(error ?? JSValue.undefined);

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Throw(Error error, Exception innerException) => throw new JSException(error, innerException);

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowArgumentNull(string message) => throw new ArgumentNullException(message);

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowVariableIsNotDefined(
      string variableName,
      Context context,
      CodeNode exceptionMaker)
    {
      string code = ExceptionHelper.GetCode(context);
      ExceptionHelper.Throw((Error) new ReferenceError(string.Format(Strings.VariableNotDefined, (object) variableName)), exceptionMaker, code);
    }

    private static string GetCode(Context context) => context.RootContext._owner?._functionDefinition?._body?.Code ?? context._module?.Script.Code;

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowVariableIsNotDefined(string variableName, CodeNode exceptionMaker) => ExceptionHelper.Throw((Error) new ReferenceError(string.Format(Strings.VariableNotDefined, (object) variableName)), exceptionMaker, (string) null);

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowIncrementPropertyWOSetter(object proprtyName) => ExceptionHelper.Throw((Error) new TypeError(string.Format(Strings.IncrementPropertyWOSetter, proprtyName)));

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowIncrementReadonly(object entityName) => ExceptionHelper.Throw((Error) new TypeError(string.Format(Strings.IncrementReadonly, entityName)));

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowUnknownToken(string code, int index)
    {
      CodeCoordinates codeCoordinates = CodeCoordinates.FromTextPosition(code, index, 0);
      ExceptionHelper.Throw((Error) new SyntaxError(string.Format(Strings.UnknowIdentifier, (object) ((IEnumerable<string>) code.Substring(index, System.Math.Min(50, code.Length - index)).Split(Tools.TrimChars)).FirstOrDefault<string>(), (object) codeCoordinates)));
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowSyntaxError(string message) => ExceptionHelper.Throw((Error) new SyntaxError(message));

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void ThrowSyntaxError(string message, string code, int position) => ExceptionHelper.ThrowSyntaxError(message, code, position, 0);

    [DebuggerStepThrough]
    internal static void ThrowSyntaxError(string message, string code, int position, int length)
    {
      CodeCoordinates codeCoordinates = CodeCoordinates.FromTextPosition(code, position, length);
      ExceptionHelper.Throw((Error) new SyntaxError(message + " " + codeCoordinates?.ToString()));
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static T ThrowIfNotExists<T>(T obj, object name) where T : JSValue
    {
      if (obj._valueType == JSValueType.NotExists)
        ExceptionHelper.Throw((Error) new ReferenceError("Variable \"" + name?.ToString() + "\" is not defined."));
      return obj;
    }

    [DebuggerStepThrough]
    internal static void ThrowReferenceError(
      string message,
      string code,
      int position,
      int length)
    {
      CodeCoordinates codeCoordinates = CodeCoordinates.FromTextPosition(code, position, 0);
      ExceptionHelper.Throw((Error) new ReferenceError(message + " " + codeCoordinates?.ToString()));
    }

    [DebuggerStepThrough]
    internal static void ThrowReferenceError(string message) => ExceptionHelper.Throw((Error) new ReferenceError(message));

    [DebuggerStepThrough]
    internal static void ThrowTypeError(string message, CodeNode exceptionMaker, Context context)
    {
      string code = ExceptionHelper.GetCode(context);
      ExceptionHelper.Throw((Error) new TypeError(message), exceptionMaker, code);
    }

    [DebuggerStepThrough]
    internal static void ThrowTypeError(string message) => ExceptionHelper.Throw((Error) new TypeError(message));

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Throw(Exception exception) => throw exception;
  }
}
