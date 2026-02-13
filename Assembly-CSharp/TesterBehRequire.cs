// Decompiled with JetBrains decompiler
// Type: TesterBehRequire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class TesterBehRequire : BehaviourActionTester
{
  private string _type;
  private int _amount;
  private RequireCondition _cond;

  public TesterBehRequire(string pType, int pAmount, RequireCondition pCondition = RequireCondition.AtLeast)
  {
    this._type = pType;
    this._amount = pAmount;
    this._cond = pCondition;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    MetaTypeAsset metaTypeAsset = AssetManager.meta_type_library.get(this._type);
    if (metaTypeAsset == null)
    {
      Debug.LogError((object) ("TesterBehRequire: No asset found for type: " + this._type));
      return BehResult.Stop;
    }
    int num = 0;
    foreach (NanoObject nanoObject in metaTypeAsset.get_list())
      ++num;
    switch (this._cond)
    {
      case RequireCondition.AtLeast:
        if (num >= this._amount)
          break;
        goto default;
      case RequireCondition.AtMost:
        if (num <= this._amount)
          break;
        goto default;
      case RequireCondition.Exactly:
        if (num != this._amount)
          goto default;
        break;
      default:
        pObject.wait = 1.5f;
        return BehResult.Stop;
    }
    return BehResult.Continue;
  }
}
