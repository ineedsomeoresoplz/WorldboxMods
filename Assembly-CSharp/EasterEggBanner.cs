// Decompiled with JetBrains decompiler
// Type: EasterEggBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class EasterEggBanner : MonoBehaviour
{
  [SerializeField]
  private GameObject _container_with_elements;
  private float _cur_random_accumulation;
  private const float BASE_CHANCE = 0.1f;
  private const float ACCUMULATION_STEP = 0.01f;
  private bool? _dragging_item;
  public Image main_image;

  private void OnEnable() => this.nextChance();

  private void nextChance()
  {
    bool flag = Randy.randomChance(0.1f + this._cur_random_accumulation);
    if (!flag)
      this._cur_random_accumulation += 0.01f;
    else
      this._cur_random_accumulation = 0.0f;
    this._container_with_elements.SetActive(flag);
  }

  private void clearChance()
  {
    this._cur_random_accumulation = 0.0f;
    this._container_with_elements.SetActive(false);
  }

  private void Update()
  {
    if (!this._container_with_elements.activeSelf)
      return;
    bool flag = Config.isDraggingItem();
    int num1 = flag ? 1 : 0;
    bool? draggingItem = this._dragging_item;
    int num2 = draggingItem.GetValueOrDefault() ? 1 : 0;
    if (num1 == num2 & draggingItem.HasValue)
      return;
    this._dragging_item = new bool?(flag);
    if (flag)
      return;
    this.clearChance();
  }
}
