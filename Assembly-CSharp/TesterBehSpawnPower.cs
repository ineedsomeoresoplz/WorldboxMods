// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnPower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class TesterBehSpawnPower : BehaviourActionTester
{
  protected string _power;

  public TesterBehSpawnPower(string pPower = null) => this._power = pPower;

  public override BehResult execute(AutoTesterBot pObject)
  {
    string power = this._power;
    int num1 = Randy.randomInt(0, MapBox.width);
    int num2 = Randy.randomInt(0, MapBox.height);
    if (!AssetManager.powers.dict.ContainsKey(power))
    {
      Debug.LogError((object) ("TESTER ERROR... " + power));
      return BehResult.Continue;
    }
    GodPower pPower = AssetManager.powers.get(power);
    string currentBrush = Config.current_brush;
    Config.current_brush = Brush.getRandom();
    pObject.debugString = "rand_power_" + power;
    BehaviourActionBase<AutoTesterBot>.world.player_control.clickedFinal(new Vector2Int(num1, num2), pPower);
    Config.current_brush = currentBrush;
    return base.execute(pObject);
  }
}
