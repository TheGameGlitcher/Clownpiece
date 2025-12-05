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
using UnityEngine;

namespace Clownpiece.Cards.LunaticCards.LunaticFairyTeammates
{
    public sealed class LunaticHellFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticHellFairy);
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
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 6,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 3,
            UpgradedValue1: 4,
            Value2: 4,
            UpgradedValue2: null,
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
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Expel,
            UpgradedRelativeKeyword: Keyword.Expel,

            RelativeEffects: new List<string>() { "DummyTorchLinkedFairySe", "Firepower", "FirepowerNegative" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedFairySe", "Firepower", "FirepowerNegative" },
            RelativeCards: new List<string>() { "HellFairy" },
            UpgradedRelativeCards: new List<string>() { "HellFairy+" },

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

    [EntityLogic(typeof(LunaticHellFairyDef))]
    public sealed class LunaticHellFairy : ClownCard
    {
        public LunaticHellFairy() : base()
        {
            IsTorchLinkedFairy = true;
            IsTransformed = true;
            TransformTo = typeof(HellFairy);
            Value3 = 4;
            Value4 = 5;
            Value5 = 5;
            Value6 = 1;

        }

        public DamageInfo Damage2
        {
            get
            {
                return DamageInfo.Attack(Value2, false);
            }
        }

        public int UpgradedValue
        {
            get
            {
                if (IsUpgraded)
                    return Value4;
                else
                    return Value3;
            }
        }

        int count = 0;

        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(OnEnemyDied));
            ReactBattleEvent(Battle.Player.TurnStarted, OnPlayerTurnStarting);
            ReactBattleEvent(Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(OnPlayerDamageDealt));
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return GetPassiveActions();
        }

        public IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.DieSource == this && !args.Unit.HasStatusEffect<Servant>() && !Battle.BattleShouldEnd)
            {
                yield return DebuffAction<TempFirepowerNegative>(Battle.Player, Value3);
                yield return BuffAction<Firepower>(Value4);
            }
        }

        public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (Battle.BattleShouldEnd)
                yield break;

            else if (Loyalty >= 4)
            {
                Loyalty = 1 + Loyalty + ActiveCost;
                NotifyActivating();

                IsActiveUsed = true;

                foreach (BattleAction ba in PassiveOne()) yield return ba;
                foreach (BattleAction ba in PassiveTwo()) yield return ba;

                yield return new DiscardAction(this);
                IsActiveUsed = false;
            }
        }

        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (Battle.BattleShouldEnd)
                yield break;

            if (args.Cause == ActionCause.Card && args.ActionSource == this && IsActiveUsed)
            {
                count++;
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.Damage > 0f && count >= Value5)
                {
                    yield return DebuffAction<FirepowerNegative>(Battle.Player, Value6);
                    count = 0;
                }
            }
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!Summoned || Battle.BattleShouldEnd)
                yield break;

            NotifyActivating();
            Loyalty += PassiveCost;

            for (int i = 0; i < Battle.FriendPassiveTimes && !Battle.BattleShouldEnd; ++i)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return PerformAction.Effect(Battle.Player, "LunaticHellFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                switch (base.BattleRng.NextInt(0, 1))
                {
                    case 0:
                        foreach (BattleAction ba in PassiveOne()) yield return ba;
                        break;

                    case 1:
                        foreach (BattleAction ba in PassiveTwo()) yield return ba;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException($"{nameof(LunaticHellFairy)}'s chosen passive ID is invalid!");
                }
            }
        }

        public IEnumerable<BattleAction> PassiveOne()
        {
            for (int atkNum = 1; atkNum <= Value1; atkNum++)
            {
                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);

                if (target != null)
                    yield return AttackAction(target, Damage, "火激光B");

                else
                    throw new ArgumentNullException("Attack target can not be null!");
            }
        }

        public IEnumerable<BattleAction> PassiveTwo()
        {
            for (int atkNum = 1; atkNum <= UpgradedValue; atkNum++)
            {
                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);

                if (target != null)
                    yield return AttackAction(target, Damage2, "火激光");

                else
                    throw new ArgumentNullException("Attack target can not be null!");
            }
        }
    }
}
