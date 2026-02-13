// Decompiled with JetBrains decompiler
// Type: TesterBehWaitFor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class TesterBehWaitFor : BehaviourActionTester
{
  private string _type;
  private int _amount;

  public TesterBehWaitFor(string pType, int pAmount)
  {
    this._type = pType;
    this._amount = pAmount;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    MetaTypeAsset metaTypeAsset = AssetManager.meta_type_library.get(this._type);
    if (metaTypeAsset == null)
    {
      Debug.LogError((object) ("TesterBehWaitFor: No asset found for type: " + this._type));
      return BehResult.Stop;
    }
    int num = 0;
    foreach (NanoObject nanoObject in metaTypeAsset.get_list())
    {
      ++num;
      if (num >= this._amount)
        return BehResult.Continue;
    }
    pObject.wait = 1.5f;
    return BehResult.RepeatStep;
  }
}
