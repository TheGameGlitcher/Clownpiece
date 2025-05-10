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
using Clownpiece.CustomClasses;
using Cysharp.Threading.Tasks.Triggers;
using System.Linq;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using Clownpiece.Cards.CardsB;
using UnityEngine;

namespace Clownpiece.Cards.CardsRB
{
    public sealed class HitAndRunDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HitAndRun);
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
            Type: CardType.Attack,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Red = 1 },
            UpgradedCost: new ManaGroup() { Hybrid = 1, HybridColor = 7, Any = 1 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 14,
            UpgradedDamage: 18,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
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

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Block,
            UpgradedRelativeKeyword: Keyword.Block,

            RelativeEffects: new List<string>() { "Graze" },
            UpgradedRelativeEffects: new List<string>() { "Graze" },
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
    [EntityLogic(typeof(HitAndRunDef))]
    public sealed class HitAndRun : ClownCard
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int attackCount = 0;
            attackCount = Battle.EnemyGroup.Alives.Count((enemy) => enemy.Intentions.Any(delegate (Intention i)
            {
                if (!(i is AttackIntention))
                {
                    SpellCardIntention spellCardIntention = i as SpellCardIntention;
                    if (spellCardIntention == null || spellCardIntention.Damage == null)
                    {
                        return false;
                    }
                }
                return true;
            }));

            yield return AttackAction(selector.SelectedEnemy);

            if (attackCount > 0)
            {
                yield return BuffAction<Graze>(Value1);
            }

        }
    }
}