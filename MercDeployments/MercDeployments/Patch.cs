using BattleTech;
using BattleTech.Framework;
using BattleTech.Save;
using BattleTech.Save.SaveGameStructure;
using BattleTech.UI;
using DG.Tweening;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MercDeployments {

    [HarmonyPatch(typeof(AAR_SalvageScreen), "OnCompleted")]
    public static class AAR_SalvageScreen_OnCompleted_Patch {
        static void Postfix(AAR_SalvageScreen __instance) {
            try {
                Settings settings = Helper.LoadSettings();
                System.Random rnd = new System.Random();
                Fields.NewArrival = false;
                Fields.DeploymentRemainingDays = rnd.Next(Fields.MinDays, Fields.MaxDays+1);
                }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "_OnFirstPlayInit")]
    public static class SimGameState_FirstPlayInit_Patch {
        static void Postfix(SimGameState __instance) {
            Fields.Deployment = false;
            Fields.NewArrival = true;
        }
    }

    [HarmonyPatch(typeof(TaskTimelineWidget), "RemoveEntry")]
    public static class TaskTimelineWidget_RemoveEntry_Patch {
        static bool Prefix(WorkOrderEntry entry) {
            try {
                if (!Fields.NewArrival && (entry.ID.Equals("Days Until Mission")) && Fields.DeploymentRemainingDays > 0) {
                    return false;
                }
                return true;
            }
            catch (Exception e) {
                Logger.LogError(e);
                return true;
            }
        }
    }


    [HarmonyPatch(typeof(TaskTimelineWidget), "RegenerateEntries")]
    public static class TaskTimelineWidget_RegenerateEntries_Patch {
        static void Postfix(TaskTimelineWidget __instance) {
            try {
                if (!Fields.NewArrival)
                {
                    Fields.TimeLineEntry = new WorkOrderEntry_Notification(WorkOrderType.NotificationGeneric, "Days Until Mission", "Days Until Mission");
                    Fields.TimeLineEntry.SetCost(Fields.DeploymentRemainingDays);
                    __instance.AddEntry(Fields.TimeLineEntry, false);
                }
                __instance.RefreshEntries();
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(GameInstance), "Load")]
    public static class GameInstance_Load_Patch {
        static void Prefix(GameInstanceSave save) {
            try {
                Helper.LoadState(save.InstanceGUID, save.SaveTime);
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(SGTravelManager), "DisplayEnteredOrbitPopup")]
    public static class Entered_Orbit_Patch {
        static void Postfix(SGTravelManager __instance) {
            try {
                Fields.NewArrival = true;
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }

    [HarmonyPatch(typeof(SGTimePlayPause), "ToggleTime")]
    public static class SGTimePlayPause_ToggleTime_Patch {
        static bool Prefix(SGTimePlayPause __instance) {
            try {
                if (!Fields.NewArrival && Fields.DeploymentRemainingDays == 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
                return true;
            }
        }
    }


    [HarmonyPatch(typeof(SimGameState), "OnDayPassed")]
    public static class SimGameState_OnDayPassed_Patch {
        static void Prefix(SimGameState __instance, int timeLapse) {
            Settings settings = Helper.LoadSettings();
            try {
                int num = (timeLapse <= 0) ? 1 : timeLapse;
                if (!Fields.NewArrival)
                {
                    Fields.DeploymentRemainingDays -= num;
                }

                if (__instance.CurSystem.SystemContracts.Count() == 0)
                {
                    Fields.NewArrival = true;
                    Fields.DeploymentRemainingDays = 0;
                }

                if (Fields.TimeLineEntry != null) {
                    Fields.TimeLineEntry.PayCost(num);
                    TaskManagementElement taskManagementElement4 = null;
                    TaskTimelineWidget timelineWidget = (TaskTimelineWidget)AccessTools.Field(typeof(SGRoomManager), "timelineWidget").GetValue(__instance.RoomManager);
                    Dictionary<WorkOrderEntry, TaskManagementElement> ActiveItems = (Dictionary<WorkOrderEntry, TaskManagementElement>)AccessTools.Field(typeof(TaskTimelineWidget), "ActiveItems").GetValue(timelineWidget);
                    if (ActiveItems.TryGetValue(Fields.TimeLineEntry, out taskManagementElement4)) {
                        taskManagementElement4.UpdateItem(0);
                    }
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }

        static void Postfix(SimGameState __instance) {
                if (!Fields.NewArrival) {
                    if (Fields.DeploymentRemainingDays <= 0) {
                        __instance.PauseTimer();
                        __instance.StopPlayMode();
                        SimGameInterruptManager interruptQueue = (SimGameInterruptManager)AccessTools.Field(typeof(SimGameState), "interruptQueue").GetValue(__instance);
                        interruptQueue.QueueGenericPopup("Forced Mission", "You must take a mission.");
                        __instance.RoomManager.RefreshTimeline();
                    }
                }
            }
        }
    [HarmonyPatch(typeof(SGNavigationScreen), "OnTravelCourseAccepted")]
    public static class SGNavigationScreen_OnTravelCourseAccepted_Patch
    {
        static bool Prefix(SGNavigationScreen __instance)
        {
            try
            {
                Fields.NewArrival = true;
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                return true;
            }
        }
    }
}