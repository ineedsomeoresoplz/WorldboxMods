// Decompiled with JetBrains decompiler
// Type: TaskExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
public static class TaskExtensions
{
  public static void WaitAndUnwrapException(this Task task)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    task.GetAwaiter().GetResult();
  }

  public static void WaitAndUnwrapException(this Task task, CancellationToken cancellationToken)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait(cancellationToken);
    }
    catch (AggregateException ex)
    {
      ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
      throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
    }
  }

  public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
  {
    return task != null ? task.GetAwaiter().GetResult() : throw new ArgumentNullException(nameof (task));
  }

  public static TResult WaitAndUnwrapException<TResult>(
    this Task<TResult> task,
    CancellationToken cancellationToken)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait(cancellationToken);
      return task.Result;
    }
    catch (AggregateException ex)
    {
      throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
    }
  }

  public static void WaitWithoutException(this Task task)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait();
    }
    catch (AggregateException ex)
    {
    }
  }

  public static void WaitWithoutException(this Task task, CancellationToken cancellationToken)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    try
    {
      task.Wait(cancellationToken);
    }
    catch (AggregateException ex)
    {
      cancellationToken.ThrowIfCancellationRequested();
    }
  }
}
