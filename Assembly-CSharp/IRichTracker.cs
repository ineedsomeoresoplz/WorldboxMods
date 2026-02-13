// Decompiled with JetBrains decompiler
// Type: IRichTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public interface IRichTracker
{
  void trackViewing(string pText);

  void trackWatching();

  void trackUsing(string pPower);

  void updateUsing(int pAmount, string pPower);

  void inspectKingdom(string pKingdom);

  void inspectVillage(string pVillage);

  void inspectUnit(string pUnit);

  void spectatingUnit(string pUnit);

  void trackActivity(string pText);
}
