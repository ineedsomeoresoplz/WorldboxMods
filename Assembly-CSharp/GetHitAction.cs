// Decompiled with JetBrains decompiler
// Type: GetHitAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public delegate bool GetHitAction(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null);
