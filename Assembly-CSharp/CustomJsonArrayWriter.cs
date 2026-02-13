// Decompiled with JetBrains decompiler
// Type: CustomJsonArrayWriter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System.IO;

#nullable disable
public class CustomJsonArrayWriter(TextWriter writer) : JsonTextWriter(writer)
{
  protected virtual void WriteIndent()
  {
    if (((JsonWriter) this).WriteState != 3)
      base.WriteIndent();
    else
      ((JsonWriter) this).WriteIndentSpace();
  }
}
