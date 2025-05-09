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
using System.Linq;
using Clownpiece.Cards.FairyTeammates;

namespace Clownpiece.Cards.LunaticCards.LunaticFairyTeammates
{
    public sealed class LunaticMoonFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticMoonFairy);
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
            Colors: new List<ManaColor>() { ManaColor.Colorless },
            IsXCost: false,
            Cost: new ManaGroup() { Colorless = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 9,
            UpgradedDamage: 10,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: 2,
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
            RelativeKeyword: Keyword.Purified,
            UpgradedRelativeKeyword: Keyword.Purified,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { "MoonFairy" },
            UpgradedRelativeCards: new List<string>() { "MoonFairy+" },

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

                if (Battle != null)
                {
                    if (Battle.Player.HasStatusEffect<PermaPurifySe>())
                    {
                        StatusEffect status = Battle.Player.GetStatusEffect<PermaPurifySe>();
                        totalDmg = totalDmg + Value1 * status.Level;
                    }

                    List<Card> list = (from card in Battle.EnumerateAllCardsButExile() where card.IsPurified select card).ToList();
                    if (list.Count > 0)
                        totalDmg = totalDmg + list.Count * Value1;
                }

                return DamageInfo.Attack(totalDmg, false);
            }
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.Player.TurnStarted, OnPlayerTurnStarting);
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return GetPassiveActions();
        }

        public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (!Summoned || Battle.BattleShouldEnd)
                yield break;

            if (Loyalty >= 4)
            {
                Loyalty = 1 + Loyalty + ActiveCost;
                IsActiveUsed = true;
                NotifyActivating();

                List<Card> list = (from card in Battle.HandZone where !card.IsPurified && card.Cost.HasTrivial select card).ToList();
                foreach (Card card in list)
                {
                    card.NotifyActivating();
                    card.IsPurified = true;
                    Counter++;

                    if (Counter == Value2)
                    {
                        yield return new ApplyStatusEffectAction<PermaPurifySe>(Battle.Player, Value3, Value3, null, null, 0.15f);
                        Counter = 0;
                    }
                }

                yield return new DiscardAction(this);
                IsActiveUsed = false;
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
                yield return PerformAction.Effect(Battle.Player, "LunaticMoonFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return new DamageAction(Battle.Player, Battle.EnemyGroup.Alives, newDmg, "JunkoLunatic");
            }
        }
    }
}
