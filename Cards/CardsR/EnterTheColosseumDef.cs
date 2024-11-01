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
using Clownpiece.Cards.CardsB;

namespace Clownpiece.Cards.CardsR
{
    public sealed class EnterTheColosseumDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(EnterTheColosseum);
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
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 1, Any = 2 },
            UpgradedCost: new ManaGroup() { Red = 1, Any = 1 },
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 3,
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
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Exile,
            UpgradedKeywords: Keyword.Exile,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "ManiaSe", "TempFirepower", "LockedOn" },
            UpgradedRelativeEffects: new List<string>() { "ManiaSe", "TempFirepower", "LockedOn" },
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
    [EntityLogic(typeof(EnterTheColosseumDef))]
    public sealed class EnterTheColosseum : ClownCard
    {
        public EnterTheColosseum() : base()
        {
            Value3 = 2;
            Value4 = 2;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<TempFirepower>(Value2);
            yield return DebuffAction<LockedOn>(base.Battle.Player, Value3, Value3);
            if (base.Battle.Player.HasStatusEffect<ManicFlameSe>())
                yield return new ApplyStatusEffectAction<PermaManiaSe>(selector.SelectedEnemy, Value1, Value1, null, null, 0.15f);
            else
                yield return new ApplyStatusEffectAction<ManiaSe>(selector.SelectedEnemy, Value1, Value1, null, null, 0.15f);
            yield return new ApplyStatusEffectAction<LockedOn>(selector.SelectedEnemy, Value4, Value4, null, null, 0.15f);
        }
    }
}

