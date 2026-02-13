// Decompiled with JetBrains decompiler
// Type: UnitTextManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitTextManager : MonoBehaviour
{
  [SerializeField]
  private DragSnapElement _follow_object;
  private List<UnitTextPhrases> _phrases = new List<UnitTextPhrases>();

  private void Start()
  {
    foreach (UnitTextPhrases componentsInChild in ((Component) this).GetComponentsInChildren<UnitTextPhrases>(true))
      this._phrases.Add(componentsInChild);
  }

  private void Update()
  {
    if (!SelectedUnit.isSet() || SelectedUnit.unit.isLying() || Object.op_Equality((Object) this._follow_object, (Object) null) || Time.frameCount % 50 != 0 || !Config.isDraggingItem() || !Config.dragging_item_object.HasComponent<UnitAvatarLoader>())
      return;
    this.startNewCurse();
  }

  public void startNew(string pText)
  {
    using (ListPool<UnitTextPhrases> list = new ListPool<UnitTextPhrases>())
    {
      foreach (UnitTextPhrases phrase in this._phrases)
      {
        if (!phrase.isTweening())
          list.Add(phrase);
      }
      if (list.Count == 0)
        return;
      list.GetRandom<UnitTextPhrases>().startNewTween(pText, Config.dragging_item_object?.transform);
    }
  }

  public void startNewCurse() => this.startNew(this.getRandomInsultText());

  public void startNewWhat()
  {
    if (SelectedUnit.unit.isLying())
      return;
    if (Randy.randomChance(0.3f))
    {
      this.startNewCurse();
    }
    else
    {
      int capacity = Randy.randomInt(1, 7);
      bool flag = Randy.randomBool();
      using (StringBuilderPool stringBuilderPool = new StringBuilderPool(capacity))
      {
        for (int index = 0; index < capacity; ++index)
        {
          if (flag)
            stringBuilderPool.Append('?');
          else
            stringBuilderPool.Append('!');
        }
        this.startNew(stringBuilderPool.ToString());
      }
    }
  }

  public void spawnAvatarText(Actor pActor = null)
  {
    if (!pActor.isRekt() && pActor.isLying() || pActor == null && SelectedUnit.unit.isLying())
      this.startNewCurse();
    else
      this.startNew(this.getAssetText(pActor.isRekt() ? (ActorAsset) null : pActor.getActorAsset()));
  }

  public string getAssetText(ActorAsset pAsset = null)
  {
    string str = $"click_{(pAsset ?? SelectedUnit.unit.asset).id}_{Randy.randomInt(1, 3).ToString()}";
    return LocalizedTextManager.instance.contains(str) ? LocalizedTextManager.getText(str) : this.getRandomInsultText();
  }

  private string getRandomInsultText() => InsultStringGenerator.getRandomText();
}
