// Decompiled with JetBrains decompiler
// Type: WorldBehaviourAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class WorldBehaviourAsset : Asset
{
  [DefaultValue(true)]
  public bool enabled = true;
  [DefaultValue(true)]
  public bool enabled_on_minimap = true;
  [DefaultValue(1f)]
  public float interval = 1f;
  [DefaultValue(1f)]
  public float interval_random = 1f;
  public WorldBehaviourAction action;
  public WorldBehaviourAction action_world_clear;
  [DefaultValue(true)]
  public bool stop_when_world_on_pause = true;
  [NonSerialized]
  public WorldBehaviour manager;
}
