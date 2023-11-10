// Decompiled with JetBrains decompiler
// Type: NiL.JS.BaseLibrary.Promise
// Assembly: NiL.JS, Version=2.5.1514.0, Culture=neutral, PublicKeyToken=fa941a7c2a4de689
// MVID: 02CF2D1B-C531-464C-A1C8-3EDB7D11C54C
// Assembly location: C:\Users\Admin\Desktop\RE\XAML.UI\NiL.JS.dll

using NiL.JS.Core;
using NiL.JS.Core.Functions;
using NiL.JS.Core.Interop;
using NiL.JS.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NiL.JS.BaseLibrary
{
  public sealed class Promise
  {
    private readonly object _sync = new object();
    private System.Threading.Tasks.Task _innerTask;
    private Function _callback;
    private System.Threading.Tasks.Task<JSValue> _task;
    private JSValue _innerResult;

    [Hidden]
    public PromiseState State => !this.Task.IsCompleted ? PromiseState.Pending : Promise.statusToState(this._task.Status);

    [Hidden]
    public System.Threading.Tasks.Task<JSValue> Task => this._task;

    [Hidden]
    public bool Complited => this._task.IsCompleted;

    [Hidden]
    public JSValue Result => this.Task.Status != TaskStatus.RanToCompletion ? (this.Task.Exception.GetBaseException() as JSException).Error : this.Task.Result;

    [Hidden]
    public Function Callback => this._callback;

    public Promise(Function callback)
      : this()
    {
      this._callback = callback ?? Function.Empty;
      this._innerTask = new System.Threading.Tasks.Task(new Action(this.callbackInvoke));
      this._innerTask.Start();
    }

    private Promise() => this._task = new System.Threading.Tasks.Task<JSValue>((Func<JSValue>) (() =>
    {
      if (this._innerTask == null)
        return JSValue.undefined;
      try
      {
        this._innerTask.Wait();
      }
      catch
      {
      }
      switch (Promise.statusToState(this._innerTask.Status))
      {
        case PromiseState.Fulfilled:
          return this._innerResult;
        case PromiseState.Rejected:
          throw this._innerTask.Exception;
        default:
          return JSValue.undefined;
      }
    }));

    internal Promise(System.Threading.Tasks.Task<JSValue> task)
      : this()
    {
      Action<System.Threading.Tasks.Task<JSValue>> continuationAction = (Action<System.Threading.Tasks.Task<JSValue>>) (t =>
      {
        if (t.Status == TaskStatus.RanToCompletion)
        {
          this.handlePromiseCascade(t.Result);
        }
        else
        {
          if (t.Status == TaskStatus.Faulted)
          {
            this._task.Start();
            throw t.Exception.GetBaseException();
          }
          this._task.Start();
        }
      });
      this._innerTask = task.ContinueWith(continuationAction);
    }

    private void handlePromiseCascade(JSValue value)
    {
      System.Threading.Tasks.Task<JSValue> task = (value?.Value is Promise promise ? promise.Task : (System.Threading.Tasks.Task<JSValue>) null) ?? value?.Value as System.Threading.Tasks.Task<JSValue>;
      if (task != null)
      {
        task.ContinueWith((Action<System.Threading.Tasks.Task<JSValue>>) (t => this.handlePromiseCascade(t.Result)));
      }
      else
      {
        this._innerResult = value;
        this._task.Start();
      }
    }

    private void callbackInvoke()
    {
      bool statusSeted = false;
      bool reject = false;
      try
      {
        this._callback.Call(new Arguments((Context) null)
        {
          (JSValue) new ExternalFunction((ExternalFunctionDelegate) ((self, args) =>
          {
            if (!statusSeted)
            {
              statusSeted = true;
              this.handlePromiseCascade(args[0]);
            }
            return (JSValue) null;
          })),
          (JSValue) new ExternalFunction((ExternalFunctionDelegate) ((self, args) =>
          {
            if (!statusSeted)
            {
              reject = true;
              statusSeted = true;
              this.handlePromiseCascade(args[0]);
            }
            return (JSValue) null;
          }))
        });
      }
      catch (JSException ex)
      {
        this._innerResult = ex.Error;
        if (!statusSeted)
          this._task.Start();
        throw;
      }
      catch
      {
        this._innerResult = JSValue.Wrap((object) new Error("Unknown error"));
        if (!statusSeted)
          this._task.Start();
        throw;
      }
      if (!statusSeted)
        this._task.Start();
      if (reject)
        throw new JSException(this._innerResult);
    }

    public static Promise resolve(JSValue data) => new Promise(Promise.fromResult(data));

    public static Promise race(IIterable promises) => promises == null ? new Promise(Promise.fromException((Exception) new JSException((Error) new TypeError("Invalid argruments for Promise.race(...)")))) : new Promise(Promise.whenAny(promises.AsEnumerable().Select<JSValue, System.Threading.Tasks.Task<JSValue>>(new Func<JSValue, System.Threading.Tasks.Task<JSValue>>(Promise.convertToTask)).ToArray<System.Threading.Tasks.Task<JSValue>>()).ContinueWith<JSValue>((Func<System.Threading.Tasks.Task<System.Threading.Tasks.Task<JSValue>>, JSValue>) (x => x.Result.Result)));

    public static Promise all(IIterable promises) => promises == null ? new Promise(Promise.fromException((Exception) new JSException((Error) new TypeError("Invalid argruments for Promise.all(...)")))) : new Promise(Promise.whenAll(promises.AsEnumerable().Select<JSValue, System.Threading.Tasks.Task<JSValue>>(new Func<JSValue, System.Threading.Tasks.Task<JSValue>>(Promise.convertToTask)).ToArray<System.Threading.Tasks.Task<JSValue>>()).ContinueWith<JSValue>((Func<System.Threading.Tasks.Task<JSValue[]>, JSValue>) (x => (JSValue) new Array((IEnumerable) x.Result))));

    private static System.Threading.Tasks.Task<JSValue> convertToTask(JSValue arg) => (arg.Value is Promise promise ? promise.Task : (System.Threading.Tasks.Task<JSValue>) null) ?? Promise.fromResult(arg);

    public Promise @catch(Function onRejection) => this.then((Function) null, onRejection);

    public Promise then(Function onFulfilment, Function onRejection) => this.then(onFulfilment == null ? (Func<JSValue, JSValue>) null : (Func<JSValue, JSValue>) (value => onFulfilment.Call(JSValue.undefined, new Arguments()
    {
      value
    })), onRejection == null ? (Func<JSValue, JSValue>) null : (Func<JSValue, JSValue>) (value => onRejection.Call(JSValue.undefined, new Arguments()
    {
      value
    })));

    [Hidden]
    public Promise then(Func<JSValue, JSValue> onFulfilment, Func<JSValue, JSValue> onRejection)
    {
      if (onFulfilment == null && onRejection == null)
        return Promise.resolve(JSValue.undefined);
      System.Threading.Tasks.Task<JSValue> task1 = onFulfilment == null ? (System.Threading.Tasks.Task<JSValue>) null : this._task.ContinueWith<JSValue>((Func<System.Threading.Tasks.Task<JSValue>, JSValue>) (task => onFulfilment(this.Result)), TaskContinuationOptions.OnlyOnRanToCompletion);
      System.Threading.Tasks.Task<JSValue> task2 = onRejection == null ? (System.Threading.Tasks.Task<JSValue>) null : this._task.ContinueWith<JSValue>((Func<System.Threading.Tasks.Task<JSValue>, JSValue>) (task =>
      {
        Exception exception = (Exception) task.Exception;
        while (exception.InnerException != null)
          exception = exception.InnerException;
        return exception is JSException jsException2 ? onRejection(jsException2.Error) : onRejection(JSValue.Wrap((object) task.Exception));
      }), TaskContinuationOptions.NotOnRanToCompletion);
      if (task1 == null)
        return new Promise(task2);
      if (task2 == null)
        return new Promise(task1);
      return new Promise(Promise.whenAny(task1, task2).ContinueWith<JSValue>((Func<System.Threading.Tasks.Task<System.Threading.Tasks.Task<JSValue>>, JSValue>) (x =>
      {
        return !x.IsFaulted ? x.Result.Result : throw x.Exception;
      })));
    }

    private static System.Threading.Tasks.Task<System.Threading.Tasks.Task<JSValue>> whenAny(
      params System.Threading.Tasks.Task<JSValue>[] tasks)
    {
      System.Threading.Tasks.Task<JSValue> result = (System.Threading.Tasks.Task<JSValue>) null;
      System.Threading.Tasks.Task<System.Threading.Tasks.Task<JSValue>> task = new System.Threading.Tasks.Task<System.Threading.Tasks.Task<JSValue>>((Func<System.Threading.Tasks.Task<JSValue>>) (() => result));
      Action<System.Threading.Tasks.Task<JSValue>> continuationAction = (Action<System.Threading.Tasks.Task<JSValue>>) (t =>
      {
        lock (task)
        {
          if (result != null)
            return;
          result = t;
          task.Start();
        }
      });
      for (int index = 0; index < tasks.Length; ++index)
        tasks[index].ContinueWith(continuationAction, TaskContinuationOptions.NotOnCanceled);
      return task;
    }

    private static PromiseState statusToState(TaskStatus status)
    {
      switch (status)
      {
        case TaskStatus.Created:
        case TaskStatus.WaitingForActivation:
        case TaskStatus.WaitingToRun:
        case TaskStatus.Running:
        case TaskStatus.WaitingForChildrenToComplete:
          return PromiseState.Pending;
        case TaskStatus.RanToCompletion:
          return PromiseState.Fulfilled;
        case TaskStatus.Canceled:
        case TaskStatus.Faulted:
          return PromiseState.Rejected;
        default:
          return PromiseState.Rejected;
      }
    }

    private static System.Threading.Tasks.Task<JSValue[]> whenAll(System.Threading.Tasks.Task<JSValue>[] tasks)
    {
      JSValue[] result = new JSValue[tasks.Length];
      System.Threading.Tasks.Task<JSValue[]> task = new System.Threading.Tasks.Task<JSValue[]>((Func<JSValue[]>) (() => result));
      int count = tasks.Length;
      Action<System.Threading.Tasks.Task<JSValue>> continuationAction = (Action<System.Threading.Tasks.Task<JSValue>>) (t =>
      {
        int index = System.Array.IndexOf<System.Threading.Tasks.Task<JSValue>>(tasks, t);
        result[index] = !t.IsCanceled ? t.Result : throw new OperationCanceledException();
        if (Interlocked.Decrement(ref count) != 0)
          return;
        task.Start();
      });
      for (int index = 0; index < tasks.Length; ++index)
        tasks[index].ContinueWith(continuationAction, TaskContinuationOptions.OnlyOnRanToCompletion);
      return task;
    }

    private static System.Threading.Tasks.Task<JSValue> fromException(Exception exception)
    {
      System.Threading.Tasks.Task<JSValue> task = new System.Threading.Tasks.Task<JSValue>((Func<JSValue>) (() =>
      {
        throw exception;
      }));
      task.Start();
      return task;
    }

    private static System.Threading.Tasks.Task<JSValue> fromResult(JSValue arg) => System.Threading.Tasks.Task.FromResult<JSValue>(arg);
  }
}
