/******************************************************************************/
/*
  Project   - MudBun
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using UnityEditor;
using UnityEngine;

namespace MudBun
{
  public class MudBunConfigMenu
  {
    //[MenuItem("Window/MudBun/Update Compatibility")]
    public static void UpdateCompatibility()
    {
      #if !MUDBUN_DEV
      CompatibilityManager.CheckCompatibility();
      #endif
    }

    //[MenuItem("Window/MudBun/Configure MudBun")]
    public static void SelectConfigFile()
    {
      var config = Resources.Load("MudBun Config");
      if (config == null)
        return;

      Selection.activeObject = config;
    }

    [MenuItem("Window/MudBun/Quick Creation Panel")]
    public static void OpenQuickCreationWindow()
    {
      QuickCreationWindow.Open();
    }

    //[MenuItem("Window/MudBun/About MudBun")]
    public static void OpenAboutWindow()
    {
      // TODO
    }
  }
}