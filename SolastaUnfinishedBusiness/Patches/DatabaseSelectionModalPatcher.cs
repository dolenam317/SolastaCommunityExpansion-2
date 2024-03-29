﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using SolastaUnfinishedBusiness.Api.Infrastructure;

namespace SolastaUnfinishedBusiness.Patches;

public static class DatabaseSelectionModalPatcher
{
    [HarmonyPatch(typeof(DatabaseSelectionModal), "BuildMonsters")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class BuildMonsters_Patch
    {
        public static bool Prefix(DatabaseSelectionModal __instance)
        {
            //PATCH: unleashes all NPC definitions to be used as monsters (DMP)
            if (!Main.Settings.UnleashNpcAsEnemy)
            {
                return true;
            }

            __instance.allMonsters.SetRange(DatabaseRepository.GetDatabase<MonsterDefinition>()
                .Where(x => !x.GuiPresentation.Hidden)
                .OrderBy(d => d.dungeonMakerPresence + Gui.Localize(d.GuiPresentation.Title)));

            var service = ServiceRepository.GetService<IGamingPlatformService>();

            for (var index = __instance.allMonsters.Count - 1; index >= 0; --index)
            {
                if (!service.IsContentPackAvailable(__instance.allMonsters[index].ContentPack))
                {
                    __instance.allMonsters.RemoveAt(index);
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(DatabaseSelectionModal), "BuildNpcs")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class BuildNpcs_Patch
    {
        public static bool Prefix(DatabaseSelectionModal __instance)
        {
            //PATCH: unleashes all monster definitions to be used as NPCs (DMP)
            if (!Main.Settings.UnleashEnemyAsNpc)
            {
                return true;
            }

            __instance.allNpcs.SetRange(DatabaseRepository.GetDatabase<MonsterDefinition>()
                .Where(x => x.dungeonMakerPresence
                    is MonsterDefinition.DungeonMaker.Monster
                    or MonsterDefinition.DungeonMaker.NPC)
                .OrderBy(d => d.dungeonMakerPresence + Gui.Localize(d.GuiPresentation.Title)));

            var service = ServiceRepository.GetService<IGamingPlatformService>();

            for (var index = __instance.allNpcs.Count - 1; index >= 0; --index)
            {
                if (!service.IsContentPackAvailable(__instance.allNpcs[index].ContentPack))
                {
                    __instance.allNpcs.RemoveAt(index);
                }
            }

            return false;
        }
    }
}
