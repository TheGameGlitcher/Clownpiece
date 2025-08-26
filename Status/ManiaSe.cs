using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;
using Clownpiece.Localization;
using Cysharp.Threading.Tasks.Triggers;
using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Intentions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using static UnityEngine.UI.GridLayoutGroup;

namespace Clownpiece.Status
{
    public sealed class ManiaSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ManiaSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return ClownpieceLocalization.StatusEffectsBatchLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("ManiaSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Negative,
                            IsVerbose: false,
                            IsStackable: true,
                            StackActionTriggerLevel: null,
                            HasLevel: true,
                            LevelStackType: StackType.Add,
                            HasDuration: false,
                            DurationStackType: StackType.Add,
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: false,
                            CountStackType: StackType.Add,
                            LimitStackType: StackType.Add,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Lunatic"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(ManiaSeDef))]
    public sealed class ManiaSe : ClownStatus
    {
        bool error = false;
        IEnemyMove attack;
        Intention intention;

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, this.OnPlayerTurnStarting);
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, this.OnEnemyTurnStarting);

            EnemyUnit enemy = (EnemyUnit)this.Owner;

            this.randomAttack();

            List<Intention> intentions = enemy.Intentions.Where(intention =>
            {
                return intention is SpellCardIntention || intention is AttackIntention || intention is CountDownIntention || intention is DoNothingIntention
                    || intention is SpawnIntention || intention is KokoroDarkIntention || intention is SleepIntention;
            }).ToList();

            List<IEnemyMove> _turnMoves = enemy._turnMoves.Where(turnMove =>
            {
                return turnMove.Intention is SpellCardIntention || turnMove.Intention is AttackIntention || turnMove.Intention is CountDownIntention || intention is DoNothingIntention
                    || turnMove.Intention is SpawnIntention || turnMove.Intention is KokoroDarkIntention || turnMove.Intention is SleepIntention;
            }).ToList();

            intention.Source = enemy;

            if (!(intentions.Any(intention => intention is AttackIntention)) &&
               (intentions.Any(intention => intention is SpellCardIntention spell && spell.Damage == null) ||
               !(intentions.Any(intention => intention is SpellCardIntention))))
            {
                base.NotifyActivating();
                _turnMoves.Add(attack);
                intentions.Add(intention);
            }

            enemy._turnMoves.Clear();
            enemy.ClearIntentions();

            foreach (IEnemyMove attack2 in _turnMoves)
                enemy._turnMoves.Add(attack2);

            enemy.Intentions = intentions;
            enemy.NotifyIntentionsChanged();
        }


        private IEnumerable<BattleAction> OnEnemyTurnStarting(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            Level--;

            if (Level == 0)
                yield return new RemoveStatusEffectAction(this);
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            EnemyUnit enemy = (EnemyUnit)this.Owner;

            this.randomAttack();

            List<Intention> intentions = enemy.Intentions.Where(intention =>
            {
                return intention is SpellCardIntention || intention is AttackIntention || intention is CountDownIntention || intention is DoNothingIntention
                    || intention is SpawnIntention || intention is KokoroDarkIntention || intention is SleepIntention;
            }).ToList();

            List<IEnemyMove> _turnMoves = enemy._turnMoves.Where(turnMove =>
            {
                return turnMove.Intention is SpellCardIntention || turnMove.Intention is AttackIntention || turnMove.Intention is CountDownIntention || intention is DoNothingIntention
                    || turnMove.Intention is SpawnIntention || turnMove.Intention is KokoroDarkIntention || turnMove.Intention is SleepIntention;
            }).ToList();

            intention.Source = enemy;

            if (!(intentions.Any(intention => intention is AttackIntention)) && 
               (intentions.Any(intention => intention is SpellCardIntention spell && spell.Damage == null) || 
               !(intentions.Any(intention => intention is SpellCardIntention))))
            {
                base.NotifyActivating();
                _turnMoves.Add(attack);
                intentions.Add(intention);
            }

            enemy._turnMoves.Clear();
            enemy.ClearIntentions();

            foreach (IEnemyMove attack2 in _turnMoves)
                enemy._turnMoves.Add(attack2);

            enemy.Intentions = intentions;
            enemy.NotifyIntentionsChanged();
        }


        private void randomAttack()
        {
            EnemyUnit enemy = (EnemyUnit)this.Owner;
            int level = base.GameRun.CurrentStage.Level;
            EnemyType encounterType = base.Battle.EnemyGroup.EnemyType;
            GameDifficulty difficulty = base.GameRun.Difficulty;
            int RNG = base.GameRun.BattleRng.NextInt(1, 20);
            int atkDamage = 0;
            int atkTimes = 0;
            bool accuracy = false;
            string atkName = "";

            if (RNG >= 1 && RNG <= 9)
            {
                atkName = "Manic Barrage";

                switch (level)
                {
                    case 1:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 2;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }
                                        else
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;

                                        }
                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 2:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 7;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 3;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }
                                        else
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 3;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 7;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 3:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 3;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 3;
                                        }    
                                        else
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 10;
                                            atkTimes = 3;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 4:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        atkDamage = 7;
                                        atkTimes = 4;

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        atkDamage = 8;
                                        atkTimes = 4;

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        atkDamage = 8;
                                        atkTimes = 5;

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    default:
                        {
                            error = true;
                            break;
                        }
                }
            }

            else if (RNG >= 10 && RNG <= 19)
            {
                atkName = "Manic Bash";
                atkTimes = 1;

                switch (level)
                {
                    case 1:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 11;
                                        }
                                        else
                                        {
                                            atkDamage = 8;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 12;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 10;
                                        }
                                        else
                                        {
                                            atkDamage = 9;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 12;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 10;
                                        }
                                        else
                                        {
                                            atkDamage = 9;
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }


                            }

                            break;
                        }

                    case 2:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 14;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 12;
                                        }
                                        else
                                        {
                                            atkDamage = 10;
                                        }


                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 15;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 13;
                                        }
                                        else
                                        {
                                            atkDamage = 11;
                                        }


                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 18;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 15;
                                        }
                                        else
                                        {
                                            atkDamage = 11;
                                        }


                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 3:
                        {
                            accuracy = true;

                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 15;
                                        }
                                        else
                                        {
                                            atkDamage = 10;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 20;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 15;
                                        }
                                        else
                                        {
                                            atkDamage = 10;
                                        }


                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 25;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 20;
                                        }
                                        else
                                        {
                                            atkDamage = 15;
                                        }


                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 4:
                        {
                            atkTimes = 1;
                            accuracy = true;

                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        atkDamage = 35;

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        atkDamage = 40;

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        atkDamage = 45;

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    default:
                        {
                            error = true;
                            break;
                        }
                }
            }

            else if (RNG == 20)
            {
                atkName = "Overdrive Barrage";

                switch (level)
                {
                    case 1:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 2;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 2;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }
                                        else
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 3;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 7;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 2:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 3;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 3;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 7;
                                            atkTimes = 3;
                                        }
                                        else
                                        {
                                            atkDamage = 8;
                                            atkTimes = 2;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 9;
                                            atkTimes = 4;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 5;
                                            atkTimes = 4;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 3;
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 3:
                        {
                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 7;
                                            atkTimes = 4;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 3;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 2;
                                            accuracy = true;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 2;
                                            accuracy = true;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 2;
                                            accuracy = true;
                                        }

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        if (encounterType == EnemyType.Boss)
                                        {
                                            atkDamage = 8;
                                            atkTimes = 3;
                                            accuracy = true;
                                        }
                                        else if (encounterType == EnemyType.Elite)
                                        {
                                            atkDamage = 6;
                                            atkTimes = 3;
                                            accuracy = true;
                                        }
                                        else
                                        {
                                            atkDamage = 5;
                                            atkTimes = 3;
                                            accuracy = true;
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        error = true;
                                        break;
                                    }
                            }

                            break;
                        }

                    case 4:
                        {
                            atkName = "Overturning Barrage";

                            switch (difficulty)
                            {
                                case GameDifficulty.Easy:
                                case GameDifficulty.Normal:
                                    {
                                        atkDamage = 4;
                                        atkTimes = 8;

                                        break;
                                    }

                                case GameDifficulty.Hard:
                                    {
                                        atkDamage = 4;
                                        atkTimes = 9;
                                        accuracy = true;

                                        break;
                                    }

                                case GameDifficulty.Lunatic:
                                    {
                                        atkDamage = 4;
                                        atkTimes = 10;
                                        accuracy = true;

                                        break;
                                    }

                                default:
                                {
                                    error = true;
                                    break;
                                }
                            }

                            break;
                        }

                    default:
                        {
                            error = true;
                            break;
                        }
                }
            }

            else
            {
                error = true;
            }

            if (error)
                Debug.Log("If you're seeing this, the random attack replacement somehow failed picking an outcome.");

            attack = enemy.AttackMove(atkName, enemy.Gun1, atkDamage, atkTimes, accuracy, "Instant", false);
            intention = Intention.Attack(atkDamage, atkTimes, accuracy);
        }
    }
}
