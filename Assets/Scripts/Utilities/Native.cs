using UnityEngine;

namespace Utilities
{
    public class Native : MonoBehaviour {

        public static bool CheckAppIsInstalled(string bundle)
        {

#if UNITY_EDITOR
            return false;
#endif
#if UNITY_ANDROID
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        Debug.Log(" ********LaunchOtherApp ");
        AndroidJavaObject launchIntent = null;
        //if the app is installed, no errors. Else, doesn't get past next line
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundle);
            //        
            //        ca.Call("startActivity",launchIntent);
        }
        catch (Exception ex)
        {
            Debug.Log("exception" + ex.Message);
        }
        if (launchIntent == null)
            return false;
        return true;
#else
            return false;
#endif
        }

//     public static void RegisterReceiver(AppInstalledCallback callback)
//     {

// #if UNITY_EDITOR
//         return;
// #endif

//         Debug.Log(Application.platform);

//         AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//         AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
   
//         AndroidJavaObject intentFilter = new AndroidJavaObject("android.content.IntentFilter");
//         AndroidJavaClass intent = new AndroidJavaClass("android.content.Intent");

//         AndroidJavaObject br = new AndroidJavaObject("com.acorngames.nativereceiverplugin.AppInstallReceiver");
//         br.Call("setCallback", callback);

//         br.Call("register", ca.Call<AndroidJavaObject>("getApplicationContext"));

//         intentFilter.Call("addAction", intent.GetStatic<string>("ACTION_PACKAGE_ADDED"));
//         intentFilter.Call("addAction", intent.GetStatic<string>("ACTION_PACKAGE_INSTALL"));
//         intentFilter.Call("addDataScheme", "package");

//         //ca.Call("registerReceiver", br, intentFilter);
//     }

        public static void Vibrate(int level)
        {
            int ampl = 255;

            if(level == 1)
            {
                ampl = 50;
            }
            else if(level == 2)
            {
                ampl = 180;
            }

            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass vib = new AndroidJavaClass("com.acorngames.nativereceiverplugin.Vibration");
            vib.CallStatic("vibrate", ca.Call<AndroidJavaObject>("getApplicationContext"), ampl, 50);
        }

        public static int getSDKInt()
        {

#if UNITY_EDITOR
            return 0;
#endif
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
        }
    }
}
