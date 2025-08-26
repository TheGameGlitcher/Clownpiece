using Clownpiece.CustomClasses;
using Clownpiece.Localization;
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
    public sealed class EmbodimentOfImpuritySeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(EmbodimentOfImpuritySe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return ClownpieceLocalization.StatusEffectsBatchLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("EmbodimentOfImpuritySe.png", BepinexPlugin.embeddedSource);
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
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Default"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(EmbodimentOfImpuritySeDef))]
    public sealed class EmbodimentOfImpuritySe : ClownStatus
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<ManaEventArgs>(base.Battle.ManaConsumed, new EventSequencedReactor<ManaEventArgs>(this.OnManaConsumed));
        }
        private IEnumerable<BattleAction> OnManaConsumed(ManaEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            ManaGroup value = args.Value;

            if (value.Colorless > 0)
            {
                base.NotifyActivating();
                int num = Level * value.Colorless;
                yield return this.BuffAction<Reflect>(num);
                if (this.Battle.Player.HasStatusEffect<Reflect>())
                    this.Battle.Player.GetStatusEffect<Reflect>().Gun = "JunkoLunatic";
            }
        }
    }
}