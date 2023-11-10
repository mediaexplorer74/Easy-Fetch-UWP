// Decompiled with JetBrains decompiler
// Type: NiL.JS.Module
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Statements;
using System;
using System.Collections.Generic;

namespace NiL.JS
{
  public class Module
  {
    private static readonly char[] _pathSplitChars = new char[2]
    {
      '\\',
      '/'
    };

    public ExportTable Exports { get; } = new ExportTable();

    public List<IModuleResolver> ModuleResolversChain { get; } = new List<IModuleResolver>();

    public ModuleEvaluationState EvaluationState { get; private set; }

    [Obsolete]
    public CodeBlock Root => this.Script.Root;

    [Obsolete]
    public string Code => this.Script.Code;

    public Script Script { get; private set; }

    public Context Context { get; private set; }

    public string FilePath { get; private set; }

    public Module(string code)
      : this(code, (CompilerMessageCallback) null, Options.None)
    {
    }

    public Module(string path, string code, GlobalContext globalContext)
      : this(path, code, (CompilerMessageCallback) null, Options.None, globalContext)
    {
    }

    public Module(string path, string code)
      : this(path, code, (CompilerMessageCallback) null, Options.None, (GlobalContext) null)
    {
    }

    public Module(string code, CompilerMessageCallback messageCallback)
      : this(code, messageCallback, Options.None)
    {
    }

    public Module(string virtualPath, string code, CompilerMessageCallback messageCallback)
      : this(virtualPath, code, messageCallback, Options.None, (GlobalContext) null)
    {
    }

    public Module(string code, CompilerMessageCallback messageCallback, Options options)
      : this((string) null, code, messageCallback, options)
    {
    }

    public Module(
      string code,
      CompilerMessageCallback messageCallback,
      Options options,
      GlobalContext globalContext)
      : this((string) null, code, messageCallback, options, globalContext)
    {
    }

    public Module(
      string virtualPath,
      string code,
      CompilerMessageCallback messageCallback = null,
      Options options = Options.None,
      GlobalContext globalContext = null)
      : this(virtualPath, Script.Parse(code, messageCallback, options), globalContext)
    {
    }

    public Module(string virtualPath, Script script, GlobalContext globalContext = null)
    {
      if (script == null)
        throw new ArgumentNullException();
      this.FilePath = virtualPath;
      this.Context = new Context((Context) (globalContext ?? Context.CurrentGlobalContext), true, (Function) null);
      this.Context._module = this;
      this.Context._thisBind = (JSValue) new GlobalObject(this.Context);
      this.Script = script;
      this.Context._strict = this.Script.Root._strict;
    }

    public Module()
      : this("")
    {
    }

    public void Run()
    {
      this.EvaluationState = ModuleEvaluationState.Evaluating;
      this.Script.Evaluate(this.Context);
      this.EvaluationState = ModuleEvaluationState.Evaluated;
    }

    public void Run(int timeLimitInMilliseconds)
    {
      int start = Environment.TickCount;
      bool debugging = this.Context.Debugging;
      this.Context.Debugging = true;
      DebuggerCallback debuggerCallback = (DebuggerCallback) ((context, e) =>
      {
        if (Environment.TickCount - start >= timeLimitInMilliseconds)
          throw new TimeoutException();
      });
      this.Context.DebuggerCallback += debuggerCallback;
      try
      {
        this.Run();
      }
      catch
      {
        this.EvaluationState = ModuleEvaluationState.Fail;
        throw;
      }
      finally
      {
        this.Context.Debugging = debugging;
        this.Context.DebuggerCallback -= debuggerCallback;
      }
    }

    internal Module Import(string importArg)
    {
      ModuleRequest moduleRequest = new ModuleRequest(this, importArg, Module.makeAbsolutePath(this, importArg));
      Module result = (Module) null;
      for (int index = 0; index < this.ModuleResolversChain.Count && !this.ModuleResolversChain[index].TryGetModule(moduleRequest, out result); ++index)
        result = (Module) null;
      if (result == null)
        throw new InvalidOperationException("Unable to load module \"" + moduleRequest.CmdArgument + "\"");
      if (result.FilePath == null)
        result.FilePath = moduleRequest.AbsolutePath;
      if (result.EvaluationState == ModuleEvaluationState.Default)
      {
        result.ModuleResolversChain.AddRange((IEnumerable<IModuleResolver>) this.ModuleResolversChain);
        result.Run();
      }
      return result;
    }

    private static string makeAbsolutePath(Module initiator, string path)
    {
      string[] collection = initiator.FilePath.Split(Module._pathSplitChars);
      string[] strArray = path.Split(Module._pathSplitChars);
      LinkedList<string> values = new LinkedList<string>((IEnumerable<string>) collection);
      if (strArray.Length != 0 && strArray[0] == "" || strArray[0].EndsWith(":"))
        values.Clear();
      else
        values.RemoveLast();
      for (int index = 0; index < strArray.Length; ++index)
        values.AddLast(strArray[index]);
      LinkedListNode<string> linkedListNode = values.First;
      while (linkedListNode != null)
      {
        if (linkedListNode.Value == "." || linkedListNode.Value == "" && linkedListNode.Previous != values.First)
        {
          linkedListNode = linkedListNode.Next;
          values.Remove(linkedListNode.Previous);
        }
        else if (linkedListNode.Value == ".." && linkedListNode.Previous != null)
        {
          linkedListNode = linkedListNode.Next;
          values.Remove(linkedListNode.Previous);
          values.Remove(linkedListNode.Previous);
        }
        else
          linkedListNode = linkedListNode.Next;
      }
      if (values.Last.Value.IndexOf('.') == -1)
        values.Last.Value += ".js";
      if (values.Count == 0 || !values.First.Value.EndsWith(":"))
        values.AddFirst("");
      return string.Join("/", (IEnumerable<string>) values);
    }
  }
}
