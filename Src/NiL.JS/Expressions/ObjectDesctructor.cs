// Decompiled with JetBrains decompiler
// Type: NiL.JS.Expressions.ObjectDesctructor
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using System;
using System.Collections.Generic;

namespace NiL.JS.Expressions
{
  public sealed class ObjectDesctructor : Expression
  {
    private readonly Expression _definition;

    protected internal override bool ContextIndependent => false;

    protected internal override PredictedType ResultType => PredictedType.Object;

    public bool Force { get; internal set; }

    public ObjectDesctructor(Expression definition)
    {
      switch (definition)
      {
        case ObjectDefinition _:
        case ArrayDefinition _:
          this._definition = ObjectDesctructor.CheckObjectDefinition(definition as ObjectDefinition, false) || ObjectDesctructor.CheckObjectDefinition(definition as ArrayDefinition, false) ? definition : throw new ArgumentException(nameof (definition));
          break;
        default:
          throw new ArgumentException(nameof (definition));
      }
    }

    public static bool CheckObjectDefinition(ArrayDefinition arrayDefinition, bool @throw)
    {
      if (arrayDefinition == null)
        return true;
      for (int index = 0; index < arrayDefinition.Elements.Length; ++index)
      {
        if (!ExpressionTree.canBeAssignee(arrayDefinition.Elements[index]))
        {
          if (@throw)
            ExceptionHelper.ThrowReferenceError(Strings.InvalidLefthandSideInAssignment);
          return false;
        }
      }
      return true;
    }

    public static bool CheckObjectDefinition(ObjectDefinition objectDefinition, bool @throw)
    {
      if (objectDefinition == null)
        return true;
      for (int index = 0; index < objectDefinition.Values.Length; ++index)
      {
        if (!ExpressionTree.canBeAssignee(objectDefinition.Values[index]))
        {
          if (@throw)
            ExceptionHelper.ThrowReferenceError(Strings.InvalidLefthandSideInAssignment);
          return false;
        }
      }
      for (int index = 0; index < objectDefinition.ComputedProperties.Length; ++index)
      {
        if (!ExpressionTree.canBeAssignee(objectDefinition.ComputedProperties[index].Value))
        {
          if (@throw)
            ExceptionHelper.ThrowReferenceError(Strings.InvalidLefthandSideInAssignment);
          return false;
        }
      }
      return true;
    }

    public override JSValue Evaluate(Context context) => throw new InvalidOperationException();

    protected internal override JSValue EvaluateForWrite(Context context) => (JSValue) new ObjectDesctructor.DestructuringAcceptor(this._definition, context, this.Force);

    public override bool Build(
      ref CodeNode _this,
      int expressionDepth,
      Dictionary<string, VariableDescriptor> variables,
      CodeContext codeContext,
      InternalCompilerMessageCallback message,
      FunctionInfo stats,
      Options opts)
    {
      return this._definition.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
    }

    public IList<Variable> GetTargetVariables()
    {
      List<Variable> result = new List<Variable>();
      ObjectDesctructor.collectTargetVariables(this._definition as ObjectDefinition, result);
      ObjectDesctructor.collectTargetVariables(this._definition as ArrayDefinition, result);
      return (IList<Variable>) result;
    }

    private static void collectTargetVariables(
      ArrayDefinition arrayDefinition,
      List<Variable> result)
    {
      if (arrayDefinition == null)
        return;
      for (int index = 0; index < arrayDefinition.Elements.Length; ++index)
      {
        if (arrayDefinition.Elements[index] is Variable)
        {
          result.Add((Variable) arrayDefinition.Elements[index]);
        }
        else
        {
          ObjectDesctructor.collectTargetVariables(arrayDefinition.Elements[index] as ObjectDefinition, result);
          ObjectDesctructor.collectTargetVariables(arrayDefinition.Elements[index] as ArrayDefinition, result);
        }
      }
    }

    private static void collectTargetVariables(
      ObjectDefinition objectDefinition,
      List<Variable> result)
    {
      if (objectDefinition == null)
        return;
      for (int index = 0; index < objectDefinition.Values.Length; ++index)
      {
        if (objectDefinition.Values[index] is Variable)
        {
          result.Add((Variable) objectDefinition.Values[index]);
        }
        else
        {
          ObjectDesctructor.collectTargetVariables(objectDefinition.Values[index] as ObjectDefinition, result);
          ObjectDesctructor.collectTargetVariables(objectDefinition.Values[index] as ArrayDefinition, result);
        }
      }
      for (int index = 0; index < objectDefinition.ComputedProperties.Length; ++index)
      {
        if (objectDefinition.ComputedProperties[index].Value is Variable)
        {
          result.Add((Variable) objectDefinition.ComputedProperties[index].Value);
        }
        else
        {
          ObjectDesctructor.collectTargetVariables(objectDefinition.ComputedProperties[index].Value as ObjectDefinition, result);
          ObjectDesctructor.collectTargetVariables(objectDefinition.ComputedProperties[index].Value as ArrayDefinition, result);
        }
      }
    }

    public override string ToString() => this._definition.ToString();

    private sealed class DestructuringAcceptor : JSValue
    {
      private readonly Context _context;
      private readonly bool _force;
      private readonly Expression _definition;

      public DestructuringAcceptor(Expression definition, Context context, bool force)
      {
        this._definition = definition;
        this._context = context;
        this._force = force;
      }

      public override void Assign(JSValue value)
      {
        this.assignValues(value, this._definition as ObjectDefinition);
        this.assignValues(value, this._definition as ArrayDefinition);
      }

      private void assignValues(JSValue source, ArrayDefinition targetMap)
      {
        if (targetMap == null)
          return;
        JSValue[] jsValueArray = new JSValue[targetMap.Elements.Length];
        for (int index = 0; index < targetMap.Elements.Length; ++index)
          jsValueArray[index] = source[Tools.Int32ToString(index)].CloneImpl(false);
        Arguments setterArgs = (Arguments) null;
        for (int index = 0; index < targetMap.Elements.Length; ++index)
        {
          if (targetMap.Elements[index] is ObjectDefinition)
          {
            this.assignValues(jsValueArray[index], targetMap.Elements[index] as ObjectDefinition);
            this.assignValues(jsValueArray[index], targetMap.Elements[index] as ArrayDefinition);
          }
          else
            setterArgs = this.assign(targetMap.Elements[index].EvaluateForWrite(this._context), jsValueArray[index], (object) targetMap.Elements[index], setterArgs);
        }
      }

      private void assignValues(JSValue source, ObjectDefinition targetMap)
      {
        if (targetMap == null)
          return;
        int index1 = 0;
        JSValue[] jsValueArray = new JSValue[targetMap.FieldNames.Length + targetMap.ComputedProperties.Length];
        int index2 = 0;
        while (index2 < targetMap.FieldNames.Length)
        {
          jsValueArray[index1] = source[targetMap.FieldNames[index2]].CloneImpl(false);
          ++index2;
          ++index1;
        }
        int index3 = 0;
        while (index3 < targetMap.ComputedProperties.Length)
        {
          jsValueArray[index1] = source.GetProperty(targetMap.ComputedProperties[index3].Key.Evaluate(this._context), false, PropertyScope.Common).CloneImpl(false);
          ++index3;
          ++index1;
        }
        Arguments setterArgs = (Arguments) null;
        int index4 = 0;
        int index5 = 0;
        while (index5 < targetMap.FieldNames.Length)
        {
          if (targetMap.Values[index5] is ObjectDefinition)
          {
            this.assignValues(jsValueArray[index4], targetMap.Values[index5] as ObjectDefinition);
            this.assignValues(jsValueArray[index4], targetMap.Values[index5] as ArrayDefinition);
          }
          else
            setterArgs = this.assign(targetMap.Values[index5].EvaluateForWrite(this._context), jsValueArray[index4], (object) targetMap.FieldNames[index5], setterArgs);
          ++index5;
          ++index4;
        }
        int index6 = 0;
        while (index6 < targetMap.ComputedProperties.Length)
        {
          if (targetMap.ComputedProperties[index6].Value is ObjectDefinition)
          {
            this.assignValues(jsValueArray[index4], targetMap.ComputedProperties[index6].Value as ObjectDefinition);
            this.assignValues(jsValueArray[index4], targetMap.ComputedProperties[index6].Value as ArrayDefinition);
          }
          else
            setterArgs = this.assign(targetMap.ComputedProperties[index6].Value.EvaluateForWrite(this._context), jsValueArray[index4], (object) targetMap.ComputedProperties[index6].Value, setterArgs);
          ++index6;
          ++index4;
        }
      }

      private Arguments assign(
        JSValue target,
        JSValue value,
        object targetName,
        Arguments setterArgs)
      {
        if (target._valueType == JSValueType.Property)
        {
          if (setterArgs == null)
            setterArgs = new Arguments();
          JSValue objectSource = this._context._objectSource;
          setterArgs.Reset();
          setterArgs.Add(value);
          Function setter = (target._oValue as NiL.JS.Core.PropertyPair).setter;
          if (setter != null)
            setter.Call(objectSource, setterArgs);
          else if (this._context._strict)
            ExceptionHelper.ThrowTypeError(string.Format(Strings.CannotAssignReadOnly, targetName));
        }
        else if ((target._attributes & JSValueAttributesInternal.ReadOnly) != JSValueAttributesInternal.None)
        {
          if (this._force)
          {
            target._attributes &= ~JSValueAttributesInternal.ReadOnly;
            target.Assign(value);
            target._attributes |= JSValueAttributesInternal.ReadOnly;
          }
          else if (this._context._strict)
            ExceptionHelper.ThrowTypeError(string.Format(Strings.CannotAssignReadOnly, targetName));
        }
        else
          target.Assign(value);
        return setterArgs;
      }
    }
  }
}
