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
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using System.Linq;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.Cards.Neutral.Blue;
using LBoL.EntityLib.StatusEffects.Basic;
using Clownpiece.CustomClasses;

namespace Clownpiece.Cards.CardsB
{
    public sealed class StatueOfLunacyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StatueOfLunacy);
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
            GunName: "Simple1",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Defense,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2, Any = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: 12,
            UpgradedBlock: 0,
            Shield: 0,
            UpgradedShield: 10,
            Value1: 6,
            UpgradedValue1: 5,
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
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Block,
            UpgradedRelativeKeyword: Keyword.Shield,

            RelativeEffects: new List<string>() { "DummyLunacySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe" },
            RelativeCards: new List<string>() { "FireworksOfLunacy" },
            UpgradedRelativeCards: new List<string>() { "FireworksOfLunacy+" },

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
    [EntityLogic(typeof(StatueOfLunacyDef))]
    public sealed class StatueOfLunacy : ClownCard
    {
        public StatueOfLunacy() : base()
        {
            CanTransform = true;
            TransformTo = typeof(FireworksOfLunacy);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return this.DefenseAction();
            yield return this.BuffAction<Reflect>(this.Value1);

            List<Card> list = this.Battle.HandZone.ToList<Card>();
            if (list.Count > 0)
            {
                SelectHandInteraction selectHandInteraction = new SelectHandInteraction(0, this.Value2, (IEnumerable<Card>)list);
                selectHandInteraction.Source = (GameEntity)this;
                SelectHandInteraction interaction = selectHandInteraction;
                yield return (BattleAction)new InteractionAction((Interaction)interaction);

                IReadOnlyList<Card> cards = interaction.SelectedCards;
                yield return (BattleAction)new DiscardManyAction((IEnumerable<Card>)cards);

                if (cards.Count > 0)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        yield return this.DefenseAction();
                        yield return this.BuffAction<Reflect>(this.Value1);
                    }
                }
                interaction = (SelectHandInteraction)null;
                cards = (IReadOnlyList<Card>)null;
            }

            if (this.Battle.Player.HasStatusEffect<Reflect>())
                this.Battle.Player.GetStatusEffect<Reflect>().Gun = "ESanaeSkill1";
        }
    }
}
