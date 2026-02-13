// Decompiled with JetBrains decompiler
// Type: DiplomacyRelation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DiplomacyRelation : CoreSystemObject<DiplomacyRelationData>
{
  public Kingdom kingdom1;
  public Kingdom kingdom2;
  internal KingdomOpinion opinion_k1;
  internal KingdomOpinion opinion_k2;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.diplomacy;

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  public KingdomOpinion getOpinion(Kingdom pMain, Kingdom pTarget)
  {
    this.recalculate(pMain, pTarget);
    return this.opinion_k1.target == pTarget ? this.opinion_k1 : this.opinion_k2;
  }

  private void recalculate(Kingdom k1 = null, Kingdom k2 = null)
  {
    if (this.opinion_k1 == null)
    {
      this.opinion_k1 = new KingdomOpinion(this.kingdom1, this.kingdom2);
      this.opinion_k2 = new KingdomOpinion(this.kingdom2, this.kingdom1);
    }
    try
    {
      this.opinion_k1.calculate(this.kingdom1, this.kingdom2, this);
      this.opinion_k2.calculate(this.kingdom2, this.kingdom1, this);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      Debug.LogError((object) this.data.id);
      Debug.LogError((object) this.data.kingdom1_id);
      Debug.LogError((object) this.data.kingdom2_id);
      Debug.LogError((object) ("kingdom1 " + (this.kingdom1 == null).ToString()));
      bool flag = this.kingdom2 == null;
      Debug.LogError((object) ("kingdom2 " + flag.ToString()));
      flag = this.kingdom1 == k1;
      Debug.LogError((object) flag.ToString());
      flag = this.kingdom2 == k2;
      Debug.LogError((object) flag.ToString());
      Debug.LogError((object) JsonUtility.ToJson((object) this.kingdom1));
      Debug.LogError((object) JsonUtility.ToJson((object) this.kingdom2));
      Debug.LogError((object) JsonUtility.ToJson((object) k1));
      Debug.LogError((object) JsonUtility.ToJson((object) k2));
      throw ex;
    }
  }

  public override void Dispose()
  {
    this.opinion_k1?.Dispose();
    this.opinion_k2?.Dispose();
    this.opinion_k1 = (KingdomOpinion) null;
    this.opinion_k2 = (KingdomOpinion) null;
    this.kingdom1 = (Kingdom) null;
    this.kingdom2 = (Kingdom) null;
    base.Dispose();
  }
}
