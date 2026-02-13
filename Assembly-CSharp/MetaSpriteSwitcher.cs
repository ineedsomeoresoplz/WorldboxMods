// Decompiled with JetBrains decompiler
// Type: MetaSpriteSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MetaSpriteSwitcher : SpriteSwitcher
{
  public MetaType meta_type;

  protected override bool hasAny()
  {
    return AssetManager.meta_type_library.getAsset(this.meta_type).has_any();
  }
}
