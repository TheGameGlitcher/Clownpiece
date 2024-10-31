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
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using Clownpiece.CustomClasses;
using System.Linq;
using System.Collections;


namespace Clownpiece.Cards.CardsB
{
    public sealed class MoonFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MoonFairy);
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
            Type: CardType.Friend,
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Colorless },
            IsXCost: false,
            Cost: new ManaGroup() { Colorless = 1, Any = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: 12,
            UpgradedDamage: 15,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: null,
            Value2: null,
            UpgradedValue2: null,
            Mana: null,
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: 5,
            UpgradedLoyalty: 6,
            PassiveCost: 1,
            UpgradedPassiveCost: null,
            ActiveCost: null,
            UpgradedActiveCost: null,
            UltimateCost: -8,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Purified,
            UpgradedRelativeKeyword: Keyword.Purified,

            RelativeEffects: new List<string>() { "DummyLunacySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe" },
            RelativeCards: new List<string>() { "LunaticMoonFairy" },
            UpgradedRelativeCards: new List<string>() { "LunaticMoonFairy+" },

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

    [EntityLogic(typeof(MoonFairyDef))]
    public sealed class MoonFairy : ClownCard
    {
        public MoonFairy() : base()
        {
            CanTransform = true;
            TransformTo = typeof(LunaticMoonFairy);
        }


        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return this.GetPassiveActions();
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            for (int i = 0; i < this.Battle.FriendPassiveTimes && !this.Battle.BattleShouldEnd; ++i)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return PerformAction.Effect(base.Battle.Player, "MoonFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return AttackAction(this.Battle.EnemyGroup.Alives, "冰封噩梦B");
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {

            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return AttackAction(this.Battle.EnemyGroup.Alives, "冰封噩梦B");

                List<Card> list = (from card in base.Battle.HandZone where !card.IsPurified && card.Cost.HasTrivial select card).ToList<Card>();
                if (list.Count > 0)
                {
                    for (int i = 0; i < Value1; i++)
                    {
                        Card card2 = list.Sample(base.GameRun.BattleRng);
                        card2.NotifyActivating();
                        card2.IsPurified = true;
                    }
                }
                else
                {
                    List<Card> list2 = (from card in base.Battle.HandZone where !card.IsPurified select card).ToList<Card>();
                    if (list2.Count > 0)
                    {
                        for (int i = 0; i < Value1;  i++)
                        {
                            Card card2 = list2.Sample(base.GameRun.BattleRng);
                            card2.NotifyActivating();
                            card2.IsPurified = true;
                        }
                    }
                }
            }
        }
    }
}
