// Decompiled with JetBrains decompiler
// Type: ActorBagExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class ActorBagExtensions
{
  public static ActorBag add(this ActorBag pBag, ResourceContainer pResourceContainer)
  {
    return pBag.add(pResourceContainer.id, pResourceContainer.amount);
  }

  public static ActorBag add(this ActorBag pBag, string pID, int pAmount)
  {
    if (pBag == null)
      pBag = new ActorBag();
    if (pBag.dict == null)
      pBag.dict = new Dictionary<string, ResourceContainer>();
    ResourceContainer resourceContainer;
    if (pBag.dict.TryGetValue(pID, out resourceContainer))
      resourceContainer.amount += pAmount;
    else
      resourceContainer = new ResourceContainer(pID, pAmount);
    pBag.dict[pID] = resourceContainer;
    pBag.last_item_to_render = (string) null;
    return pBag;
  }

  public static ActorBag remove(this ActorBag pBag, string pID, int pAmount)
  {
    if (pBag.isEmpty())
      return (ActorBag) null;
    ResourceContainer resourceContainer;
    if (pBag.dict.TryGetValue(pID, out resourceContainer))
    {
      resourceContainer.amount -= pAmount;
      if (resourceContainer.amount <= 0)
        pBag.dict.Remove(pID);
      else
        pBag.dict[pID] = resourceContainer;
      pBag.last_item_to_render = (string) null;
    }
    return pBag;
  }

  public static Dictionary<string, ResourceContainer> getResources(this ActorBag pBag) => pBag.dict;

  public static bool hasResources(this ActorBag pBag)
  {
    if (pBag == null)
      return false;
    int? count = pBag.dict?.Count;
    int num = 0;
    return count.GetValueOrDefault() > num & count.HasValue;
  }

  public static bool isEmpty(this ActorBag pBag) => !pBag.hasResources();

  public static string getRandomResourceID(this ActorBag pBag)
  {
    if (pBag.isEmpty() || pBag.dict.Count == 0)
      return string.Empty;
    int num1 = Randy.randomInt(0, pBag.dict.Count);
    int num2 = 0;
    foreach (string key in pBag.dict.Keys)
    {
      if (num2 == num1)
        return key;
      ++num2;
    }
    return string.Empty;
  }

  public static int getResource(this ActorBag pBag, string pID)
  {
    ResourceContainer resourceContainer;
    return pBag.isEmpty() || !pBag.dict.TryGetValue(pID, out resourceContainer) ? 0 : resourceContainer.amount;
  }

  public static void empty(this ActorBag pBag)
  {
    if (!pBag.isEmpty())
      pBag.dict.Clear();
    pBag = (ActorBag) null;
  }

  public static string getItemIDToRender(this ActorBag pBag)
  {
    if (pBag.hasResources())
    {
      if (!string.IsNullOrEmpty(pBag.last_item_to_render))
        return pBag.last_item_to_render;
      using (Dictionary<string, ResourceContainer>.ValueCollection.Enumerator enumerator = pBag.getResources().Values.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          ResourceContainer current = enumerator.Current;
          pBag.last_item_to_render = current.id;
          return pBag.last_item_to_render;
        }
      }
    }
    return string.Empty;
  }

  public static int countResources(this ActorBag pBag) => pBag.isEmpty() ? 0 : pBag.dict.Count;
}
