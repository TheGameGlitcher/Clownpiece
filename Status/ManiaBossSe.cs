using Clownpiece.CustomClasses;

using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
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
    public sealed class ManiaBossSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ManiaBossSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("ManiaBossSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Negative,
                            IsVerbose: false,
                            IsStackable: false,
                            StackActionTriggerLevel: null,
                            HasLevel: false,
                            LevelStackType: null,
                            HasDuration: true,
                            DurationStackType: StackType.Add,
                            DurationDecreaseTiming: DurationDecreaseTiming.TurnEnd,
                            HasCount: false,
                            CountStackType: StackType.Max,
                            LimitStackType: StackType.Max,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Lunatic"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(ManiaBossSeDef))]
    public sealed class ManiaBossSe : ClownStatus
    {
        public override bool ShouldPreventCardUsage(Card card)
        {
            return card.CardType == CardType.Defense;
        }

        public override string PreventCardUsageMessage
        {
            get
            {
                return "I'm not defending! Attack! Attack!";
            }
        }
    }
}