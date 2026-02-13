// Decompiled with JetBrains decompiler
// Type: MapSizeTextUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MapSizeTextUpdater : MonoBehaviour
{
  public Text text_counter;

  private void Update() => this.updateVars();

  private void updateVars()
  {
    Text component = ((Component) this).GetComponent<Text>();
    component.text = LocalizedTextManager.getText(AssetManager.map_sizes.get(Config.customMapSize).getLocaleID()).ToUpper();
    ((Component) component).GetComponent<LocalizedText>().checkSpecialLanguages();
    string[] sizes = MapSizeLibrary.getSizes();
    int num1 = sizes.IndexOf<string>(Config.customMapSize);
    Text textCounter = this.text_counter;
    int num2 = num1 + 1;
    string str1 = num2.ToString();
    num2 = sizes.Length;
    string str2 = num2.ToString();
    string str3 = $"{str1}/{str2}";
    textCounter.text = str3;
  }
}
