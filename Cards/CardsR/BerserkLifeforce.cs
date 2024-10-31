﻿using Clownpiece.Cards.Templates;
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
using System.Diagnostics;

namespace Clownpiece.Cards.CardsR
{
    public sealed class BerserkLifeforceDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BerserkLifeforce);
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
            Rarity: Rarity.Rare,
            Type: CardType.Ability,
            TargetType: TargetType.AllEnemies,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 2, Any = 2 },
            UpgradedCost: null,
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
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "MightMakesRightSe" },
            UpgradedRelativeEffects: new List<string>() { "MightMakesRightSe" },
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
    [EntityLogic(typeof(BerserkLifeforceDef))]
    public sealed class BerserkLifeforce : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup.Alives)
            {
                if (enemyUnit.IsAlive)
                {
                    yield return new ApplyStatusEffectAction<FirepowerIsJustice>(enemyUnit, null, null, null, null, 0.1f);
                }
            }
            yield return this.BuffAction<MightMakesRightSe>(0, 0, 0, 0, 0.1f);
        }
    }
}
