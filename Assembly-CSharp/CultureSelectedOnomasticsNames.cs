// Decompiled with JetBrains decompiler
// Type: CultureSelectedOnomasticsNames
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CultureSelectedOnomasticsNames : OnomasticsNameGenerator
{
  [SerializeField]
  private GameObject _main_container;
  [SerializeField]
  private GameObject _separator;
  private Culture _culture;
  private string _last_template;

  private MetaType _meta_type => MetaType.Unit;

  public void load(Culture pCulture)
  {
    string templateString = this.getTemplateString(pCulture);
    if (this._culture == pCulture && templateString == this._last_template)
      return;
    this._culture = pCulture;
    this._last_template = templateString;
    this.clickRegenerate();
  }

  public void update()
  {
    bool flag = this._culture.isRekt();
    this._main_container.SetActive(!flag);
    this._separator.SetActive(!flag);
    if (flag)
      return;
    this.updateNameGeneration(this._culture.getOnomasticData(this._meta_type));
  }

  public void click() => this.clickRegenerate();

  private string getTemplateString(Culture pCulture)
  {
    return pCulture.getOnomasticData(this._meta_type).getShortTemplate();
  }
}
