// Decompiled with JetBrains decompiler
// Type: WindowStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public struct WindowStats
{
  public int opens;
  public int closes;
  public int shows;
  public int hides;
  public string previous;
  public string current;

  public void setCurrent(string pCurrent)
  {
    if (this.current == pCurrent)
      return;
    if (this.current != null && this.previous != this.current)
      this.previous = this.current;
    this.current = pCurrent;
  }
}
