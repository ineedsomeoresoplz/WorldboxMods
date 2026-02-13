// Decompiled with JetBrains decompiler
// Type: SpectateUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpectateUnit : MonoBehaviour
{
  private Actor _actor;
  public Text unitName;
  public UnitAvatarLoader avatarLoader;
  public KingdomBanner kingdomBannerR;
  public KingdomBanner kingdomBannerL;
  public Text text_age;
  public Text text_kills;
  public StatBar health_bar;

  public void updateStats()
  {
    this.unitName.text = this._actor.coloredName;
    this.text_age.text = Toolbox.formatNumber((long) this._actor.getAge());
    this.text_kills.text = Toolbox.formatNumber((long) this._actor.data.kills);
    this.health_bar.setBar((float) this._actor.getHealth(), (float) this._actor.getMaxHealth(), "/" + this._actor.getMaxHealth().ToText(4), false);
    if (this._actor.hasKingdom() && this._actor.isKingdomCiv())
    {
      ((Component) this.kingdomBannerR).gameObject.SetActive(true);
      ((Component) this.kingdomBannerL).gameObject.SetActive(true);
      this.kingdomBannerR.load((NanoObject) this._actor.kingdom);
      this.kingdomBannerL.load((NanoObject) this._actor.kingdom);
    }
    else
    {
      ((Component) this.kingdomBannerR).gameObject.SetActive(false);
      ((Component) this.kingdomBannerL).gameObject.SetActive(false);
    }
  }

  public void clickKingdomElement()
  {
    if (Input.touchCount > 1)
      return;
    SelectedMetas.selected_kingdom = this._actor.kingdom;
    ScrollWindow.showWindow("kingdom");
  }

  public void clickLocate()
  {
    if (Input.touchCount > 1 || ScrollWindow.isAnimationActive())
      return;
    WorldLog.locationFollow(this._actor);
  }

  public void clickInspect()
  {
    if (Input.touchCount > 1 || ScrollWindow.isAnimationActive() || this._actor == null || !this._actor.isAlive())
      return;
    ScrollWindow.moveAllToLeftAndRemove();
    ActionLibrary.openUnitWindow(this._actor);
  }

  private void OnEnable()
  {
  }

  private void Update()
  {
    if (!MoveCamera.inSpectatorMode())
      return;
    if (!MoveCamera.isCameraFollowingUnit(this._actor))
      this._actor = MoveCamera.getFocusUnit();
    if (this._actor == null)
      return;
    this.updateStats();
  }
}
