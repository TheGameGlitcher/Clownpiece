using LBoL.Base;
using LBoL.Core;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle;
using Clownpiece.CustomClasses;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.Core.Battle.BattleActions;
using Clownpiece.Status;
using LBoL.Core.StatusEffects;

namespace Clownpiece.Cards.CardsB
{
    public sealed class PurestFairyEssenceDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PurestFairyEssence);
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
            Type: CardType.Defense,
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
            Block: 18,
            UpgradedBlock: 24,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: null,
            Value2: 5,
            UpgradedValue2: 7,
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
            UpgradedRelativeKeyword: Keyword.Block,

            RelativeEffects: new List<string>() { "LockedOn", "Poison", "PurifyNextTurnSe" },
            UpgradedRelativeEffects: new List<string>() { "LockedOn", "Poison", "PurifyNextTurnSe" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

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
    [EntityLogic(typeof(PurestFairyEssenceDef))]
    public sealed class PurestFairyEssence : ClownCard
    {
        public PurestFairyEssence() : base()
        {
            Value3 = 3;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DebuffAction<PurifyNextTurnSe>(base.Battle.Player, Value1);
            yield return DefenseAction();
            yield return new ApplyStatusEffectAction<PoisonTouchSe>(base.Battle.Player, 1, null, Value2, null, 0f, true);
            yield return new ApplyStatusEffectAction<LockOnTouchSe>(base.Battle.Player, 1, null, Value3, null, 0f, true);
        }
    }
}