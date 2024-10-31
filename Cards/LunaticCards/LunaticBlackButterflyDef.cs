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
using Clownpiece.Cards.CardsR;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using Clownpiece.CustomClasses;


namespace Clownpiece.Cards.CardsB
{
    public sealed class LunaticBlackButterflyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticBlackButterfly);
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

            IsPooled: false,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Friend,
            TargetType: TargetType.RandomEnemy,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: 10,
            UpgradedDamage: 15,
            Block: null,
            UpgradedBlock: null,
            Shield: 10,
            UpgradedShield: null,
            Value1: 10,
            UpgradedValue1: null,
            Value2: 4,
            UpgradedValue2: 2,
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
            UpgradedActiveCost: null,
            UltimateCost: -7,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Expel | Keyword.Shield,
            UpgradedRelativeKeyword: Keyword.Expel | Keyword.Shield,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { "Lunatic" },
            UpgradedRelativeCards: new List<string>() { "Lunatic" },

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

    [EntityLogic(typeof(LunaticBlackButterflyDef))]
    public sealed class LunaticBlackButterfly : ClownCard
    {
        public LunaticBlackButterfly() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(BlackButterfly);
        }
        public DamageInfo activeDmg
        {
            get
            {
                return DamageInfo.Attack(this.Value1, false);
            }
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            this.ReactBattleEvent<DieEventArgs>(this.Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(this.OnEnemyDied));
            this.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, this.OnPlayerTurnStarting);
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return this.GetPassiveActions();
        }
        public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (base.Loyalty == 4)
            {
                base.Loyalty = 1 + base.Loyalty + base.ActiveCost;


                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                if (target != null)
                {
                    yield return (BattleAction)new DamageAction(base.Battle.Player, (IEnumerable<Unit>) this.Battle.EnemyGroup.Alives, DamageInfo.HpLose(Value1), "BlackFairy4");
                    yield return (BattleAction)new AddCardsToDiscardAction(new Card[1] { (Card)Library.CreateCard<Lunatic>() });
                }

                else
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
            }
        }

        public IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.DieSource == this && !args.Unit.HasStatusEffect<Servant>() && !(base.Battle.BattleShouldEnd))
            {
                yield return this.SacrificeAction(Value2);
                yield return this.DefenseAction();
            }
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
                yield return PerformAction.Effect(base.Battle.Player, "LunaticBlackButterfly", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);


                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                if (target != null)
                {
                    yield return AttackAction(target, this.Damage, "BlackFairy3");
                }
            }
        }
    }
}
