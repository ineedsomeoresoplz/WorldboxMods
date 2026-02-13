// Decompiled with JetBrains decompiler
// Type: MapUploader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using System;

#nullable disable
public static class MapUploader
{
  public static Promise<string> uploadMap()
  {
    string str = DateTime.UtcNow.ToString("yyyyMMdd");
    return S3Manager.instance.uploadFileToAWS3($"wbox/{str.ToString()}/{Auth.userId}_{Guid.NewGuid().ToString()}.wbox", MapUploader.getMapData());
  }

  private static byte[] getMapData()
  {
    return SaveManager.getMapFromPath(SaveManager.currentSavePath).toZip();
  }
}
