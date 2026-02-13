// Decompiled with JetBrains decompiler
// Type: ActorDataObsolete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
public class ActorDataObsolete
{
  public List<long> saved_items;
  [DefaultValue(null)]
  public ActorBag inventory;
  public ActorData status;
  [DefaultValue(-1)]
  public long cityID = -1;
  public int x;
  public int y;
}
