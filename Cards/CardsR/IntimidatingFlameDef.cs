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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using Clownpiece.Status;
using LBoL.Core.Battle.BattleActions;
using Clownpiece.CustomClasses;

namespace Clownpiece.Cards.CardsR
{
    public sealed class IntimidatingFlameDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(IntimidatingFlame);
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
            Rarity: Rarity.Common,
            Type: CardType.Defense,
            TargetType: TargetType.AllEnemies,
            Colors: new List<ManaColor>() { ManaColor.Red },
            Kicker: null,
            UpgradedKicker: null,
            IsXCost: false,
            Cost: new ManaGroup() { Red = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: 15,
            UpgradedBlock: 20,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: null,
            Value2: 3,
            UpgradedValue2: 4,
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

            Keywords: Keyword.Block,
            UpgradedKeywords: Keyword.Block,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { },
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
    [EntityLogic(typeof(IntimidatingFlameDef))]
    public sealed class IntimidatingFlame : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup.Alives)
            {
                if (enemyUnit.IsAlive)
                {
                    yield return new ApplyStatusEffectAction<TempFirepowerNegative>(enemyUnit, new int?(this.Value2), null, null, null, 0f, true);
                }
            }
            yield return new ApplyStatusEffectAction<Weak>(base.Battle.Player, new int?(this.Value1), this.Value1, null, null, 0f, true);
        }
    }
}