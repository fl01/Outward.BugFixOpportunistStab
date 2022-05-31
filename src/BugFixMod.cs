using System.Collections;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Outward.BugFixOpportunistStab
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BugFixMod : BaseUnityPlugin
    {
        public const string GUID = "fl01.bugfix-opportunistStab";
        public const string NAME = "BugFix Opportunist Stab";
        public const string VERSION = "1.0.0";
        public static ManualLogSource Log;
        internal void Awake()
        {
            Log = base.Logger;
            new Harmony(GUID).PatchAll();
            base.StartCoroutine(Fix());
        }

        private IEnumerator Fix()
        {
            while (!ResourcesPrefabManager.Instance.Loaded)
            {
                yield return new WaitForSeconds(1f);
            }

            var item = ResourcesPrefabManager.Instance.GetItemPrefab(8100071);
            if (item != null)
            {
                var effects = item.transform.GetComponentsInChildren<StatusEffectCondition>();
                var confusion = effects.FirstOrDefault(f => f.StatusEffectPrefab?.IdentifierName == "Confusion");
                var missingEffect = effects.FirstOrDefault(f => f.StatusEffectPrefab == null);
                if (missingEffect != null && confusion != null)
                {
                    missingEffect.StatusEffectPrefab = confusion.StatusEffectPrefab;
                    Log.LogInfo($"Stab effect fix has been applied");
                }
            }
        }
    }
}
