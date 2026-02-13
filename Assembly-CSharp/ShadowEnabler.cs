// Decompiled with JetBrains decompiler
// Type: ShadowEnabler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShadowEnabler : MonoBehaviour
{
  public Shadow[] shadowObjects = new Shadow[0];
  private bool isEnabled;

  private void Awake()
  {
    this.shadowObjects = ((Component) this).GetComponentsInChildren<Shadow>(true);
  }

  private void Update()
  {
    bool flag = (double) ((Component) this).transform.localScale.y == 1.0;
    if (this.isEnabled == flag)
      return;
    this.isEnabled = flag;
    this.toggle();
  }

  private void OnDisable()
  {
    this.isEnabled = false;
    this.toggle();
  }

  private void OnEnable()
  {
    this.isEnabled = false;
    this.toggle();
  }

  private void toggle()
  {
    for (int index = 0; index < this.shadowObjects.Length; ++index)
    {
      Shadow shadowObject = this.shadowObjects[index];
      if (!Object.op_Equality((Object) shadowObject, (Object) null))
        ((Behaviour) shadowObject).enabled = this.isEnabled;
    }
  }
}
