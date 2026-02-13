// Decompiled with JetBrains decompiler
// Type: AutoSavesWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AutoSavesWindow : MonoBehaviour
{
  [SerializeField]
  private AutoSaveElement _element_prefab;
  private List<AutoSaveElement> elements = new List<AutoSaveElement>();
  private Queue<AutoSaveData> _showQueue = new Queue<AutoSaveData>();
  [SerializeField]
  private VerticalLayoutGroup _elements_parent;
  private float _timer;

  private void OnEnable()
  {
    this.prepareList();
    this.prepareSaves();
  }

  private void prepareSaves()
  {
    this._showQueue.Clear();
    using (ListPool<AutoSaveData> autoSaves = AutoSaveManager.getAutoSaves())
    {
      for (int index = 0; index < autoSaves.Count; ++index)
        this._showQueue.Enqueue(autoSaves[index]);
    }
  }

  private void Update()
  {
    if ((double) this._timer > 0.0)
    {
      this._timer -= Time.deltaTime;
    }
    else
    {
      this._timer = 0.02f;
      this.showNextItemFromQueue();
    }
  }

  private void showNextItemFromQueue()
  {
    if (this._showQueue.Count == 0)
      return;
    this.renderMapElement(this._showQueue.Dequeue());
  }

  private void prepareList()
  {
    foreach (Component element in this.elements)
      Object.Destroy((Object) element.gameObject);
    this.elements.Clear();
  }

  private void renderMapElement(AutoSaveData pData)
  {
    AutoSaveElement autoSaveElement = Object.Instantiate<AutoSaveElement>(this._element_prefab, ((Component) this._elements_parent).transform);
    this.elements.Add(autoSaveElement);
    autoSaveElement.load(pData);
  }
}
