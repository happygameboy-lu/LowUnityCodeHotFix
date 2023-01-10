using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager
{
   public static string GetUrl(string filepath)
   {
      string path = filepath;
      if (Application.platform==RuntimePlatform.WindowsEditor)
      {
         
      }else if (Application.platform==RuntimePlatform.WindowsPlayer)
      {
         
      }else if (Application.platform==RuntimePlatform.Android)
      {
         
      }
      else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor)
      {
         
      }
      return path;
   }
}
