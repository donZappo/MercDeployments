using BattleTech;
using BattleTech.Framework;
using System.Collections.Generic;
using System;

namespace MercDeployments {
    public class Settings {
        public float MissionChancePerDay = 0.1f;
        public float DeploymentSalaryMultiplier = 4f;
        public int MaxMonth = 6;
        public int DeploymentBreakRepCost = -30;
        public int DeploymentBreakMRBRepCost = -50;
        public float BonusPercentage = 0.05f;
        public float MaxDeploymentPercentage = 1f;
        public float MaxDeploymentsPerMonth = 6;
        public double DCV_Low = -0.25;
        public double DCV_Medium = -0.18;
        public double DCV_High = 0.05;
        public int MaxContracts_Low = -5;
        public int MaxContracts_Medium = -3;
        public int MaxContracts_High = 1;
        public int PayMultiplier_Low = -4;
        public int PayMultiplier_Medium = -2;
        public int PayMultiplier_High = 1;
        public double PercentChangeLow = 0;
        public double PercentChangeMedium = 0.0025;
        public double PercentChangeHigh = 0.0050;
        public int PilotLimitLow = 7;
        public int PilotLimitMedium = 13;
        public int PilotLimitHigh = 17;
        public double DCV_Adjustment = 0.01;
        public double MaxContractsAdjustment = 0.25;
        public double PayMultiplierAdjustment = 0.25;

    }
    
    public static class Fields {
            
        public static bool Deployment = false;
        public static Faction DeploymentEmployer = Faction.INVALID_UNSET;
        public static Faction DeploymentTarget = Faction.INVALID_UNSET;
        public static int DeploymentDifficulty = 1;
        public static float DeploymentNegotiatedSalvage = 1;
        public static float DeploymentNegotiatedPayment = 0;
        public static int DeploymentSalary = 100000;
        public static int DeploymentSalvage = 0;
        public static int DeploymentLenght = 0;
        public static int DeploymentRemainingDays = 0;
        public static int MissionsDoneCurrentMonth = 0;
        public static int DaysSinceLastMission = 0;


        public static Dictionary<string, Contract> DeploymentContracts = new Dictionary<string, Contract>();

        public static Dictionary<string, int> AlreadyRaised = new Dictionary<string, int>();
        public static bool InvertCBills = false;
        public static bool SkipPreparePostfix = false;
        public static bool PaymentCall = false;
        public static WorkOrderEntry_Notification TimeLineEntry = null;
        public static WorkOrderEntry_Notification PaymentTime = null;
    }

    public struct PotentialContract {
        // Token: 0x040089A4 RID: 35236
        public ContractOverride contractOverride;

        // Token: 0x040089A5 RID: 35237
        public Faction employer;

        // Token: 0x040089A6 RID: 35238
        public Faction target;

        // Token: 0x040089A7 RID: 35239
        public int difficulty;
    }
}