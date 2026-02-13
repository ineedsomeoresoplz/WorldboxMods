// Decompiled with JetBrains decompiler
// Type: PreviewUploader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using System;
using System.IO;

#nullable disable
public static class PreviewUploader
{
  public static Promise<string> uploadImagePreview()
  {
    string str = DateTime.UtcNow.ToString("yyyyMMdd");
    return S3Manager.instance.uploadFileToAWS3($"png/{str.ToString()}/{Auth.userId}_{Guid.NewGuid().ToString()}.png", PreviewUploader.getImagePreview());
  }

  private static byte[] getImagePreview() => File.ReadAllBytes(SaveManager.getPngSlotPath());
}
