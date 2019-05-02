using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using HBS.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MercDeployments {

    public class SaveFields{
        public  bool Deployment = false;
        public  Faction DeploymentEmployer = Faction.INVALID_UNSET;
        public  Faction DeploymentTarget = Faction.INVALID_UNSET;
        public  int DeploymentDifficulty = 1;
        public  float DeploymentNegotiatedSalvage = 1;
        public  float DeploymentNegotiatedPayment = 0;
        public  int DeploymentSalary = 100000;
        public  int DeploymentSalvage = 0;
        public  int DeploymentLenght = 0;
        public  int DeploymentRemainingDays = 0;
        public  Dictionary<string,int> AlreadyRaised = new Dictionary<string,int>();
        public int MissionsDoneCurrentMonth = 0;
        public int DaysSinceLastMission = 0;
        public bool ResetContracts = false;
        public bool NewArrival = true;


        public SaveFields(bool Deployment, Faction DeploymentEmployer, 
                Faction DeploymentTarget, int DeploymentDifficulty, float DeploymentNegotiatedSalvage, 
                float DeploymentNegotiatedPayment, int DeploymentSalary, int DeploymentSalvage, Dictionary<string, int> AlreadyRaised, int DeploymentLenght, 
                int DeploymentRemainingDays, int MissionsDoneCurrentMonth, int DaysSinceLastMission, bool ResetContracts, bool NewArrival) {

            this.Deployment = Deployment;
            this.DeploymentEmployer = DeploymentEmployer;
            this.DeploymentTarget = DeploymentTarget;
            this.DeploymentDifficulty = DeploymentDifficulty;
            this.DeploymentNegotiatedSalvage = DeploymentNegotiatedSalvage;
            this.DeploymentNegotiatedPayment = DeploymentNegotiatedPayment;
            this.DeploymentSalary = DeploymentSalary;
            this.DeploymentSalvage = DeploymentSalvage;
            this.AlreadyRaised = AlreadyRaised;
            this.DeploymentLenght = DeploymentLenght;
            this.DeploymentRemainingDays = DeploymentRemainingDays;
            this.MissionsDoneCurrentMonth = MissionsDoneCurrentMonth;
            this.DaysSinceLastMission = DaysSinceLastMission;
            this.ResetContracts = ResetContracts;
            this.NewArrival = NewArrival;
        }
    }

    public class Helper {
        public static Settings LoadSettings() {
            try {
                using (StreamReader r = new StreamReader($"{ MercDeployments.ModDirectory}/settings.json")) {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
                return null;
            }
        }

        public static void SaveState(string instanceGUID, DateTime saveTime) {
            try {
                int unixTimestamp = (int)(saveTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string baseDirectory = Directory.GetParent(Directory.GetParent($"{ MercDeployments.ModDirectory}").FullName).FullName;
                string filePath = baseDirectory + $"/ModSaves/MercDeployments/" + instanceGUID + "-" + unixTimestamp + ".json";
                (new FileInfo(filePath)).Directory.Create();
                using (StreamWriter writer = new StreamWriter(filePath, true)) {
                    /*JsonSerializerSettings settings = new JsonSerializerSettings {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,one 
                        Formatting = Formatting.Indented
                    };*/
                    SaveFields fields = new SaveFields(Fields.Deployment, 
                        Fields.DeploymentEmployer, Fields.DeploymentTarget, Fields.DeploymentDifficulty,
                        Fields.DeploymentNegotiatedSalvage, Fields.DeploymentNegotiatedPayment, Fields.DeploymentSalary, 
                        Fields.DeploymentSalvage, Fields.AlreadyRaised, Fields.DeploymentLenght, Fields.DeploymentRemainingDays, Fields.MissionsDoneCurrentMonth, 
                        Fields.DaysSinceLastMission, Fields.ResetContracts, Fields.NewArrival);
                    string json = JsonConvert.SerializeObject(fields);
                    writer.Write(json);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
            }
        }

        public static void LoadState(string instanceGUID, DateTime saveTime) {
            try {
                int unixTimestamp = (int)(saveTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string baseDirectory = Directory.GetParent(Directory.GetParent($"{ MercDeployments.ModDirectory}").FullName).FullName;
                string filePath = baseDirectory + $"/ModSaves/MercDeployments/" + instanceGUID + "-" + unixTimestamp + ".json";
                if (File.Exists(filePath)) {
                    using (StreamReader r = new StreamReader(filePath)) {
                        string json = r.ReadToEnd();
                        SaveFields save = JsonConvert.DeserializeObject<SaveFields>(json);
                        Fields.Deployment = save.Deployment;
                        Fields.DeploymentEmployer = save.DeploymentEmployer;
                        Fields.DeploymentTarget = save.DeploymentTarget;
                        Fields.DeploymentDifficulty = save.DeploymentDifficulty;
                        Fields.DeploymentNegotiatedSalvage = save.DeploymentNegotiatedSalvage;
                        Fields.DeploymentNegotiatedPayment = save.DeploymentNegotiatedPayment;
                        Fields.DeploymentSalary = save.DeploymentSalary;
                        Fields.DeploymentSalvage = save.DeploymentSalvage;
                        Fields.AlreadyRaised = save.AlreadyRaised;
                        Fields.DeploymentLenght = save.DeploymentLenght;
                        Fields.DeploymentRemainingDays = save.DeploymentRemainingDays;
                        Fields.MissionsDoneCurrentMonth = save.MissionsDoneCurrentMonth;
                        Fields.DaysSinceLastMission = save.DaysSinceLastMission;
                        Fields.ResetContracts = save.ResetContracts;
                        Fields.NewArrival = save.NewArrival;
                    }
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
            }
        }
    }
}
