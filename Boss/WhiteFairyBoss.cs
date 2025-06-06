﻿using Cysharp.Threading.Tasks;
using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Core.Intentions;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.JadeBoxes;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.ReflectionHelpers;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using LBoL.Base.Extensions;
using UnityEngine.UI;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.StatusEffects.Basic;
using Spine;
using LBoL.EntityLib.StatusEffects.Enemy;
using Clownpiece.Cards.Templates;
using static Clownpiece.BepinexPlugin;
using Clownpiece.Status;
using System.Security.Cryptography;
using Clownpiece.CustomClasses;
using static Clownpiece.Boss.BlackFairyBossDef;
using Unity.IO.LowLevel.Unsafe;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation;
using LBoL.EntityLib.Cards.Enemy;

namespace Clownpiece.Boss
{
    public sealed class WhiteFairyBossDef : EnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(WhiteFairyBoss);
        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "EnemyUnitEn.yaml");
            return loc;
        }
        public override EnemyUnitConfig MakeConfig()
        {
            var config = new EnemyUnitConfig(
            Id: "",
            RealName: true,
            OnlyLore: false,
            BaseManaColor: new List<ManaColor>() { ManaColor.Red, ManaColor.Black },
            Order: 10,
            ModleName: "WhiteFairyBoss",
            NarrativeColor: "#ffffff",
            Type: EnemyType.Boss,
            IsPreludeOpponent: false,
            HpLength: null,
            MaxHpAdd: null,
            MaxHp: 130,
            Damage1: 10,
            Damage2: 4,
            Damage3: 20,
            Damage4: 30,
            Power: 1,
            Defend: 15,
            Count1: 1,
            Count2: 2,
            MaxHpHard: 140,
            Damage1Hard: 12,
            Damage2Hard: 5,
            Damage3Hard: 23,
            Damage4Hard: 33,
            PowerHard: 1,
            DefendHard: 17,
            Count1Hard: 1,
            Count2Hard: 2,
            MaxHpLunatic: 160,
            Damage1Lunatic: 12,
            Damage2Lunatic: 5,
            Damage3Lunatic: 25,
            Damage4Lunatic: 35,
            PowerLunatic: 1,
            DefendLunatic: 20,
            Count1Lunatic: 1,
            Count2Lunatic: 2,
            PowerLoot: new MinMax(50, 50),
            BluePointLoot: new MinMax(50, 50),
            Gun1: new List<string> { "WhiteFairy1" },
            Gun2: new List<string> { "WhiteFairy2" },
            Gun3: new List<string> { "WhiteFairy3" },
            Gun4: new List<string> { "WhiteFairy4" }
            );
            return config;
        }
        public sealed class WhiteFairyBossModelDef : UnitModelTemplate
        {

            public override IdContainer GetId() => new WhiteFairyBossDef().UniqueId;

            public override LocalizationOption LoadLocalization()
            {
                var loc = new GlobalLocalization(embeddedSource);
                loc.LocalizationFiles.AddLocaleFile(Locale.En, "UnitModelEn.yaml");
                return loc;
            }

            public override ModelOption LoadModelOptions()
            {
                return new ModelOption(ResourcesHelper.LoadSpineUnitAsync("WhiteFairy"));
            }


            public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("SpellCard.png", directorySource, ppu: 336);


            public override UnitModelConfig MakeConfig()
            {
                var config = UnitModelConfig.FromName("WhiteFairy").Copy();
                return config;

            }

        }

        [EntityLogic(typeof(WhiteFairyBossDef))]
        public sealed class WhiteFairyBoss : ClownEnemyUnit
        {
            public WhiteFairyBoss() : base()
            {
                Counter = 1;
                SpellCounter = 0;
                Gun5 = "冰封噩梦B";
                Gun6 = "JunkoLunatic";
                Gun7 = "Junko3B";
            }
            private string SpellCardName
            {
                get
                {
                    if (this.HasStatusEffect<LunaticTorchBossSe>())
                        return base.GetSpellCardName(new int?(4), 6);
                    else if (this.HasStatusEffect<ManicTorchBossSe>())
                        return base.GetSpellCardName(new int?(4), 7);
                    else if (this.HasStatusEffect<PureTorchBossSe>())
                        return base.GetSpellCardName(new int?(4), 8);
                    else if (this.HasStatusEffect<UnstableTorchBossSe>())
                    {
                        switch (FairyRNG)
                        {
                            case 0:
                                return base.GetSpellCardName(new int?(5), 6);
                            case 1:
                                return base.GetSpellCardName(new int?(5), 7);
                            case 2:
                                return base.GetSpellCardName(new int?(5), 8);
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            public enum MoveType
            {
                Attack,
                MultiAttack,
                Defend,
                AddCards,
                Spell
            }

            MoveType NextMove;
            MoveType TempMove;
            EnemyUnit FairyAlly;

            protected override void OnEnterBattle(BattleController battle)
            {
                base.ReactBattleEvent<GameEventArgs>(base.Battle.BattleStarted, new Func<GameEventArgs, IEnumerable<BattleAction>>(this.OnBattleStarted));
                base.ReactBattleEvent<GameEventArgs>(base.Battle.AllEnemyTurnStarted, new Func<GameEventArgs, IEnumerable<BattleAction>> (this.OnEnemyTurnStarted));
                base.ReactBattleEvent<StatusEffectApplyEventArgs>(base.StatusEffectAdded, new Func<StatusEffectApplyEventArgs, IEnumerable<BattleAction>>(this.OnStatusAdded));
                base.ReactBattleEvent<DieEventArgs>(base.Battle.EnemyDied, new Func<DieEventArgs, IEnumerable<BattleAction>>(this.OnEnemyDied));
            }

            public override void OnSpawn(EnemyUnit spawner)
            {
                this.React(new ApplyStatusEffectAction<MirrorImage>(this, null, null, null, null, 0f, true));
                foreach (EnemyUnit enemy in base.Battle.AllAliveEnemies)
                {
                    if (enemy is BlackFairyBoss)
                        FairyAlly = enemy;
                }
            }

            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
            {
                foreach (EnemyUnit enemy in base.Battle.AllAliveEnemies)
                {
                    if (enemy is BlackFairyBoss)
                        FairyAlly = enemy;
                }
                yield break;
            }

            private IEnumerable<BattleAction> OnEnemyTurnStarted(GameEventArgs args)
            {
                Counter++;
                yield break;
            }

            private IEnumerable<BattleAction> OnStatusAdded(StatusEffectApplyEventArgs args)
            {
                if (args.Effect is LunaticTorchBossSe || args.Effect is ManicTorchBossSe || args.Effect is PureTorchBossSe)
                {
                    switch (SpellCounter)
                    {
                        case 0:
                            yield return PerformAction.Chat(this, "Let's play with this while we can!", 1.50f, 0f, 0f);
                            break;

                        case 1:
                            yield return PerformAction.Chat(this, "Let's swap!", 1.50f, 0f, 1.50f);
                            break;
                            
                        case 2:
                            yield return PerformAction.Chat(this, "No, no, do it like this!", 1.50f, 0f, 1.50f);
                            break;

                        case 3:
                            yield return PerformAction.Chat(this, "Seriously, are you even trying? Watch me!", 1.50f, 0f, 1.50f);
                            break;
                        default:
                            yield return PerformAction.Chat(this, "Give me that!", 1.50f, 0f, 1.50f);
                            break;
                    }
                }

                if (args.Effect is LunaticBossSe)
                {
                    switch (SpellCounter)
                    {
                        case 0:
                            yield return PerformAction.Chat(this, "Let's go!\nLet's go!", 1.50f, 0f, 0.4f);
                            break;

                        case 1:
                            yield return PerformAction.Chat(this, "Sure, we can swap.", 1.50f, 0f, 0.4f);
                            break;

                        case 2:
                            yield return PerformAction.Chat(this, "Come on, I was just getting started!", 1.50f, 0f, 0.4f);
                            break;

                        case 3:
                            yield return PerformAction.Chat(this, "I was doing just fine, look at her health!", 1.50f, 0f, 0.4f);
                            break;
                        default:
                            yield return PerformAction.Chat(this, "Wait, I'm not done yet!", 1.50f, 0f, 0.4f);
                            break;
                    }
                }

                if (args.Effect is UnstableTorchBossSe)
                    yield return PerformAction.Chat(this, "Let's see what this thing can really do!", 1.50f, 0f, 1.50f);
            }

            private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
            {
                if (args.Unit is BlackFairyBoss && this.IsAlive)
                {
                    yield return PerformAction.Chat(this, "Don't worry, I can handle this!", 1.50f, 0f, 1.50f);

                    if (this.HasStatusEffect<LunaticTorchBossSe>())
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<LunaticTorchBossSe>());

                    if (this.HasStatusEffect<ManicTorchBossSe>())
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<ManicTorchBossSe>());

                    if (this.HasStatusEffect<PureTorchBossSe>())
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<PureTorchBossSe>());

                    if (this.HasStatusEffect<LunaticBossSe>())
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<LunaticBossSe>());

                    yield return new ApplyStatusEffectAction<UnstableTorchBossSe>(this, null, null, null, null, 0.2f, true);
                }
            }

            private IEnumerable<BattleAction> FairyAttack(int moveName, int damage, string gunName = null, int attackTimes = 1)
            {
                yield return new EnemyMoveAction(this, base.GetMove(moveName), true);

                yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(damage), gunName);

                for (int i = 0; i < attackTimes - 1; i++)
                    yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(damage), "Instant");
            }

            private IEnumerable<BattleAction> FairyDefend()
            {
                yield return new EnemyMoveAction(this, base.GetMove(2), true);
                if (IsLunatic)
                    yield return new CastBlockShieldAction(this, base.Defend / 2, base.Defend / 2, BlockShieldType.Normal, true);
                else
                    yield return new CastBlockShieldAction(this, base.Defend, 0, BlockShieldType.Normal, true);
            }

            private IEnumerable<BattleAction> SpellActions(string gun)
            {
                yield return new EnemyMoveAction(this, this.SpellCardName, true);
                switch (gun)
                {
                    case "冰封噩梦B":
                        if (IsUnstable)
                        {
                            yield return PerformAction.Chat(this, "Let's go!\nI'm not scared!", 1.3f, 0, 1.3f);
                            yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(Damage4 + SpellCounter * 2), gun);
                            yield return new AddCardsToDiscardAction(Library.CreateCards<LBoL.EntityLib.Cards.Enemy.Lunatic>(Count2, false));
                        }
                        else
                        {
                            yield return PerformAction.Chat(this, "Purple is a cute color... eat it!", 1.3f, 0, 1.3f);
                            yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(Damage4 + SpellCounter * 2), gun);
                            yield return new AddCardsToDiscardAction(Library.CreateCards<LBoL.EntityLib.Cards.Enemy.Lunatic>(Count1, false));
                        }
                        break;
                    case "JunkoLunatic":
                        if (IsUnstable)
                        {
                            yield return PerformAction.Chat(this, "I won't back down!", 1.3f, 0, 1.3f);
                            yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(Damage4 + SpellCounter * 2), gun);
                            yield return new ApplyStatusEffectAction<ManiaBossSe>(base.Battle.Player, 1, 1, 1, null, 0.1f);
                        }
                        else
                        {
                            yield return PerformAction.Chat(this, "Red should be the most dangerous, right?", 1.3f, 0, 1.3f);
                            yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(Damage3 + SpellCounter * 2), gun);
                            yield return new ApplyStatusEffectAction<ManiaBossSe>(base.Battle.Player, 1, 1, 1, null, 0.1f);
                        }
                        break;
                    case "Junko3B":
                        if (IsUnstable)
                        {
                            yield return PerformAction.Chat(this, "I'll win for both of us!", 1.3f, 0, 1.3f);
                            yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(Damage4 + SpellCounter * 2), gun);
                            yield return new ApplyStatusEffectAction<TurnStartPurify>(base.Battle.Player, 3, 3, 3, null, 0.1f);
                        }
                        else
                        {
                            yield return PerformAction.Chat(this, "Bet you didn't expect this!", 1.3f, 0, 1.3f);
                            yield return new DamageAction(this, base.Battle.Player, DamageInfo.Attack(Damage3 + SpellCounter * 2), gun);
                            yield return new ApplyStatusEffectAction<TurnStartPurify>(base.Battle.Player, 2, 2, 2, null, 0.1f);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(gun));
                }

                SpellCounter++;

                if (!IsUnstable)
                {
                    if (this.HasStatusEffect<LunaticTorchBossSe>())
                    {
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<LunaticTorchBossSe>());
                        yield return new RemoveStatusEffectAction(FairyAlly.GetStatusEffect<LunaticBossSe>());
                    }

                    if (this.HasStatusEffect<ManicTorchBossSe>())
                    {
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<ManicTorchBossSe>());
                        yield return new RemoveStatusEffectAction(FairyAlly.GetStatusEffect<LunaticBossSe>());
                    }

                    if (this.HasStatusEffect<PureTorchBossSe>())
                    {
                        yield return new RemoveStatusEffectAction(this.GetStatusEffect<PureTorchBossSe>());
                        yield return new RemoveStatusEffectAction(FairyAlly.GetStatusEffect<LunaticBossSe>());
                    }

                    this.RollRNG(0, 2);
                    switch (FairyRNG)
                    {
                        case 0:
                            yield return new ApplyStatusEffectAction<LunaticTorchBossSe>(FairyAlly, null, null, null, null, 0.2f, true);
                            yield return new ApplyStatusEffectAction<LunaticBossSe>(this, null, null, null, null, 0.2f, true);
                            break;
                        case 1:
                            yield return new ApplyStatusEffectAction<ManicTorchBossSe>(FairyAlly, null, null, null, null, 0.2f, true);
                            yield return new ApplyStatusEffectAction<LunaticBossSe>(this, null, null, null, null, 0.2f, true);
                            break;
                        case 2:
                            yield return new ApplyStatusEffectAction<PureTorchBossSe>(FairyAlly, null, null, null, null, 0.2f, true);
                            yield return new ApplyStatusEffectAction<LunaticBossSe>(this, null, null, null, null, 0.2f, true);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(FairyRNG));
                    }
                }
            }

            protected override IEnumerable<IEnemyMove> GetTurnMoves()
            {
                switch (this.NextMove)
                {
                    case MoveType.Attack:
                        if (IsLunatic)
                            yield return new SimpleEnemyMove(Intention.Attack(Damage1 + 2, Count1, false).WithMoveName(base.GetMove(0)), FairyAttack(0, Damage1 + 2, Gun3));
                        else
                            yield return new SimpleEnemyMove(Intention.Attack(Damage1, Count1, false).WithMoveName(base.GetMove(0)), FairyAttack(0, Damage1, Gun1));
                        break;
                    case MoveType.MultiAttack:
                        if (IsLunatic)
                            yield return new SimpleEnemyMove(Intention.Attack(Damage2 + 1, Count2, false).WithMoveName(base.GetMove(1)), FairyAttack(1, Damage2 + 1, Gun4, 2));
                        else
                            yield return new SimpleEnemyMove(Intention.Attack(Damage2, Count2, false).WithMoveName(base.GetMove(1)), FairyAttack(1, Damage2, Gun2, 2));
                        break;
                    case MoveType.Defend:
                        yield return new SimpleEnemyMove(Intention.Defend().WithMoveName(base.GetMove(2)), FairyDefend());
                        break;
                    case MoveType.AddCards:
                        if (IsLunatic)
                            yield return new SimpleEnemyMove(Intention.AddCard().WithMoveName(base.GetMove(3)), new AddCardsToDiscardAction(Library.CreateCards<Riguang>(Count2, false)));
                        else
                            yield return new SimpleEnemyMove(Intention.AddCard().WithMoveName(base.GetMove(3)), new AddCardsToDiscardAction(Library.CreateCards<Riguang>(Count1, false)));
                        break;
                    case MoveType.Spell:
                        if (this.HasStatusEffect<LunaticTorchBossSe>())
                            yield return new SimpleEnemyMove(Intention.SpellCard(this.SpellCardName, Damage3 + SpellCounter * 2, true), SpellActions(Gun5));
                        else if (this.HasStatusEffect<ManicTorchBossSe>())
                            yield return new SimpleEnemyMove(Intention.SpellCard(this.SpellCardName, Damage3 + SpellCounter * 2, true), SpellActions(Gun6));
                        else if (this.HasStatusEffect<PureTorchBossSe>())
                            yield return new SimpleEnemyMove(Intention.SpellCard(this.SpellCardName, Damage3 + SpellCounter * 2, true), SpellActions(Gun7));
                        else if (this.HasStatusEffect<UnstableTorchBossSe>())
                        {
                            string spellGun;
                            RollRNG(0, 2);
                            switch (FairyRNG)
                            {
                                case 0:
                                    spellGun = Gun5;
                                    break;
                                case 1:
                                    spellGun = Gun6;
                                    break;
                                case 2:
                                    spellGun = Gun7;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(FairyRNG));
                            }
                            yield return new SimpleEnemyMove(Intention.SpellCard(this.SpellCardName, Damage4, true), SpellActions(spellGun));
                        }
                        else
                            throw new ArgumentOutOfRangeException();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(NextMove));
                }
            }

            protected override void UpdateMoveCounters()
            {

                switch (NextMove)
                {
                    case MoveType.Attack:
                    case MoveType.MultiAttack:
                    case MoveType.Defend:
                    case MoveType.AddCards:
                        RollRNG(0, 3);
                        switch (FairyRNG)
                        {
                            case 0:
                                if ((Counter % 4 == 0) && (this.HasStatusEffect<LunaticTorchBossSe>() || this.HasStatusEffect<ManicTorchBossSe>() || this.HasStatusEffect<PureTorchBossSe>() || this.HasStatusEffect<UnstableTorchBossSe>()))
                                    TempMove = MoveType.Spell;
                                else
                                    TempMove = MoveType.Attack;
                                if (TempMove == NextMove)
                                    UpdateMoveCounters();
                                break;
                            case 1:
                                if ((Counter % 4 == 0) && (this.HasStatusEffect<LunaticTorchBossSe>() || this.HasStatusEffect<ManicTorchBossSe>() || this.HasStatusEffect<PureTorchBossSe>() || this.HasStatusEffect<UnstableTorchBossSe>()))
                                    TempMove = MoveType.Spell;
                                else
                                    TempMove = MoveType.MultiAttack;
                                if (TempMove == NextMove)
                                    UpdateMoveCounters();
                                break;
                            case 2:
                                if ((Counter % 4 == 0) && (this.HasStatusEffect<LunaticTorchBossSe>() || this.HasStatusEffect<ManicTorchBossSe>() || this.HasStatusEffect<PureTorchBossSe>() || this.HasStatusEffect<UnstableTorchBossSe>()))
                                    TempMove = MoveType.Spell;
                                else
                                    TempMove = MoveType.Defend;
                                if (TempMove == NextMove)
                                    UpdateMoveCounters();
                                break;
                            case 3:
                                if ((Counter % 4 == 0) && (this.HasStatusEffect<LunaticTorchBossSe>() || this.HasStatusEffect<ManicTorchBossSe>() || this.HasStatusEffect<PureTorchBossSe>() || this.HasStatusEffect<UnstableTorchBossSe>()))
                                    TempMove = MoveType.Spell;
                                else
                                    TempMove = MoveType.AddCards;
                                if (TempMove == NextMove)
                                    UpdateMoveCounters();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(FairyRNG));
                        }
                        break;

                    case MoveType.Spell:
                        if (IsUnstable)
                            TempMove = MoveType.Attack;
                        else
                            TempMove = MoveType.Defend;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(NextMove));
                }

                this.NextMove = this.TempMove;
            }
        }
    }
}
