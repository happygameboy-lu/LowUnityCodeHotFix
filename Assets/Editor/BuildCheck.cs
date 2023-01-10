using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildCheck : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder { get; }
    //构建之前
    public void OnPreprocessBuild(BuildReport report)
    {
        
    }
    //构建之后
    public void OnPostprocessBuild(BuildReport report)
    {
        
    }
}
