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

namespace Clownpiece.Cards.CardsB
{
    public sealed class FireworksOfLunacyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(FireworksOfLunacy);
        }

        public override CardImages LoadCardImages()
        {
            return null;
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
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Attack,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2, Any = 2 },
            UpgradedCost: new ManaGroup() { Black = 2, Any = 2 },
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

            Unfinished: true,
            Illustrator: null,
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
            yield return this.SacrificeAction(Value1);

            List<Card> list = this.Battle.HandZone.ToList<Card>();
            if (list.Count > 0)
            {
                SelectHandInteraction selectHandInteraction = new SelectHandInteraction(0, this.Value2, (IEnumerable<Card>)list);
                selectHandInteraction.Source = (GameEntity)this;
                SelectHandInteraction interaction = selectHandInteraction;
                yield return (BattleAction)new InteractionAction((Interaction)interaction);
                IReadOnlyList<Card> cards = interaction.SelectedCards;
                yield return (BattleAction)new DiscardManyAction((IEnumerable<Card>)cards);
                yield return this.AttackAction(selector);
                yield return this.AttackAction(selector);
                if (cards.Count > 0)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        yield return this.AttackAction(selector);
                    }
                }
                interaction = (SelectHandInteraction)null;
                cards = (IReadOnlyList<Card>)null;
            }
        }
    }
}
