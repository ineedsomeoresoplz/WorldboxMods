// Decompiled with JetBrains decompiler
// Type: EffectAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public delegate BaseEffect EffectAction(
  BaseEffect pEffect,
  WorldTile pTile = null,
  string pParam1 = null,
  string pParam2 = null,
  float pFloatParam1 = 0.0f,
  Actor pActor = null);
