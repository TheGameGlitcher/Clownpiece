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
using LBoL.EntityLib.Cards.Character.Reimu;
using Clownpiece.Cards.CardsR;
using Clownpiece.Cards.LunaticCards.LunaticFairyTeammates;

namespace Clownpiece.Cards.LunaticCards.NonTeammateCards
{
    public sealed class HellsLunacyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HellsLunacy);
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
            Rarity: Rarity.Common,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 2 },
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
            Value1: 1,
            UpgradedValue1: null,
            Value2: null,
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
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "DummyLunacySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe" },
            RelativeCards: new List<string>() { "HellsNature", "LunaticHellFairy" },
            UpgradedRelativeCards: new List<string>() { "HellsNature+", "LunaticHellFairy+" },

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
    [EntityLogic(typeof(HellsLunacyDef))]
    public sealed class HellsLunacy : ClownCard
    {
        public HellsLunacy() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(HellsNature);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<LunaticHellFairy> cards = Library.CreateCards<LunaticHellFairy>(Value1, false).ToList();
            foreach (LunaticHellFairy card in cards)
            {
                card.IsUpgraded = IsUpgraded;
                card.Summoned = true;
            }

            yield return new AddCardsToHandAction(cards, AddCardsType.Normal);
        }
    }
}
