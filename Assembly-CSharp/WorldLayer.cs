// Decompiled with JetBrains decompiler
// Type: WorldLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldLayer : MapLayer
{
  public override void update(float pElapsed)
  {
  }

  public override void draw(float pElapsed) => this.UpdateDirty(pElapsed);
}
