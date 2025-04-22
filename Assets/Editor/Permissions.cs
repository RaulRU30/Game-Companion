#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;


public static class Permissions
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
        if (target != BuildTarget.iOS)
            return;

        string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");

        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        PlistElementDict rootDict = plist.root;

        rootDict.SetString("NSLocalNetworkUsageDescription", "This app uses the local network to discover and communicate with a VR device.");

        PlistElementArray bonjourServices = rootDict.CreateArray("NSBonjourServices");
        bonjourServices.AddString("_udp."); // required to allow UDP traffic over local network

        PlistElementDict atsDict = rootDict.CreateDict("NSAppTransportSecurity");
        atsDict.SetBoolean("NSAllowsArbitraryLoads", true);
        atsDict.SetBoolean("NSAllowsLocalNetworking", true);

        // Save changes
        plist.WriteToFile(plistPath);

        UnityEngine.Debug.Log("✅ Info.plist successfully updated with Local Network and ATS permissions.");
    }
}

#endif
