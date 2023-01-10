using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;


public class MenuItems
{
    const string m_scriptableMenu = "Assets/Create/脚本对象/";
    [UnityEditor.InitializeOnLoadMethod]
    static void OnEditorUpdate()
    {
        
    }

    [MenuItem(m_scriptableMenu+"CreateHotfixCodeSetting")]
    private static void CreateHotfixCodeSetting()
    {
        HotfixCodeSetting hotfixSetting = new HotfixCodeSetting();
        hotfixSetting.enable = HybridCLR.Editor.SettingsUtil.HybridCLRSettings.enable;
        hotfixSetting.hotUpdateAssemblies = HybridCLR.Editor.SettingsUtil.PatchingHotUpdateAssemblyFiles.ToArray();
        hotfixSetting.patchAOTAssemblies = HybridCLR.Editor.SettingsUtil.HybridCLRSettings.patchAOTAssemblies.ToList().Select(dll => dll + ".dll").ToArray();
        string jsonStr = JsonConvert.SerializeObject(hotfixSetting);
        StreamWriter sw = new StreamWriter($"{Application.streamingAssetsPath}/HotfixCodeSetting.json");
        sw.Write(jsonStr);
        sw.Close();
        AssetDatabase.Refresh();
    }
}

