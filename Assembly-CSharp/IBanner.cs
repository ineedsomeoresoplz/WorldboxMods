// Decompiled with JetBrains decompiler
// Type: IBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public interface IBanner : IBaseMono, IRefreshElement
{
  MetaCustomizationAsset meta_asset { get; }

  MetaTypeAsset meta_type_asset { get; }

  NanoObject GetNanoObject();

  void load(NanoObject pObject);

  string getName();

  void showTooltip();

  void jump(float pSpeed = 0.1f, bool pSilent = false)
  {
  }

  void IRefreshElement.refresh()
  {
    NanoObject nanoObject = this.GetNanoObject();
    if (nanoObject == null || !nanoObject.isAlive())
      return;
    this.load(nanoObject);
  }
}
