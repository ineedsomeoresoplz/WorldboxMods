// Decompiled with JetBrains decompiler
// Type: Login
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Threading.Tasks;

#nullable disable
public class Login
{
  public static string createLoginQueueItemAsJSON(string username, string password) => "" + "";

  public static async void GetEmailForUsername(
    string username,
    string password,
    Action<string, string> resultCallback)
  {
    await Task.Yield();
  }

  private static void UnsubscribeLoginQueueListener()
  {
  }
}
