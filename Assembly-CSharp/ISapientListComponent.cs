// Decompiled with JetBrains decompiler
// Type: ISapientListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public interface ISapientListComponent
{
  void setSapientCounter(Text pCounter);

  void setNonSapientCounter(Text pCounter);

  void setShowSapientOnly();

  void setShowNonSapientOnly();

  void setDefault();
}
