﻿using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PMAPI.CustomSubstances
{
    public static class CustomSubstanceManager
    {
        internal static void Init()
        {
            // Warm up the manager so it loads everything properly
            SubstanceManager.GetParameter(Substance.Stone);

            CreateErrorSubstance();
        }

        internal static Dictionary<Substance, SubstanceParameters.Param> customSubstances = new();
        internal static Dictionary<Substance, CustomSubstanceParams> customParams = new();
        internal static SubstanceParameters.Param errorSubParams;

        //private static int id = -1;
        private static Dictionary<string, int> currentId = new();
        /// <summary>
        /// Registers custom substance in game
        /// </summary>
        /// <param name="eid">Extended ID</param>
        /// <param name="substanceParams">Parameters used by the game</param>
        /// <param name="cParams">Parameters used by PMAPI</param>
        /// <returns>ID of registered substance used by the game</returns>
        public static Substance RegisterSubstance(string eid, SubstanceParameters.Param substanceParams, CustomSubstanceParams cParams)
        {
            var modAttrib = Assembly.GetCallingAssembly().GetCustomAttribute<PMAPIModAttribute>();
            if (!currentId.TryGetValue(modAttrib.id, out var id))
            {
              id = modAttrib.substanceStart;
            }
            var guideKey = substanceParams.displayNameKey + "_GUIDE";

            MelonLogger.Msg("current id" + id + " name " + eid);
            customSubstances.Add((Substance)id, substanceParams);
            customParams.Add((Substance)id, cParams);

            
            EIDManager.eidDictionary.Add((Substance)id, $"{modAttrib.id}:{eid}");
            CustomLocalizer.AddEnString(guideKey, cParams.enGuide);
            CustomLocalizer.AddJpString(guideKey, cParams.jpGuide);
            CustomLocalizer.AddZhHansString(guideKey, cParams.zhHansGuide);
            CustomLocalizer.AddDeString(guideKey, cParams.deGuide);
            CustomLocalizer.AddEsString(guideKey, cParams.esGuide);
            CustomLocalizer.AddFrString(guideKey, cParams.frGuide);
            CustomLocalizer.AddRuString(guideKey, cParams.ruGuide);
            CustomLocalizer.AddEnString(substanceParams.displayNameKey, cParams.enName);
            CustomLocalizer.AddJpString(substanceParams.displayNameKey, cParams.jpName);

            CustomLocalizer.AddZhHansString(substanceParams.displayNameKey, cParams.zhHansName);
            CustomLocalizer.AddDeString(substanceParams.displayNameKey, cParams.deName);
            CustomLocalizer.AddEsString(substanceParams.displayNameKey, cParams.esName);
            CustomLocalizer.AddFrString(substanceParams.displayNameKey, cParams.frName);
            CustomLocalizer.AddRuString(substanceParams.displayNameKey, cParams.ruName);

            MelonLogger.Msg("Substance registered: " + EIDManager.eidDictionary[(Substance)id]);
            currentId[modAttrib.id] = id + 1;

            return (Substance)id;
        }

        private static void CreateErrorSubstance()
        {
            var defStone = SubstanceManager.GetParameter(Substance.Stone);
            errorSubParams = defStone.MemberwiseClone().Cast<SubstanceParameters.Param>();

            Material errorMat = Material.GetDefaultMaterial();
            errorMat.shader = Shader.FindBuiltin("Hidden/InternalErrorShader");
            CustomMaterialManager.RegisterMaterial(errorMat);
            errorSubParams.material = errorMat.name;
            errorSubParams.displayNameKey = "SUB_PMAPI_MODERROR";

            CustomLocalizer.AddEnString(errorSubParams.displayNameKey, "Mod error");
            CustomLocalizer.AddJpString(errorSubParams.displayNameKey, "Moddu erroru");

            CustomLocalizer.AddZhHansString(errorSubParams.displayNameKey, "Mod error");
            CustomLocalizer.AddDeString(errorSubParams.displayNameKey, "Mod error");
            CustomLocalizer.AddEsString(errorSubParams.displayNameKey, "Mod error");
            CustomLocalizer.AddFrString(errorSubParams.displayNameKey, "le Mod error");
            CustomLocalizer.AddRuString(errorSubParams.displayNameKey, "Ошибка мода");
        }

        public static Substance GetSubstanceByEID(string eid) => EIDManager.eidDictionary.FirstOrDefault(x => x.Value == eid).Key;

        public static string GetEIDBySubstance(Substance substance) => EIDManager.eidDictionary[substance];
    }

    /// <summary>
    /// Substance parameters used by PMAPI
    /// </summary>
    public class CustomSubstanceParams
    {
        /// <summary>
        /// Name in English
        /// </summary>
        public string enName;

        /// <summary>
        /// Name in English
        /// </summary>
        public string enGuide;

        /// <summary>
        /// Name in Japanese
        /// </summary>
        public string jpName;

        /// <summary>
        /// Name in Japanese
        /// </summary>
        public string jpGuide;

        /// <summary>
        /// Name in Simplified Chinese
        /// </summary>
        public string zhHansName;

        /// <summary>
        /// Name in Simplified Chinese
        /// </summary>
        public string zhHansGuide;

        /// <summary>
        /// Name in German
        /// </summary>
        public string deName;

        /// <summary>
        /// Name in German
        /// </summary>
        public string deGuide;

        /// <summary>
        /// Name in Spanish
        /// </summary>
        public string esName;

        /// <summary>
        /// Name in Spanish
        /// </summary>
        public string esGuide;

        /// <summary>
        /// Name in French
        /// </summary>
        public string frName;

        /// <summary>
        /// Name in French
        /// </summary>
        public string frGuide;

        /// <summary>
        /// Name in Russian
        /// </summary>
        public string ruName;

        /// <summary>
        /// Name in Russian
        /// </summary>
        public string ruGuide;

        /// <summary>
        /// Delegate that is called whenever object is initalizing its script
        /// </summary>
        public Func<CubeBase, Component> behInit;
    }
}
