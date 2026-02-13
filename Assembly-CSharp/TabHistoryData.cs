// Decompiled with JetBrains decompiler
// Type: TabHistoryData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public struct TabHistoryData
{
  public readonly MetaType meta_type;
  public readonly long id;

  public TabHistoryData(NanoObject pObject)
  {
    this.meta_type = pObject.getMetaType();
    this.id = pObject.id;
  }

  public NanoObject getNanoObject()
  {
    return AssetManager.meta_type_library.getAsset(this.meta_type).get(this.id);
  }
}
