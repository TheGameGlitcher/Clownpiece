using Clownpiece.CustomClasses;

using Clownpiece.Status;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Clownpiece.Cards.CardsRB
{
    public sealed class MoonQuakeDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MoonQuake);
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

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Attack,
            TargetType: TargetType.AllEnemies,
            Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Hybrid = 2, HybridColor = 7 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 15,
            UpgradedDamage: 18,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: null,
            Value2: 1,
            UpgradedValue2: 2,
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

            RelativeEffects: new List<string>() { "Vulnerable" },
            UpgradedRelativeEffects: new List<string>() { "Vulnerable" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

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
    [EntityLogic(typeof(MoonQuakeDef))]
    public sealed class MoonQuake : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (this.IsUpgraded)
            {
                yield return this.AttackAction((IEnumerable<Unit>)this.Battle.AllAliveEnemies, "Junko3B");
            }
            else
            {
                yield return this.AttackAction((IEnumerable<Unit>)this.Battle.AllAliveEnemies, "Junko3C");
            }
            foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup.Alives)
            {
                if (enemyUnit.IsAlive)
                {
                    yield return new ApplyStatusEffectAction<Vulnerable>(enemyUnit, Value2, Value2, null, null, 0.1f, true);
                }
            }

            yield return new ApplyStatusEffectAction<Vulnerable>(base.Battle.Player, Value1, Value1, null, null, 0.1f, true);
        }
    }
}