using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

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
    typeof(Waypoint),
    nameof(Waypoint.EncounterWaypoint)
)]
internal static class MinimapPatch
{
    [HarmonyPrefix]
    private static void Prefix()
    {
        // Plugin.Logger.LogDebug("Minimap.UpdateMinimap Started");
    }

    [HarmonyPostfix]
    private static void Postfix(ref Waypoint __result)
    {
        // Plugin.Logger.LogDebug($"NearbyIconDistance before patch: {__result.NearbyIconDistance}");
        if(__result.NearbyIconDistance > 0.0)
            __result.UseNearbyIcon = true;
        // Plugin.Logger.LogDebug($"NearbyIconDistance after patch: {__result.NearbyIconDistance}");

        // Plugin.Logger.LogDebug("Waypoint.EncounterWaypoint patched\n");
        // Plugin.Logger.LogDebug("");
    }
}
