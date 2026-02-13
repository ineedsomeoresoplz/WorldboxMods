// Decompiled with JetBrains decompiler
// Type: RequestHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
[ObfuscateLiterals]
public class RequestHelper
{
  private static string _salt = "";

  public static string salt
  {
    get
    {
      if (RequestHelper._salt == "")
      {
        try
        {
          RequestHelper._salt = RequestHelper.SHA256CheckSum(typeof (RequestHelper).Assembly.Location);
        }
        catch (Exception ex)
        {
          RequestHelper._salt = "err";
        }
      }
      return RequestHelper._salt;
    }
  }

  public static string SHA256CheckSum(string filePath)
  {
    using (SHA256 shA256 = SHA256.Create())
    {
      using (BufferedStream inputStream = new BufferedStream((Stream) File.OpenRead(filePath), 1200000))
        return BitConverter.ToString(shA256.ComputeHash((Stream) inputStream)).Replace("-", string.Empty).ToLower();
    }
  }
}
