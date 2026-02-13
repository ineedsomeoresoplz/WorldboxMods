// Decompiled with JetBrains decompiler
// Type: BehTryToSocialize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehTryToSocialize : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    pActor.resetSocialize();
    Actor randomActorAround = this.getRandomActorAround(pActor);
    if (randomActorAround == null)
      return BehResult.Stop;
    pActor.beh_actor_target = (BaseSimObject) randomActorAround;
    if (pActor.canFallInLoveWith(randomActorAround))
      pActor.becomeLoversWith(randomActorAround);
    pActor.resetSocialize();
    randomActorAround.resetSocialize();
    return pActor.hasTelepathicLink() && randomActorAround.hasTelepathicLink() ? this.forceTask(pActor, "socialize_do_talk", false) : this.forceTask(pActor, "socialize_go_to_target", false);
  }

  private Actor getRandomActorAround(Actor pActor)
  {
    using (ListPool<Actor> listPool1 = new ListPool<Actor>(4))
    {
      using (ListPool<Actor> listPool2 = new ListPool<Actor>(4))
      {
        bool flag1 = pActor.subspecies.needOppositeSexTypeForReproduction();
        bool flag2 = pActor.hasCulture() && pActor.culture.hasTrait("animal_whisperers");
        int num = pActor.hasTelepathicLink() ? 1 : 0;
        if (num != 0)
          this.fillUnitsViaTelepathicLink(pActor, listPool1, listPool2);
        int pChunkRadius = 1;
        if (num != 0)
          pChunkRadius = 2;
        foreach (Actor actor in Finder.getUnitsFromChunk(pActor.current_tile, pChunkRadius, pRandom: true))
        {
          if (pActor.canTalkWith(actor))
          {
            if (pActor.isKingdomCiv())
            {
              if (actor.isKingdomMob())
              {
                if (!flag2)
                  continue;
              }
              else if (!actor.isKingdomCiv())
                ;
            }
            else if (!pActor.isSameSpecies(actor))
              continue;
            if (flag1 && pActor.canFallInLoveWith(actor))
            {
              listPool1.Add(actor);
              break;
            }
            listPool2.Add(actor);
            if (listPool2.Count > 3)
              break;
          }
        }
        if (listPool1.Count > 0)
          return listPool1.GetRandom<Actor>();
        return listPool2.Count > 0 ? listPool2.GetRandom<Actor>() : (Actor) null;
      }
    }
  }

  private void fillUnitsViaTelepathicLink(
    Actor pActor,
    ListPool<Actor> pBestTargets,
    ListPool<Actor> pNormalTargets)
  {
    if (pActor.hasFamily())
    {
      foreach (Actor unit in pActor.family.units)
      {
        if (pActor.canTalkWith(unit))
          pNormalTargets.Add(unit);
      }
    }
    foreach (Actor parent in pActor.getParents())
    {
      if (pActor.canTalkWith(parent))
        pBestTargets.Add(parent);
    }
  }
}
