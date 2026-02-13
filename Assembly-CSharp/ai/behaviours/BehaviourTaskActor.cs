// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehaviourTaskActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

[Serializable]
public class BehaviourTaskActor : BehaviourTaskBase<BehaviourActionActor>
{
  public bool move_from_block;
  public bool ignore_fight_check;
  public bool in_combat;
  public string force_hand_tool = string.Empty;
  public bool flag_boat_related;
  public bool diet;
  public bool cancellable_by_reproduction;
  public bool cancellable_by_socialize;
  public bool is_fireman;
  public string path_icon = "ui/Icons/iconWarning";
  public bool show_icon;
  public float speed_multiplier = 1f;
  [NonSerialized]
  public UnitHandToolAsset cached_hand_tool_asset;
  private Sprite _cached_sprite;

  protected override string locale_key_prefix => "task_unit";

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this._cached_sprite, (Object) null))
    {
      this._cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
      if (Object.op_Equality((Object) this._cached_sprite, (Object) null))
        Debug.LogError((object) ("No sprite found for " + this.path_icon));
    }
    return this._cached_sprite;
  }

  public void setIcon(string pPath)
  {
    this.path_icon = pPath;
    this.show_icon = true;
  }
}
