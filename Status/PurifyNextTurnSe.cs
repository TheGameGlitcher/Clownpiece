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
using LBoL.Core;
using LBoL.EntityLib.StatusEffects;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using static UnityEngine.UI.GridLayoutGroup;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using Clownpiece.CustomClasses;

namespace Clownpiece.Status
{
    public sealed class PurifyNextTurnSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PurifyNextTurnSe);
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
            return ResourceLoader.LoadSprite("PurifyNextTurnSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Positive,
                            IsVerbose: false,
                            IsStackable: true,
                            StackActionTriggerLevel: null,
                            HasLevel: true,
                            LevelStackType: StackType.Add,
                            HasDuration: false,
                            DurationStackType: StackType.Add,
                            DurationDecreaseTiming: DurationDecreaseTiming.TurnEnd,
                            HasCount: false,
                            CountStackType: StackType.Add,
                            LimitStackType: StackType.Add,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.Purify,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Default"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(PurifyNextTurnSeDef))]
    public sealed class PurifyNextTurnSe : ClownStatus
    {
        public PurifyNextTurnSe() : base()
        {
            Counter = 1;
        }
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted), (GameEventPriority)99999);
            base.HandleOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, new GameEventHandler<UnitEventArgs>(this.OnPlayerTurnEnding));
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            Counter--;

            base.NotifyActivating();
            if (base.Battle.BattleMana.HasTrivial)
                yield return ConvertManaAction.Purify(base.Battle.BattleMana, base.Level);
        }

        private void OnPlayerTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                return;

            if (Counter == 0)
                this.React(new RemoveStatusEffectAction(this, true, 0.1f));
        }
    }
}