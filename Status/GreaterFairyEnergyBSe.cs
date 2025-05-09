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
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.Core.Cards;

namespace Clownpiece.Status
{
    public sealed class GreaterFairyEnergyBSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GreaterFairyEnergyBSe);
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
            return ResourceLoader.LoadSprite("GreaterFairyEnergyBSe.png", BepinexPlugin.embeddedSource);
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
                            DurationStackType: StackType.Keep,
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: false,
                            CountStackType: StackType.Add,
                            LimitStackType: StackType.Add,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.Replenish,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Default"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(GreaterFairyEnergyBSeDef))]
    public sealed class GreaterFairyEnergyBSe : ClownStatus
    {

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(this.Owner.TurnEnded, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnEnded));
        }

        private IEnumerable<BattleAction> OnOwnerTurnEnded(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            List<Card> list = new List<Card>();
            for (int i = 0; i < Level; i++)
            {
                Card card = Library.CreateCard<BManaCard>();
                card.IsReplenish = true;
                list.Add(card);
            }

            this.NotifyActivating();
            yield return new AddCardsToDiscardAction(list);
        }
    }
}

