// Decompiled with JetBrains decompiler
// Type: LoadingScreenSheepAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LoadingScreenSheepAnimation : MonoBehaviour
{
  internal static float angle;

  private void Update()
  {
    LoadingScreenSheepAnimation.angle += Time.deltaTime * 20f;
    ((Component) this).transform.localEulerAngles = new Vector3(0.0f, 0.0f, LoadingScreenSheepAnimation.angle);
  }
}
