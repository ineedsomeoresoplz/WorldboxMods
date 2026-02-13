// Decompiled with JetBrains decompiler
// Type: ShadowEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ShadowEditor : MonoBehaviour
{
  public static ShadowEditor instance;
  public bool isEnabled;
  public Vector2 shadow_bound = new Vector2(0.5f, 0.14f);
  public float shadow_distortion = 0.08f;
}
