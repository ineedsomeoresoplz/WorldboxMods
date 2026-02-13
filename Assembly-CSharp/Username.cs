// Decompiled with JetBrains decompiler
// Type: Username
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Threading.Tasks;

#nullable disable
public class Username
{
  public static bool isValid(string strToCheck) => false;

  public static async Task<bool> isTaken(string pUsername)
  {
    if (!Username.isValid(pUsername))
      return false;
    await Task.Yield();
    return false;
  }
}
