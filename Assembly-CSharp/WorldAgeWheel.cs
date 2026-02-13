// Decompiled with JetBrains decompiler
// Type: WorldAgeWheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldAgeWheel : MonoBehaviour
{
  private const int ANGLE_PER_PIECE = 45;
  [SerializeField]
  private Sprite _sprite_arrow;
  [SerializeField]
  private Sprite _sprite_arrow_pause;
  [SerializeField]
  private Image _image_arrow_main;
  [SerializeField]
  private Image _image_arrow_secondary;
  [SerializeField]
  private Transform _arrow_container_main;
  [SerializeField]
  private Transform _arrow_container_secondary;
  [SerializeField]
  private Transform _dimming_container;
  private WorldAgeWheelPiece[] _pieces;
  private Tweener _floating_tween;
  private bool _initialized;
  private float _target_arrow_angle_main;
  private float _target_arrow_angle_secondary;
  private float _current_arrow_angle_main;
  private float _current_arrow_angle_secondary;

  private WorldAgeManager _era_manager => World.world.era_manager;

  private MapStats _map_stats => World.world.map_stats;

  private void Awake()
  {
    this._floating_tween = (Tweener) TweenSettingsExtensions.SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveY(((Component) this).transform, ((Component) this).transform.localPosition.y - 4f, 2.5f, false), (Ease) 4), -1, (LoopType) 1);
  }

  public void init(WorldAgeElementAction pOnClickAction)
  {
    this._initialized = true;
    this._pieces = ((Component) this).GetComponentsInChildren<WorldAgeWheelPiece>();
    for (int pIndex = 0; pIndex < this._pieces.Length; ++pIndex)
    {
      WorldAgeWheelPiece piece = this._pieces[pIndex];
      piece.mask.alphaHitTestMinimumThreshold = 0.5f;
      piece.init(pIndex);
      piece.setAge(this._era_manager.getAgeFromSlot(pIndex));
      piece.addClickCallback(pOnClickAction);
    }
    this.updateElements();
  }

  private void Update() => this.updateArrowAnimation();

  private void updateArrowAnimation()
  {
    this.setArrowPosition(this._arrow_container_main, ref this._current_arrow_angle_main, ref this._target_arrow_angle_main);
  }

  private void setArrowPosition(
    Transform pContainer,
    ref float pCurrentAngle,
    ref float pTargetAngle)
  {
    pCurrentAngle = Mathf.LerpAngle(pCurrentAngle, pTargetAngle, Time.deltaTime * 5f);
    if (Mathf.Approximately(pCurrentAngle, pTargetAngle))
      pCurrentAngle = pTargetAngle;
    this.setRotation(pContainer, pCurrentAngle);
  }

  private void finishArrowAnimation()
  {
    this._current_arrow_angle_main = this._target_arrow_angle_main;
    this._current_arrow_angle_secondary = this._target_arrow_angle_secondary;
  }

  private void OnEnable()
  {
    TweenExtensions.Play<Tweener>(this._floating_tween);
    if (this._initialized)
      this.updateElements();
    this.finishArrowAnimation();
    this.updateArrowAnimation();
  }

  public void updateElements()
  {
    this.updateArrows();
    this.updateDimming();
    if (this._era_manager.isPaused())
      this._image_arrow_main.sprite = this._sprite_arrow_pause;
    else
      this._image_arrow_main.sprite = this._sprite_arrow;
  }

  private void updateArrows() => this._target_arrow_angle_main = this.getInitialAngle() + 22f;

  private void updateDimming()
  {
  }

  private float getInitialAngle() => (float) (45 * this._era_manager.getCurrentSlotIndex());

  private void setRotation(Transform pElement, float pAngle)
  {
    Quaternion quaternion = Quaternion.AngleAxis(pAngle, Vector3.back);
    pElement.localRotation = quaternion;
  }

  private void OnDisable() => TweenExtensions.Pause<Tweener>(this._floating_tween);

  public IReadOnlyCollection<WorldAgeWheelPiece> getPieces()
  {
    return (IReadOnlyCollection<WorldAgeWheelPiece>) this._pieces;
  }

  public WorldAgeWheelPiece getPiece(int pIndex) => this._pieces[pIndex];
}
