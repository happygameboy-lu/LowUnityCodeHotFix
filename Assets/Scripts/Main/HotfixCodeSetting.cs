
using UnityEngine;

public class HotfixCodeSetting
{
   [Header("开启HybridCLR插件")]
   public bool enable = true;
   //实际热更+预留
   [Header("热更新dlls")]
   public string[] hotUpdateAssemblies;
   [Header("补充元数据AOT dlls")]
   public string[] patchAOTAssemblies;
   
}
