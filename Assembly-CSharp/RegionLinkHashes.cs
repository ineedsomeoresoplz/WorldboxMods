// Decompiled with JetBrains decompiler
// Type: RegionLinkHashes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class RegionLinkHashes
{
  private static readonly Dictionary<int, RegionLink> _dict = new Dictionary<int, RegionLink>();
  private static readonly StackPool<RegionLink> _pool = new StackPool<RegionLink>();

  public static void addHash(int pHash, MapRegion pRegion)
  {
    RegionLink pLink;
    if (!RegionLinkHashes._dict.TryGetValue(pHash, out pLink))
    {
      pLink = RegionLinkHashes._pool.get();
      pLink.reset();
      pLink.id = pHash;
      RegionLinkHashes._dict[pLink.id] = pLink;
    }
    if (!pLink.regions.Add(pRegion))
      return;
    pRegion.addLink(pLink);
  }

  public static int getCount() => RegionLinkHashes._dict.Count;

  public static void clear()
  {
    foreach (RegionLink pObject in RegionLinkHashes._dict.Values)
    {
      pObject.reset();
      RegionLinkHashes._pool.release(pObject);
    }
    RegionLinkHashes._dict.Clear();
  }

  public static RegionLink getHash(int pHash)
  {
    RegionLink hash;
    RegionLinkHashes._dict.TryGetValue(pHash, out hash);
    return hash;
  }

  public static void remove(RegionLink pLink, MapRegion pRegion)
  {
    pLink.regions.Remove(pRegion);
    if (pLink.regions.Count != 0 || !RegionLinkHashes._dict.Remove(pLink.id))
      return;
    pLink.reset();
    RegionLinkHashes._pool.release(pLink);
  }

  public static void debug(DebugTool pTool)
  {
    pTool.setText("hashes", (object) RegionLinkHashes._dict.Count);
  }
}
