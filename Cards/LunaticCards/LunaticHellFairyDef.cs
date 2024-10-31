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
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using Clownpiece.CustomClasses;


namespace Clownpiece.Cards.CardsB
{
    public sealed class LunaticHellFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticHellFairy);
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
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: 5,
            UpgradedDamage: 8,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 5,
            UpgradedValue1: null,
            Value2: 3,
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
            UpgradedActiveCost: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Expel,
            UpgradedRelativeKeyword: Keyword.Expel,

            RelativeEffects: new List<string>() { "TempFirepowerNegative", "Firepower", "FirepowerNegative" },
            UpgradedRelativeEffects: new List<string>() { "TempFirepowerNegative", "Firepower", "FirepowerNegative" },
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

    [EntityLogic(typeof(LunaticHellFairyDef))]
    public sealed class LunaticHellFairy : ClownCard
    {
        public LunaticHellFairy() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(HellFairy);
            Value3 = 2;
            Value4 = 1;
            Value5 = 5;
            Value6 = 1;
            Value7 = 2;
        }
        public DamageInfo activeDmg
        {
            get
            {
                return DamageInfo.Attack(this.Value1, false);
            }
        }

        int count = 0;

        protected override void OnEnterBattle(BattleController battle)
        {
            this.ReactBattleEvent<DieEventArgs>(this.Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(this.OnEnemyDied));
            this.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, this.OnPlayerTurnStarting);
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return this.GetPassiveActions();
        }

        public IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.DieSource == this && !args.Unit.HasStatusEffect<Servant>() && !(base.Battle.BattleShouldEnd))
            {
                yield return this.DebuffAction<TempFirepowerNegative>(base.Battle.Player, Value3);
                yield return this.BuffAction<Firepower>(Value4);
            }
        }

        public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (base.Loyalty == 4)
            {
                base.Loyalty = 1 + base.Loyalty + base.ActiveCost;

                IsActiveUsed = true;

                for (int atkNum = 1; atkNum <= Value5; atkNum++)
                {
                    EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                    if (target != null)
                    {
                        yield return AttackAction(target, this.Damage, "火激光B");
                    }
                }

                IsActiveUsed = false;
            }
        }

        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (args.Cause == ActionCause.Card && args.ActionSource == this && IsActiveUsed == true)
            {
                count++;
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.Damage > 0f && count >= 2)
                {
                    yield return DebuffAction<FirepowerNegative>(base.Battle.Player, Value6);
                    count = 0;
                }
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
                yield return PerformAction.Effect(base.Battle.Player, "LunaticHellFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                for (int atkNum = 1; atkNum <= this.Value2; atkNum++)
                {
                    EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                    if (target != null)
                    {
                        yield return AttackAction(target, activeDmg, "火激光B");
                    }
                }
            }
        }
    }
}
