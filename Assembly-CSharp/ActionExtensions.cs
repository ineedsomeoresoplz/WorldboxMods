// Decompiled with JetBrains decompiler
// Type: ActionExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public static class ActionExtensions
{
  public static bool[] Run(this WorldAction pAction, BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    Delegate[] invocationList = pAction.GetInvocationList();
    bool[] flagArray = new bool[invocationList.Length];
    int num = 0;
    foreach (WorldAction worldAction in invocationList)
      flagArray[num++] = worldAction(pTarget, pTile);
    return flagArray;
  }

  public static bool RunAnyTrue(this WorldAction pAction, BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    Delegate[] invocationList = pAction.GetInvocationList();
    bool flag = false;
    foreach (WorldAction worldAction in invocationList)
    {
      if (worldAction(pTarget, pTile))
        flag = true;
    }
    return flag;
  }

  public static bool[] Run(
    this AttackAction pAction,
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    Delegate[] invocationList = pAction.GetInvocationList();
    bool[] flagArray = new bool[invocationList.Length];
    int num = 0;
    foreach (AttackAction attackAction in invocationList)
      flagArray[num++] = attackAction(pSelf, pTarget, pTile);
    return flagArray;
  }

  public static bool RunAnyTrue(
    this AttackAction pAction,
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    Delegate[] invocationList = pAction.GetInvocationList();
    bool flag = false;
    foreach (AttackAction attackAction in invocationList)
    {
      if (attackAction(pSelf, pTarget, pTile))
        flag = true;
    }
    return flag;
  }
}
