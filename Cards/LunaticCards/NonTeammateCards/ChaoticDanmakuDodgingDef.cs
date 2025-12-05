using Clownpiece.CustomClasses;
using Clownpiece.Status;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;

namespace Clownpiece.Cards.LunaticCards.NonTeammateCards
{
    public sealed class ChaoticDanmakuDodgingDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ChaoticDanmakuDodging);
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
            Rarity: Rarity.Common,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Red = 1 },
            UpgradedCost: new ManaGroup() { Hybrid = 1, HybridColor = 7, Any = 1 },
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

            RelativeEffects: new List<string>() { "DummyTorchLinkedSe", "Fragil", "Vulnerable", "Graze" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedSe", "Fragil", "Vulnerable", "Graze" },
            RelativeCards: new List<string>() { "GracefulFairyDodging" },
            UpgradedRelativeCards: new List<string>() { "GracefulFairyDodging+" },

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
    [EntityLogic(typeof(ChaoticDanmakuDodgingDef))]
    public sealed class ChaoticDanmakuDodging : ClownCard
    {
        public ChaoticDanmakuDodging() : base()
        {
            IsTorchLinked = true;
            IsTransformed = true;
            TransformTo = typeof(GracefulFairyDodging);
            Value3 = 3;
            Value4 = 1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ApplyStatusEffectAction<Fragil>(base.Battle.Player, Value1, Value1, null, null, 0.1f, true);
            yield return new ApplyStatusEffectAction<Vulnerable>(base.Battle.Player, Value2, Value2, null, null, 0.1f, true);

            yield return this.BuffAction<Graze>(this.Value2);

            yield return new DrawManyCardAction(this.Value3);

            yield return this.BuffAction<ChaoticGrazeSe>(Value4);
            yield return this.BuffAction<ChaoticDrawSe>(Value4);
        }
    }
}