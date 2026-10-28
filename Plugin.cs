using BepInEx;
using HarmonyLib;

namespace MyMycoModRoot;

[MycoMod(null, ModFlags.IsClientSide)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private Harmony _harmony;

    private void Awake()
    {
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
    [HarmonyPostfix]
    private static void Postfix(ref Waypoint __result)
    {
        if(__result.NearbyIconDistance > 0.0)
            __result.UseNearbyIcon = true;
    }
}
