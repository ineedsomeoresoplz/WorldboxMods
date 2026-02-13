// Decompiled with JetBrains decompiler
// Type: EditorHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public static class EditorHelper
{
  public static bool HasArgument(string pName)
  {
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      if (commandLineArg.Contains(pName))
        return true;
    }
    return false;
  }

  public static string GetArgument(string pName)
  {
    string[] commandLineArgs = Environment.GetCommandLineArgs();
    for (int index = 0; index < commandLineArgs.Length; ++index)
    {
      if (commandLineArgs[index].Contains(pName))
        return commandLineArgs[index + 1];
    }
    return (string) null;
  }
}
