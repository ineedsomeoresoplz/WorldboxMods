// Decompiled with JetBrains decompiler
// Type: DebugMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DebugMessage : MonoBehaviour
{
  public GameObject prefab;
  public static bool log_enabled;
  public static DebugMessage instance;
  public List<DebugMessageFly> list;
  private List<DebugMessageFly> messagesToMove = new List<DebugMessageFly>();

  private void Start()
  {
    DebugMessage.instance = this;
    this.list = new List<DebugMessageFly>();
  }

  public void moveAll(DebugMessageFly pMessage)
  {
    this.messagesToMove.Clear();
    foreach (DebugMessageFly debugMessageFly in this.list)
    {
      if (!Object.op_Equality((Object) debugMessageFly, (Object) pMessage) && (double) Toolbox.Dist(0.0f, ((Component) debugMessageFly).transform.localPosition.y, 0.0f, ((Component) pMessage).transform.localPosition.y) < 1.0)
        this.messagesToMove.Add(debugMessageFly);
    }
    foreach (DebugMessageFly debugMessageFly in this.messagesToMove)
      debugMessageFly.moveUp();
  }

  public DebugMessageFly getOldMessage(Transform pTransform)
  {
    foreach (DebugMessageFly oldMessage in this.list)
    {
      if (Object.op_Equality((Object) oldMessage.originTransform, (Object) pTransform))
        return oldMessage;
    }
    return (DebugMessageFly) null;
  }

  public static void log(Transform pTransofrm, string pMessage)
  {
    if (!Debug.isDebugBuild || !DebugMessage.log_enabled)
      return;
    DebugMessageFly oldMessage = DebugMessage.instance.getOldMessage(pTransofrm);
    if (Object.op_Inequality((Object) oldMessage, (Object) null))
    {
      oldMessage.addString(pMessage);
    }
    else
    {
      TextMesh component1 = Object.Instantiate<GameObject>(DebugMessage.instance.prefab).gameObject.GetComponent<TextMesh>();
      ((Renderer) ((Component) component1).gameObject.GetComponent<MeshRenderer>()).sortingOrder = 100;
      ((Component) component1).transform.parent = ((Component) DebugMessage.instance).transform;
      DebugMessageFly component2 = ((Component) component1).GetComponent<DebugMessageFly>();
      component2.originTransform = pTransofrm;
      component2.addString(pMessage);
      DebugMessage.instance.list.Add(component2);
    }
  }
}
