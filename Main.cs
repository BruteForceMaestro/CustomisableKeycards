using Exiled.API.Features;
using HarmonyLib;
using System;

namespace CustomisableKeycards
{
    public class Main : Plugin<Config>
    {
        readonly Harmony harmony = new Harmony("com.nutmaster.permissionpatch");
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(4, 2, 3);
        public static Main Instance { get; set; }
        public override void OnEnabled()
        {
            Instance = this;
            harmony.PatchAll();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            harmony.UnpatchAll();
            base.OnDisabled();
        }
    }
}