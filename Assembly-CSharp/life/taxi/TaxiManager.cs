// Decompiled with JetBrains decompiler
// Type: life.taxi.TaxiManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace life.taxi;

public class TaxiManager
{
  public static List<TaxiRequest> list = new List<TaxiRequest>();
  private static float timer_check = 0.0f;

  public static void newRequest(Actor pActor, WorldTile pTileTarget)
  {
    if (pActor.is_inside_boat)
      return;
    TaxiRequest taxiRequest1 = (TaxiRequest) null;
    foreach (TaxiRequest taxiRequest2 in TaxiManager.list)
    {
      if (taxiRequest2.isSameKingdom(pActor.kingdom) && !taxiRequest2.isState(TaxiRequestState.Transporting) && !taxiRequest2.isState(TaxiRequestState.Finished) && taxiRequest2.getTileTarget().isSameIsland(pTileTarget) && taxiRequest2.getTileStart().isSameIsland(pActor.current_tile))
      {
        taxiRequest1 = taxiRequest2;
        break;
      }
    }
    if (taxiRequest1 != null)
    {
      taxiRequest1.addActor(pActor);
    }
    else
    {
      TaxiRequest taxiRequest3 = new TaxiRequest(pActor, pActor.kingdom, pActor.current_tile, pTileTarget);
      TaxiManager.list.Add(taxiRequest3);
    }
  }

  public static void cancelRequest(TaxiRequest pRequest)
  {
    pRequest.cancel();
    TaxiManager.list.Remove(pRequest);
  }

  public static TaxiRequest getRequestForActor(Actor pActor)
  {
    foreach (TaxiRequest requestForActor in TaxiManager.list)
    {
      if (requestForActor.hasActor(pActor))
        return requestForActor;
    }
    return (TaxiRequest) null;
  }

  public static TaxiRequest getNewRequestForBoat(Actor pBoatActor)
  {
    TaxiRequest newRequestForBoat1 = (TaxiRequest) null;
    foreach (TaxiRequest newRequestForBoat2 in TaxiManager.list)
    {
      if (newRequestForBoat2.isAlreadyUsedByBoat(pBoatActor))
        return newRequestForBoat2;
      if (newRequestForBoat2.isState(TaxiRequestState.Pending) && newRequestForBoat2.isStillLegit() && newRequestForBoat2.isSameKingdom(pBoatActor.kingdom) && (newRequestForBoat1 == null || newRequestForBoat1.countActors() < newRequestForBoat2.countActors()))
        newRequestForBoat1 = newRequestForBoat2;
    }
    return newRequestForBoat1;
  }

  public static void clear()
  {
    foreach (TaxiRequest taxiRequest in TaxiManager.list)
      taxiRequest.clear();
    TaxiManager.list.Clear();
  }

  public static void finish(TaxiRequest pRequest)
  {
    pRequest.finish();
    TaxiManager.list.Remove(pRequest);
  }

  public static void cancelTaxiRequestForActor(Actor pActor)
  {
    TaxiRequest requestForActor = TaxiManager.getRequestForActor(pActor);
    if (requestForActor == null)
      return;
    TaxiManager.cancelRequest(requestForActor);
  }

  public static void update(float pElapsed)
  {
    if ((double) TaxiManager.timer_check > 0.0)
    {
      TaxiManager.timer_check -= pElapsed;
    }
    else
    {
      TaxiManager.timer_check = 5f;
      int index = 0;
      while (TaxiManager.list.Count > index)
      {
        TaxiRequest taxiRequest = TaxiManager.list[index];
        if (taxiRequest.isStillLegit())
        {
          ++index;
        }
        else
        {
          taxiRequest.finish();
          TaxiManager.list.RemoveAt(index);
        }
      }
    }
  }

  public static void removeDeadUnits()
  {
    foreach (TaxiRequest taxiRequest in TaxiManager.list)
      taxiRequest.removeDeadUnits();
  }
}
