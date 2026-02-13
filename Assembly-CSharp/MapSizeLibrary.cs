// Decompiled with JetBrains decompiler
// Type: MapSizeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;

#nullable disable
[Skip]
public class MapSizeLibrary : AssetLibrary<MapSizeAsset>
{
  public const string tiny = "tiny";
  public const string small = "small";
  public const string standard = "standard";
  public const string large = "large";
  public const string huge = "huge";
  public const string gigantic = "gigantic";
  public const string titanic = "titanic";
  public const string iceberg = "iceberg";
  private static string[] _mapSizes;

  public static string[] getSizes() => MapSizeLibrary._mapSizes;

  public override void init()
  {
    base.init();
    MapSizeAsset pAsset1 = new MapSizeAsset();
    pAsset1.id = "tiny";
    pAsset1.size = 2;
    pAsset1.path_icon = "actor_traits/iconTiny";
    this.add(pAsset1);
    MapSizeAsset pAsset2 = new MapSizeAsset();
    pAsset2.id = "small";
    pAsset2.size = 3;
    pAsset2.path_icon = "iconAntBlack";
    this.add(pAsset2);
    MapSizeAsset pAsset3 = new MapSizeAsset();
    pAsset3.id = "standard";
    pAsset3.size = 4;
    pAsset3.path_icon = "iconTileSand";
    this.add(pAsset3);
    MapSizeAsset pAsset4 = new MapSizeAsset();
    pAsset4.id = "large";
    pAsset4.size = 5;
    pAsset4.path_icon = "iconTileSoil";
    pAsset4.show_warning = true;
    this.add(pAsset4);
    MapSizeAsset pAsset5 = new MapSizeAsset();
    pAsset5.id = "huge";
    pAsset5.size = 6;
    pAsset5.path_icon = "iconTileHighSoil";
    pAsset5.show_warning = true;
    this.add(pAsset5);
    MapSizeAsset pAsset6 = new MapSizeAsset();
    pAsset6.id = "gigantic";
    pAsset6.size = 7;
    pAsset6.path_icon = "iconTileMountains";
    pAsset6.show_warning = true;
    this.add(pAsset6);
    MapSizeAsset pAsset7 = new MapSizeAsset();
    pAsset7.id = "titanic";
    pAsset7.size = 8;
    pAsset7.path_icon = "iconTitanic";
    pAsset7.show_warning = true;
    this.add(pAsset7);
    MapSizeAsset pAsset8 = new MapSizeAsset();
    pAsset8.id = "iceberg";
    pAsset8.size = 9;
    pAsset8.path_icon = "iconIceberg";
    pAsset8.show_warning = true;
    this.add(pAsset8);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.convertToOldFormat();
  }

  private void convertToOldFormat()
  {
    MapSizeLibrary._mapSizes = new string[this.list.Count];
    for (int index = 0; index < MapSizeLibrary._mapSizes.Length; ++index)
      MapSizeLibrary._mapSizes[index] = this.list[index].id;
  }

  public static int getSize(string pId)
  {
    MapSizeAsset mapSizeAsset = AssetManager.map_sizes.get(pId);
    return mapSizeAsset == null ? 2 : mapSizeAsset.size;
  }

  public static MapSizeAsset getPresetAsset(int pMapSize)
  {
    foreach (MapSizeAsset presetAsset in AssetManager.map_sizes.list)
    {
      if (presetAsset.size == pMapSize)
        return presetAsset;
    }
    return (MapSizeAsset) null;
  }

  public static string getPreset(int pMapSize)
  {
    foreach (MapSizeAsset mapSizeAsset in AssetManager.map_sizes.list)
    {
      if (mapSizeAsset.size == pMapSize)
        return mapSizeAsset.id;
    }
    return (string) null;
  }

  public static bool isSizeValid(int pMapSize)
  {
    return MapSizeLibrary.getPresetAsset(pMapSize) != null && pMapSize <= MapSizeLibrary.getSize(Config.maxMapSize);
  }

  public override void editorDiagnosticLocales()
  {
    foreach (MapSizeAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
    base.editorDiagnosticLocales();
  }
}
