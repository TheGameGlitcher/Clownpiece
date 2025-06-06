﻿using Clownpiece.Cards.Templates;
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
using LBoL.Core.StatusEffects;
using Clownpiece.Cards.CardsB;

namespace Clownpiece.Cards.LunaticCards.NonTeammateCards
{
    public sealed class MutationDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Mutation);
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
            GunName: "Simple1",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Skill,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Any = 1 },
            UpgradedCost: new ManaGroup() { Any = 2 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 8,
            UpgradedDamage: null,
            Block: 12,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
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

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.Accuracy,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "DummyLunacySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe" },
            RelativeCards: new List<string>() { "Adaptation" },
            UpgradedRelativeCards: new List<string>() { "Adaptation+" },

            Owner: "Clownpiece",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: "Radal ",
            SubIllustrator: new List<string>() { "Radal" }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(MutationDef))]
    public sealed class Mutation : ClownCard
    {
        public Mutation() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(Adaptation);
            Value3 = 2;
            Value4 = 1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Mutation> list = Library.CreateCards<Mutation>(2, IsUpgraded).ToList();
            Mutation first = list[0];
            Mutation second = list[1];

            first.ChoiceCardIndicator = 1;
            second.ChoiceCardIndicator  = 2;

            first.SetBattle(Battle);
            second.SetBattle(Battle);

            MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(list, false, false, false)
            {
                Source = this
            };

            yield return new InteractionAction(interaction, false);

            if (interaction.SelectedCard == first)
            {
                for (int i = 0; i < Value2; i++)
                {
                    yield return AttackAction(selector);
                }

                yield return DebuffAction<LockedOn>(Battle.Player, Value1, Value1);
            }
            else
            {
                yield return DefenseAction();
                yield return DebuffAction<Weak>(Battle.Player, Value3, Value3);
                yield return BuffAction<Graze>(Value4, Value4);
            }
        }
    }
}
