// Decompiled with JetBrains decompiler
// Type: CorruptedTreesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class CorruptedTreesManager : MonoBehaviour
{
  private string currentString = "";
  private List<CorruptedTreeObject> _objects;
  public GameObject win_icon;

  public void Start()
  {
    this._objects = new List<CorruptedTreeObject>();
    for (int index = 0; index < ((Component) this).transform.childCount; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CorruptedTreesManager.\u003C\u003Ec__DisplayClass3_0 cDisplayClass30 = new CorruptedTreesManager.\u003C\u003Ec__DisplayClass3_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass30.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass30.tObj = ((Component) ((Component) this).transform.GetChild(index)).GetComponent<CorruptedTreeObject>();
      // ISSUE: reference to a compiler-generated field
      this._objects.Add(cDisplayClass30.tObj);
      // ISSUE: reference to a compiler-generated field
      ((Component) ((Component) cDisplayClass30.tObj).transform.GetChild(0)).gameObject.SetActive(false);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent) ((Component) cDisplayClass30.tObj).GetComponent<Button>().onClick).AddListener(new UnityAction((object) cDisplayClass30, __methodptr(\u003CStart\u003Eb__0)));
    }
    this.win_icon.gameObject.SetActive(false);
  }

  public void click(CorruptedTreeObject pObject)
  {
    if (pObject.used)
      return;
    pObject.used = true;
    ((Component) ((Component) pObject).transform.GetChild(0)).gameObject.SetActive(true);
    ((Behaviour) ((Component) pObject).GetComponent<Image>()).enabled = false;
    string str1;
    string str2 = str1 = "162534" ?? "";
    this.currentString += ((Object) ((Component) pObject).transform).name;
    string strB = str2.Substring(0, this.currentString.Length);
    if (str1.CompareTo(strB) == 0)
    {
      this.win();
    }
    else
    {
      if (strB.Contains(this.currentString))
        return;
      this.lost();
    }
  }

  private void win()
  {
    this.win_icon.gameObject.SetActive(true);
    AchievementLibrary.the_corrupted_trees.check();
  }

  private void lost()
  {
    foreach (Component component in this._objects)
      component.GetComponent<UiCreature>().click();
  }
}
