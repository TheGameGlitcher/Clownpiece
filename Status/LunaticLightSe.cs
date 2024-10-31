using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using static UnityEngine.UI.GridLayoutGroup;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using Clownpiece.CustomClasses;
using Clownpiece.Cards.CardsB;

namespace Clownpiece.Status
{
    public sealed class LunaticLightSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticLightSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "StatusEffectEn.yaml");
            return loc;
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("LunaticLightSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
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
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: true,
                            CountStackType: StackType.Keep,
                            LimitStackType: StackType.Keep,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { "Firepower" },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Lunatic"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(LunaticLightSeDef))]
    public sealed class LunaticLightSe : ClownStatus
    {
        public LunaticLightSe() : base()
        {
            Value1 = 1;
            Value2 = 5;
            Counter = 1;
        }

        protected override void OnAdded(Unit unit)
        {
            Count = 5;
            Counter = 1;
            this.NotifyActivating();
            React(new ApplyStatusEffectAction<Firepower>(Owner, Value1, null, null, null, 0.2f, false));
            base.ReactOwnerEvent<UnitEventArgs>(this.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
        }

        protected override void OnRemoved(Unit unit)
        {
            base.React(new ApplyStatusEffectAction<FirepowerNegative>(Owner, Counter, null, null, null, 0.2f, false));
            Count = 5;
            Counter = 1;
        }

        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (Count == 1)
            {
                this.NotifyActivating();
                Value3++;
                yield return new ApplyStatusEffectAction<Firepower>(Owner, Value1, null, null, null, 0.2f, false);
                Count = 5;
            }
            else
            {
                Count--;
            }
        }
    }
}

