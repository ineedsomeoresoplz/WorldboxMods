// Decompiled with JetBrains decompiler
// Type: ButtonTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public struct ButtonTrigger(Button pButton, EventTrigger.Entry pEntry, int pIndex)
{
  public Button button { get; } = pButton;

  public EventTrigger.Entry entry { get; } = pEntry;

  public int index { get; } = pIndex;
}
