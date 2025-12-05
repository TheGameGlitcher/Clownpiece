using Clownpiece.CustomClasses;

using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using static Clownpiece.Boss.BlackFairyBossDef;
using static Clownpiece.Boss.WhiteFairyBossDef;
using static UnityEngine.UI.GridLayoutGroup;

namespace Clownpiece.Status
{
    public sealed class ManicTorchBossSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ManicTorchBossSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("ManicTorchBossSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Special,
                            IsVerbose: false,
                            IsStackable: false,
                            StackActionTriggerLevel: null,
                            HasLevel: false,
                            LevelStackType: null,
                            HasDuration: false,
                            DurationStackType: null,
                            DurationDecreaseTiming: DurationDecreaseTiming.TurnEnd,
                            HasCount: false,
                            CountStackType: StackType.Max,
                            LimitStackType: StackType.Max,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "LunaticTorch"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(ManicTorchBossSeDef))]
    public sealed class ManicTorchBossSe : ClownStatus
    {
        protected override void OnAdded(Unit unit)
        {
            if (!(base.Battle.Player.HasStatusEffect<LunaticLightSe>()))
                React(new ApplyStatusEffectAction<LunaticLightSe>(base.Battle.Player, new int?(1), null, null, null, 0.2f, false));
        }
    }
}