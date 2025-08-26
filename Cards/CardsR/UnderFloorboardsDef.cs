using Clownpiece.CustomClasses;
using Clownpiece.Localization;
using Clownpiece.Status;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.JadeBoxes;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.InputSystem.Controls;

namespace Clownpiece.Cards.CardsR
{
    public sealed class UnderFloorboardsDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(UnderFloorboards);
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
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 1, Any = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 3,
            UpgradedValue1: null,
            Value2: null,
            UpgradedValue2: null,
            Mana: new ManaGroup() { },
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
            RelativeKeyword: Keyword.TempMorph,
            UpgradedRelativeKeyword: Keyword.TempMorph,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { "YinyangCard" },

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
    [EntityLogic(typeof(UnderFloorboardsDef))]
    public sealed class UnderFloorboards : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card[] cardArray = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.CanBeLoot, false), Value1, (CardConfig config) => config.Owner == "Reimu");
            MiniSelectCardInteraction selectCardInteraction = new MiniSelectCardInteraction((IEnumerable<Card>)cardArray);
            selectCardInteraction.Source = (GameEntity)this;
            MiniSelectCardInteraction interaction = selectCardInteraction;
            yield return (BattleAction)new InteractionAction((Interaction)interaction);

            Card selectedCard = interaction.SelectedCard;
            selectedCard.SetTurnCost(this.Mana);
            yield return (BattleAction)new AddCardsToHandAction(new Card[1]{selectedCard});

            interaction = (MiniSelectCardInteraction)null;

            if (this.IsUpgraded)
            {
                yield return (BattleAction)new AddCardsToHandAction((IEnumerable<Card>)Library.CreateCards<YinyangCard>(1));
            }
        }
    }
}