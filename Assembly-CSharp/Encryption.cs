// Decompiled with JetBrains decompiler
// Type: Encryption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
public class Encryption
{
  private const string INIT_VECTOR = "sayHiIfUReadThis";
  private const int KEY_SIZE = 256 /*0x0100*/;

  public static string EncryptString(string pPlainText, string pPassPhrase)
  {
    byte[] bytes1 = Encoding.UTF8.GetBytes("sayHiIfUReadThis");
    byte[] bytes2 = Encoding.UTF8.GetBytes(pPlainText);
    using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(pPassPhrase, (byte[]) null))
    {
      byte[] bytes3 = passwordDeriveBytes.GetBytes(32 /*0x20*/);
      RijndaelManaged rijndaelManaged1 = new RijndaelManaged();
      rijndaelManaged1.Mode = CipherMode.CBC;
      using (RijndaelManaged rijndaelManaged2 = rijndaelManaged1)
      {
        using (ICryptoTransform encryptor = rijndaelManaged2.CreateEncryptor(bytes3, bytes1))
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
            {
              cryptoStream.Write(bytes2, 0, bytes2.Length);
              cryptoStream.FlushFinalBlock();
              return Convert.ToBase64String(memoryStream.ToArray());
            }
          }
        }
      }
    }
  }

  public static string DecryptString(string pCipherText, string pPassPhrase)
  {
    byte[] bytes1 = Encoding.UTF8.GetBytes("sayHiIfUReadThis");
    byte[] buffer = Convert.FromBase64String(pCipherText);
    using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(pPassPhrase, (byte[]) null))
    {
      byte[] bytes2 = passwordDeriveBytes.GetBytes(32 /*0x20*/);
      RijndaelManaged rijndaelManaged1 = new RijndaelManaged();
      rijndaelManaged1.Mode = CipherMode.CBC;
      using (RijndaelManaged rijndaelManaged2 = rijndaelManaged1)
      {
        using (ICryptoTransform decryptor = rijndaelManaged2.CreateDecryptor(bytes2, bytes1))
        {
          using (MemoryStream memoryStream = new MemoryStream(buffer))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
            {
              byte[] numArray = new byte[buffer.Length];
              int count = cryptoStream.Read(numArray, 0, numArray.Length);
              return Encoding.UTF8.GetString(numArray, 0, count);
            }
          }
        }
      }
    }
  }
}
