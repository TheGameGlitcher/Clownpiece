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
using LBoL.EntityLib.StatusEffects;
using Clownpiece.CustomClasses;
using LBoL.Core.StatusEffects;

namespace Clownpiece.Cards.CardsB
{
    public sealed class AdaptationDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Adaptation);
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
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Any = 1 },
            UpgradedCost: new ManaGroup() { Any = 2 },
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: 20,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: null,
            Value2: 2,
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
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Block,
            UpgradedRelativeKeyword: Keyword.Block,

            RelativeEffects: new List<string>() { "DummyLunacySe", "LockedOn", "Graze", "Weak" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe", "LockedOn", "Graze", "Weak" },
            RelativeCards: new List<string>() { "Mutation" },
            UpgradedRelativeCards: new List<string>() { "Mutation+" },

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
    [EntityLogic(typeof(AdaptationDef))]
    public sealed class Adaptation : ClownCard
    {
        public Adaptation() : base()
        {
            CanTransform = true;
            TransformTo = typeof(Mutation);
            Value3 = 2;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Adaptation> list = Library.CreateCards<Adaptation>(2, this.IsUpgraded).ToList<Adaptation>();
            Adaptation first = list[0];
            Adaptation second = list[1];

            first.ShowWhichDescription = 1;
            second.ShowWhichDescription = 2;

            first.SetBattle(this.Battle);
            second.SetBattle(this.Battle);

            MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(list, false, false, false)
            {
                Source = this
            };

            yield return new InteractionAction(interaction, false);

            if (interaction.SelectedCard == first)
            {
                yield return DebuffAction<LockedOn>(base.Battle.Player, Value1, Value1);
                yield return BuffAction<Graze>(Value2, Value2);
            }
            else
            {
                yield return DefenseAction();
                yield return DebuffAction<Weak>(base.Battle.Player, Value3, Value3);
            }    
        }
    }
}
