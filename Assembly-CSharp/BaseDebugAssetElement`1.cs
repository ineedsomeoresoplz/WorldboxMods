// Decompiled with JetBrains decompiler
// Type: BaseDebugAssetElement`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BaseDebugAssetElement<TAsset> : MonoBehaviour where TAsset : Asset
{
  public static TAsset selected_asset;
  internal TAsset asset;
  public Sprite no_animation;
  public Button asset_button;
  public Text title;
  public Text stats_description;
  public Text stats_values;
  internal RectTransform rect_transform;

  private void Awake()
  {
    this.rect_transform = ((Component) this).GetComponent<RectTransform>();
    Button.ButtonClickedEvent onClick = this.asset_button.onClick;
    BaseDebugAssetElement<TAsset> debugAssetElement = this;
    // ISSUE: virtual method pointer
    UnityAction unityAction = new UnityAction((object) debugAssetElement, __vmethodptr(debugAssetElement, showAssetWindow));
    ((UnityEvent) onClick).AddListener(unityAction);
    // ISSUE: method pointer
    this.asset_button.OnHover(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__8_0)));
  }

  public virtual void setData(TAsset pAsset)
  {
    this.asset = pAsset;
    this.title.text = this.asset.id;
    this.initAnimations();
    this.initStats();
  }

  protected virtual void initAnimations() => throw new NotImplementedException();

  public virtual void update() => throw new NotImplementedException();

  public virtual void stopAnimations() => throw new NotImplementedException();

  public virtual void startAnimations() => throw new NotImplementedException();

  protected virtual void initStats()
  {
    this.stats_description.text = "";
    this.stats_values.text = "";
  }

  protected void showStat(string pID, object pValue)
  {
    Text statsDescription = this.stats_description;
    statsDescription.text = $"{statsDescription.text}{LocalizedTextManager.getText(pID)}\n";
    Text statsValues = this.stats_values;
    statsValues.text = $"{statsValues.text}{pValue?.ToString()}\n";
  }

  protected virtual void showAssetWindow()
  {
    BaseDebugAssetElement<TAsset>.selected_asset = this.asset;
  }
}
