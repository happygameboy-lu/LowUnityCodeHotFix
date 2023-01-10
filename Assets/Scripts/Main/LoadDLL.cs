using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using HybridCLR;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.Events;

public class LoadDLL : MonoBehaviour
{
    private HotfixCodeSetting hotFixSetting;
    async UniTask Start()
    {
#if !UNITY_EDITOR
                        string streampath = Path.Combine(Application.persistentDataPath,"StreamingAssets");
                        if (!Directory.Exists(streampath))
                        {
                            Directory.CreateDirectory(streampath);
                        }
#endif
        var temPath = GetPath("HotfixCodeSetting.json");
        UnityWebRequest www = UnityWebRequest.Get(temPath);
        await www.SendWebRequest();
        
        if (!String.IsNullOrEmpty(www.error))
        {
            Debug.LogError($"load HotfixCodeSetting error={www.error}");
        }
        else
        {
            var settingStr = www.downloadHandler.text;
            try
            {
                hotFixSetting = JsonConvert.DeserializeObject<HotfixCodeSetting>(settingStr);
                if (hotFixSetting!=null)
                {
                    if (hotFixSetting.enable)
                    {
#if !UNITY_EDITOR
                        for (int i = 0; i < hotFixSetting.hotUpdateAssemblies.Length; i++)
                        {
                            temPath=GetPath($"{hotFixSetting.hotUpdateAssemblies[i]}.bytes");
                            if (!temPath.Equals($"{hotFixSetting.hotUpdateAssemblies[i]}.bytes"))
                            {
                                www=UnityWebRequest.Get(temPath);
                                await www.SendWebRequest();
                                if (!String.IsNullOrEmpty(www.error))
                                {
                                    Debug.LogError(www.error);
                                }
                                await UniTask.WaitUntil(() => String.IsNullOrEmpty(www.error)&&www.isDone);
                               
                                Debug.Log($"Reflection url={www.url}");
                                if (www.downloadHandler.data==null||www.downloadHandler.data.Length==0)
                                {
                                    Debug.LogError("没下载到热更");
                                }
                                System.Reflection.Assembly.Load(www.downloadHandler.data);
                                
                            }
                        }
#endif
                    }

                    LoadMetadataForAOTAssemblies(GameStart());
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"JsonConvert error:{e}");
            }
        }
        www.Dispose();
        www = null;
    }
    
    async UniTask LoadMetadataForAOTAssemblies(UniTask LoacCallBack)
    {
#if !UNITY_EDITOR
        if (hotFixSetting.enable)
        {
            // / 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            // / 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            UnityWebRequest www;
            for (int i = 0; i < hotFixSetting.patchAOTAssemblies.Length; i++)
            {
                string path = GetPath($"{hotFixSetting.patchAOTAssemblies[i]}.bytes");
                if (!path.Equals($"{hotFixSetting.patchAOTAssemblies[i]}.bytes"))
                {
                    www = UnityWebRequest.Get(path);
                    await www.SendWebRequest();
                    if (!String.IsNullOrEmpty(www.error))
                    {
                        Debug.LogError($"LoadMetadata error={www.error},path={path}");
                    }
                    else
                    {
                        LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(www.downloadHandler.data, mode);
                        Debug.Log($"LoadMetadataForAOTAssembly:{hotFixSetting.patchAOTAssemblies[i]}. mode:{mode} ret:{err}");
                    }
                }
            }
        }
#endif
        LoacCallBack.Forget();
    }

    async  UniTask GameStart()
    {
        string temPath = GetPath("GameEntry");
        var www=UnityWebRequestAssetBundle.GetAssetBundle(temPath);
        await www.SendWebRequest();
        if (!String.IsNullOrEmpty(www.error))
        {
            Debug.LogError($"load GameEntry error={www.error}");
        }
        else
        {
            AssetBundle abAssets=DownloadHandlerAssetBundle.GetContent(www);
            GameObject.Instantiate(abAssets.LoadAsset<GameObject>("GameEntry"));
        }
        www.Dispose();
        www = null;
    }

    private string GetPath(string relativePath)
    {
        string path = Path.Combine(Application.persistentDataPath,"StreamingAssets",relativePath);
        if (File.Exists(path))
        {
            return path;
        }

        path = Path.Combine(Application.streamingAssetsPath,relativePath);
        if (File.Exists(path))
        {
            return path;
        }
        return relativePath;
    }
}
