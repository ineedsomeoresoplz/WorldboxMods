// Decompiled with JetBrains decompiler
// Type: GenomePart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public readonly struct GenomePart(string id, float pValue) : IEquatable<GenomePart>
{
  public readonly string id = !string.IsNullOrEmpty(id) ? id : throw new ArgumentNullException("id cannot be null or empty");
  public readonly float value = pValue;

  public override bool Equals(object pObject)
  {
    return pObject is GenomePart pOther && this.Equals(pOther);
  }

  public bool Equals(GenomePart pOther)
  {
    return string.Equals(this.id, pOther.id, StringComparison.OrdinalIgnoreCase);
  }

  public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(this.id);

  public override string ToString() => $"{this.id}: {this.value}";
}
