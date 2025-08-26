using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;
using Clownpiece.Localization;
using Clownpiece.Status;
using Cysharp.Threading.Tasks.Triggers;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Intentions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clownpiece.Cards.CardsR
{
    public sealed class BloodlustDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Bloodlust);
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
            Rarity: Rarity.Uncommon,
            Type: CardType.Skill,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 1, Any = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
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
            UpgradedValue2: 5,
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
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "Vulnerable", "TempFirepower" },
            UpgradedRelativeEffects: new List<string>() { "Vulnerable", "TempFirepower"  },
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
    [EntityLogic(typeof(BloodlustDef))]
    public sealed class Bloodlust : ClownCard
    {
        public Bloodlust() : base()
        {
            Value3 = 1;
            Value4 = 1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int attackCount = 0;
            attackCount = base.Battle.EnemyGroup.Alives.Count((EnemyUnit enemy) => enemy.Intentions.Any(delegate (Intention i)
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

            yield return new ApplyStatusEffectAction<Vulnerable>(selector.SelectedEnemy, Value3, Value3, null, null, 0.15f);
            yield return DebuffAction<Vulnerable>(base.Battle.Player, Value4, Value4);
            yield return new DrawManyCardAction(Value1);

            if (attackCount > 0)
            {
                yield return BuffAction<TempFirepower>(Value2);
            }
        }
    }
}