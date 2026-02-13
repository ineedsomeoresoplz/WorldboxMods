// Decompiled with JetBrains decompiler
// Type: Beehive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class Beehive : BaseBuildingComponent
{
  public int honey;

  public void addHoney()
  {
    if (this.honey >= 10)
      return;
    ++this.honey;
    if (this.honey != 10)
      return;
    this.building.setHaveResourcesToCollect(true);
  }
}
