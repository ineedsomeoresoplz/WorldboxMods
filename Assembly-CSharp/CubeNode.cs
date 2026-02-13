// Decompiled with JetBrains decompiler
// Type: CubeNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class CubeNode : 
  MonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler,
  IInitializePotentialDragHandler
{
  private const float SCALE_HIGHLIGHTED = 1.6f;
  private const float SCALE_NORMAL = 1f;
  private const float TOOLTIP_SCALE_MIN = 0.4f;
  private const float TOOLTIP_SCALE_MAX = 1f;
  public Vector4 logical_pos;
  internal List<CubeNode> connected_nodes = new List<CubeNode>();
  private List<CubeNodeConnection> connections = new List<CubeNodeConnection>();
  [SerializeField]
  private Image _image;
  [SerializeField]
  private Text _text;
  private CubeOverview _cube_overview;
  internal float render_depth;
  internal float scale_mod_spawn = 1f;
  internal float bonus_scale = 1f;
  internal bool highlighted;
  private float _timer_change;
  private TooltipData _tooltip_data;
  private CubeNodeAssetData _data;

  public BaseUnlockableAsset current_asset => this._data.asset;

  private void Start()
  {
    this._cube_overview = ((Component) this).gameObject.GetComponentInParent<CubeOverview>();
    this.initClick();
    this.initTooltip();
  }

  public void update() => this._timer_change -= Time.deltaTime;

  public void setDebugText(string pText) => this._text.text = pText;

  public void clear()
  {
    this.connected_nodes.Clear();
    this.connections.Clear();
    this._timer_change = 0.0f;
  }

  protected void initClick()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(setPressed)));
  }

  protected void initTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    Object.Destroy((Object) tipButton);
  }

  private void showTooltip()
  {
    this._cube_overview.setLatestTouched(this);
    this._tooltip_data = AssetManager.knowledge_library.get(this._data.knowledge_type).show_tooltip(((Component) this).transform, this._data.asset);
  }

  public void setupAsset(CubeNodeAssetData pData)
  {
    if ((double) this._timer_change > 0.0)
      return;
    this._timer_change = 2f;
    this._data = pData;
    this._image.sprite = this._data.asset.getSprite();
  }

  public void updateTooltip()
  {
    if (!this.highlighted || !Tooltip.isShowingFor((object) ((Component) this).transform))
      return;
    this._tooltip_data.tooltip_scale = Mathf.Lerp(0.4f, 1f, this.render_depth);
  }

  public void setHighlighted()
  {
    if (this.highlighted)
      return;
    this.highlighted = true;
    this.scale_mod_spawn = 1.6f;
    this.showTooltip();
  }

  public void setPressed() => this._cube_overview.isDragging();

  public void setColor(Color pColor) => ((Graphic) this._image).color = pColor;

  public void addConnection(CubeNode pNode, CubeNodeConnection pConnection)
  {
    this.connected_nodes.Add(pNode);
    this.connections.Add(pConnection);
  }

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    ((Component) this._cube_overview)?.SendMessage(nameof (OnInitializePotentialDrag), (object) pEventData);
  }

  public void OnBeginDrag(PointerEventData pEventData)
  {
    ((Component) this._cube_overview)?.SendMessage(nameof (OnBeginDrag), (object) pEventData);
  }

  public void OnDrag(PointerEventData pEventData)
  {
    ((Component) this._cube_overview)?.SendMessage(nameof (OnDrag), (object) pEventData);
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    ((Component) this._cube_overview)?.SendMessage(nameof (OnEndDrag), (object) pEventData);
  }
}
