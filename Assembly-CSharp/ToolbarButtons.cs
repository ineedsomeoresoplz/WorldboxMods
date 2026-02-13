// Decompiled with JetBrains decompiler
// Type: ToolbarButtons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ToolbarButtons : MonoBehaviour, IShakable
{
  public static ToolbarButtons instance;
  public Image main_background;
  public Sprite button_sprite_normal;
  public Sprite button_sprite_unit_exists;

  public float shake_duration { get; } = 0.5f;

  public float shake_strength { get; } = 4f;

  public Tweener shake_tween { get; set; }

  public static Sprite getSpriteButtonNormal()
  {
    return Object.op_Equality((Object) ToolbarButtons.instance, (Object) null) ? (Sprite) null : ToolbarButtons.instance.button_sprite_normal;
  }

  public static Sprite getSpriteButtonUnitExists()
  {
    return Object.op_Equality((Object) ToolbarButtons.instance, (Object) null) ? (Sprite) null : ToolbarButtons.instance.button_sprite_unit_exists;
  }

  private void Awake() => ToolbarButtons.instance = this;

  public void resetBar()
  {
    ((Component) this).gameObject.SetActive(false);
    ((Component) this).gameObject.SetActive(true);
  }

  private void Update()
  {
    if (Time.frameCount % 30 != 0)
      return;
    PowerButton.checkActorSpawnButtons();
  }

  public Vector3 getPowerBarLeftCornerViewportPos()
  {
    RectTransform rectTransform = ((Graphic) this.main_background).rectTransform;
    Vector3[] vector3Array1 = new Vector3[4];
    Vector3[] vector3Array2 = vector3Array1;
    rectTransform.GetWorldCorners(vector3Array2);
    return vector3Array1[1];
  }

  Transform IShakable.get_transform() => ((Component) this).transform;
}
