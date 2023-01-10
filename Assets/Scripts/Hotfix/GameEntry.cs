using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using HybridCLR;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour
{
    public Text logText;
    // Start is called before the first frame update
    void Start()
    {
        if (logText==null)
        {
            logText = GameObject.Find("Text")?.GetComponent<Text>();
        }
        logText.text = "热更完成\n家里科技馆";
        Debug.LogError("开始程序22");
    }
}
