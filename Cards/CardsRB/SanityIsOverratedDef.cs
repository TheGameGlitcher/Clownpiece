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
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using System.Linq;
using LBoL.Base.Extensions;
using Clownpiece.CustomClasses;
using Clownpiece.Status;
using LBoL.Core.Units;
using Clownpiece.Cards.CardsB;
using LBoL.EntityLib.StatusEffects.Basic;

namespace Clownpiece.Cards.CardsR
{
    public sealed class SanityIsOverratedDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SanityIsOverrated);
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
            TargetType: TargetType.AllEnemies,
            Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Hybrid = 2, HybridColor = 7, Any = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: 3,
            Value2: 4,
            UpgradedValue2: 6,
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

            Keywords: Keyword.Exile,
            UpgradedKeywords: Keyword.Exile,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "ManiaSe", "Graze", "TempElectric" },
            UpgradedRelativeEffects: new List<string>() { "ManiaSe", "Graze", "TempElectric" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

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
    [EntityLogic(typeof(SanityIsOverratedDef))]
    public sealed class SanityIsOverrated : ClownCard
    {
        public SanityIsOverrated() : base()
        {
            Value3 = 3;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (EnemyUnit enemy in base.Battle.AllAliveEnemies)
            {
                if (base.Battle.Player.HasStatusEffect<ManicFlameSe>())
                    yield return new ApplyStatusEffectAction<PermaManiaSe>(enemy, Value1, Value1, null, null, 0.15f);
                else
                    yield return new ApplyStatusEffectAction<ManiaSe>(enemy, Value1, Value1, null, null, 0.15f);
            }

            yield return this.BuffAction<Graze>(Value3);
            yield return this.BuffAction<TempElectric>(Value2);
        }
    }
}

