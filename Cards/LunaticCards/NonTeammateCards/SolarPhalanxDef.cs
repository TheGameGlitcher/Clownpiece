using Clownpiece.Cards.CardsB;
using Clownpiece.Cards.LunaticCards.LunaticFairyTeammates;
using Clownpiece.CustomClasses;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;

namespace Clownpiece.Cards.LunaticCards.NonTeammateCards
{
    public sealed class SolarPhalanxDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SolarPhalanx);
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

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2, Any = 1 },
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

            RelativeEffects: new List<string>() { "DummyTorchLinkedSe" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedSe" },
            RelativeCards: new List<string>() { "NaturesDefense", "LunaticSunflowerFairy" },
            UpgradedRelativeCards: new List<string>() { "NaturesDefense+", "LunaticSunflowerFairy+" },

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
    [EntityLogic(typeof(SolarPhalanxDef))]
    public sealed class SolarPhalanx : ClownCard
    {
        public SolarPhalanx() : base()
        {
            IsTorchLinked = true;
            IsTransformed = true;
            TransformTo = typeof(NaturesDefense);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<LunaticSunflowerFairy> cards = Library.CreateCards<LunaticSunflowerFairy>(Value1, false).ToList<LunaticSunflowerFairy>();
            foreach (LunaticSunflowerFairy card in cards)
            {
                card.IsUpgraded = this.IsUpgraded;
                card.Summoned = true;
            }

            yield return new AddCardsToHandAction(cards, AddCardsType.Normal);
        }
    }
}
