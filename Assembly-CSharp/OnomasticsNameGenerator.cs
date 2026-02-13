// Decompiled with JetBrains decompiler
// Type: OnomasticsNameGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class OnomasticsNameGenerator : MonoBehaviour
{
  private const int MAX_STRING_LENGTH = 250;
  private const int AMOUNT_NAME_EXAMPLES = 20;
  private const float TIMER_NAME_INTERVAL = 0.2f;
  private float _timer_name;
  private int _index_name;
  public Text text_name_examples;

  private string _male_color => ColorStyleLibrary.m.color_text_pumpkin;

  private string _female_color => ColorStyleLibrary.m.color_text_pumpkin_light;

  private string _separator_color => ColorStyleLibrary.m.color_text_grey_dark;

  protected void updateNameGeneration(OnomasticsData pData)
  {
    this._timer_name += Time.deltaTime;
    if ((double) this._timer_name <= 0.20000000298023224)
      return;
    this._timer_name = 0.0f;
    if (this._index_name >= 20 || Toolbox.removeRichTextTags(this.text_name_examples.text).Length >= 250)
      return;
    ++this._index_name;
    ActorSex pSex = Randy.randomBool() ? ActorSex.Female : ActorSex.Male;
    string name = pData.generateName(pSex);
    if (name == "Rebr")
    {
      if (this.text_name_examples.text != string.Empty)
        return;
      pSex = pSex == ActorSex.Female ? ActorSex.Male : ActorSex.Female;
      name = pData.generateName(pSex);
    }
    if (string.IsNullOrEmpty(name))
      return;
    string text = this.text_name_examples.text;
    if (text.Length > 0)
      text += ", ";
    this.text_name_examples.text = text + Toolbox.coloredText(name, pSex == ActorSex.Male ? this._male_color : this._female_color);
    this.textExamplesEffect(((Component) this.text_name_examples).gameObject.transform, pPower: 0.03f);
  }

  private void textExamplesEffect(
    Transform pTransformTarget,
    float pDefaultScale = 1f,
    float pPower = 0.1f,
    float pDuration = 0.3f)
  {
    ShortcutExtensions.DOKill((Component) pTransformTarget, true);
    pTransformTarget.localScale = new Vector3(pDefaultScale, pDefaultScale, pDefaultScale);
    ShortcutExtensions.DOPunchScale(pTransformTarget, new Vector3(pPower, pPower, pPower), pDuration, 1, 1f);
  }

  protected void clickRegenerate()
  {
    this._index_name = 0;
    this.text_name_examples.text = string.Empty;
    ((Graphic) this.text_name_examples).color = Toolbox.makeColor(this._separator_color);
  }
}
