using BepInEx;
using BepInEx.Logging;
using System;
using HarmonyLib;
using UnityEngine;

namespace MonMycoModRoot;

[MycoMod(null, ModFlags.IsClientSide)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;

    private Harmony _harmony;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();
        Logger.LogInfo("Harmony patches applied");
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}

[HarmonyPatch(
    typeof(UpgradePopup),
    nameof(UpgradePopup.Setup),
    new Type[]
    {
        typeof(PickupInfo),
        typeof(bool)
    }
)]
internal static class UpgradePopupPatch
{
    [HarmonyPrefix]
    private static void Prefix(ref PickupInfo info)
    {
        Plugin.Logger.LogDebug("UpgradePopup.Setup Started");
        
        info.name = $"[MODDED] {info.name}";

        if (info.rarity == Rarity.Standard)
            info.color = Color.lightPink;
    }
    
    [HarmonyPostfix]
    private static void Postfix()
    {
        Plugin.Logger.LogDebug("UpgradePopup.Setup finished");
    }
}