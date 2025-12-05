using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;

using Clownpiece.Status;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
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
    public sealed class ForgottenMaidDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ForgottenMaid);
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
            DebugLevel: 3,
            Revealable: false,

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: true,
            IsUpgradable: true,
            Rarity: Rarity.Rare,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2, Any = 3 },
            UpgradedCost: new ManaGroup() { Black = 1, Any = 3 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: null,
            Value2: 3,
            UpgradedValue2: null,
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

            Keywords: Keyword.Exile,
            UpgradedKeywords: Keyword.Exile,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

            Owner: "Clownpiece",
            Pack: "",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: true,
            Illustrator: null,
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(ForgottenMaidDef))]
    public sealed class ForgottenMaid : ClownCard
    {
        public ForgottenMaid() : base()
        {
            Value3 = 2;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < Value3; i++)
            {
                List<Card> cards = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.Valid, CardTypeWeightTable.OnlyFriend, false), Value2, (CardConfig config) => config.Cost.Amount < 5).ToList<Card>();
                foreach (Card card in cards)
                {
                    card.Summoned = true;
                }
                if (cards.Count > 0)
                {
                    MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(cards, false, false, false) { Source = this };
                    yield return new InteractionAction(interaction, false);
                    Card selectedCard = interaction.SelectedCard;
                    yield return new AddCardsToHandAction(new Card[] { selectedCard });
                }
            }
        }
    }
}
