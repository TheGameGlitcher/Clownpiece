using Clownpiece.Cards.LunaticCards.LunaticFairyTeammates;
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
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clownpiece.Cards.CardsNone
{
    public sealed class LunaticTorchIgnitionDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticTorchIgnition);
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

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { },
            IsXCost: false,
            Cost: new ManaGroup() { },
            UpgradedCost: new ManaGroup() { },
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
            Value2: 0,
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

            Keywords: Keyword.Exile | Keyword.Retain,
            UpgradedKeywords: Keyword.Exile | Keyword.Retain,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "LunaticTorchSe" },
            UpgradedRelativeEffects: new List<string>() { "LunaticTorchSe", "Firepower" },
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
    [EntityLogic(typeof(LunaticTorchIgnitionDef))]
    public sealed class LunaticTorchIgnition : ClownCard
    {
        public LunaticTorchIgnition() : base()
        {
            Value3 = 1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            IEnumerable<EnemyUnit> enemies = base.Battle.AllAliveEnemies;

            yield return new ApplyStatusEffectAction<LunaticTorchSe>(base.Battle.Player, Value1, null, null, null, 0f, true);

            if (IsUpgraded)
            {
                yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, Value2, null, null, null, 0f, true);

                foreach (EnemyUnit enemy in enemies)
                {
                    yield return new ApplyStatusEffectAction<Firepower>(enemy, Value3, null, null, null, 0f, true);
                }
            }
        }
    }
}
