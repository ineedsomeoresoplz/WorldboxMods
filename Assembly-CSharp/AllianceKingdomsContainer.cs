// Decompiled with JetBrains decompiler
// Type: AllianceKingdomsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class AllianceKingdomsContainer : 
  AllianceBannersContainer<KingdomBanner, Kingdom, KingdomData>
{
  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  protected void showBanner(Kingdom pKingdom)
  {
    KingdomBanner next = this.pool_elements.getNext();
    next.load((NanoObject) pKingdom);
    ((Component) next).AddComponent<DraggableLayoutElement>();
  }
}
