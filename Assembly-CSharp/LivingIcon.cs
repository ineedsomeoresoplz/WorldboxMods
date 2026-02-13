// Decompiled with JetBrains decompiler
// Type: LivingIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LivingIcon : MonoBehaviour
{
  private Vector3 init_position;
  private float speed_back;
  private float speed_away;
  private float return_timer;
  public static int killed_mod = 1;

  private void Awake() => this.init_position = ((Component) this).transform.position;

  public void kill()
  {
    ++LivingIcon.killed_mod;
    ((Behaviour) this).enabled = false;
  }

  public void Update()
  {
    Vector3 mousePosition = Input.mousePosition;
    if ((double) Vector2.Distance(Vector2.op_Implicit(((Component) this).transform.position), Vector2.op_Implicit(mousePosition)) < (double) (80 /*0x50*/ + LivingIcon.killed_mod * 10))
    {
      if ((double) this.speed_away == 0.0 && LivingIcon.killed_mod > 6)
        this.speed_away = (float) (LivingIcon.killed_mod * 10);
      this.speed_away += 200f * Time.deltaTime * (float) LivingIcon.killed_mod;
    }
    else if ((double) this.speed_away > 0.0)
    {
      this.speed_away -= 500f * Time.deltaTime;
      if ((double) this.speed_away < 0.0)
        this.speed_away = 0.0f;
    }
    if ((double) this.speed_away > 0.0)
    {
      ((Component) this).transform.position = Vector2.op_Implicit(Vector2.MoveTowards(Vector2.op_Implicit(((Component) this).transform.position), Vector2.op_Implicit(mousePosition), -1f * this.speed_away * Time.deltaTime));
      this.return_timer = 1f;
      this.speed_back = 0.0f;
      rotate();
    }
    else if ((double) this.return_timer > 0.0)
      this.return_timer -= Time.deltaTime;
    else if ((double) Vector2.Distance(Vector2.op_Implicit(((Component) this).transform.position), Vector2.op_Implicit(this.init_position)) > 1.0)
    {
      this.speed_back += Time.deltaTime * 400f;
      ((Component) this).transform.position = Vector2.op_Implicit(Vector2.MoveTowards(Vector2.op_Implicit(((Component) this).transform.position), Vector2.op_Implicit(this.init_position), Time.deltaTime * this.speed_back));
    }
    else
      this.speed_back = 0.0f;

    void rotate()
    {
      Vector3 eulerAngles = ((Component) this).transform.eulerAngles;
      eulerAngles.z += 10f;
      ((Component) this).transform.eulerAngles = eulerAngles;
    }
  }
}
