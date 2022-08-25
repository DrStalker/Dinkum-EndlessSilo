using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using BepInEx;
//using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;


namespace EndlessSilo;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
        private static Harmony _harmony;
    
    private void Awake()
    {
        _harmony = Harmony.CreateAndPatchAll(typeof(NepEndlessSiloPatch));
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }
}

[HarmonyPatch]
internal class NepEndlessSiloPatch
{
    //At the start of SprinklerTile.waterTiles() if the object is a Silo set the content level to 200
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SprinklerTile), nameof(SprinklerTile.waterTiles))]
    private static void NepEndlessSilo(SprinklerTile __instance, int xPos, int yPos, List<int[]> waterTanks)
    {
        if (__instance.isSilo)
        {
            WorldManager.manageWorld.onTileStatusMap[xPos, yPos] = 200;
        }


    }

}
