using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAndroidShock : MonoBehaviour
{
    // Start is called before the first frame update
     void Start()
    {
        // 实例化 UnityPlayer 类
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        // 实例化 Android 继承的 UnityPlayerActivity 的 Activity
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        // 实例化你的 Android 类
        AndroidJavaObject vibratorUtil = new AndroidJavaObject("com.utils.unityshock.VibratorUtil"); // 替换为你的包名和类名

        // 调用 Android 方法
        long[] shock = new long[] { 0, 150 };
        vibratorUtil.Call("startVibrator",currentActivity , shock, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
