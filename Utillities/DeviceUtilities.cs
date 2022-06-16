using System;
using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;

namespace CriticalConditionBackend.Utillities
{
    public static class DeviceUtilities
    {
        public static int CalculateFMEARiskScore(Device model)
        {
            int safetyScore = (int)Enum.Parse<Safety>(model.Safety, true);
            int detectionScore = (int)Enum.Parse<Detection>(model.Detection, true);

            int FMEARiskScore = (UtilizationRate(model) + Unavailability(model) + AgeFactor(model)) * (safetyScore + (int)Importance(model) + FinancialScore(model)) * detectionScore;

            return FMEARiskScore;
        }
        public static string UtilizationRateString(Device model)
        {
            return UtilizationRate(model) switch
            {
                5 => "Excessive",
                4 => "Above Average",
                3 => "Average",
                2 => "Below Average",
                1 => "Limited",
                _ => "Not Calculated"
            };
        }
        public static string UnavailabilityString(Device model)
        {
            return Unavailability(model) switch
            {
                5 => "Very High",
                4 => "High",
                3 => "Moderate",
                2 => "Low",
                1 => "Very Low",
                _ => "Not Calculated"
            };
        }
        public static string ImportanceString(Device model)
        {
            return Importance(model) switch
            {
                5 => "Very Important",
                4 => "Important",
                3 => "Normal",
                2 => "Not Important",
                1 => "Significant",
                _ => "Not Calculated"
            };
        }
        public static string FinancialScoreString(Device model)
        {
            return FinancialScore(model) switch
            {
                3 => "Very Expensive",
                2 => "Expensive",
                1 => "Normal",
                _ => "Not Calculated"
            };
        }
        public static string AgeFactorString(Device model)
        {
            return AgeFactor(model) switch
            {
                5 => "Very Old",
                4 => "Old",
                3 => "Decent",
                2 => "Relatively New",
                1 => "New",
                _ => "Not Calculated"
            };
        }
        public static string DetectionString(Device model)
        {
            return model.Detection switch
            {
                "NoChance" => "No Chance",
                "LowChance" => "Low Chance",
                "ModerateChance" => "Moderate Chance",
                "HighChance" => "High Chance",
                "EasyToDetect" => "Easy to Detect",
                _ => "Not Calculated"
            };
        }
        private static int UtilizationRate(Device model)
        {
            if ((model.NumberOfWorkingDays / 365f) * 100 >= 80) return 5;
            else if ((model.NumberOfWorkingDays / 365f) * 100 < 80 && (model.NumberOfWorkingDays / 365f) * 100 >= 65) return 4;
            else if ((model.NumberOfWorkingDays / 365f) * 100 < 65 && (model.NumberOfWorkingDays / 365f) * 100 >= 50) return 3;
            else if ((model.NumberOfWorkingDays / 365f) * 100 < 50 && (model.NumberOfWorkingDays / 365f) * 100 >= 30) return 2;
            else if ((model.NumberOfWorkingDays / 365f) * 100 < 30) return 1;
            else return 1;
        }
        private static int Unavailability(Device model)
        {
            if ((model.DownTime / 365f) * 100 >= 50) return 5;
            else if ((model.DownTime / 365f) * 100 < 50 && (model.DownTime / 365f) * 100 >= 40) return 4;
            else if ((model.DownTime / 365f) * 100 < 40 && (model.DownTime / 365f) * 100 >= 30) return 3;
            else if ((model.DownTime / 365f) * 100 < 30 && (model.DownTime / 365f) * 100 >= 20) return 2;
            else if ((model.DownTime / 365f) * 100 < 20 && (model.DownTime / 365f) * 100 >= 10) return 1;
            else return 1;
        }
        private static int AgeFactor(Device model)
        {
            if (DateTime.Now.Year - model.PurchaseDate.Year >= 10) return 5;
            else if (DateTime.Now.Year - model.PurchaseDate.Year >= 8 && DateTime.Now.Year - model.PurchaseDate.Year < 10) return 4;
            else if (DateTime.Now.Year - model.PurchaseDate.Year >= 6 && DateTime.Now.Year - model.PurchaseDate.Year < 8) return 3;
            else if (DateTime.Now.Year - model.PurchaseDate.Year >= 5 && DateTime.Now.Year - model.PurchaseDate.Year < 6) return 2;
            else return 1;
        }
        private static double Importance(Device model)
        {
            double functionScore = (double)Enum.Parse<Function>(model.Function, true);
            double areaScore = (double)Enum.Parse<Area>(model.Area, true);

            return Math.Ceiling((functionScore + areaScore) / 2);
        }
        private static int FinancialScore(Device model)
        {
            if ((model.ServiceCost / model.PurchasingCost) >= 0.5) return 3;
            else if ((model.ServiceCost / model.PurchasingCost) >= 0.3 && (model.ServiceCost / model.PurchasingCost) < 0.5) return 2;
            else return 1;
        }
    }
}
