// Decompiled with JetBrains decompiler
// Type: WindowMetaGeneric`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class WindowMetaGeneric<TMetaObject, TData> : 
  StatsWindow,
  IMetaWindow,
  IInterestingPeopleWindow,
  IMetaWithFamiliesWindow
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
{
  [SerializeField]
  protected Image species_icon;
  [SerializeField]
  private Image _favorite_icon;
  protected NameInput _name_input;
  private BannerGeneric<TMetaObject, TData>[] _main_banners;
  internal string _initial_name;

  protected virtual TMetaObject meta_object => throw new NotImplementedException();

  public TMetaObject getMetaObject() => this.meta_object;

  public ICoreObject getCoreObject() => (ICoreObject) this.meta_object;

  protected override void create()
  {
    this.initMonoFields();
    this.initNameInput();
    this.initStuff();
    base.create();
  }

  private void initMonoFields()
  {
    this._name_input = ((Component) ((Component) this).gameObject.transform.FindRecursive("NameInputElement"))?.GetComponent<NameInput>();
    this._main_banners = ((Component) this).gameObject.transform.FindAllRecursive<BannerGeneric<TMetaObject, TData>>((Func<Transform, bool>) (p => ((Object) p).name == "Main Banner"));
  }

  internal override bool checkCancelWindow()
  {
    return (object) this.meta_object == null || !this.meta_object.isAlive() || base.checkCancelWindow();
  }

  protected virtual void initStuff()
  {
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.startShowingWindow();
  }

  public virtual void startShowingWindow()
  {
    this.clear();
    this.loadBanners();
    this.loadNameInput();
    this.showTopPartInformation();
  }

  protected virtual void showTopPartInformation()
  {
    if (Object.op_Inequality((Object) this.species_icon, (Object) null))
      this.species_icon.sprite = this.getActorIcon();
    this.updateFavoriteIcon();
  }

  private void loadBanners()
  {
    foreach (BannerBase mainBanner in this._main_banners)
      mainBanner.load((NanoObject) this.meta_object);
  }

  public void reloadBanner() => this.loadBanners();

  protected virtual void loadNameInput()
  {
    // ISSUE: method pointer
    ((UnityEvent<string>) this._name_input.inputField.onEndEdit).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CloadNameInput\u003Eb__18_0)));
    string pText = this.meta_object.data.name.Trim();
    this._initial_name = pText;
    this._name_input.setText(pText);
    ColorAsset color = this.meta_object.getColor();
    if (color != null)
      ((Graphic) this._name_input.textField).color = color.getColorText();
    else
      ((Graphic) this._name_input.textField).color = Toolbox.color_white;
    if (!this.meta_object.data.custom_name)
      return;
    this._name_input.SetOutline();
  }

  protected virtual void clear()
  {
  }

  protected virtual void OnDisable() => this._name_input.inputField.DeactivateInputField();

  protected virtual void initNameInput()
  {
  }

  protected virtual bool onNameChange(string pInput)
  {
    if (string.IsNullOrWhiteSpace(pInput) || this.meta_object.isRekt())
      return false;
    string pName = pInput.Trim();
    if (this._initial_name == pName)
      return false;
    this.meta_object.data.custom_name = true;
    this.meta_object.setName(pName, true);
    this._initial_name = pName;
    this._name_input.SetOutline();
    return true;
  }

  public void pressFavorite()
  {
    if ((object) this.meta_object == null)
      return;
    this.meta_object.setFavorite(!this.meta_object.isFavorite());
    this.updateFavoriteIcon();
    this.refreshMetaList();
    SpriteSwitcher.checkAllStates();
    if (!this.meta_object.isFavorite())
      return;
    WorldTip.showNowTop(this.getTipFavorite());
  }

  private void updateFavoriteIcon()
  {
    if (Object.op_Equality((Object) this._favorite_icon, (Object) null))
      return;
    if (this.meta_object.isFavorite())
      ((Graphic) this._favorite_icon).color = ColorStyleLibrary.m.favorite_selected;
    else
      ((Graphic) this._favorite_icon).color = ColorStyleLibrary.m.favorite_not_selected;
  }

  internal void tryShowPastNames()
  {
    List<NameEntry> pastNames1 = this.meta_object.data.past_names;
    // ISSUE: explicit non-virtual call
    if ((pastNames1 != null ? (__nonvirtual (pastNames1.Count) > 1 ? 1 : 0) : 0) == 0)
      return;
    List<NameEntry> pastNames2 = this.meta_object.data.past_names;
    // ISSUE: explicit non-virtual call
    this.showStatRow("past_names", (object) (pastNames2 != null ? __nonvirtual (pastNames2.Count) : 1), MetaType.None, -1L, "iconVillages", "past_names", new TooltipDataGetter(this.getTooltipPastNames));
  }

  internal TooltipData getTooltipPastNames()
  {
    return new TooltipData()
    {
      tip_name = "past_names",
      past_names = new ListPool<NameEntry>((ICollection<NameEntry>) this.meta_object.data.past_names),
      meta_type = this.meta_type
    };
  }

  protected string getTipFavorite() => "favorited";

  public virtual IEnumerable<Actor> getInterestingUnitsList()
  {
    return ((IMetaObject) (object) this.meta_object).getUnits();
  }

  public virtual IEnumerable<Family> getFamilies()
  {
    return ((IMetaObject) (object) this.meta_object).getFamilies();
  }

  public virtual bool hasFamilies() => ((IMetaObject) (object) this.meta_object).hasFamilies();

  protected Sprite getActorIcon()
  {
    return !(this.meta_object is IMetaObject metaObject) ? (Sprite) null : metaObject.getSpriteIcon();
  }
}
