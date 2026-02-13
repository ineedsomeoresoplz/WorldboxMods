// Decompiled with JetBrains decompiler
// Type: MapTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MapTag : MonoBehaviour
{
  public bool tagEnabled;
  public MapTagType tagType;
  public Sprite buttonOn;
  public Sprite buttonOff;
  public string icon;
  public CanvasGroup tagGroup;

  private void Start() => this.updateSprite();

  public void clickButton()
  {
  }

  public void clickListWorldsButton()
  {
  }

  private void updateSprite()
  {
  }
}
