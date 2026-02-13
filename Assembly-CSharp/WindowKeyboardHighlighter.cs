// Decompiled with JetBrains decompiler
// Type: WindowKeyboardHighlighter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
internal class WindowKeyboardHighlighter : MonoBehaviour
{
  private static List<InputField> inputFields = new List<InputField>();
  private bool rescan;
  private int noInputs;
  private bool anyFocused;

  private void OnEnable()
  {
    if (!TouchScreenKeyboard.isSupported)
      Object.Destroy((Object) ((Component) this).GetComponent<WindowKeyboardHighlighter>());
    else
      this.findInputFields();
  }

  private void findInputFields()
  {
    this.noInputs = 0;
    this.rescan = false;
    WindowKeyboardHighlighter.inputFields.Clear();
    foreach (Transform componentsInChild in ((Component) ((Component) this).transform).GetComponentsInChildren<Transform>())
    {
      if (((Component) componentsInChild).HasComponent<InputField>())
        WindowKeyboardHighlighter.inputFields.Add(((Component) componentsInChild).GetComponent<InputField>());
    }
  }

  private void OnDisable() => WindowKeyboardHighlighter.inputFields.Clear();

  private void up(InputField pInput)
  {
    int num = Screen.height / 4 * 3;
    if ((double) ((Component) pInput).transform.position.y >= (double) num)
      return;
    Vector3 localPosition = ((Component) this).gameObject.transform.localPosition;
    localPosition.y += 10f;
    ((Component) this).transform.localPosition = localPosition;
  }

  private void down()
  {
    if ((double) ((Component) this).gameObject.transform.localPosition.y <= 10.0)
      return;
    Vector3 localPosition = ((Component) this).gameObject.transform.localPosition;
    localPosition.y -= 5f;
    ((Component) this).transform.localPosition = localPosition;
  }

  private void Update()
  {
    this.anyFocused = false;
    if (WindowKeyboardHighlighter.inputFields.Count == 0)
      ++this.noInputs;
    foreach (InputField inputField in WindowKeyboardHighlighter.inputFields)
    {
      if (!((Component) inputField).gameObject.activeInHierarchy)
        this.rescan = true;
      if (inputField.isFocused)
      {
        this.up(inputField);
        this.anyFocused = true;
      }
    }
    if (!this.anyFocused)
      this.down();
    if (!this.rescan && this.noInputs <= 60)
      return;
    this.findInputFields();
  }
}
