// Decompiled with JetBrains decompiler
// Type: CenterTipCaller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CenterTipCaller : MonoBehaviour
{
  public string tip_title;
  public string tip_id;

  public void Show()
  {
    Tooltip.show((object) this, "normal", new TooltipData()
    {
      tip_name = this.tip_title,
      tip_description = this.tip_id
    });
  }
}
