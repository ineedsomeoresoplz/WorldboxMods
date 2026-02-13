// Decompiled with JetBrains decompiler
// Type: ICoreObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public interface ICoreObject
{
  string name { get; }

  long getID();

  int getAge();

  bool isAlive();

  bool isFavorite();

  void switchFavorite();
}
