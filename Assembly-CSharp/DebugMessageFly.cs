// Decompiled with JetBrains decompiler
// Type: DebugMessageFly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DebugMessageFly : MonoBehaviour
{
  private List<string> listString = new List<string>();
  public Transform originTransform;
  private TextMesh textMesh;

  private void Awake() => this.textMesh = ((Component) this).GetComponent<TextMesh>();

  public void addString(string pText)
  {
    if ((double) this.textMesh.color.a < 0.30000001192092896)
      this.listString.Clear();
    else if (this.listString.Count > 20)
      this.listString.RemoveAt(0);
    this.listString.Add(pText);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this.originTransform.localPosition.x, this.originTransform.localPosition.y);
    ((Component) this).transform.localPosition = vector3;
    string str1 = "";
    foreach (string str2 in this.listString)
      str1 = $"{str1}{str2}\n";
    this.textMesh.text = str1;
    Color color = this.textMesh.color;
    color.a = 1f;
    this.textMesh.color = color;
  }

  public void moveUp()
  {
    Vector3 localPosition = ((Component) this).transform.localPosition;
    localPosition.y += 3f;
    ((Component) this).transform.localPosition = localPosition;
  }

  private void Update()
  {
    Vector3 localScale = ((Component) this).transform.localScale;
    localScale.x += 2f * Time.deltaTime;
    if ((double) localScale.x > 1.0)
      localScale.x = 1f;
    ((Component) this).transform.localScale = localScale;
    Vector3 localPosition = ((Component) this).transform.localPosition;
    localPosition.y += 0.5f * Time.deltaTime;
    ((Component) this).transform.localPosition = localPosition;
    Color color = this.textMesh.color;
    color.a -= 0.3f * Time.deltaTime;
    this.textMesh.color = color;
    if ((double) color.a > 0.0)
      return;
    Object.Destroy((Object) ((Component) this).gameObject);
    DebugMessage.instance.list.Remove(this);
  }
}
