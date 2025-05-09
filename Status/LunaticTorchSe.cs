using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System.Linq;
using LBoL.Base.Extensions;
using LBoL.EntityLib.Cards.Enemy;
using System;
using Clownpiece.Cards.CardsB;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.Presentation.UI.Panels;
using Clownpiece.CustomClasses;
using LBoL.Presentation.UI;



namespace Clownpiece.Status
{
    public sealed class LunaticTorchSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticTorchSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "StatusEffectEn.yaml");
            return loc;
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("LunaticTorchSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Special,
                            IsVerbose: false,
                            IsStackable: true,
                            StackActionTriggerLevel: null,
                            HasLevel: false,
                            LevelStackType: StackType.Add,
                            HasDuration: false,
                            DurationStackType: StackType.Add,
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: false,
                            CountStackType: StackType.Keep,
                            LimitStackType: StackType.Keep,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { "LunaticLightSe"},
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "LunaticTorch"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(LunaticTorchSeDef))]
    public sealed class LunaticTorchSe : ClownStatus
    {
		protected override void OnAdded(Unit unit)
        { 
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, this.OnPlayerTurnEnding);
            this.ReactOwnerEvent<CardsEventArgs>(this.Battle.CardsAddedToDiscard, this.OnAddCard);
            this.ReactOwnerEvent<CardsEventArgs>(this.Battle.CardsAddedToHand, this.OnAddCard);
            this.ReactOwnerEvent<CardsEventArgs>(this.Battle.CardsAddedToExile, this.OnAddCard);
            this.ReactOwnerEvent<CardsAddingToDrawZoneEventArgs>(this.Battle.CardsAddedToDrawZone, this.OnAddCardToDraw);

            React(PerformAction.Chat(base.Battle.Player, "Hey, fairies!\nLet's crank up the speed!", 2.2f, 0f, 0.1f));

            List<Card> cards = base.Battle.EnumerateAllCards().Where((Card card) => (card is ClownCard cCard) && (cCard.CanTransform == true)).ToList<Card>();

            if (cards.Count > 0 && base.Owner.HasStatusEffect<LunaticTorchSe>())
            {
                UiManager.GetPanel<PlayBoard>().CancelTargetSelecting(true);
                UiManager.GetPanel<PlayBoard>().RewindRequests();

                foreach (ClownCard sourceCard in cards)
                {
                    Card card = Library.CreateCard(sourceCard.TransformTo, sourceCard.IsUpgraded);
                    card.Summoned = sourceCard.Summoned;

                    base.React(new TransformCardAction(sourceCard, card));
                }
            }

            foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup.Alives)
            {
                if ((enemyUnit.IsAlive) && !(enemyUnit.HasStatusEffect<LunaticLightSe>()))
                {
                    base.React(new ApplyStatusEffectAction<LunaticLightSe>(enemyUnit, new int?(1), null, null, null, 0.2f, false));
                }
            }
        }

        protected override void OnRemoved(Unit unit)
        {
            if (base.Battle.BattleShouldEnd)
                return;

            List<Card> cards = base.Battle.EnumerateAllCards().Where((Card card) => (card is ClownCard cCard) && (cCard.IsTransformed == true)).ToList<Card>();
            if (cards.Count > 0 && !(base.Owner.HasStatusEffect<LunaticTorchSe>()))
            {
                foreach (ClownCard sourceCard in cards)
                {
                    Card card = Library.CreateCard(sourceCard.TransformTo, sourceCard.IsUpgraded);
                    card.Summoned = sourceCard.Summoned;

                    base.React(new TransformCardAction(sourceCard, card));
                }

                foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup.Alives)
                {
                    if ((enemyUnit.IsAlive) && (enemyUnit.HasStatusEffect<LunaticLightSe>()))
                    {
                        base.React(new RemoveStatusEffectAction(enemyUnit.GetStatusEffect<LunaticLightSe>()));
                    }
                }
            }
        }

        private IEnumerable<BattleAction> OnAddCard(CardsEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            List<Card> cards = base.Battle.EnumerateAllCards().Where((Card card) => (card is ClownCard cCard) && (cCard.CanTransform == true)).ToList<Card>();
            if (cards.Count > 0 && base.Owner.HasStatusEffect<LunaticTorchSe>())
            {
                foreach (ClownCard sourceCard in cards)
                {
                    Card card = Library.CreateCard(sourceCard.TransformTo, sourceCard.IsUpgraded);
                    card.Summoned = sourceCard.Summoned;

                    yield return new TransformCardAction(sourceCard, card);
                }
            }
        }

        private IEnumerable<BattleAction> OnAddCardToDraw(CardsAddingToDrawZoneEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            List<Card> cards = base.Battle.EnumerateAllCards().Where((Card card) => (card is ClownCard cCard) && (cCard.CanTransform == true)).ToList<Card>();
            if (cards.Count > 0 && base.Owner.HasStatusEffect<LunaticTorchSe>())
            {
                foreach (ClownCard sourceCard in cards)
                {
                    Card card = Library.CreateCard(sourceCard.TransformTo, sourceCard.IsUpgraded);
                    card.Summoned = sourceCard.Summoned;

                    yield return (BattleAction)new TransformCardAction(sourceCard, card);
                }
            }
        }
        private IEnumerable<BattleAction> OnPlayerTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            foreach (EnemyUnit enemyUnit in base.Battle.EnemyGroup.Alives)
            {
                if ((enemyUnit.IsAlive) && !(enemyUnit.HasStatusEffect<LunaticLightSe>()))
                {
                    yield return new ApplyStatusEffectAction<LunaticLightSe>(enemyUnit, new int?(1), null, null, null, 0.2f, false);
                }
            }
        }
    }
}
