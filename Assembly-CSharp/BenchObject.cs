// Decompiled with JetBrains decompiler
// Type: BenchObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BenchObject
{
  public int derp;

  public void update(float pElapsed)
  {
    this.updateMove(pElapsed);
    this.updateMove(pElapsed);
    this.updateMove(pElapsed);
    this.updateMove(pElapsed);
    this.updateMove(pElapsed);
  }

  public void updateMove(float pElapsed)
  {
    this.derp += 22;
    if (this.derp != 1000)
      return;
    this.derp += 10;
    if (this.derp < 10)
      this.derp += 5;
    else
      this.derp -= 5;
  }
}
