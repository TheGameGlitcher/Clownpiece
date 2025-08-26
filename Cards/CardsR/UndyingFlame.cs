using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;
using Clownpiece.Localization;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clownpiece.Cards.CardsR
{
    public sealed class UndyingFlameDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UndyingFlame);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(this, extension: ".png");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            return ClownpieceLocalization.CardsBatchLoc.AddEntity(this);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
            Index: BepinexPlugin.sequenceTable.Next(typeof(CardConfig)),
            Id: "",
            Order: 10,
            AutoPerform: true,
            Perform: new string[0][],
            GunName: "GuihuoB",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Attack,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 2 },
            UpgradedCost: new ManaGroup() { Red = 1 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 10,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: 1,
            Value2: 1,
            UpgradedValue2: null,
            Mana: new ManaGroup() { Any = 1},
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: null,
            UpgradedLoyalty: null,
            PassiveCost: null,
            UpgradedPassiveCost: null,
            ActiveCost: null,
            UpgradedActiveCost: null,
            ActiveCost2: null,
            UpgradedActiveCost2: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.TempMorph,
            UpgradedRelativeKeyword: Keyword.TempMorph,

            RelativeEffects: new List<string>() { "Vulnerable" },
            UpgradedRelativeEffects: new List<string>() { "Vulnerable" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

            Owner: "Clownpiece",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: "Radal",
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(UndyingFlameDef))]
    public sealed class UndyingFlame : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (base.Battle.DrawZone.Count > 0)
            {
                int max = Value2;
                int i = 1;
                while (i <= max && base.Battle.DrawZone.Count != 0 && base.Battle.HandZone.Count != base.Battle.MaxHand)
                {
                    List<Card> list = (from card in base.Battle.DrawZone where card.CardType == CardType.Attack select card).ToList<Card>();
                    if (list.Count > 0)
                    {
                        Card card2 = list.Sample(base.GameRun.BattleRng);
                        card2.SetTurnCost(this.Mana);
                        card2.IsTempMorph = true;
                        yield return new MoveCardAction(card2, CardZone.Hand);
                    }
                    i++;
                }
            }

            yield return AttackAction(selector.SelectedEnemy);
            yield return new ApplyStatusEffectAction<Vulnerable>(selector.SelectedEnemy, Value1, Value1);
        }
    }
}
