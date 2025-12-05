using Clownpiece.Cards.FairyTeammates;
using Clownpiece.CustomClasses;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;

namespace Clownpiece.Cards.LunaticCards.LunaticFairyTeammates
{
    public sealed class LunaticBlackButterflyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticBlackButterfly);
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

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Friend,
            TargetType: TargetType.RandomEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 10,
            UpgradedDamage: 15,
            Block: null,
            UpgradedBlock: null,
            Shield: 10,
            UpgradedShield: null,
            Value1: 8,
            UpgradedValue1: 10,
            Value2: 2,
            UpgradedValue2: 1,
            Mana: null,
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: 1,
            UpgradedLoyalty: null,
            PassiveCost: 1,
            UpgradedPassiveCost: null,
            ActiveCost: -4,
            ActiveCost2: null,
            UpgradedActiveCost: null,
            UpgradedActiveCost2: null,
            UltimateCost: -7,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Expel | Keyword.Shield,
            UpgradedRelativeKeyword: Keyword.Expel | Keyword.Shield,

            RelativeEffects: new List<string>() { "DummyTorchLinkedFairySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedFairySe" },
            RelativeCards: new List<string>() { "BlackButterfly" },
            UpgradedRelativeCards: new List<string>() { "BlackButterfly+" },

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

    [EntityLogic(typeof(LunaticBlackButterflyDef))]
    public sealed class LunaticBlackButterfly : ClownCard
    {
        public LunaticBlackButterfly() : base()
        {
            IsTorchLinkedFairy = true;
            IsTransformed = true;
            TransformTo = typeof(BlackButterfly);
        }

        public DamageInfo LifeLoss
        {
            get
            {
                if (Battle != null)
                {
                    return DamageInfo.HpLose(Value1 + base.Battle.Player.TotalFirepower, false);
                }

                else
                {
                    return DamageInfo.HpLose(Value1, false);
                }
            }
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(OnEnemyDied));
            ReactBattleEvent(Battle.Player.TurnStarted, OnPlayerTurnStarting);
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return GetPassiveActions();
        }
        public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (Battle.BattleShouldEnd)
                yield break;

            else if (Loyalty >= 4)
            {
                Loyalty = 1 + Loyalty + ActiveCost;
                IsActiveUsed = true;
                NotifyActivating();

                yield return PassiveOne();
                yield return PassiveTwo();

                yield return new DiscardAction(this);
                IsActiveUsed = false;
            }
        }

        public IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.DieSource == this && !args.Unit.HasStatusEffect<Servant>() && IsActiveUsed)
            {
                yield return SacrificeAction(Value2);
                yield return DefenseAction();
            }
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!Summoned || Battle.BattleShouldEnd)
            {
                yield break;
            }
            NotifyActivating();
            Loyalty += PassiveCost;
            for (int i = 0; i < Battle.FriendPassiveTimes && !Battle.BattleShouldEnd; ++i)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return PerformAction.Effect(Battle.Player, "LunaticBlackButterfly", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                switch (base.BattleRng.NextInt(0, 1))
                {
                    case 0:
                        yield return PassiveOne();
                        break;

                    case 1:
                        yield return PassiveTwo();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException($"{nameof(LunaticBlackButterfly)}'s chosen passive ID is invalid!");
                }
            }
        }

        public BattleAction PassiveOne()
        {
            EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);

            if (target != null)
                return AttackAction(target, Damage, "BlackFairy3");

            else
                throw new ArgumentNullException("Attack target can not be null!");
        }

        public BattleAction PassiveTwo()
        {
            return AttackAction(this.Battle.AllAliveEnemies, "BlackFairy4", LifeLoss);
        }
    }
}
