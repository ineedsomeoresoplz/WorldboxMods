// Decompiled with JetBrains decompiler
// Type: HappinessAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class HappinessAsset : Asset, ILocalizedAsset, IMultiLocalesAsset
{
  public HappinessDelegateCalc calc;
  public int value;
  public string pot_task_id;
  public int pot_amount;
  public int index;
  public string path_icon;
  public bool ignored_by_psychopaths;
  [DefaultValue(true)]
  public bool show_change_happiness_effect = true;
  public int dialogs_amount = 4;
  private Sprite _cached_sprite;

  public virtual Sprite getSprite()
  {
    if (this._cached_sprite == null)
      this._cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this._cached_sprite;
  }

  public string getLocaleID() => "happiness_" + this.id;

  public IEnumerable<string> getLocaleIDs()
  {
    for (int i = 0; i < this.dialogs_amount; ++i)
      yield return this.getHappinnessDialogID() + i.ToString();
  }

  public string getHappinnessDialogID() => $"happiness_dialog_{this.id}_";

  public string getTextSingleReport()
  {
    int num = Random.Range(0, this.dialogs_amount);
    return this.getHappinnessDialogID() + num.ToString();
  }

  public string getRandomTextSingleReportLocalized()
  {
    return LocalizedTextManager.getText(this.getTextSingleReport());
  }
}
