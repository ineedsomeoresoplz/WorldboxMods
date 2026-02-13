// Decompiled with JetBrains decompiler
// Type: ChangeTextInHindi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ChangeTextInHindi : MonoBehaviour
{
  private void Start()
  {
    string text = ((Component) this).gameObject.GetComponent<Text>().text;
    ((Component) this).gameObject.GetComponent<Text>().SetHindiText(text);
  }
}
