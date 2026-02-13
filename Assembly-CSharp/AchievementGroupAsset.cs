// Decompiled with JetBrains decompiler
// Type: AchievementGroupAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class AchievementGroupAsset : BaseCategoryAsset, ILocalizedAsset
{
  [NonSerialized]
  public List<Achievement> achievements_list = new List<Achievement>();

  public override string getLocaleID() => "achievement_group_" + this.id;
}
