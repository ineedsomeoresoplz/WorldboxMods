// Decompiled with JetBrains decompiler
// Type: CubeOverview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class CubeOverview : 
  MonoBehaviour,
  IInitializePotentialDragHandler,
  IEventSystemHandler,
  IBeginDragHandler,
  IDragHandler,
  IEndDragHandler
{
  [SerializeField]
  private DragSnapElement _knob_perspective_strength_x;
  [SerializeField]
  private DragSnapElement _knob_perspective_strength_y;
  [SerializeField]
  private DragSnapElement _knob_perspective_strength_z;
  [SerializeField]
  private DragSnapElement _knob_perspective_strength_main;
  [SerializeField]
  private DragSnapElement _knob_warp;
  [SerializeField]
  private DragSnapElement _knob_lense;
  [SerializeField]
  private DragSnapElement _knob_spacing;
  [SerializeField]
  private DragSnapElement _knob_speed_outer;
  [SerializeField]
  private DragSnapElement _knob_speed_inner;
  [SerializeField]
  private DragSnapElement _knob_speed_4d;
  [SerializeField]
  private DragSnapElement _knob_icon_size;
  [SerializeField]
  private DragSnapElement _knob_connection_size;
  [SerializeField]
  private DragSnapElement _knob_reset;
  private CubeNode _active_node;
  [SerializeField]
  private CubeNode _prefab_node;
  [SerializeField]
  private CubeNodeConnection _prefab_connection;
  [SerializeField]
  private RectTransform _parent_connections;
  [SerializeField]
  private RectTransform _parent_nodes;
  [SerializeField]
  private GameObject _object_main;
  private float _offset_target_x = -0.015f;
  private float _offset_target_y = 0.07f;
  private bool _is_dragging;
  private Vector2 _last_mouse_delta;
  private float _offset_x;
  private float _offset_y;
  internal bool highlighted;
  private List<CubeNode> _nodes_by_index = new List<CubeNode>();
  private List<CubeNode> _nodes = new List<CubeNode>();
  private ObjectPoolGenericMono<CubeNode> _pool_nodes;
  private ObjectPoolGenericMono<CubeNodeConnection> _pool_connections;
  private Quaternion _rotation_q = Quaternion.identity;
  private Quaternion _rotation_q_2 = Quaternion.identity;
  private List<CubeNodeAssetData> _all_available_assets = new List<CubeNodeAssetData>();
  private CubeNode _latest_touched_node;
  private KnowledgeAsset _filter_asset;
  private float _angle_4d;
  private const float DRAGGING_SMOOTHING_TIME = 0.1f;
  private const float ROTATION_BOUNDS = 0.7f;
  private const float ROTATION_BOUNDS_MARGIN = 1.05f;
  private const float DRAG_SPEED = 0.46f;
  private const float DRAG_ROTATE_SPEED = 0.005f;
  private const float MIN_NODE_CURSOR_DISTANCE = 40f;
  public float RADIUS_NODE_PLACEMENT = 30f;
  private const float NODE_SCALE_MIN = 0.4f;
  private const float NODE_SCALE_MAX = 1.2f;
  private Color _color_node_back = Toolbox.makeColor("#1D7A74");
  private Color _color_node_front = Toolbox.makeColor("#DDDDDD");
  private Color _node_highlighted = Toolbox.makeColor("#FFFFFF");
  private Color _color_connection_back = Toolbox.makeColor("#1D7A74", 0.5f);
  private Color _color_connection_default = Toolbox.makeColor("#3AFFF5", 1f);
  private const float PERSPECTIVE_STRENGTH_MAIN = 3f;
  private const float PERSPECTIVE_STRENGTH_MAIN_MOD = 1f;
  private const float PERSPECTIVE_STRENGTH_AXIS = 1f;
  private const float SPACING_MOD = 1f;
  private const float SPEED_MOD_OUTER = 0.2f;
  private const float SPEED_MOD_INNER = 0.2f;
  private const float SPEED_MOD_4D = 0.3f;
  private const float MOD_NODE_SIZE = 1f;
  private const float MOD_CONNECTION_SIZE = 1f;
  private const float WARP_MOD = 0.0f;
  private const float LENSE_MOD = 0.0f;
  private const float FOLD_MOD = 0.0f;
  private float _perspective_strength_main_mod = 1f;
  private float _perspective_strength_main = 3f;
  private float _perspective_strength_x = 1f;
  private float _perspective_strength_y = 1f;
  private float _perspective_strength_z = 1f;
  private float _mod_lense;
  private float _mod_warp;
  private float _spacing_mod = 1f;
  private float _speed_mod_inner = 0.2f;
  private float _speed_mod_outer = 0.2f;
  private float _speed_mod_4d = 0.3f;
  private float _mod_node_size = 1f;
  private float _mod_connection_size = 1f;
  public float spacing = 25f;
  private static readonly Vector4[] _hypercube_positions = new Vector4[16 /*0x10*/]
  {
    new Vector4(-1f, -1f, -1f, -1f),
    new Vector4(1f, -1f, -1f, -1f),
    new Vector4(-1f, 1f, -1f, -1f),
    new Vector4(1f, 1f, -1f, -1f),
    new Vector4(-1f, -1f, 1f, -1f),
    new Vector4(1f, -1f, 1f, -1f),
    new Vector4(-1f, 1f, 1f, -1f),
    new Vector4(1f, 1f, 1f, -1f),
    new Vector4(-1f, -1f, -1f, 1f),
    new Vector4(1f, -1f, -1f, 1f),
    new Vector4(-1f, 1f, -1f, 1f),
    new Vector4(1f, 1f, -1f, 1f),
    new Vector4(-1f, -1f, 1f, 1f),
    new Vector4(1f, -1f, 1f, 1f),
    new Vector4(-1f, 1f, 1f, 1f),
    new Vector4(1f, 1f, 1f, 1f)
  };
  private static readonly int[,] _hypercube_connections = new int[32 /*0x20*/, 2]
  {
    {
      0,
      1
    },
    {
      0,
      2
    },
    {
      0,
      4
    },
    {
      0,
      8
    },
    {
      1,
      3
    },
    {
      1,
      5
    },
    {
      1,
      9
    },
    {
      2,
      3
    },
    {
      2,
      6
    },
    {
      2,
      10
    },
    {
      3,
      7
    },
    {
      3,
      11
    },
    {
      4,
      5
    },
    {
      4,
      6
    },
    {
      4,
      12
    },
    {
      5,
      7
    },
    {
      5,
      13
    },
    {
      6,
      7
    },
    {
      6,
      14
    },
    {
      7,
      15
    },
    {
      8,
      9
    },
    {
      8,
      10
    },
    {
      8,
      12
    },
    {
      9,
      11
    },
    {
      9,
      13
    },
    {
      10,
      11
    },
    {
      10,
      14
    },
    {
      11,
      15
    },
    {
      12,
      13
    },
    {
      12,
      14
    },
    {
      13,
      15
    },
    {
      14,
      15
    }
  };

  protected void Awake()
  {
    this._pool_nodes = new ObjectPoolGenericMono<CubeNode>(this._prefab_node, (Transform) this._parent_nodes);
    this._pool_connections = new ObjectPoolGenericMono<CubeNodeConnection>(this._prefab_connection, (Transform) this._parent_connections);
  }

  private void initStartPositions()
  {
    for (int index = 0; index < CubeOverview._hypercube_positions.Length; ++index)
    {
      CubeNodeAssetData random = this._all_available_assets.GetRandom<CubeNodeAssetData>();
      CubeNode next = this._pool_nodes.getNext();
      next.setupAsset(random);
      next.logical_pos = CubeOverview._hypercube_positions[index];
      next.setDebugText(index.ToString() ?? "");
      ((Object) ((Component) next).gameObject).name = index.ToString();
      this._nodes.Add(next);
      this._nodes_by_index.Add(next);
    }
    this.updateNodesVisual();
  }

  private void prepareConnections()
  {
    for (int index = 0; index < CubeOverview._hypercube_connections.GetLength(0); ++index)
      this.makeConnection(this._nodes_by_index[CubeOverview._hypercube_connections[index, 0]], this._nodes_by_index[CubeOverview._hypercube_connections[index, 1]]);
  }

  private Vector3 project4Dto3D(Vector4 p)
  {
    float num1 = this._perspective_strength_main * this._perspective_strength_main_mod;
    float num2 = Mathf.Exp(-Mathf.Abs(p.w) * this._mod_lense);
    float num3 = p.w;
    float num4 = Mathf.Sin(num3 * this._mod_warp);
    if ((double) this._mod_warp > 0.0)
      num3 = num4;
    float num5 = num1 - num3;
    if ((double) Mathf.Abs(num5) < 0.0099999997764825821)
      num5 = 0.01f * Mathf.Sign(num5);
    float num6 = ((double) num5 == 0.0 ? 0.0f : num1 / num5) * num2;
    return new Vector3(p.x * num6 * this._perspective_strength_x, p.y * num6 * this._perspective_strength_y, p.z * num6 * this._perspective_strength_z);
  }

  private void updateRotationAndSpeeds()
  {
    if (!this._is_dragging)
      this._angle_4d += Time.deltaTime * this._speed_mod_4d;
    this._perspective_strength_main = !Input.GetMouseButton(0) ? Mathf.Lerp(this._perspective_strength_main, 3f, 0.1f) : Mathf.Lerp(this._perspective_strength_main, 4f, 0.1f);
    float num1 = -this._offset_x;
    float num2 = -this._offset_y;
    float offsetY1 = this._offset_y;
    float offsetY2 = this._offset_y;
    if (!this._is_dragging)
    {
      num1 += this._speed_mod_inner;
      num2 += this._speed_mod_inner;
      offsetY1 += this._speed_mod_outer;
      offsetY2 += this._speed_mod_outer;
    }
    this._rotation_q = Quaternion.op_Multiply(Quaternion.Euler(num1, num2, 0.0f), this._rotation_q);
    this._rotation_q_2 = Quaternion.op_Multiply(Quaternion.Euler(offsetY1, offsetY2, 0.0f), this._rotation_q_2);
  }

  private void updateNodesVisual()
  {
    float angle4d = this._angle_4d;
    foreach (CubeNode node in this._nodes)
    {
      int num1 = (double) node.logical_pos.w < 0.0 ? 1 : 0;
      float num2 = this.spacing * this._spacing_mod;
      Vector3 vector3_1 = Vector3.op_Multiply(this.project4Dto3D(this.rotate4D(node.logical_pos, angle4d)), num2);
      Vector3 vector3_2 = Quaternion.op_Multiply(num1 != 0 ? this._rotation_q : this._rotation_q_2, vector3_1);
      ((Component) node).transform.localPosition = vector3_2;
      this.calculateNodeDepth(node, this.RADIUS_NODE_PLACEMENT);
      this.updateNodeColorAndScale(node);
    }
    this.sortNodesByDepth();
  }

  private Vector4 rotate4D(Vector4 pPoint, float pAngle)
  {
    float num1 = Mathf.Cos(pAngle);
    float num2 = Mathf.Sin(pAngle);
    double num3 = (double) pPoint.x * (double) num1 - (double) pPoint.w * (double) num2;
    float num4 = (float) ((double) pPoint.x * (double) num2 + (double) pPoint.w * (double) num1);
    float num5 = (float) ((double) pPoint.y * (double) num1 - (double) pPoint.z * (double) num2);
    float num6 = (float) ((double) pPoint.y * (double) num2 + (double) pPoint.z * (double) num1);
    double num7 = (double) num5;
    double num8 = (double) num6;
    double num9 = (double) num4;
    return new Vector4((float) num3, (float) num7, (float) num8, (float) num9);
  }

  protected void OnEnable()
  {
    ShortcutExtensions.DOKill((Component) this._object_main.transform, false);
    this._object_main.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this._object_main.transform, 1f, 0.6f), (Ease) 27);
    this.fillAssets();
    this.clearContent();
    this.initStartPositions();
    this.prepareConnections();
    this._is_dragging = false;
  }

  private CubeNodeConnection makeConnection(CubeNode pNode1, CubeNode pNode2)
  {
    CubeNodeConnection next = this._pool_connections.getNext();
    next.node_1 = pNode1;
    next.node_2 = pNode2;
    pNode1.addConnection(pNode2, next);
    pNode2.addConnection(pNode1, next);
    if ((double) pNode1.logical_pos.w < 0.0 && (double) pNode2.logical_pos.w < 0.0)
      next.setConnection(true);
    else
      next.setConnection(false);
    ((Object) ((Component) next).gameObject).name = $"connection {((Object) ((Component) pNode1).gameObject).name}-{((Object) ((Component) pNode2).gameObject).name}";
    return next;
  }

  private void fillAssets()
  {
    this._all_available_assets.Clear();
    if (this._filter_asset != null)
    {
      this.loadUnlockables(this._filter_asset.get_library(), this._filter_asset.id);
      this._filter_asset = (KnowledgeAsset) null;
    }
    else
    {
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.actor_library, "units");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.items, "items");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.gene_library, "genes");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.traits, "traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.subspecies_traits, "subspecies_traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.culture_traits, "culture_traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.language_traits, "language_traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.clan_traits, "clan_traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.religion_traits, "religion_traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.kingdoms_traits, "kingdom_traits");
      this.loadUnlockables((ILibraryWithUnlockables) AssetManager.plots_library, "plots");
    }
  }

  private void loadUnlockables(ILibraryWithUnlockables pLibrary, string pType)
  {
    foreach (BaseUnlockableAsset elements in pLibrary.elements_list)
    {
      if (elements.show_in_knowledge_window && !elements.isTemplateAsset())
        this._all_available_assets.Add(new CubeNodeAssetData(elements, pType));
    }
  }

  private void Update()
  {
    if ((InputHelpers.mouseSupported || Object.op_Equality((Object) this._latest_touched_node, (Object) null) ? 1 : (!Tooltip.isShowingFor((object) ((Component) this._latest_touched_node).transform) ? 1 : 0)) != 0)
      this.updateRotationAndSpeeds();
    foreach (CubeNode node in this._nodes)
      node.update();
    if (!this._is_dragging)
    {
      this.smoothOffsets();
      this._active_node = this.getHighlightedNode();
      this.highlightNode(this._active_node);
    }
    this.updateNodesVisual();
    this.updateConnectionPositions();
    this.updateKnobs();
  }

  private void updateKnobs()
  {
    float num1 = 0.05f;
    if (Object.op_Inequality((Object) this._knob_perspective_strength_main, (Object) null))
    {
      this._perspective_strength_main_mod += this._knob_perspective_strength_main.getDragMod() * 0.03f * num1;
      this._perspective_strength_main_mod = Mathf.Clamp(this._perspective_strength_main_mod, 0.1f, 1f);
    }
    if (Object.op_Inequality((Object) this._knob_perspective_strength_x, (Object) null))
    {
      this._perspective_strength_x += this._knob_perspective_strength_x.getDragMod() * num1;
      this._perspective_strength_x = Mathf.Clamp(this._perspective_strength_x, 0.0f, 2f);
    }
    if (Object.op_Inequality((Object) this._knob_perspective_strength_y, (Object) null))
    {
      this._perspective_strength_y += this._knob_perspective_strength_y.getDragMod() * num1;
      this._perspective_strength_y = Mathf.Clamp(this._perspective_strength_y, 0.0f, 2f);
    }
    if (Object.op_Inequality((Object) this._knob_perspective_strength_z, (Object) null))
    {
      this._perspective_strength_z += this._knob_perspective_strength_z.getDragMod() * num1;
      this._perspective_strength_z = Mathf.Clamp(this._perspective_strength_z, 0.0f, 2f);
    }
    if (Object.op_Inequality((Object) this._knob_spacing, (Object) null))
    {
      this._spacing_mod += this._knob_spacing.getDragMod() * num1;
      this._spacing_mod = Mathf.Clamp(this._spacing_mod, 0.0f, 3f);
    }
    if (Object.op_Inequality((Object) this._knob_warp, (Object) null))
    {
      this._mod_warp += this._knob_warp.getDragMod() * num1;
      this._mod_warp = Mathf.Clamp(this._mod_warp, 0.0f, 10f);
    }
    if (Object.op_Inequality((Object) this._knob_lense, (Object) null))
    {
      this._mod_lense += this._knob_lense.getDragMod() * num1;
      this._mod_lense = Mathf.Clamp(this._mod_lense, 0.0f, 2f);
    }
    if (Object.op_Inequality((Object) this._knob_speed_outer, (Object) null))
    {
      this._speed_mod_outer += this._knob_speed_outer.getDragMod() * num1;
      this._speed_mod_outer = Mathf.Clamp(this._speed_mod_outer, 0.0f, 20f);
    }
    if (Object.op_Inequality((Object) this._knob_speed_inner, (Object) null))
    {
      this._speed_mod_inner += this._knob_speed_inner.getDragMod() * num1;
      this._speed_mod_inner = Mathf.Clamp(this._speed_mod_inner, 0.0f, 20f);
    }
    if (Object.op_Inequality((Object) this._knob_connection_size, (Object) null))
    {
      this._mod_connection_size += this._knob_connection_size.getDragMod() * num1;
      this._mod_connection_size = Mathf.Clamp(this._mod_connection_size, 0.0f, 10f);
    }
    if (Object.op_Inequality((Object) this._knob_icon_size, (Object) null))
    {
      this._mod_node_size += this._knob_icon_size.getDragMod() * num1;
      this._mod_node_size = Mathf.Clamp(this._mod_node_size, 0.0f, 20f);
    }
    if (Object.op_Inequality((Object) this._knob_speed_4d, (Object) null))
    {
      this._speed_mod_4d += this._knob_speed_4d.getDragMod() * num1;
      this._speed_mod_4d = Mathf.Clamp(this._speed_mod_4d, 0.0f, 20f);
    }
    if (!Object.op_Inequality((Object) this._knob_reset, (Object) null))
      return;
    float num2 = Math.Abs(this._knob_reset.getDragMod());
    this._perspective_strength_main = Mathf.Lerp(this._perspective_strength_main, 3f, num2 * num1);
    this._perspective_strength_x = Mathf.Lerp(this._perspective_strength_x, 1f, num2 * num1);
    this._perspective_strength_y = Mathf.Lerp(this._perspective_strength_y, 1f, num2 * num1);
    this._perspective_strength_z = Mathf.Lerp(this._perspective_strength_z, 1f, num2 * num1);
    this._spacing_mod = Mathf.Lerp(this._spacing_mod, 1f, num2 * num1);
    this._speed_mod_outer = Mathf.Lerp(this._speed_mod_outer, 0.2f, num2 * num1);
    this._speed_mod_inner = Mathf.Lerp(this._speed_mod_inner, 0.2f, num2 * num1);
    this._speed_mod_4d = Mathf.Lerp(this._speed_mod_4d, 0.3f, num2 * num1);
    this._mod_connection_size = Mathf.Lerp(this._mod_connection_size, 1f, num2 * num1);
    this._mod_node_size = Mathf.Lerp(this._mod_node_size, 1f, num2 * num1);
    this._perspective_strength_main_mod = Mathf.Lerp(this._perspective_strength_main_mod, 1f, num2 * num1);
    this._mod_warp = Mathf.Lerp(this._mod_warp, 0.0f, num2 * num1);
    this._mod_lense = Mathf.Lerp(this._mod_lense, 0.0f, num2 * num1);
  }

  private void updateConnectionPositions()
  {
    foreach (CubeNodeConnection cubeNodeConnection in (IEnumerable<CubeNodeConnection>) this._pool_connections.getListTotal())
    {
      cubeNodeConnection.update();
      float num1 = 1f;
      CubeNode node1 = cubeNodeConnection.node_1;
      CubeNode node2 = cubeNodeConnection.node_2;
      if (cubeNodeConnection.inner_cube)
        num1 = 3f;
      if (node1.highlighted || node2.highlighted)
        num1 = 6f;
      float num2 = num1 * this._mod_connection_size;
      ((Graphic) cubeNodeConnection.image).color = Color.Lerp(this._color_connection_back, this._color_connection_default, (double) node1.render_depth <= (double) node2.render_depth ? node2.render_depth : node1.render_depth);
      Vector2 vector2_1 = Vector2.op_Implicit(((Component) node1).transform.localPosition);
      Vector2 vector2_2 = Vector2.op_Implicit(((Component) node2).transform.localPosition);
      ((Component) cubeNodeConnection).transform.localPosition = Vector2.op_Implicit(Vector2.op_Division(Vector2.op_Addition(vector2_1, vector2_2), 2f));
      ((Component) cubeNodeConnection).transform.localScale = new Vector3(Vector3.Distance(Vector2.op_Implicit(vector2_1), Vector2.op_Implicit(vector2_2)), num2, 1f);
      Vector3 vector3 = Vector2.op_Implicit(Vector2.op_Subtraction(vector2_2, vector2_1));
      ((Component) cubeNodeConnection).transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(vector3.y, vector3.x) * 57.29578f);
    }
  }

  public CubeNodeAssetData getRandom() => this._all_available_assets.GetRandom<CubeNodeAssetData>();

  public void setLatestTouched(CubeNode pNode) => this._latest_touched_node = pNode;

  public void setFilterAsset(KnowledgeAsset pAsset) => this._filter_asset = pAsset;

  private void highlightAllConnectonsFromDrag(float pLight)
  {
    foreach (CubeNodeConnection cubeNodeConnection in (IEnumerable<CubeNodeConnection>) this._pool_connections.getListTotal())
    {
      if ((double) cubeNodeConnection.mod_light <= (double) pLight)
        cubeNodeConnection.mod_light = pLight;
    }
  }

  private void highlightNode(CubeNode pHighlighted = null)
  {
    foreach (CubeNode node in this._nodes)
    {
      if (!Object.op_Equality((Object) node, (Object) pHighlighted) && node.highlighted)
      {
        node.highlighted = false;
        Tooltip.hideTooltipNow();
      }
    }
    pHighlighted?.setHighlighted();
  }

  private CubeNode getClosestNodeToCursor()
  {
    CubeNode closestNodeToCursor = (CubeNode) null;
    float num1 = float.MaxValue;
    Vector2 vector2_1 = Vector2.op_Implicit(Input.mousePosition);
    if (!InputHelpers.mouseSupported && InputHelpers.touchCount == 0)
      return this._active_node;
    foreach (CubeNode node in this._nodes)
    {
      Vector2 vector2_2 = Vector2.op_Implicit(((Component) node).transform.position);
      float num2 = Vector2.Distance(vector2_1, vector2_2);
      if ((double) num2 <= 40.0)
      {
        if (Object.op_Equality((Object) node, (Object) this._active_node))
          return node;
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          closestNodeToCursor = node;
        }
      }
    }
    return closestNodeToCursor;
  }

  private void smoothOffsets()
  {
    this._offset_x = Mathf.Lerp(this._offset_x, this._offset_target_x, 0.1f);
    this._offset_y = Mathf.Lerp(this._offset_y, this._offset_target_y, 0.1f);
  }

  internal bool isDragging() => this._is_dragging;

  private void calculateNodeDepth(CubeNode pElement, float pRadius)
  {
    float z = ((Component) pElement).transform.localPosition.z;
    float num = Mathf.InverseLerp(-pRadius, pRadius, z);
    pElement.render_depth = num;
  }

  private void sortNodesByDepth()
  {
    foreach (Component node in this._nodes)
      node.transform.SetAsLastSibling();
    this._nodes.Sort((Comparison<CubeNode>) ((a, b) => a.render_depth.CompareTo(b.render_depth)));
  }

  private void clearContent()
  {
    foreach (CubeNode node in this._nodes)
      node.clear();
    foreach (CubeNodeConnection cubeNodeConnection in (IEnumerable<CubeNodeConnection>) this._pool_connections.getListTotal())
      cubeNodeConnection.clear();
    this._rotation_q = Quaternion.identity;
    this._rotation_q_2 = Quaternion.identity;
    this._pool_connections.clear();
    this._pool_nodes.clear();
    this._nodes.Clear();
    this._nodes_by_index.Clear();
  }

  public void OnDrag(PointerEventData eventData)
  {
    this._is_dragging = true;
    Vector2 delta = eventData.delta;
    if ((double) ((Vector2) ref delta).magnitude > (double) ((Vector2) ref this._last_mouse_delta).magnitude)
      this.highlightAllConnectonsFromDrag(0.35f);
    this._last_mouse_delta = delta;
    this._offset_x = (float) (-(double) delta.y * 0.46000000834465027);
    this._offset_y = delta.x * 0.46f;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    this._is_dragging = false;
    Vector2 delta = eventData.delta;
    this._offset_target_x += (float) (-(double) delta.y * 0.004999999888241291);
    this._offset_target_y += delta.x * 0.005f;
    if ((double) Mathf.Abs(this._offset_target_x) > 0.699999988079071 || (double) Mathf.Abs(this._offset_target_y) > 0.699999988079071)
    {
      if ((double) Mathf.Abs(this._offset_target_x) > (double) Mathf.Abs(this._offset_target_y))
        this._offset_target_y = (float) ((double) this._offset_target_y / (double) Mathf.Abs(this._offset_target_x) * 0.699999988079071);
      else
        this._offset_target_x = (float) ((double) this._offset_target_x / (double) Mathf.Abs(this._offset_target_y) * 0.699999988079071);
    }
    this._offset_target_x = Mathf.Clamp(this._offset_target_x, -0.7f, 0.7f);
    this._offset_target_y = Mathf.Clamp(this._offset_target_y, -0.7f, 0.7f);
    this.highlightAllConnectonsFromDrag(1f);
  }

  public void OnInitializePotentialDrag(PointerEventData eventData)
  {
    eventData.useDragThreshold = false;
    this._last_mouse_delta = Vector2.zero;
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    this._offset_x = this._offset_target_x = 0.0f;
    this._offset_y = this._offset_target_y = 0.0f;
    Tooltip.hideTooltipNow();
  }

  private void updateNodeColorAndScale(CubeNode pNode)
  {
    Color pColor = !pNode.current_asset.isUnlockedByPlayer() ? Toolbox.color_black : (!pNode.highlighted ? Color.Lerp(this._color_node_back, this._color_node_front, pNode.render_depth) : Color.Lerp(this._color_node_back, this._node_highlighted, pNode.render_depth));
    pNode.setColor(pColor);
    float num1 = Mathf.Lerp(0.4f, 1.2f, pNode.render_depth);
    if (Mathf.Approximately(num1, 0.4f))
      pNode.setupAsset(this.getRandom());
    float num2 = num1 * (pNode.scale_mod_spawn * pNode.bonus_scale) * this._mod_node_size;
    ((Component) pNode).transform.localScale = new Vector3(num2, num2, num2);
    pNode.updateTooltip();
  }

  private CubeNode getHighlightedNode()
  {
    if (this._is_dragging)
      return (CubeNode) null;
    if ((double) this._offset_x > 1.0499999523162842 || (double) this._offset_x < -1.0499999523162842)
      return (CubeNode) null;
    return (double) this._offset_y > 1.0499999523162842 || (double) this._offset_y < -1.0499999523162842 ? (CubeNode) null : this.getClosestNodeToCursor();
  }
}
