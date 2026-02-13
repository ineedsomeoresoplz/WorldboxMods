// Decompiled with JetBrains decompiler
// Type: CloudLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CloudLibrary : AssetLibrary<CloudAsset>
{
  private static string[] _sprites_small = new string[2]
  {
    "effects/clouds/cloud_small_1",
    "effects/clouds/cloud_small_2"
  };
  private static string[] _sprites_big = new string[3]
  {
    "effects/clouds/cloud_big_1",
    "effects/clouds/cloud_big_2",
    "effects/clouds/cloud_big_3"
  };
  private static string[] _sprites_all = new string[5]
  {
    "effects/clouds/cloud_small_1",
    "effects/clouds/cloud_small_2",
    "effects/clouds/cloud_big_1",
    "effects/clouds/cloud_big_2",
    "effects/clouds/cloud_big_3"
  };

  public override void init()
  {
    base.init();
    CloudAsset pAsset1 = new CloudAsset();
    pAsset1.id = "cloud_rain";
    pAsset1.color_hex = "#5D728C";
    pAsset1.drop_id = "rain";
    pAsset1.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset1.path_sprites = CloudLibrary._sprites_big;
    pAsset1.speed_max = 4f;
    this.add(pAsset1);
    CloudAsset pAsset2 = new CloudAsset();
    pAsset2.id = "cloud_rage";
    pAsset2.color_hex = "#FF3030";
    pAsset2.drop_id = "rage";
    pAsset2.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset2.path_sprites = CloudLibrary._sprites_big;
    pAsset2.speed_max = 4f;
    pAsset2.considered_disaster = true;
    pAsset2.draw_light_area = true;
    this.add(pAsset2);
    CloudAsset pAsset3 = new CloudAsset();
    pAsset3.id = "cloud_lightning";
    pAsset3.color_hex = "#445366";
    pAsset3.drop_id = "rain";
    pAsset3.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset3.cloud_action_2 = new CloudAction(CloudLibrary.spawnLightning);
    pAsset3.path_sprites = CloudLibrary._sprites_big;
    pAsset3.speed_max = 4f;
    pAsset3.considered_disaster = true;
    this.add(pAsset3);
    CloudAsset pAsset4 = new CloudAsset();
    pAsset4.id = "cloud_acid";
    pAsset4.color_hex = "#17D41C";
    pAsset4.drop_id = "acid";
    pAsset4.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset4.path_sprites = CloudLibrary._sprites_big;
    pAsset4.speed_max = 4f;
    pAsset4.considered_disaster = true;
    this.add(pAsset4);
    CloudAsset pAsset5 = new CloudAsset();
    pAsset5.id = "cloud_lava";
    pAsset5.color_hex = "#D17119";
    pAsset5.drop_id = "lava";
    pAsset5.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset5.path_sprites = CloudLibrary._sprites_big;
    pAsset5.speed_max = 3f;
    pAsset5.considered_disaster = true;
    pAsset5.draw_light_area = true;
    this.add(pAsset5);
    CloudAsset pAsset6 = new CloudAsset();
    pAsset6.id = "cloud_fire";
    pAsset6.color_hex = "#D14219";
    pAsset6.drop_id = "fire";
    pAsset6.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset6.path_sprites = CloudLibrary._sprites_big;
    pAsset6.speed_max = 3f;
    pAsset6.considered_disaster = true;
    pAsset6.draw_light_area = true;
    this.add(pAsset6);
    CloudAsset pAsset7 = new CloudAsset();
    pAsset7.id = "cloud_snow";
    pAsset7.color_hex = "#B8FFFA";
    pAsset7.drop_id = "snow";
    pAsset7.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset7.path_sprites = CloudLibrary._sprites_big;
    pAsset7.considered_disaster = true;
    pAsset7.speed_max = 4f;
    this.add(pAsset7);
    CloudAsset pAsset8 = new CloudAsset();
    pAsset8.id = "cloud_ash";
    pAsset8.color_hex = "#C6B077";
    pAsset8.drop_id = "ash";
    pAsset8.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset8.path_sprites = CloudLibrary._sprites_big;
    pAsset8.considered_disaster = true;
    pAsset8.speed_max = 4f;
    this.add(pAsset8);
    CloudAsset pAsset9 = new CloudAsset();
    pAsset9.id = "cloud_magic";
    pAsset9.color_hex = "#C976CC";
    pAsset9.drop_id = "magic_rain";
    pAsset9.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset9.path_sprites = CloudLibrary._sprites_small;
    pAsset9.considered_disaster = true;
    pAsset9.speed_max = 7f;
    pAsset9.draw_light_area = true;
    this.add(pAsset9);
    CloudAsset pAsset10 = new CloudAsset();
    pAsset10.id = "cloud_normal";
    pAsset10.max_alpha = 0.5f;
    pAsset10.interval_action_1 = 2f;
    pAsset10.drop_id = "life_seed";
    pAsset10.cloud_action_1 = new CloudAction(CloudLibrary.dropAction);
    pAsset10.path_sprites = CloudLibrary._sprites_all;
    pAsset10.speed_min = 2f;
    pAsset10.normal_cloud = true;
    this.add(pAsset10);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    using (ListPool<Sprite> list = new ListPool<Sprite>())
    {
      foreach (CloudAsset cloudAsset in this.list)
      {
        if (cloudAsset.color_hex != null)
          cloudAsset.color = Toolbox.makeColor(cloudAsset.color_hex);
        list.Clear();
        foreach (string pathSprite in cloudAsset.path_sprites)
        {
          Sprite sprite = SpriteTextureLoader.getSprite(pathSprite);
          if (Object.op_Equality((Object) sprite, (Object) null))
            BaseAssetLibrary.logAssetError("cloud sprite not found", pathSprite);
          else
            list.Add(sprite);
        }
        cloudAsset.cached_sprites = list.ToArray<Sprite>();
      }
    }
  }

  public static void dropAction(Cloud pCloud)
  {
    if ((double) pCloud.alive_time < 3.0)
      return;
    float effectTextureWidth = pCloud.effect_texture_width;
    float effectTextureHeight = pCloud.effect_texture_height;
    int x = (int) ((Component) pCloud).transform.localPosition.x;
    int y = (int) ((Component) pCloud).transform.localPosition.y;
    WorldTile tile = World.world.GetTile(x + (int) Randy.randomFloat(-effectTextureWidth, effectTextureWidth), y + (int) Randy.randomFloat(-effectTextureHeight + pCloud.spriteShadow.offset.y, effectTextureHeight + pCloud.spriteShadow.offset.y));
    if (tile == null)
      return;
    World.world.drop_manager.spawn(tile, pCloud.asset.drop_id, 10f);
  }

  public static void spawnLightning(Cloud pCloud)
  {
    if (!Randy.randomChance(0.01f))
      return;
    int x = (int) ((Component) pCloud).transform.localPosition.x;
    int y = (int) ((Component) pCloud).transform.localPosition.y;
    float effectTextureWidth = pCloud.effect_texture_width;
    float effectTextureHeight = pCloud.effect_texture_height;
    WorldTile tile = World.world.GetTile(x + (int) Randy.randomFloat(effectTextureWidth * 0.5f, effectTextureWidth), y + (int) Randy.randomFloat(-effectTextureHeight + pCloud.spriteShadow.offset.y, effectTextureHeight + pCloud.spriteShadow.offset.y));
    if (tile == null)
      return;
    if (Randy.randomBool())
      MapBox.spawnLightningMedium(tile, 0.15f);
    else
      MapBox.spawnLightningSmall(tile, 0.15f);
  }
}
