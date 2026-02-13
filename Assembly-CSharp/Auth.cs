// Decompiled with JetBrains decompiler
// Type: Auth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using System;

#nullable disable
public static class Auth
{
  public static UserLoginWindow userLoginWindow;
  public static bool isLoggedIn = false;
  public static string userId;
  public static string userName;
  public static string displayName;
  public static string emailAddress;
  private static bool initialized = false;
  public static bool authLoaded = false;
  public static Promise authLoadedPromise = new Promise();

  public static void initializeAuth()
  {
    if (Auth.initialized)
      return;
    Auth.initialized = true;
  }

  public static void AuthStateChanged(object sender, EventArgs eventArgs)
  {
  }

  public static void signOut()
  {
  }

  public static bool isValidUsername(string username) => false;

  public static bool isValidEmail(string email) => false;
}
