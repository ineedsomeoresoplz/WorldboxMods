// Decompiled with JetBrains decompiler
// Type: FaveWorldButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class FaveWorldButton : MonoBehaviour
{
  public Image icon;

  private void Start()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(faveWorld)));
  }

  private void OnEnable() => this.updateFavoriteIconFor(SaveManager.currentSlot);

  private void faveWorld()
  {
    int currentSlot = SaveManager.currentSlot;
    PlayerConfig.instance.data.favorite_world = PlayerConfig.instance.data.favorite_world != currentSlot ? currentSlot : -1;
    PlayerConfig.saveData();
    this.updateFavoriteIconFor(currentSlot);
  }

  private void updateFavoriteIconFor(int pId)
  {
    if (PlayerConfig.instance.data.favorite_world == pId)
      ((Graphic) this.icon).color = ColorStyleLibrary.m.favorite_selected;
    else
      ((Graphic) this.icon).color = ColorStyleLibrary.m.favorite_not_selected;
  }
}
