// Decompiled with JetBrains decompiler
// Type: HoveringBgIconManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HoveringBgIconManager : MonoBehaviour
{
  [SerializeField]
  private HoveringIcon _icon_prefab;
  private ObjectPoolGenericMono<HoveringIcon> _pool_icons;
  private CanvasGroup _canvas_group;
  private RectTransform _rect;
  private List<RectTransform> _places = new List<RectTransform>();
  [SerializeField]
  public bool _random_scale = true;
  [SerializeField]
  private Transform _icon_pool;
  [SerializeField]
  private Transform _icons;
  private static HoveringBgIconManager _instance;

  private void Awake()
  {
    if (this._pool_icons != null)
      return;
    HoveringBgIconManager._instance = this;
    this._rect = ((Component) this).GetComponent<RectTransform>();
    this._canvas_group = ((Component) this).GetComponent<CanvasGroup>();
    this._pool_icons = new ObjectPoolGenericMono<HoveringIcon>(this._icon_prefab, this._icon_pool);
    for (int index = 0; index < this._icons.childCount; ++index)
    {
      RectTransform child = this._icons.GetChild(index) as RectTransform;
      this._places.Add(child);
      ((Object) ((Component) child).gameObject).name = "Placing " + index.ToString();
    }
  }

  private void OnDisable() => this._pool_icons.clear();

  public void fadeIn()
  {
    ((Component) this._icons).gameObject.SetActive(true);
    DOTweenModuleUI.DOFade(this._canvas_group, 1f, 0.2f);
    this._canvas_group.interactable = true;
    this._canvas_group.blocksRaycasts = true;
  }

  public void fadeOut()
  {
    this._canvas_group.interactable = false;
    this._canvas_group.blocksRaycasts = false;
    DOTweenModuleUI.DOFade(this._canvas_group, 0.0f, 0.2f);
    this.clear();
    this.resetPlaces();
    ((Component) this._icons).gameObject.SetActive(false);
  }

  private void resetPlaces()
  {
    if (Randy.randomBool())
      return;
    Rect rect1 = this._rect.rect;
    float num1 = ((Rect) ref rect1).width / 2f;
    Rect rect2 = this._rect.rect;
    float num2 = ((Rect) ref rect2).height / 2f;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(num1, num2, 0.0f);
    for (int index = 0; index < this._places.Count; ++index)
    {
      RectTransform place = this._places[index];
      ShortcutExtensions.DOKill((Component) place, false);
      place.anchoredPosition = Vector2.op_Implicit(vector3);
    }
  }

  private void shufflePlaces()
  {
    this.resetPlaces();
    Rect rect1 = this._rect.rect;
    float width = ((Rect) ref rect1).width;
    Rect rect2 = this._rect.rect;
    float height = ((Rect) ref rect2).height;
    for (int index = 0; index < this._places.Count; ++index)
    {
      RectTransform place = this._places[index];
      float num1 = Randy.randomFloat(0.15f, 0.35f);
      Vector2 vector2 = Vector2.op_Implicit(new Vector3(Randy.randomFloat(0.0f, width), Randy.randomFloat(0.0f, height), 0.0f));
      double num2 = (double) num1;
      DOTweenModuleUI.DOAnchorPos(place, vector2, (float) num2, false);
    }
  }

  public void animate(WindowAsset pWindowAsset)
  {
    this.clear();
    this.shufflePlaces();
    float num1 = Randy.randomFloat(0.0f, 360f);
    string str1 = "ui/Icons/";
    using (ListPool<string> list = new ListPool<string>(16 /*0x10*/))
    {
      foreach (HoveringBGIconsGetter invocation in pWindowAsset.get_hovering_icons.GetInvocationList())
      {
        foreach (string str2 in invocation(pWindowAsset))
        {
          if (str2.EndsWith("/"))
          {
            foreach (Object sprite in SpriteTextureLoader.getSpriteList(str1 + str2))
            {
              string str3 = str1 + str2 + sprite.name;
              list.Add(str3);
            }
          }
          else
          {
            string str4 = str1 + str2;
            list.Add(str4);
          }
        }
      }
      foreach (RectTransform place in this._places)
      {
        string random = list.GetRandom<string>();
        HoveringIcon next = this._pool_icons.getNext();
        next.clear();
        ((Component) next).transform.SetParent((Transform) place, false);
        next.rect.anchoredPosition = Vector2.op_Implicit(Vector3.zero);
        ((Component) next).transform.rotation = Quaternion.identity;
        next.image.sprite = SpriteTextureLoader.getSprite(random);
        if (this._random_scale)
        {
          float num2 = Randy.randomFloat(0.4f, 1f);
          ((Component) next).transform.localScale = new Vector3(num2, num2, num2);
        }
        else
          ((Component) next).transform.localScale = ((Transform) place).localScale;
        Vector3 localScale = ((Component) next).transform.localScale;
        ((Graphic) next.image).color = new Color(localScale.x, localScale.x, localScale.x, 1f);
        num1 += Randy.randomFloat(20f, 130f);
        ((Component) next).transform.eulerAngles = new Vector3(0.0f, 0.0f, num1);
        next.init();
      }
    }
  }

  public static void show() => HoveringBgIconManager._instance.fadeIn();

  public static void hide() => HoveringBgIconManager._instance.fadeOut();

  public static void showWindow(WindowAsset pWindowAsset)
  {
    HoveringBgIconManager._instance.animate(pWindowAsset);
  }

  public static void dropAll()
  {
    foreach (HoveringIcon hoveringIcon in (IEnumerable<HoveringIcon>) HoveringBgIconManager._instance._pool_icons.getListTotal())
    {
      if (((Component) hoveringIcon).gameObject.activeSelf)
      {
        UiCreature component = ((Component) hoveringIcon).GetComponent<UiCreature>();
        if (!component.dropped)
          component.click();
      }
    }
  }

  public static void randomDrop()
  {
    using (ListPool<UiCreature> list = new ListPool<UiCreature>(HoveringBgIconManager._instance._pool_icons.countActive()))
    {
      foreach (HoveringIcon hoveringIcon in (IEnumerable<HoveringIcon>) HoveringBgIconManager._instance._pool_icons.getListTotal())
      {
        if (((Component) hoveringIcon).gameObject.activeSelf)
        {
          UiCreature component = ((Component) hoveringIcon).GetComponent<UiCreature>();
          if (!component.dropped)
            list.Add(component);
        }
      }
      if (list.Count == 0)
        return;
      list.GetRandom<UiCreature>().click();
    }
  }

  private void clear()
  {
    this._pool_icons.clear();
    this._pool_icons.resetParent();
  }
}
