using Clownpiece.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.StatusEffects.Basic;
using System.Linq;
using Clownpiece.CustomClasses;
using Clownpiece.Cards.CardsB;

namespace Clownpiece.Cards.LunaticCards.NonTeammateCards
{
    public sealed class FireworksOfLunacyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FireworksOfLunacy);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(this, extension: ".png");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(BepinexPlugin.embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "CardsEn.yaml");
            return loc;
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
            Index: BepinexPlugin.sequenceTable.Next(typeof(CardConfig)),
            Id: "",
            Order: 10,
            AutoPerform: true,
            Perform: new string[0][],
            GunName: "瘴气四溢B",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Attack,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2, Any = 2 },
            UpgradedCost: new ManaGroup() { Black = 2, Any = 2 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 12,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 3,
            UpgradedValue1: null,
            Value2: 1,
            UpgradedValue2: 2,
            Mana: null,
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
            UpgradedKeywords: Keyword.Accuracy,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "DummyLunacySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe" },
            RelativeCards: new List<string>() { "StatueOfLunacy" },
            UpgradedRelativeCards: new List<string>() { "StatueOfLunacy+" },

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
    [EntityLogic(typeof(FireworksOfLunacyDef))]
    public sealed class FireworksOfLunacy : ClownCard
    {
        public FireworksOfLunacy() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(StatueOfLunacy);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int attackAmount = 2;
            yield return SacrificeAction(Value1);

            List<Card> list = Battle.HandZone.ToList();
            if (list.Count > 0)
            {
                SelectHandInteraction selectHandInteraction = new SelectHandInteraction(0, Value2, list);
                selectHandInteraction.Source = this;
                SelectHandInteraction interaction = selectHandInteraction;
                yield return new InteractionAction(interaction);
                IReadOnlyList<Card> cards = interaction.SelectedCards;
                yield return new DiscardManyAction(cards);
                if (cards.Count > 0)
                    attackAmount = attackAmount + cards.Count;

                interaction = null;
                cards = null;
            }

            for (int i = 0; i < attackAmount; i++)
                yield return AttackAction(selector);
        }
    }
}
