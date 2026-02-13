// Decompiled with JetBrains decompiler
// Type: CitiesKingdomsContainersController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CitiesKingdomsContainersController : MonoBehaviour
{
  [SerializeField]
  private CitiesBannersContainer _banners_cities;
  [SerializeField]
  private GameObject _line_cities;
  [SerializeField]
  private KingdomsBannersContainer _banners_kingdoms;
  [SerializeField]
  private GameObject _line_kingdoms;

  public void update(NanoObject pNano)
  {
    this._banners_cities.update(pNano);
    this._banners_kingdoms.update(pNano);
    IMetaObject metaObject = (IMetaObject) pNano;
    bool flag1 = metaObject.hasCities();
    ((Component) this._banners_cities).gameObject.SetActive(flag1);
    this._line_cities.SetActive(flag1);
    bool flag2 = metaObject.hasKingdoms();
    ((Component) this._banners_kingdoms).gameObject.SetActive(flag2);
    this._line_kingdoms.SetActive(flag2);
  }
}
