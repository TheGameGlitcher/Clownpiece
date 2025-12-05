using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;

using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Neutral.Blue;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clownpiece.Cards.CardsB
{
    public sealed class MockeryDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Mockery);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(this, extension: ".png");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.CardLoc.AddEntity(this);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
            Index: BepinexPlugin.sequenceTable.Next(typeof(CardConfig)),
            Id: "",
            Order: 10,
            AutoPerform: true,
            Perform: new string[0][],
            GunName: "Simple1",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            Kicker: null,
            UpgradedKicker: null,
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 4,
            UpgradedValue1: null,
            Value2: 2,
            UpgradedValue2: 3,
            Mana: new ManaGroup() { Any = 1 },
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

            RelativeEffects: new List<string>() { "LockedOn" },
            UpgradedRelativeEffects: new List<string>() { "LockedOn" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

            Owner: "Clownpiece",
            Pack: "",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: "Radal",
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(MockeryDef))]
    public sealed class Mockery : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DebuffAction<LockedOn>(base.Battle.Player, Value1, Value1);

            if (base.Battle.DrawZone.Count > 0)
            {
                int max = Value2;
                int i = 0;
                while (i < max && base.Battle.DrawZone.Count != 0 && base.Battle.HandZone.Count != base.Battle.MaxHand && !(base.Battle.Player.HasStatusEffect<CantDrawThisTurn>()))
                {
                    List<Card> list = (from card in base.Battle.DrawZone select card).ToList<Card>();
                    if (list.Count > 0)
                    {
                        Card card = list[i];
                        card.SetTurnCost(card.Cost - this.Mana);
                        card.IsTempMorph = true;
                    }
                    i++;
                }
                yield return new DrawManyCardAction(Value2);
            }
        }
    }
}
