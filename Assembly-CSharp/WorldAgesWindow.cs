// Decompiled with JetBrains decompiler
// Type: WorldAgesWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldAgesWindow : MonoBehaviour
{
  private const float SLOW = 0.5f;
  private const float NORMAL = 1f;
  private const float FAST = 2f;
  private const float FAST_VERY = 5f;
  private const float FAST_ULTRA = 10f;
  private const float FAST_SONIC = 20f;
  private static WorldAgesWindow _instance;
  [SerializeField]
  private Text _age_name;
  [SerializeField]
  private WorldAgeButton _age_button_prefab;
  [SerializeField]
  private Sprite _play_sprite;
  [SerializeField]
  private Sprite _pause_sprite;
  [SerializeField]
  private Sprite _age_speed_sprite_slow;
  [SerializeField]
  private Sprite _age_speed_sprite_normal;
  [SerializeField]
  private Sprite _age_speed_sprite_fast;
  [SerializeField]
  private Sprite _age_speed_sprite_fast_very;
  [SerializeField]
  private Sprite _age_speed_sprite_fast_ultra;
  [SerializeField]
  private Sprite _age_speed_sprite_fast_sonic;
  [SerializeField]
  private WorldAgeWheel _age_wheel;
  [SerializeField]
  private Transform _grid_age_buttons;
  [SerializeField]
  private Image _pause_button_icon;
  [SerializeField]
  private Image _age_speed_button_icon;
  [SerializeField]
  private Image _selected_age_background;
  [SerializeField]
  private Image _background_filter;
  private Dictionary<WorldAgeAsset, WorldAgeButton> _buttons = new Dictionary<WorldAgeAsset, WorldAgeButton>();
  private WorldAgeWheelPiece _selected_piece;
  [SerializeField]
  private Text _text_time_info;

  private WorldAgeManager _era_manager => World.world.era_manager;

  private MapStats _map_stats => World.world.map_stats;

  private void Awake()
  {
    WorldAgesWindow._instance = this;
    this._age_wheel.init(new WorldAgeElementAction(this.wheelPieceAction));
    this.initButtons();
  }

  private void OnEnable()
  {
    this.selectPiece(this._era_manager.getCurrentSlotIndex());
    this.updateElements();
  }

  private void OnDisable() => this.updateElements();

  public static void setAgeAndSelectPiece(WorldAgeAsset pAsset, WorldAgeWheelPiece pPiece)
  {
    WorldAgesWindow._instance.setAgeAndSelectPieceInstance(pAsset, pPiece);
  }

  private void setAgeAndSelectPieceInstance(WorldAgeAsset pAsset, WorldAgeWheelPiece pPiece)
  {
    pPiece.setAge(pAsset);
    this._era_manager.setAgeToSlot(pAsset, pPiece.getIndex());
    this.selectPiece(pPiece);
    this._era_manager.setCurrentSlotIndex(pPiece.getIndex(), 0.01f);
    this.updateElements();
  }

  private void selectPiece(int pIndex) => this.selectPiece(this._age_wheel.getPiece(pIndex));

  private void selectPiece(WorldAgeWheelPiece pPiece) => this._selected_piece = pPiece;

  private void initButtons()
  {
    for (int index = 0; index < AssetManager.era_library.list.Count; ++index)
    {
      WorldAgeAsset worldAgeAsset = AssetManager.era_library.list[index];
      WorldAgeButton worldAgeButton = this.initButton(worldAgeAsset);
      this._buttons.Add(worldAgeAsset, worldAgeButton);
    }
  }

  private WorldAgeButton initButton(WorldAgeAsset pAsset)
  {
    WorldAgeButton worldAgeButton = Object.Instantiate<WorldAgeButton>(this._age_button_prefab, this._grid_age_buttons);
    worldAgeButton.setAge(pAsset);
    worldAgeButton.addClickCallback(new WorldAgeElementAction(this.ageButtonAction));
    return worldAgeButton;
  }

  private void wheelPieceAction(BaseWorldAgeElement pPiece)
  {
    if (Object.op_Equality((Object) this._selected_piece, (Object) pPiece))
      return;
    this.selectPiece(pPiece as WorldAgeWheelPiece);
    this.updateElements();
  }

  private void ageButtonAction(BaseWorldAgeElement pElement)
  {
    if (!InputHelpers.mouseSupported)
    {
      if (!Tooltip.isShowingFor((object) ((Component) pElement).gameObject))
        return;
      Tooltip.hideTooltip();
    }
    WorldAgeAsset asset = pElement.getAsset();
    this._selected_piece.setAge(asset);
    this._era_manager.setAgeToSlot(asset, this._selected_piece.getIndex());
    this.updateElements();
  }

  public void nextAgeAction()
  {
    this._era_manager.startNextAge(0.5f);
    this.updateElements();
  }

  public void pauseAgesAction()
  {
    this._era_manager.togglePlay(this._era_manager.isPaused());
    this.updateElements();
  }

  public void randomizeAgesAction()
  {
    foreach (WorldAgeWheelPiece piece in (IEnumerable<WorldAgeWheelPiece>) this._age_wheel.getPieces())
    {
      WorldAgeAsset random = AssetManager.era_library.list.GetRandom<WorldAgeAsset>();
      piece.setAge(random);
      this._era_manager.setAgeToSlot(random, piece.getIndex());
    }
    this._era_manager.setCurrentSlotIndex(0, 0.01f);
    this.selectPiece(0);
    this.updateElements();
  }

  public void toggleAgeSpeedAction()
  {
    float agesSpeedMultiplier = this._map_stats.world_ages_speed_multiplier;
    float pMultiplier;
    if ((double) agesSpeedMultiplier <= 1.0)
    {
      if ((double) agesSpeedMultiplier != 0.5)
      {
        if ((double) agesSpeedMultiplier == 1.0)
        {
          pMultiplier = 2f;
          goto label_12;
        }
      }
      else
      {
        pMultiplier = 1f;
        goto label_12;
      }
    }
    else if ((double) agesSpeedMultiplier != 2.0)
    {
      if ((double) agesSpeedMultiplier != 5.0)
      {
        if ((double) agesSpeedMultiplier == 10.0)
        {
          pMultiplier = 20f;
          goto label_12;
        }
      }
      else
      {
        pMultiplier = 10f;
        goto label_12;
      }
    }
    else
    {
      pMultiplier = 5f;
      goto label_12;
    }
    pMultiplier = 1f;
label_12:
    this._era_manager.setAgesSpeedMultiplier(pMultiplier);
    this.updateElements();
  }

  private void updateElements()
  {
    WorldAgeAsset currentAge = this._era_manager.getCurrentAge();
    this._age_name.text = LocalizedTextManager.getText(currentAge.getLocaleID());
    ((Graphic) this._age_name).color = currentAge.title_color;
    this.updatePiePieces();
    this.updateAgeButtonSelectors();
    this._age_wheel.updateElements();
    this._pause_button_icon.sprite = this._era_manager.isPaused() ? this._play_sprite : this._pause_sprite;
    Image ageSpeedButtonIcon = this._age_speed_button_icon;
    float agesSpeedMultiplier = this._map_stats.world_ages_speed_multiplier;
    Sprite sprite1;
    if ((double) agesSpeedMultiplier <= 2.0)
    {
      if ((double) agesSpeedMultiplier != 0.5)
      {
        if ((double) agesSpeedMultiplier != 1.0)
        {
          if ((double) agesSpeedMultiplier == 2.0)
          {
            sprite1 = this._age_speed_sprite_fast;
            goto label_14;
          }
        }
        else
        {
          sprite1 = this._age_speed_sprite_normal;
          goto label_14;
        }
      }
      else
      {
        sprite1 = this._age_speed_sprite_slow;
        goto label_14;
      }
    }
    else if ((double) agesSpeedMultiplier != 5.0)
    {
      if ((double) agesSpeedMultiplier != 10.0)
      {
        if ((double) agesSpeedMultiplier == 20.0)
        {
          sprite1 = this._age_speed_sprite_fast_sonic;
          goto label_14;
        }
      }
      else
      {
        sprite1 = this._age_speed_sprite_fast_ultra;
        goto label_14;
      }
    }
    else
    {
      sprite1 = this._age_speed_sprite_fast_very;
      goto label_14;
    }
    sprite1 = this._age_speed_sprite_normal;
label_14:
    Sprite sprite2 = sprite1;
    ageSpeedButtonIcon.sprite = sprite2;
    this._selected_age_background.sprite = World.world_era.getBackground();
    float num = 0.8f;
    if (this._era_manager.isPaused())
      num = 0.4f;
    Color color1;
    // ISSUE: explicit constructor call
    ((Color) ref color1).\u002Ector(num, num, num);
    ((Graphic) this._selected_age_background).color = color1;
    Color color2 = ((Graphic) this._background_filter).color;
    color2.r = World.world_era.title_color.r;
    color2.g = World.world_era.title_color.g;
    color2.b = World.world_era.title_color.b;
    ((Graphic) this._background_filter).color = color2;
    this.updateTextTimeInfo();
  }

  private void updatePiePieces()
  {
    foreach (WorldAgeWheelPiece piece in (IEnumerable<WorldAgeWheelPiece>) this._age_wheel.getPieces())
    {
      bool flag = this.isPieceSelected(piece);
      piece.setAge(this._era_manager.getAgeFromSlot(piece.getIndex()));
      piece.toggleHighlight(piece.isCurrentAge());
      piece.toggleIconFrame(!flag);
      piece.setIconActiveColor(piece.isCurrentAge());
    }
  }

  private void updateAgeButtonSelectors()
  {
    WorldAgeAsset asset = this._age_wheel.getPiece(this._era_manager.getCurrentSlotIndex()).getAsset();
    foreach (WorldAgeButton worldAgeButton in this._buttons.Values)
    {
      bool pState = worldAgeButton.getAsset() == asset;
      worldAgeButton.toggleSelectedButton(pState);
      worldAgeButton.setIconActiveColor(pState);
    }
  }

  private void updateTextTimeInfo()
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.Append(Date.getUIStringYearMonth());
      stringBuilderPool.AppendLine();
      stringBuilderPool.Append("a: ");
      stringBuilderPool.Append(this._map_stats.current_age_progress.ToString("P0"));
      stringBuilderPool.AppendLine();
      stringBuilderPool.Append("w: ");
      stringBuilderPool.Append($"{this._map_stats.world_age_slot_index + 1}/{8}");
      this._text_time_info.text = stringBuilderPool.ToString();
    }
  }

  private bool isPieceSelected(WorldAgeWheelPiece pPiece)
  {
    return Object.op_Equality((Object) pPiece, (Object) this._selected_piece);
  }
}
