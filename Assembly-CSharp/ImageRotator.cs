// Decompiled with JetBrains decompiler
// Type: ImageRotator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ImageRotator : MonoBehaviour
{
  public float rotation_speed = 70f;

  private void Update()
  {
    ((Component) this).transform.Rotate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.forward, this.rotation_speed), Time.deltaTime), (Space) 1);
  }
}
