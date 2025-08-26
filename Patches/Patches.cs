using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Stations;
using LBoL.Presentation;
using System;
using System.Collections.Generic;
using System.Text;
using static Clownpiece.Boss.ClownpieceBossSelectorDef;

namespace Clownpiece.Patches
{
    [HarmonyPatch]
    class CustomGameEventManager
    {
        [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.PlayBossBgm))]
        internal class AudioManager_PlayBossBgm_Patch_Clownpiece
        {
            private static bool Prefix(AudioManager __instance)
            {
                var station = Singleton<GameMaster>.Instance?.CurrentGameRun?.CurrentStation;
                if (station is BossStation bossStation)
                {
                    if (bossStation.EnemyGroup?.Id == nameof(ClownpieceBossSelector))
                    {
                        AudioManager.PlayInLayer1("Clownpiece");
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
