/******************************************************************************/
/*
  Project   - MudBun
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager.Requests;
#endif

using UnityEngine;

namespace MudBun
{
#if UNITY_EDITOR
  //[InitializeOnLoad]
#endif
  public class CompatibilityManager
  {
    static CompatibilityManager()
    {
      //TryCheckCompatibility();
    }

    public static void TryCheckCompatibility()
    {
#if UNITY_EDITOR
      if (Application.isPlaying)
        return;

#if !MUDBUN_DEV
      var config = GetConfig();
      if (config == null)
      {
        Debug.LogWarning("MudBun: Config file not found.");
        return;
      }

      if (!config.CheckCompatibility)
        return;

      LoadPackageList();
#endif
#endif
    }

    public static void EndCompatibilityCheck()
    {
#if UNITY_EDITOR
      if (Application.isPlaying)
        return;

      if (s_loadingPackageList)
      {
        EditorApplication.update -= UpdateLoadPackageList;
        s_loadingPackageList = false;
      }
#endif
    }

#if UNITY_EDITOR
    private static readonly string HDRPPackageId = "com.unity.render-pipelines.high-definition";

    private enum SRPVersion
    {
      SRP_3_0_0  = 030000, 
      SRP_3_1_0  = 030100, 
      SRP_3_3_0  = 030300, 
      SRP_4_1_0  = 040100, 
      SRP_4_2_0  = 040200, 
      SRP_4_3_0  = 040300, 
      SRP_4_6_0  = 040600, 
      SRP_4_8_0  = 040800, 
      SRP_4_9_0  = 040900, 
      SRP_4_10_0 = 041000, 
      SRP_5_7_2  = 050702, 
      SRP_5_8_2  = 050802, 
      SRP_5_9_0  = 050900, 
      SRP_5_10_0 = 051000, 
      SRP_5_13_0 = 051300, 
      SRP_5_16_1 = 051601, 
      SRP_6_9_0  = 060900, 
      SRP_6_9_1  = 060901, 
      SRP_6_9_2  = 060902, 
      SRP_7_0_1  = 070001, 
      SRP_7_1_1  = 070101, 
      SRP_7_1_2  = 070102, 
      SRP_7_1_5  = 070105, 
      SRP_7_1_6  = 070106, 
      SRP_7_1_7  = 070107, 
      SRP_7_1_8  = 070108, 
      SRP_7_2_0  = 070200, 
      SRP_7_2_1  = 070201, 
      SRP_7_3_1  = 070301, 
      SRP_7_4_1  = 070401, 
      SRP_7_4_2  = 070402, 
      SRP_7_4_3  = 070403, 
      SRP_7_5_1  = 070501, 
      SRP_7_5_2  = 070502, 
      SRP_8_2_0  = 080200, 
      SRP_8_3_1  = 080301, 
      SRP_9_0_0  = 090000, 
      SRP_10_0_0 = 100000, 
      SRP_10_1_0 = 100100, 
      SRP_10_2_2 = 100202, 
      SRP_RECENT = 999999,
    }

    private static Dictionary<string, SRPVersion> s_srpVersionTable = new Dictionary<string, SRPVersion>()
    {
      { "3.0.0-preview",     SRPVersion.SRP_3_0_0  }, 
      { "3.1.0-preview",     SRPVersion.SRP_3_1_0  }, 
      { "3.3.0-preview",     SRPVersion.SRP_3_3_0  }, 
      { "4.1.0-preview",     SRPVersion.SRP_4_1_0  }, 
      { "4.2.0-preview",     SRPVersion.SRP_4_2_0  }, 
      { "4.3.0-preview",     SRPVersion.SRP_4_3_0  }, 
      { "4.6.0-preview",     SRPVersion.SRP_4_6_0  }, 
      { "4.8.0-preview",     SRPVersion.SRP_4_8_0  }, 
      { "4.9.0-preview",     SRPVersion.SRP_4_9_0  }, 
      { "4.10.0-preview",    SRPVersion.SRP_4_10_0 }, 
      { "5.7.2-preview",     SRPVersion.SRP_5_7_2  }, 
      { "5.7.2",             SRPVersion.SRP_5_7_2  }, 
      { "5.8.2-preview",     SRPVersion.SRP_5_8_2  }, 
      { "5.8.2",             SRPVersion.SRP_5_8_2  }, 
      { "5.9.0-preview",     SRPVersion.SRP_5_9_0  }, 
      { "5.9.0",             SRPVersion.SRP_5_9_0  }, 
      { "5.10.0-preview",    SRPVersion.SRP_5_10_0 }, 
      { "5.10.0",            SRPVersion.SRP_5_10_0 }, 
      { "5.13.0-preview",    SRPVersion.SRP_5_13_0 }, 
      { "5.13.0",            SRPVersion.SRP_5_13_0 }, 
      { "5.16.1-preview",    SRPVersion.SRP_5_16_1 }, 
      { "5.16.1",            SRPVersion.SRP_5_16_1 }, 
      { "6.9.0",             SRPVersion.SRP_6_9_0  }, 
      { "6.9.0-preview",     SRPVersion.SRP_6_9_0  }, 
      { "6.9.1",             SRPVersion.SRP_6_9_1  }, 
      { "6.9.1-preview",     SRPVersion.SRP_6_9_1  }, 
      { "6.9.2",             SRPVersion.SRP_6_9_2  }, 
      { "6.9.2-preview",     SRPVersion.SRP_6_9_2  }, 
      { "7.0.1",             SRPVersion.SRP_7_0_1  }, 
      { "7.0.1-preview",     SRPVersion.SRP_7_0_1  }, 
      { "7.1.1",             SRPVersion.SRP_7_1_1  }, 
      { "7.1.1-preview",     SRPVersion.SRP_7_1_1  }, 
      { "7.1.2",             SRPVersion.SRP_7_1_2  }, 
      { "7.1.2-preview",     SRPVersion.SRP_7_1_2  }, 
      { "7.1.5",             SRPVersion.SRP_7_1_5  }, 
      { "7.1.5-preview",     SRPVersion.SRP_7_1_5  }, 
      { "7.1.6",             SRPVersion.SRP_7_1_6  }, 
      { "7.1.6-preview",     SRPVersion.SRP_7_1_6  }, 
      { "7.1.7",             SRPVersion.SRP_7_1_7  }, 
      { "7.1.7-preview",     SRPVersion.SRP_7_1_7  }, 
      { "7.1.8",             SRPVersion.SRP_7_1_8  }, 
      { "7.1.8-preview",     SRPVersion.SRP_7_1_8  }, 
      { "7.2.0",             SRPVersion.SRP_7_2_0  }, 
      { "7.2.0-preview",     SRPVersion.SRP_7_2_0  }, 
      { "7.2.1",             SRPVersion.SRP_7_2_1  }, 
      { "7.2.1-preview",     SRPVersion.SRP_7_2_1  }, 
      { "7.3.1",             SRPVersion.SRP_7_3_1  }, 
      { "7.3.1-preview",     SRPVersion.SRP_7_3_1  }, 
      { "7.4.1",             SRPVersion.SRP_7_4_1  }, 
      { "7.4.1-preview",     SRPVersion.SRP_7_4_1  }, 
      { "7.4.2",             SRPVersion.SRP_7_4_2  }, 
      { "7.4.2-preview",     SRPVersion.SRP_7_4_2  }, 
      { "7.4.3",             SRPVersion.SRP_7_4_3  }, 
      { "7.4.3-preview",     SRPVersion.SRP_7_4_3  }, 
      { "7.5.1",             SRPVersion.SRP_7_5_1  }, 
      { "7.5.1-preview",     SRPVersion.SRP_7_5_1  }, 
      { "7.5.2",             SRPVersion.SRP_7_5_2  }, 
      { "7.5.2-preview",     SRPVersion.SRP_7_5_2  }, 
      { "8.2.0",             SRPVersion.SRP_8_2_0  }, 
      { "8.2.0-preview",     SRPVersion.SRP_8_2_0  }, 
      { "8.3.1",             SRPVersion.SRP_8_3_1  }, 
      { "8.3.1-preview",     SRPVersion.SRP_8_3_1  }, 
      { "9.0.0",             SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.13",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.14",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.33",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.35",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.54",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.55",  SRPVersion.SRP_9_0_0  }, 
      { "9.0.0-preview.71",  SRPVersion.SRP_9_0_0  }, 
      { "10.0.0-preview.26", SRPVersion.SRP_10_0_0 }, 
      { "10.0.0-preview.27", SRPVersion.SRP_10_0_0 }, 
      { "10.1.0",            SRPVersion.SRP_10_1_0 }, 
      { "10.2.2",            SRPVersion.SRP_10_2_2 }, 
    };

    private static Dictionary<SRPVersion, string> s_hdrpPackageTable = new Dictionary<SRPVersion, string>()
    {
      { SRPVersion.SRP_10_0_0, "3ff13cd87f98e6f4cae38dac0e0f632b" }, 
      { SRPVersion.SRP_10_1_0, "3ff13cd87f98e6f4cae38dac0e0f632b" }, 
      { SRPVersion.SRP_10_2_2, "3ff13cd87f98e6f4cae38dac0e0f632b" }, 
      { SRPVersion.SRP_RECENT, "3ff13cd87f98e6f4cae38dac0e0f632b" }, 
    };

    private static bool s_loadingPackageList = false;
    private static ListRequest s_packageListRequest;

    private static MudBunConfig GetConfig()
    {
      return (MudBunConfig) Resources.Load("MudBun Config");
    }

    public static void CheckCompatibility()
    {
      LoadPackageList();
    }

    private static void LoadPackageList()
    {
      if (!s_loadingPackageList)
      {
        EditorApplication.update += UpdateLoadPackageList;
        s_loadingPackageList = true;
      }
    }

    private static void UpdateLoadPackageList()
    {
      if (s_packageListRequest == null)
      {
        s_packageListRequest = UnityEditor.PackageManager.Client.List(true);
      }

      if (s_packageListRequest == null)
      {
        EditorApplication.update -= UpdateLoadPackageList;
        s_loadingPackageList = false;
        return;
      }

      if (!s_packageListRequest.IsCompleted)
        return;

      EditorApplication.update -= UpdateLoadPackageList;
      s_loadingPackageList = false;

      if (!Application.isPlaying)
      {
        DoCompatibilityCheck();
      }

      var config = GetConfig();
      config.CheckCompatibility = false;
      AssetDatabase.SaveAssets();
    }

    private static bool DoCompatibilityCheck()
    {
      SRPVersion version = SRPVersion.SRP_RECENT;
      string versionString = "";
      string srpPackageId = "";
      string packageGuid = "";
      string packagePath = "";

      foreach (var info in s_packageListRequest.Result)
      {
        if (info.name.Equals(HDRPPackageId))
        {
          if (s_srpVersionTable.ContainsKey(info.version))
          {
            versionString = info.version;
            version = s_srpVersionTable[versionString];
            srpPackageId = HDRPPackageId;
            packageGuid = s_hdrpPackageTable[version];
            packagePath = AssetDatabase.GUIDToAssetPath(packageGuid);
            break;
          }
        }
      }

      if (!File.Exists(packagePath))
      {
        if (!packageGuid.Equals(""))
        {
          Debug.LogWarning($"MudBun: Compatibility package not found for \"{srpPackageId}\" version {versionString}.\nDid you forget to import the MudBun/Compatibility folder?");
        }

        return true;
      }

      AssetDatabase.ImportPackage(packagePath, false);
      Debug.Log($"MudBun: Updated compatibility to \"{srpPackageId}\" version {versionString}.");

      MudRendererBase.ReloadAllShaders();

      return true;
    }
#endif
  }
}

