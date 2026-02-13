// Decompiled with JetBrains decompiler
// Type: tools.debug.DebugMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace tools.debug;

public class DebugMap
{
  public static void makeDebugMap()
  {
    DebugMap.createDebugButtons();
    foreach (WorldTile tiles in World.world.tiles_list)
      MapAction.terraformTile(tiles, TileLibrary.soil_low, TopTileLibrary.grass_low, TerraformLibrary.destroy);
    int pX = 10;
    int pY = 10;
    int index = 0;
    int count = AssetManager.buildings.list.Count;
    while (index < count)
    {
      BuildingAsset pAsset = AssetManager.buildings.list[index];
      if (pAsset.id.Contains("!"))
      {
        ++index;
      }
      else
      {
        ++index;
        pX += 20;
        if (pX > 200)
        {
          pX = 10;
          pY += 10;
        }
        Building building = World.world.buildings.addBuilding(pAsset, World.world.GetTile(pX, pY));
        building.kingdom = World.world.kingdoms_wild.get("nature");
        building.updateBuild(10000);
        if (building.asset.docks)
        {
          foreach (WorldTile tile in building.tiles)
            MapAction.terraformMain(tile, TileLibrary.shallow_waters, TerraformLibrary.flash);
        }
      }
    }
    Config.paused = true;
  }

  private static void debugConstructionZone()
  {
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
      building.debugConstructions();
  }

  private static void debugNextFrame()
  {
  }

  private static void debugRuins()
  {
  }

  public static void createDebugButtons()
  {
    Button button1 = DebugMap.makeNewButton("debug_next_frame", "iconBuildings");
    // ISSUE: method pointer
    ((UnityEvent) button1.onClick).AddListener(new UnityAction((object) null, __methodptr(debugNextFrame)));
    ((Component) button1).GetComponent<RectTransform>().anchoredPosition = new Vector2(50f, -20f);
    Button button2 = DebugMap.makeNewButton("debug_ruins", "iconDemolish");
    // ISSUE: method pointer
    ((UnityEvent) button2.onClick).AddListener(new UnityAction((object) null, __methodptr(debugRuins)));
    ((Component) button2).GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -20f);
    Button button3 = DebugMap.makeNewButton("debug_construction", "iconBucket");
    // ISSUE: method pointer
    ((UnityEvent) button3.onClick).AddListener(new UnityAction((object) null, __methodptr(debugConstructionZone)));
    ((Component) button3).GetComponent<RectTransform>().anchoredPosition = new Vector2(150f, -20f);
  }

  private static Button makeNewButton(string pName, string pIcon)
  {
    Button button = Object.Instantiate<Button>((Button) Resources.Load("ui/PrefabWorldBoxButton", typeof (Button)), ((Component) World.world.canvas).transform);
    ((Object) ((Component) button).transform).name = pName;
    ((Component) button).transform.parent = ((Component) World.world.canvas).transform;
    Sprite sprite = (Sprite) Resources.Load("ui/Icons/" + pIcon, typeof (Sprite));
    ((Component) ((Component) button).transform.Find("Icon")).GetComponent<Image>().sprite = sprite;
    return button;
  }
}
