﻿using Clownpiece.Cards.Templates;
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
using System.Linq;


namespace Clownpiece.Cards.CardsB
{
    public sealed class LunaticMoonFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticMoonFairy);
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
            Colors: new List<ManaColor>() { ManaColor.Colorless },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 3 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: 10,
            UpgradedDamage: 12,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 2,
            UpgradedValue1: 4,
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
            UpgradedActiveCost: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { "MoonFairy" },
            UpgradedRelativeCards: new List<string>() { "MoonFairy+" },

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

    [EntityLogic(typeof(LunaticMoonFairyDef))]
    public sealed class LunaticMoonFairy : ClownCard
    {
        public LunaticMoonFairy() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(MoonFairy);
            Value3 = 1;
            Counter = 0;
        }
        public DamageInfo newDmg
        {
            get
            {
                int totalDmg = (int)Damage.Amount;

                if (this.Battle != null)
                {
                    if (base.Battle.BattleMana.Colorless > 0)
                        totalDmg = totalDmg + (Value1 * base.Battle.BattleMana.Colorless);

                    List<Card> list = (from card in base.Battle.EnumerateAllCardsButExile() where card.IsPurified select card).ToList<Card>();
                    if (list.Count > 0)
                        totalDmg = totalDmg + (list.Count * Value1);
                }

                return DamageInfo.Attack(totalDmg, false);
            }
        }

        protected override void OnEnterBattle(BattleController battle)
        {
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
                IsActiveUsed = true;
                this.NotifyActivating();

                List<Card> list = (from card in base.Battle.HandZone where !(card.IsPurified) && card.Cost.HasTrivial select card).ToList<Card>();
                foreach (Card card in list)
                {
                    card.NotifyActivating();
                    card.IsPurified = true;
                    Counter++;

                    if (Counter == Value2)
                    {
                        yield return new ApplyStatusEffectAction<PermaPurifySe>(base.Battle.Player, Value3, Value3, null, null, 0.15f);
                        Counter = 0;
                    }
                }

                IsActiveUsed = false;
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
                yield return PerformAction.Effect(base.Battle.Player, "LunaticMoonFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return new DamageAction(base.Battle.Player, this.Battle.EnemyGroup.Alives, newDmg, "JunkoLunatic");
            }
        }
    }
}