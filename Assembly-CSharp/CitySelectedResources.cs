// Decompiled with JetBrains decompiler
// Type: CitySelectedResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CitySelectedResources : UICityResources
{
  public void update(City pCity)
  {
    this.meta_object = pCity;
    this.showResources();
  }

  protected override void OnEnable()
  {
  }

  protected override void onListChange()
  {
    if (this.city == null)
      return;
    base.onListChange();
  }
}
