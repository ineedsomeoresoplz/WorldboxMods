// Decompiled with JetBrains decompiler
// Type: S3Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using RSG;
using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

#nullable disable
public class S3Manager : MonoBehaviour
{
  public static S3Manager instance;
  private const string ABN = "Js23DGKu7RMNik4XECoQVkNFIW//dsZNcfyKb49RlFU";
  private const string AAK = "VvrCEe1TcUBvQeiSelndpl1Plc4FoMxddSglHA2Fe0M";
  private const string ASK = "WVbbIlYTAH37Glxvl1MSDpKPffhczwdbi5FRgkSs8mkLEuLzE6YCiouHH71vVgLS";
  private string _abnn;
  private IAmazonS3 _s3_client;

  private IAmazonS3 _client
  {
    get
    {
      if (this._s3_client == null)
      {
        try
        {
          SAES.SAES saes = new SAES.SAES();
          this._abnn = saes.ToString("Js23DGKu7RMNik4XECoQVkNFIW//dsZNcfyKb49RlFU");
          this._s3_client = (IAmazonS3) new AmazonS3Client(saes.ToString("VvrCEe1TcUBvQeiSelndpl1Plc4FoMxddSglHA2Fe0M"), saes.ToString("WVbbIlYTAH37Glxvl1MSDpKPffhczwdbi5FRgkSs8mkLEuLzE6YCiouHH71vVgLS"), RegionEndpoint.USEast2);
          saes.init(false);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) "s3 manager not working");
          Debug.LogError((object) ex.Message);
        }
      }
      return this._s3_client;
    }
  }

  private void Start()
  {
    S3Manager.instance = this;
    Config.upload_available = false;
  }

  public Promise<string> uploadFileToAWS3(string pFileName, byte[] pFileRawBytes)
  {
    Promise<string> awS3 = new Promise<string>();
    try
    {
      MemoryStream memoryStream = new MemoryStream(pFileRawBytes);
      PutObjectRequest putObjectRequest = new PutObjectRequest()
      {
        BucketName = this._abnn,
        Key = pFileName,
        InputStream = (Stream) memoryStream,
        CannedACL = S3CannedACL.Private
      };
      if (((AmazonWebServiceResponse) this._client.PutObjectAsync(putObjectRequest, new CancellationToken()).WaitAndUnwrapException<PutObjectResponse>()).HttpStatusCode == HttpStatusCode.OK)
        awS3.Resolve(putObjectRequest.Key);
      else
        awS3.Reject(new Exception("Error when uploading!"));
    }
    catch (WebException ex)
    {
      using (Stream responseStream = ex.Response.GetResponseStream())
      {
        using (StreamReader streamReader = new StreamReader(responseStream))
          Debug.Log((object) streamReader.ReadToEnd());
      }
      awS3.Reject((Exception) ex);
    }
    catch (Exception ex)
    {
      awS3.Reject(ex);
    }
    return awS3;
  }
}
