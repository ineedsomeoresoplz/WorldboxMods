// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorGetResourceFromStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorGetResourceFromStorage : BehCityActor
{
  private string _resource_id;
  private int _amount;

  public BehCityActorGetResourceFromStorage(string pResourceId, int pAmount)
  {
    this._resource_id = pResourceId;
    this._amount = pAmount;
  }

  public override BehResult execute(Actor pActor)
  {
    City city = pActor.city;
    if (!city.hasStorages() || city.getResourcesAmount(this._resource_id) < this._amount)
      return BehResult.Stop;
    city.takeResource(this._resource_id, this._amount);
    pActor.addToInventory(this._resource_id, this._amount);
    return BehResult.Continue;
  }
}
