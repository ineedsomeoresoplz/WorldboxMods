// Decompiled with JetBrains decompiler
// Type: ColorTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public class ColorTool : MonoBehaviour
{
  public string colorString;
  public GameObject prefabKingdom;
  public GameObject prefabClan;
  public GameObject prefabCulture;
  public GameObject prefabAlliance;
  public Transform container;
  public string last_editor = "";

  private void resetCoords()
  {
  }

  public void InitKingdoms()
  {
    this.cleanup();
    this.last_editor = "kingdoms";
    KingdomColorsLibrary kingdomColorsLibrary = new KingdomColorsLibrary();
    kingdomColorsLibrary.init();
    kingdomColorsLibrary.post_init();
    foreach (ColorAsset pColor in kingdomColorsLibrary.list)
      this.createColorToolElement(pColor, this.prefabKingdom, this.last_editor);
  }

  public void InitCultures()
  {
    this.cleanup();
    this.last_editor = "cultures";
    CultureColorsLibrary cultureColorsLibrary = new CultureColorsLibrary();
    cultureColorsLibrary.init();
    cultureColorsLibrary.post_init();
    foreach (ColorAsset pColor in cultureColorsLibrary.list)
      this.createColorToolElement(pColor, this.prefabCulture, this.last_editor);
  }

  public void InitClans()
  {
    this.cleanup();
    this.last_editor = "clans";
    ClanColorsLibrary clanColorsLibrary = new ClanColorsLibrary();
    clanColorsLibrary.init();
    clanColorsLibrary.post_init();
    foreach (ColorAsset pColor in clanColorsLibrary.list)
      this.createColorToolElement(pColor, this.prefabClan, this.last_editor);
  }

  public void cleanup()
  {
    this.resetCoords();
    while (this.container.childCount > 0)
      Object.DestroyImmediate((Object) ((Component) this.container.GetChild(0)).gameObject);
  }

  private void createColorToolElement(ColorAsset pColor, GameObject pPrefab, string pWhat)
  {
    ColorToolElement component = Object.Instantiate<GameObject>(pPrefab, this.container).GetComponent<ColorToolElement>();
    if (this.last_editor == "kingdoms")
      component.createKingdom(pColor);
    else if (this.last_editor == "clans")
      component.createClans(pColor);
    else if (this.last_editor == "cultures")
      component.createCulture(pColor);
    ((Object) ((Component) component).transform).name = $"{pColor.index_id.ToString()}-{pColor.id}";
    ((Component) component).transform.SetSiblingIndex(pColor.index_id);
  }

  public void saveEditor()
  {
    if (this.last_editor == "kingdoms")
      this.saveKingdoms();
    else if (this.last_editor == "clans")
    {
      this.saveClans();
    }
    else
    {
      if (!(this.last_editor == "cultures"))
        return;
      this.saveCultures();
    }
  }

  private void convertToolIntoAsset(ColorToolElement pTool, ColorAsset pAsset)
  {
    pAsset.color_main = Toolbox.colorToHex(Color32.op_Implicit(pTool.colorMain), false);
    pAsset.color_main_2 = Toolbox.colorToHex(Color32.op_Implicit(pTool.colorMain2), false);
    pAsset.color_banner = Toolbox.colorToHex(Color32.op_Implicit(pTool.colorBanner), false);
    pAsset.color_text = Toolbox.colorToHex(Color32.op_Implicit(pTool.colorText), false);
    pAsset.id = pTool.id;
    pAsset.favorite = pTool.favorite;
  }

  private void saveKingdoms() => this.saveLib((ColorLibrary) new KingdomColorsLibrary());

  private void saveCultures() => this.saveLib((ColorLibrary) new CultureColorsLibrary());

  private void saveClans() => this.saveLib((ColorLibrary) new ClanColorsLibrary());

  private void saveLib(ColorLibrary pLibrary)
  {
    for (int index = 0; index < this.container.childCount; ++index)
    {
      ColorToolElement component = ((Component) this.container.GetChild(index)).GetComponent<ColorToolElement>();
      ColorAsset pAsset = new ColorAsset();
      this.convertToolIntoAsset(component, pAsset);
      pAsset.index_id = index;
      pLibrary.list.Add(pAsset);
    }
    string json = JsonUtility.ToJson((object) pLibrary, true);
    File.WriteAllText(pLibrary.getEditorPathForSave(), json);
  }
}
