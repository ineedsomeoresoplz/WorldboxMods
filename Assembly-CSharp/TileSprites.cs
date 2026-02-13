// Decompiled with JetBrains decompiler
// Type: TileSprites
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable disable
public class TileSprites
{
  private List<Tile> _tiles = new List<Tile>();

  public void addVariation(Sprite pSprite, string pID)
  {
    Tile instance = ScriptableObject.CreateInstance<Tile>();
    ((Object) instance).name = pID;
    instance.sprite = pSprite;
    this._tiles.Add(instance);
  }

  public Tile getRandom() => this._tiles.GetRandom<Tile>();

  public Tile getVariation(int pID) => this._tiles[pID];

  public Tile main => this._tiles[0];
}
