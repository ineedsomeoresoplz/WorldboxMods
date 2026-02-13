// Decompiled with JetBrains decompiler
// Type: SelectedMultipleUnitsTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class SelectedMultipleUnitsTab : SelectedNano<Actor>, ISelectedMetaWithUnit
{
  private const int MAX_UNITS_PER_FOLD = 21;
  private const int MAX_UNITS_TOTAL = 100;
  [SerializeField]
  private SelectedMetaUnitElement _unit_element;
  [SerializeField]
  private GameObject _unit_element_separator;
  [SerializeField]
  private ActorSelectedContainerStatus _container_status;
  [SerializeField]
  private ActorSelectedContainerEquipment _container_equipment;
  [SerializeField]
  private RectTransform _avatars_container;
  [SerializeField]
  private UiUnitAvatarElement _avatar_prefab;
  [SerializeField]
  private UnfoldButton _unfolder;
  [SerializeField]
  private Image _unfolder_background;
  [SerializeField]
  private Sprite _unfolder_active;
  [SerializeField]
  private Sprite _unfolder_inactive;
  private ObjectPoolGenericMono<UiUnitAvatarElement> _pool_avatars;
  private int _last_selection_version;
  private List<UiUnitAvatarElement> _showing_avatars = new List<UiUnitAvatarElement>();
  private List<int> _stats_version = new List<int>();
  private int _offset;

  protected override Actor nano_object => SelectedUnit.unit;

  public SelectedMetaUnitElement unit_element => this._unit_element;

  public GameObject unit_element_separator => this._unit_element_separator;

  private ISelectedMetaWithUnit as_meta_with_unit => (ISelectedMetaWithUnit) this;

  public int last_dirty_stats_unit { get; set; }

  public Actor last_unit { get; set; }

  public string unit_title_locale_key => (string) null;

  public bool hasUnit() => SelectedUnit.isSet();

  public Actor getUnit() => SelectedUnit.unit;

  protected override void Awake()
  {
    base.Awake();
    this._pool_avatars = new ObjectPoolGenericMono<UiUnitAvatarElement>(this._avatar_prefab, (Transform) this._avatars_container);
    this._unfolder.setCallback((UnfoldAction) (_ => this.showAvatars(this.getOffset(), this.getNextAmount())));
  }

  private void Start()
  {
    SelectedUnit.subscribeClearEvent(new SelectedUnitClearEvent(((SelectedNano<Actor>) this).clearLastObject));
  }

  protected override void updateElementsAlways(Actor pNano)
  {
    base.updateElementsAlways(pNano);
    int num1 = this.as_meta_with_unit.checkUnitElement() ? 1 : 0;
    if (this.hasUnit())
      this._unit_element.updateBarAndTask(this.getUnit());
    if (num1 != 0)
    {
      this.updateAvatars();
    }
    else
    {
      List<Actor> allSelectedList = SelectedUnit.getAllSelectedList();
      using (ListPool<int> listPool = new ListPool<int>())
      {
        for (int index = 0; index < this._showing_avatars.Count; ++index)
        {
          UiUnitAvatarElement showingAvatar = this._showing_avatars[index];
          if (index > allSelectedList.Count - 1)
          {
            listPool.Add(index);
          }
          else
          {
            Actor actor = allSelectedList[index];
            int num2 = this._stats_version[index];
            int statsDirtyVersion = actor.getStatsDirtyVersion();
            if (!actor.isRekt())
            {
              if (statsDirtyVersion != num2 || actor != showingAvatar.getActor() || showingAvatar.avatarLoader.actorStateChanged())
              {
                this._stats_version[index] = statsDirtyVersion;
                showingAvatar.load((NanoObject) actor);
              }
              else
                showingAvatar.updateTileSprite();
            }
          }
        }
        listPool.Sort();
        listPool.Reverse();
        for (int index1 = 0; index1 < listPool.Count; ++index1)
        {
          int index2 = listPool[index1];
          UiUnitAvatarElement showingAvatar = this._showing_avatars[index2];
          this._showing_avatars.RemoveAt(index2);
          this._stats_version.RemoveAt(index2);
          this._pool_avatars.release(showingAvatar);
        }
        this.updateUnfolderButton();
        if (listPool.Count <= 0)
          return;
        this.recalcTabSize();
      }
    }
  }

  protected override void updateElementsOnChange(Actor pNano)
  {
    base.updateElementsOnChange(pNano);
    this.updateAvatars();
    this.updateStatuses(pNano);
    this.updateEquipment(pNano);
  }

  private void updateStatuses(Actor pActor) => this._container_status.update((NanoObject) pActor);

  private void updateEquipment(Actor pActor) => this._container_equipment.update(pActor);

  private void updateAvatars()
  {
    int selectionVersion = SelectedUnit.getSelectionVersion();
    if (selectionVersion == this._last_selection_version)
      return;
    this._last_selection_version = selectionVersion;
    if (this._offset != 0)
      return;
    this.clear();
    this.showAvatars(this.getOffset(), this.getNextAmount());
  }

  private void showAvatars(int pOffset, int pAmount)
  {
    using (ListPool<Actor> listPool = new ListPool<Actor>((ICollection<Actor>) SelectedUnit.getAllSelected()))
    {
      listPool.Remove(SelectedUnit.unit);
      for (int index = pOffset; index < pOffset + pAmount; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SelectedMultipleUnitsTab.\u003C\u003Ec__DisplayClass44_0 cDisplayClass440 = new SelectedMultipleUnitsTab.\u003C\u003Ec__DisplayClass44_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass440.\u003C\u003E4__this = this;
        Actor pActor = listPool[index];
        // ISSUE: reference to a compiler-generated field
        cDisplayClass440.tAvatar = this._pool_avatars.getNext();
        Button button1;
        // ISSUE: reference to a compiler-generated field
        if (!((Component) cDisplayClass440.tAvatar).TryGetComponent<Button>(ref button1))
        {
          // ISSUE: reference to a compiler-generated field
          ((Component) cDisplayClass440.tAvatar).AddComponent<Button>();
        }
        // ISSUE: reference to a compiler-generated field
        UnitAvatarLoader avatarLoader = cDisplayClass440.tAvatar.avatarLoader;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass440.tAvatar.load((NanoObject) pActor);
        Button button2;
        if (!((Component) avatarLoader).TryGetComponent<Button>(ref button2))
        {
          button2 = ((Component) avatarLoader).AddComponent<Button>();
          ((UnityEventBase) button2.onClick).RemoveAllListeners();
          // ISSUE: method pointer
          ((UnityEvent) button2.onClick).AddListener(new UnityAction((object) cDisplayClass440, __methodptr(\u003CshowAvatars\u003Eb__0)));
        }
        CanvasGroup component = ((Component) avatarLoader).GetComponent<CanvasGroup>();
        component.interactable = true;
        component.blocksRaycasts = true;
        // ISSUE: reference to a compiler-generated field
        this._showing_avatars.Add(cDisplayClass440.tAvatar);
        this._stats_version.Add(pActor.getStatsDirtyVersion());
      }
      this._offset += pAmount;
      this.updateUnfolderButton();
      this.recalcTabSize();
    }
  }

  private void updateUnfolderButton()
  {
    int num = SelectedUnit.countSelected() - 1 - this._offset;
    if (num > 0)
    {
      ((Component) this._unfolder).transform.SetSiblingIndex(((Transform) this._avatars_container).childCount - 1);
      ((Component) this._unfolder).gameObject.SetActive(true);
      this._unfolder.setText($"+{num}");
      bool flag = this._offset >= 100;
      ((Selectable) this._unfolder.getButton()).interactable = !flag;
      if (flag)
        this._unfolder_background.sprite = this._unfolder_inactive;
      else
        this._unfolder_background.sprite = this._unfolder_active;
    }
    else
      ((Component) this._unfolder).gameObject.SetActive(false);
  }

  protected override void showStatsGeneral(Actor pMeta)
  {
    base.showStatsGeneral(pMeta);
    if (!this.hasUnit())
      return;
    this._unit_element.showStats(this.getUnit());
  }

  public void avatarTouchScream() => this.as_meta_with_unit.avatarTouch();

  protected override void clearLastObject()
  {
    base.clearLastObject();
    this.as_meta_with_unit.clearLastUnit();
    this._offset = 0;
  }

  private int getNextAmount() => Mathf.Min(21, SelectedUnit.countSelected() - 1 - this._offset);

  private int getOffset() => this._offset;

  private void showWorldTip(Actor pActor)
  {
    string text = LocalizedTextManager.getText("now_looking_at");
    string colorText = pActor.getColor().color_text;
    string newValue = pActor.name.ColorHex(colorText);
    string pText = text.Replace("$name$", newValue);
    WorldTip.instance.showToolbarText(pText);
  }

  private void clear()
  {
    this._offset = 0;
    this._pool_avatars.clear();
    this._showing_avatars.Clear();
    this._stats_version.Clear();
  }
}
