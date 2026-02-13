// Decompiled with JetBrains decompiler
// Type: CustomTextureAtlas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CustomTextureAtlas
{
  private static int width = 1202;
  private static int height = 2021;

  public static bool filesExists() => true;

  public static void createUnityBin()
  {
  }

  private static void save(string pData)
  {
  }

  internal static void delete(string pTexture)
  {
  }

  public static string createTextureID(string pString)
  {
    string pID = CustomTextureAtlas.width.ToString() + CustomTextureAtlas.height.ToString();
    return Toolbox.textureID(pString, pID);
  }
}
