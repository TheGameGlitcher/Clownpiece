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
using Clownpiece.Status;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.Core.Units;
using Clownpiece.CustomClasses;
using LBoL.EntityLib.StatusEffects.Others;

namespace Clownpiece.Cards.CardsR
{
    public sealed class MonsterClownDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MonsterClown);
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
            GunName: "冰封噩梦B",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Rare,
            Type: CardType.Skill,
            TargetType: TargetType.AllEnemies,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 3 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 24,
            UpgradedDamage: 15,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
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
            ActiveCost2: null,
            UpgradedActiveCost2: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.Accuracy,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "Weak", "Vulnerable", "LockedOn", "Poison", "TempFirepowerNegative" },
            UpgradedRelativeEffects: new List<string>() { "Weak", "Vulnerable", "LockedOn", "Poison", "TempFirepowerNegative" },
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
    [EntityLogic(typeof(MonsterClownDef))]
    public sealed class MonsterClown : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (this.IsUpgraded)
            {
                for (int i = 1; i <= Value2; i++)
                {
                    yield return AttackAction(selector);
                }
            }
            else
            {
                yield return AttackAction(selector);
            }

            foreach (EnemyUnit enemy in base.Battle.AllAliveEnemies)
            {
                yield return new ApplyStatusEffectAction<Weak>(enemy, Value1, Value1, null, null, 0.15f);
                yield return new ApplyStatusEffectAction<Vulnerable>(enemy, Value1, Value1, null, null, 0.15f);
                yield return new ApplyStatusEffectAction<LockedOn>(enemy, Value1, Value1, null, null, 0.15f);
                yield return new ApplyStatusEffectAction<Poison>(enemy, Value1, Value1, null, null, 0.15f);
                yield return new ApplyStatusEffectAction<TempFirepowerNegative>(enemy, Value1, Value1, null, null, 0.15f);
            }
        }
    }
}
