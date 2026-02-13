// Decompiled with JetBrains decompiler
// Type: MetaNeedsAndDialogueElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MetaNeedsAndDialogueElement : MetaNeedsElementBase
{
  protected override string getText(IMetaObject pMeta, out Actor pActorResult)
  {
    pActorResult = (Actor) null;
    return pMeta.countUnits() < 5 ? string.Empty : MetaTextReportHelper.getText(pMeta, pMeta.getMetaTypeAsset(), out pActorResult);
  }
}
