// Decompiled with JetBrains decompiler
// Type: KingdomCheckCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class KingdomCheckCache
{
  public Dictionary<long, bool> dict = new Dictionary<long, bool>();

  public long getHash(Kingdom pK1, Kingdom pK2)
  {
    int hashCode1 = pK1.GetHashCode();
    int hashCode2 = pK2.GetHashCode();
    return hashCode1 <= hashCode2 ? (long) (hashCode2 * 1000000 + hashCode1) : (long) (hashCode1 * 1000000 + hashCode2);
  }

  public void clear() => this.dict.Clear();
}
