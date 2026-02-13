// Decompiled with JetBrains decompiler
// Type: KingdomsMetaContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class KingdomsMetaContainer : ListMetaContainer<KingdomListElement, Kingdom, KingdomData>
{
  protected override IEnumerable<Kingdom> getMetaList() => this.getMeta().getKingdoms();

  protected override Comparison<Kingdom> getSorting()
  {
    return new Comparison<Kingdom>(ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>.sortByPopulation);
  }
}
