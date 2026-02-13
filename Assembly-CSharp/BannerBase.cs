// Decompiled with JetBrains decompiler
// Type: BannerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System;
using UnityEngine;

#nullable disable
public abstract class BannerBase : MonoBehaviour, IBanner, IBaseMono, IRefreshElement
{
  private Sequence _sequence;

  protected virtual MetaType meta_type
  {
    get => throw new NotImplementedException(((object) this).GetType().Name);
  }

  public MetaCustomizationAsset meta_asset
  {
    get => AssetManager.meta_customization_library.getAsset(this.meta_type);
  }

  public MetaTypeAsset meta_type_asset => AssetManager.meta_type_library.getAsset(this.meta_type);

  internal int option_1
  {
    get => this.meta_asset.option_1_get();
    set => this.meta_asset.option_1_set(value);
  }

  internal int option_2
  {
    get => this.meta_asset.option_2_get();
    set => this.meta_asset.option_2_set(value);
  }

  internal int color
  {
    get => this.meta_asset.color_get();
    set => this.meta_asset.color_set(value);
  }

  public virtual void load(NanoObject pObject)
  {
  }

  public virtual NanoObject GetNanoObject() => (NanoObject) null;

  public void jump(float pSpeed = 0.1f, bool pSilent = false)
  {
    float y = ((Component) this).transform.localPosition.y;
    TweenExtensions.Kill((Tween) this._sequence, false);
    this._sequence = DOTween.Sequence();
    TweenSettingsExtensions.Append(this._sequence, (Tween) ShortcutExtensions.DOLocalMoveY(((Component) this).transform, y + 5f, pSpeed, false));
    TweenSettingsExtensions.Append(this._sequence, (Tween) ShortcutExtensions.DOLocalMoveY(((Component) this).transform, y, pSpeed, false));
    TweenSettingsExtensions.AppendCallback(this._sequence, (TweenCallback) (() =>
    {
      if (pSilent)
        return;
      SoundBox.click();
    }));
  }

  private void OnDisable() => TweenExtensions.Kill((Tween) this._sequence, false);

  public string getName() => this.GetNanoObject().name;

  public virtual void showTooltip() => throw new NotImplementedException();

  Transform IBaseMono.get_transform() => ((Component) this).transform;

  GameObject IBaseMono.get_gameObject() => ((Component) this).gameObject;

  T IBaseMono.GetComponent<T>() => ((Component) this).GetComponent<T>();
}
