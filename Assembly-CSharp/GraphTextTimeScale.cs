// Decompiled with JetBrains decompiler
// Type: GraphTextTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GraphTextTimeScale : MonoBehaviour
{
  public GraphTimeScaleContainer graph_time_scale_container;
  private Text _text;

  public void Awake() => this._text = ((Component) this).GetComponent<Text>();

  public void Update()
  {
    this._text.text = Toolbox.formatNumber((long) AssetManager.graph_time_library.get(this.graph_time_scale_container.current_scale.ToString()).max_time_frame) + this.graph_time_scale_container.getIndexString();
  }
}
