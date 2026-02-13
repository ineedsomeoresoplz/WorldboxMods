// Decompiled with JetBrains decompiler
// Type: ILibraryWithUnlockables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public interface ILibraryWithUnlockables
{
  IEnumerable<BaseUnlockableAsset> elements_list { get; }

  int countTotalKnowledge()
  {
    int num = 0;
    foreach (BaseUnlockableAsset elements in this.elements_list)
    {
      if (elements.show_in_knowledge_window)
        ++num;
    }
    return num;
  }

  int countUnlockedByPlayer()
  {
    int num = 0;
    foreach (BaseUnlockableAsset elements in this.elements_list)
    {
      if (elements.show_in_knowledge_window && elements.isUnlockedByPlayer())
        ++num;
    }
    return num;
  }
}
