using Clownpiece.Cards.LunaticCards.NonTeammateCards;
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
    public sealed class StatueOfLunacyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StatueOfLunacy);
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
            Type: CardType.Defense,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2, Any = 2 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
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
            ActiveCost2: null,
            UpgradedActiveCost2: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Block,
            UpgradedRelativeKeyword: Keyword.Shield,

            RelativeEffects: new List<string>() { "DummyTorchLinkedSe" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedSe" },
            RelativeCards: new List<string>() { "FireworksOfLunacy" },
            UpgradedRelativeCards: new List<string>() { "FireworksOfLunacy+" },

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
    [EntityLogic(typeof(StatueOfLunacyDef))]
    public sealed class StatueOfLunacy : ClownCard
    {
        public StatueOfLunacy() : base()
        {
            IsTorchLinked = true;
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
