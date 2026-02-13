// Decompiled with JetBrains decompiler
// Type: DecisionChecks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public readonly ref struct DecisionChecks
{
  public readonly bool is_hungry;
  public readonly bool is_fighting;
  public readonly bool is_herd;
  public readonly bool is_adult;
  public readonly bool is_civ;
  public readonly bool is_sapient;
  public readonly bool city_is_in_danger;
  public readonly bool can_capture_city;

  public DecisionChecks(
    bool pIsHungry,
    bool pIsFighting,
    bool pIsHerd,
    bool pIsAdult,
    bool pIsCiv,
    bool pIsSapient,
    bool pCityIsInDanger,
    bool pCanCaptureCity)
  {
    this.is_hungry = pIsHungry;
    this.is_fighting = pIsFighting;
    this.is_herd = pIsHerd;
    this.is_adult = pIsAdult;
    this.is_civ = pIsCiv;
    this.is_sapient = pIsSapient;
    this.city_is_in_danger = pCityIsInDanger;
    this.can_capture_city = pCanCaptureCity;
  }

  public DecisionChecks(Actor pActor)
  {
    this.is_hungry = pActor.isHungry();
    this.is_fighting = pActor.isFighting();
    this.is_herd = pActor.asset.follow_herd;
    this.is_adult = pActor.isAdult();
    this.is_sapient = pActor.isSapient();
    this.is_civ = pActor.isKingdomCiv();
    this.city_is_in_danger = pActor.inOwnCityBorders() && pActor.city.isInDanger();
    ProfessionAsset professionAsset = pActor.profession_asset;
    this.can_capture_city = professionAsset != null && professionAsset.can_capture;
  }
}
