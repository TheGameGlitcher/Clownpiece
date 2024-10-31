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
using HarmonyLib;

namespace Clownpiece.Cards.CardsR
{
    public sealed class ContinuousWarfareDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ContinuousWarfare);
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
            Type: CardType.Attack,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 1, Any = 2 },
            UpgradedCost: new ManaGroup() { Red = 1, Any = 1 },
            MoneyCost: null,
            Damage: 16,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: null,
            Value2: 1,
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

            Keywords: Keyword.Exile | Keyword.Ethereal,
            UpgradedKeywords: Keyword.Exile | Keyword.Ethereal,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "LockedOn", "Firepower" },
            UpgradedRelativeEffects: new List<string>() { "LockedOn", "Firepower" },
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
    [EntityLogic(typeof(ContinuousWarfareDef))]
    public sealed class ContinuousWarfare : ClownCard
    {
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

            yield return AttackAction(selector.SelectedEnemy);
            yield return new ApplyStatusEffectAction<LockedOn>(selector.SelectedEnemy, Value1, Value1);
            yield return BuffAction<Firepower>(Value2);


            if (attackCount > 0)
            {
                List<Card> cards = new List<Card>();
                Card card = base.CloneBattleCard();
                card.IsCopy = false;
                cards.Add(card);
                yield return new AddCardsToDiscardAction(cards);
                cards = null;
            }

        }
    }
}