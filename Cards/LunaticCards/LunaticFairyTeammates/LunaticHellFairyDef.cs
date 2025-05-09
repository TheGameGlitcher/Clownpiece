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
using Clownpiece.Cards.FairyTeammates;

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
            Damage: 4,
            UpgradedDamage: 6,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 4,
            UpgradedValue1: 6,
            Value2: 2,
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

            RelativeEffects: new List<string>() { "Firepower", "FirepowerNegative" },
            UpgradedRelativeEffects: new List<string>() { "Firepower", "FirepowerNegative" },
            RelativeCards: new List<string>() { "HellFairy" },
            UpgradedRelativeCards: new List<string>() { "HellFairy+" },

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

    [EntityLogic(typeof(LunaticHellFairyDef))]
    public sealed class LunaticHellFairy : ClownCard
    {
        public LunaticHellFairy() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(HellFairy);
            Value3 = 2;
            Value4 = 1;
            Value5 = 4;
            Value6 = 1;
            Value7 = 4;
        }
        public DamageInfo activeDmg
        {
            get
            {
                return DamageInfo.Attack(Value1, false);
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

                for (int atkNum = 1; atkNum <= Value5; atkNum++)
                {
                    EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                    if (target != null)
                    {
                        yield return AttackAction(target, Damage, "火激光B");
                    }
                }

                yield return new DiscardAction(this);
                IsActiveUsed = false;
            }
        }

        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (args.Cause == ActionCause.Card && args.ActionSource == this && IsActiveUsed == true)
            {
                count++;
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.Damage > 0f && count >= Value7)
                {
                    yield return DebuffAction<FirepowerNegative>(Battle.Player, Value6);
                    count = 0;
                }
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
                yield return PerformAction.Effect(Battle.Player, "LunaticHellFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                for (int atkNum = 1; atkNum <= Value2; atkNum++)
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
