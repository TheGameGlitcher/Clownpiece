using System;
using System.Collections.Generic;
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
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using System.Linq;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.Cards.Neutral.Blue;
using LBoL.EntityLib.StatusEffects;
using Clownpiece.CustomClasses;
using LBoL.Core.StatusEffects;
using Clownpiece.Cards.LunaticCards.NonTeammateCards;
using Clownpiece.Status;
using LBoL.Core.Units;

namespace Clownpiece.Cards.CardsB
{
    public sealed class MixedMadnessDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MixedMadness);
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
            Type: CardType.Skill,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Red = 1},
            UpgradedCost: new ManaGroup() { Hybrid = 1, HybridColor = 7 },
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
            ActiveCost2: null,
            UpgradedActiveCost: null,
            UpgradedActiveCost2: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Block,
            UpgradedRelativeKeyword: Keyword.Block,

            RelativeEffects: new List<string>() { "DummyLunacySe", "ManiaSe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe", "ManiaSe" },
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
    [EntityLogic(typeof(MixedMadnessDef))]
    public sealed class MixedMadness : ClownCard
    {
        public MixedMadness() : base()
        {
            Value3 = 1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            EnemyUnit target = selector.SelectedEnemy;

            if (base.Battle.Player.HasStatusEffect<LunaticTorchSe>())
                yield return new DrawManyCardAction(Value3);

            else
                yield return new ApplyStatusEffectAction<LunaticTorchSe>(base.Battle.Player, Value1, null, null, null, 0.1f);

            if (target.IsAlive)
            {
                if (base.Battle.Player.HasStatusEffect<ManicFlameSe>())
                    yield return new ApplyStatusEffectAction<PermaManiaSe>(target, Value2, null, null, null, 0.1f);

                else
                    yield return new ApplyStatusEffectAction<ManiaSe>(target, Value2, null, null, null, 0.1f);
            }
        }
    }
}
