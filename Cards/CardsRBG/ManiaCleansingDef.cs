using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;
using Clownpiece.Localization;
using Clownpiece.Status;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.IO.LowLevel.Unsafe;

namespace Clownpiece.Cards.CardsRBG
{
    public sealed class ManiaCleansingDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ManiaCleansing);
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
            Rarity: Rarity.Rare,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Black, ManaColor.Green },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 2, Black = 2, Green = 1 },
            UpgradedCost: new ManaGroup() { Red = 1, Black = 1, Green = 1, Any = 2 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: null,
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

            Keywords: Keyword.Exile,
            UpgradedKeywords: Keyword.Exile,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Exile,
            UpgradedRelativeKeyword: Keyword.Exile,

            RelativeEffects: new List<string>() { "LunaticTorchSe" },
            UpgradedRelativeEffects: new List<string>() { },
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
    [EntityLogic(typeof(ManiaCleansingDef))]
    public sealed class ManiaCleansing : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Battle.Player.HasStatusEffect<LunaticTorchSe>())
            {
                yield return new RemoveStatusEffectAction(Battle.Player.GetStatusEffect<LunaticTorchSe>());
            }

            yield break;
        }
    }
}
