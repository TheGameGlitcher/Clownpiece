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
using LBoL.Core.StatusEffects;
using Clownpiece.Status;
using Clownpiece.CustomClasses;
using LBoL.EntityLib.Cards.Character.Sakuya;

namespace Clownpiece.Cards.CardsB
{
    public sealed class RacetrackDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Racetrack);
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

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Any = 2 },
            UpgradedCost: new ManaGroup() { Black = 1, Any = 1 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: null,
            Value2: 3,
            UpgradedValue2: null,
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

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

            Owner: "Clownpiece",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: null,
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(RacetrackDef))]
    public sealed class Racetrack : ClownCard
    {
        public Racetrack() : base()
        {
            Value3 = 1;
        }

        private List<Card> allHand;

        public override Interaction Precondition()
        {
            List<Card> list = (from hand in base.Battle.HandZone where hand != this select hand).ToList<Card>();
            if (list.Count <= base.Value1)
            {
                this.allHand = list;
            }
            if (list.Count <= this.Value1)
            {
                return null;
            }
            return new SelectHandInteraction(this.Value1, this.Value1, list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                IReadOnlyList<Card> selectedCards = ((SelectHandInteraction)precondition).SelectedCards;
                if (selectedCards != null)
                {
                    yield return new DiscardManyAction(selectedCards);
                }
            }
            else if (this.allHand.Count > 0)
            {
                yield return new DiscardManyAction(this.allHand);

            }

            for (int i = 0; i < Value2; i++)
            {
                Card card = base.Battle.DrawZone[i];
                card.DecreaseTurnCost(Mana);
            }
            yield return new DrawManyCardAction(this.Value2);
        }
    }
}