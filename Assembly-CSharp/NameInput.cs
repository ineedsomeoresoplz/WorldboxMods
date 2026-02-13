// Decompiled with JetBrains decompiler
// Type: NameInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class NameInput : MonoBehaviour
{
  public InputField inputField;
  public Text textField;
  private string LastValue;
  public bool can_be_empty;
  public bool is_onomastics;
  private Outline _outline;

  private void Start()
  {
    this.textField.horizontalOverflow = (HorizontalWrapMode) 0;
    if (this.is_onomastics)
    {
      // ISSUE: method pointer
      this.inputField.onValidateInput = new InputField.OnValidateInput((object) this, __methodptr(validateOnomastics));
    }
    else
    {
      // ISSUE: method pointer
      this.inputField.onValidateInput = new InputField.OnValidateInput((object) this, __methodptr(validate));
    }
  }

  private char validate(string pText, int pCharIndex, char pAddedChar)
  {
    return pAddedChar == '<' || pAddedChar == '>' ? char.MinValue : pAddedChar;
  }

  private char validateOnomastics(string pText, int pCharIndex, char pAddedChar)
  {
    char c1 = pAddedChar;
    bool flag1 = char.IsLetter(c1);
    bool flag2 = char.IsWhiteSpace(c1);
    bool flag3 = c1 == '\'';
    bool flag4 = pText.Length == 0;
    if (!(flag1 | flag2 | flag3))
      return char.MinValue;
    if (flag4)
      return char.ToUpper(c1);
    int c2 = (int) pText[pText.Length - 1];
    bool flag5 = char.IsLetter((char) c2);
    bool flag6 = char.IsWhiteSpace((char) c2);
    bool flag7 = c2 == 39;
    if (flag1)
    {
      if (flag5)
        c1 = char.ToLower(c1);
      else if (flag6)
        c1 = char.ToUpper(c1);
    }
    else if (flag2)
    {
      if (flag6)
        return char.MinValue;
    }
    else if (flag3 && flag7)
      return char.MinValue;
    return c1;
  }

  public void addListener(UnityAction<string> pAction)
  {
    ((UnityEvent<string>) this.inputField.onValueChanged).AddListener(pAction);
  }

  private void OnEnable()
  {
    // ISSUE: method pointer
    ((UnityEvent<string>) this.inputField.onEndEdit).AddListener(new UnityAction<string>((object) this, __methodptr(checkInput)));
  }

  private void OnDisable()
  {
    ((UnityEventBase) this.inputField.onEndEdit).RemoveAllListeners();
    if (!Object.op_Inequality((Object) this._outline, (Object) null))
      return;
    ((Behaviour) this._outline).enabled = false;
  }

  public void SetOutline()
  {
    if (Object.op_Equality((Object) this._outline, (Object) null))
      this._outline = ((Component) this.inputField).gameObject.AddOrGetComponent<Outline>();
    ((Behaviour) this._outline).enabled = true;
    Color color1 = ((Graphic) this.textField).color;
    Color color2;
    // ISSUE: explicit constructor call
    ((Color) ref color2).\u002Ector(color1.r, color1.g, color1.b, 0.2f);
    ((Shadow) this._outline).effectColor = color2;
  }

  private void checkInput(string pInput)
  {
    if (string.IsNullOrWhiteSpace(pInput) && !this.can_be_empty)
      this.inputField.text = this.LastValue;
    else
      this.LastValue = pInput;
  }

  public void setText(string pText)
  {
    this.textField.text = pText;
    this.inputField.text = pText;
    this.LastValue = pText;
  }
}
