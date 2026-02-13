// Decompiled with JetBrains decompiler
// Type: UnitGenealogyElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnitGenealogyElement : UnitElement
{
  private const int AVATARS_LIMIT_PER_UNFOLD = 128 /*0x80*/;
  private const int AVATARS_LIMIT_INITIAL = 16 /*0x10*/;
  public const float COUNT_ANIMATION_STEP_TIME = 0.025f;
  private const float COUNT_ANIMATION_LENGTH = 0.5f;
  public const float COUNT_ANIMATION_STEPS = 20f;
  public UiUnitAvatarElement prefab_avatar;
  [SerializeField]
  private UnfoldButton _prefab_unfolder;
  [SerializeField]
  private Image _sex_icon;
  [SerializeField]
  private Sprite _default_genealogy_icon;
  private UnfoldButton _siblings_unfolder;
  private UnfoldButton _children_unfolder;
  private ObjectPoolGenericMono<UiUnitAvatarElement> _pool_grandparents;
  private ObjectPoolGenericMono<UiUnitAvatarElement> _pool_parents;
  private ObjectPoolGenericMono<UiUnitAvatarElement> _pool_siblings;
  private ObjectPoolGenericMono<UiUnitAvatarElement> _pool_children;
  public Transform transform_siblings;
  public Transform transform_children;
  public Transform transform_parents;
  public Transform transform_grandparents;

  protected override void Awake()
  {
    this._pool_siblings = new ObjectPoolGenericMono<UiUnitAvatarElement>(this.prefab_avatar, this.transform_siblings);
    this._pool_children = new ObjectPoolGenericMono<UiUnitAvatarElement>(this.prefab_avatar, this.transform_children);
    this._pool_grandparents = new ObjectPoolGenericMono<UiUnitAvatarElement>(this.prefab_avatar, this.transform_grandparents);
    this._pool_parents = new ObjectPoolGenericMono<UiUnitAvatarElement>(this.prefab_avatar, this.transform_parents);
    this._siblings_unfolder = Object.Instantiate<UnfoldButton>(this._prefab_unfolder, this.transform_siblings);
    this._siblings_unfolder.setCallback((UnfoldAction) (_ => this.StartCoroutine(this.loadSiblings(true))));
    this._children_unfolder = Object.Instantiate<UnfoldButton>(this._prefab_unfolder, this.transform_children);
    this._children_unfolder.setCallback((UnfoldAction) (_ => this.StartCoroutine(this.loadChildren(true))));
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    UnitGenealogyElement genealogyElement = this;
    genealogyElement._sex_icon.sprite = !genealogyElement.actor.asset.inspect_sex ? genealogyElement._default_genealogy_icon : (!genealogyElement.actor.isSexMale() ? SpriteTextureLoader.getSprite("ui/icons/IconFemale") : SpriteTextureLoader.getSprite("ui/icons/IconMale"));
    yield return (object) genealogyElement.loadParents();
    yield return (object) genealogyElement.loadChildren();
    yield return (object) genealogyElement.loadSiblings();
    yield return (object) genealogyElement.loadGrandParents();
  }

  protected override void clear()
  {
    this._pool_siblings.clear();
    this._pool_children.clear();
    this._pool_grandparents.clear();
    this._pool_parents.clear();
    ((Component) this.prefab_avatar).gameObject.SetActive(false);
    ((Component) this._siblings_unfolder).gameObject.SetActive(false);
    this._siblings_unfolder.clear();
    ((Component) this._children_unfolder).gameObject.SetActive(false);
    this._children_unfolder.clear();
    base.clear();
  }

  private IEnumerator loadParents()
  {
    // ISSUE: unable to decompile the method.
  }

  private IEnumerator loadGrandParents()
  {
    // ISSUE: unable to decompile the method.
  }

  private IEnumerator loadChildren(bool pUnfold = false)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitGenealogyElement genealogyElement = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) genealogyElement.unfold(new UnfoldCheck(genealogyElement.childrenCheck), genealogyElement._pool_children, genealogyElement._children_unfolder, genealogyElement.transform_children, pUnfold);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator loadSiblings(bool pUnfold = false)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UnitGenealogyElement genealogyElement = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) genealogyElement.unfold(new UnfoldCheck(genealogyElement.siblingsCheck), genealogyElement._pool_siblings, genealogyElement._siblings_unfolder, genealogyElement.transform_siblings, pUnfold);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator unfold(
    UnfoldCheck pCheckAction,
    ObjectPoolGenericMono<UiUnitAvatarElement> pPool,
    UnfoldButton pFoldButton,
    Transform pParent,
    bool pUnfold = false)
  {
    // ISSUE: unable to decompile the method.
  }

  private IEnumerator counter(int pLeft, UnfoldButton pButton)
  {
    float tPerStep = (float) pLeft / 20f;
    for (float i = 0.0f; (double) i < (double) (pLeft + 1); i += tPerStep)
    {
      pButton.setText("+" + Mathf.Floor(i).ToString());
      yield return (object) new WaitForSecondsRealtime(0.025f);
    }
  }

  private bool childrenCheck(Actor pActor) => pActor.isChildOf(this.actor);

  private bool siblingsCheck(Actor pActor) => this.hasSameParent(pActor, this.actor);

  private bool hasSameParent(Actor pActor, Actor pOther)
  {
    long parentId1_1 = pOther.data.parent_id_1;
    long parentId2_1 = pOther.data.parent_id_2;
    long parentId1_2 = pActor.data.parent_id_1;
    if (parentId1_2.hasValue() && (parentId1_2 == parentId1_1 || parentId1_2 == parentId2_1))
      return true;
    long parentId2_2 = pActor.data.parent_id_2;
    return parentId2_2.hasValue() && (parentId2_2 == parentId1_1 || parentId2_2 == parentId2_1);
  }

  private IEnumerator showAvatar(Actor pActor, ObjectPoolGenericMono<UiUnitAvatarElement> pPool)
  {
    if (!pActor.isRekt())
    {
      yield return (object) new WaitForSecondsRealtime(0.025f);
      if (!pActor.isRekt())
        pPool.getNext().show(pActor);
    }
  }
}
