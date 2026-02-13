// Decompiled with JetBrains decompiler
// Type: KnowledgeWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class KnowledgeWindow : TabbedWindow
{
  [SerializeField]
  private Transform _elements_parent;
  [SerializeField]
  private KnowledgeElement _element_prefab;
  [SerializeField]
  private StatBar _progress_bar;
  [SerializeField]
  private CubeOverview _cube_overview_big;
  [SerializeField]
  private WindowMetaTab _cube_tab;

  protected override void create()
  {
    base.create();
    foreach (KnowledgeAsset pAsset in AssetManager.knowledge_library.list)
    {
      if (pAsset.show_in_knowledge_window)
      {
        KnowledgeElement knowledgeElement = Object.Instantiate<KnowledgeElement>(this._element_prefab, this._elements_parent);
        knowledgeElement.setAsset(pAsset);
        knowledgeElement.setCube(this._cube_overview_big, this._cube_tab);
      }
    }
  }

  private void OnEnable()
  {
    int pVal = 0;
    int num = 0;
    foreach (KnowledgeAsset knowledgeAsset in AssetManager.knowledge_library.list)
    {
      if (knowledgeAsset.show_in_knowledge_window)
      {
        pVal += knowledgeAsset.countUnlockedByPlayer();
        num += knowledgeAsset.countTotal();
      }
    }
    this._progress_bar.setBar((float) pVal, (float) num, "/" + num.ToText());
  }
}
