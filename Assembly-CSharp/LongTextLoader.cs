// Decompiled with JetBrains decompiler
// Type: LongTextLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LongTextLoader : MonoBehaviour
{
  public TextAsset textAsset;
  protected Text m_text;

  private void Start()
  {
    this.m_text = ((Component) this).GetComponent<Text>();
    this.create();
    this.finish();
  }

  private void finish()
  {
    RectTransform component1 = ((Component) this.m_text).GetComponent<RectTransform>();
    component1.sizeDelta = new Vector2(component1.sizeDelta.x, this.m_text.preferredHeight + 10f);
    RectTransform component2 = ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>();
    component2.sizeDelta = new Vector2(component2.sizeDelta.x, component1.sizeDelta.y);
    float num = -((Component) component2).transform.localPosition.y;
    ((Component) ((Transform) component2).parent).GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, component1.sizeDelta.y + 20f + num);
  }

  public virtual void create()
  {
    try
    {
      this.m_text.text = this.textAsset.text;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) "LongTextLoader: Text File is too long");
    }
  }
}
