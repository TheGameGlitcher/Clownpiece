using Clownpiece.CustomClasses;

using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using static UnityEngine.UI.GridLayoutGroup;

namespace Clownpiece.Status
{
    public sealed class TorchPureFlameSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(TorchPureFlameSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("TorchPureFlameSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Positive,
                            IsVerbose: false,
                            IsStackable: true,
                            StackActionTriggerLevel: null,
                            HasLevel: true,
                            LevelStackType: StackType.Add,
                            HasDuration: false,
                            DurationStackType: StackType.Add,
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: false,
                            CountStackType: StackType.Keep,
                            LimitStackType: StackType.Keep,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { "Poison", "LockedOn" },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "LunaticTorch"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(TorchPureFlameSeDef))]
    public sealed class TorchPureFlameSe : ClownStatus
    {
        protected override void OnAdded(Unit unit)
        {
            this.ReactOwnerEvent<DamageEventArgs>(this.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
        }
        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            if (args.Cause == ActionCause.Card)
            {
                this.NotifyActivating();
                yield return new ApplyStatusEffectAction<Poison>(args.Target, Level, Level, Level);
                yield return new ApplyStatusEffectAction<LockedOn>(args.Target, Level, Level, Level);
            }
        }
    }
}