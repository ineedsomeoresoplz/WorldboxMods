// Decompiled with JetBrains decompiler
// Type: LoadWorldButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class LoadWorldButton : MonoBehaviour
{
  private void Start()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(loadWorld)));
  }

  private void loadWorld()
  {
    if (SaveManager.getCurrentMeta().saveVersion == 15)
    {
      ErrorWindow.errorMessage = "No, abandon it.";
      ScrollWindow.get("error_with_reason").clickShow();
    }
    else
      ScrollWindow.showWindow("save_load_confirm");
  }
}
