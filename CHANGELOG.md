## Version 2.0.0
This will be the last major update for a while as I focus on other things, unless something catastrophic happens. Of course I'll still fix bugs and do balance changes regardless.

- Updated Lunatic Torch to now rewind the card queue during the transformation process to avoid softlocks.
- Changed the rarity of Hecatia, Tri-Bodied Goddess, from Uncommon to Rare as intended.
- Fixed an issue with Fireworks of Lunacy not attacking if the hand is empty.
- Added art for all cards to better tell them apart at a glance, as well as special art for "Lunatic Torch Relay" and Hell Sign "Stellar Inferno".
- Reworked ClownpieceA to more accurately represent her eite fight and LoLK appearance.
  - Removed the Lunatic Torch requirement for Lunatic Torch Relay triggering teammate passives.
  - Added a couple cosmetic changes to Lunatic Torch Relay's activation and Lunatic Torch's application.
  - Fairy teammates are no longer pooled. Instead, you can summon them in greater numbers using specific summon cards.
  - Reworked Black Butterfly.
    - Starting Unity reduced from 5(6) to 4(5).
    - Passive damage reduced from 10(12) to 5(6).
    - Passive cost increased from +1 to -1.
    - Ultimate cost reduced from -8 to 0.
    - Ultimate now gives 6(8) barrier instead of dealing damage.
    - Cost reduced from BB to B, though this change is purely cosmetic.
  - Reworked Lunatic Black Butterfly.
    - Passive damage reduced from 12(15) to 7(8)
    - Active damage reduced from 10 to 6(7)
    - Lunatic Black Butterfly now properly discards itself on automatic active skill usage.
    - Life loss on expel reduced from 4(2) to 2(1).
    - Removed the "Add a Lunacy to the discard" effect from the active skill.
    - Cost reduced from 2 to B, though this change is purely cosmetic.
  - Reworked Hell Fairy
    - Starting Unity reduced from 5(6) to 4(5).
    - Passive Damage reduced from 10(12) to 4(5).
    - Passive reworked to now deal damage to the enemy with the most life 2 times. The check for "most life" happens before each attack.
    - Ultimate cost reduced from -8 to 0.
    - Ultimate now gives 1 firepower instead of dealing damage.
    - Cost reduced from RR to R, though this change is purely cosmetic.
  - Reworked Lunatic Hell Fairy.
    - Removed the Expel effect.
    - Passive damage reduced from 5(8) to 4(6).
    - Lunatic Hell Fairy now properly discards itself on automatic active skill usage.
    - Active hit count reduced from 5 to 4.
    - Active unblocked damage instance count increased from 2 to 4.
    - Cost reduced from 2 to R, though this change is purely cosmetic.
  - Reworked Moon Fairy
    - Passive damage reduced from 12(15) to 8(10).
    - The amount of cards purified by the Ultimate reduced from 1(2) to 1.
    - Ultimate cost reduced from -8 to 0.
    - Cost reduced from C2 to C, though this change is purely cosmetic.
  - Reworked Lunatic Moon Fairy.
    - Passive damage reduced from 10(12) to 9(11).
    - The amount the Passive damage scales per pure card reduced from 2(4) to 1(2).
    - Lunatic Moon Fairy now properly discards itself on automatic active skill usage.
    - Cost reduced from 3 to C, though this change is purely cosmetic.
  - Added Butterfly Backup.
  - Changed ClownpieceA's starter card from Black Butterfly to Butterfly Backup.
  - Added Lunatic Support.
  - Added Hell's Nature.
  - Added Hell's Lunacy.
  - Added Lunar Support.
  - Added Moon of Lunacy.
  - Removed Forgotten Maid.
  - Added Nature's Defense.
  - Added Solar Phalanx.
  - Added Sunflower Fairy.
  - Added Luantic Sunflower Fairy.
  - Added Dreams of Playtime.
  - Added Nightmares of Lunacy.
  - Added Dream World Fairy.
  - Added Lunatic Dream World Fairy.
- Added a boss for Act 1 for other characters to encounter.
  - Boss consists of two fairy enemies that utilize Clownpiece's mechanics... and a couple surprises.
- Updated all cards to work with SideLoader v0.9.7841 for the Koishi release.
- Changed Mania Cleansing's cost from BBRRG(1BRRG) to BBRRG(2BRG).
- Added Mixed Madness.

## Version 0.1.4
- Fixed various yaml errors.
- Nerfed Torch of Mania to activate once per turn instead of on every debuff inflict.
- Fixed Residual Impurity's return effect causing errors if a card is currently in the queue.
- **Look for 0.2.0 coming soon! Featuring ClownpieceA rework and an Act 1 boss.**

## Verison 0.1.3
- Fixed various yaml errors.
- Decreased Undying Flame's upgrade Vulnerable count from 2 to 1.
- Fixed Undying Flame's attack and status effect actions not triggering if the draw pile is empty.
- Lowered the cost of Lunatic Torch Ignition from 2(1) to 1(0).
- Lowered Moon Fairy's base card purification count from 2 to 1. The count now increases to 2 when upgraded.
- Increased the time it takes to trigger Moon Fairy's ultimate by lowering starting unity to 4(5) from 5(6).
- Changed Lunatic Moon Fairy's damage to scale with the level of Permanent Purify instead of unspent colorless mana.
- Increased Black Butterfly's passive damage from 6(10) to 10(12) and Lunatic Black Butterfly's passive damage from 10(15) to 12(15).
- Removed Chaotic Danmaku Dodging from the card pool. Now it can only be obtained through Luantic Torch as intended.
- Terrorize's description was made more clear.
- Added upgrade effects for some cards that didn't have them.
  - Hell's Inferno now costs RR base. Hell's Inferno+ retains the default R.
  - Berserk Lifeforce+ now costs 2R.
  - Fairy Swarm+ now deals 18 damage.
- Increased the card limit of Fairy Parkour+ to 6 from 5 to compensate for the extra draw.
- Increased Earth Hecatia's barrier scaling per unspent mana from 2(3) to 3(4).
- Lowered the cost of Flames of Mania+ to RR from 1RR.

## Version 0.1.2
- Fixed poor wording on certain cards and effects.
- Fixed Black Butterfly's Active Skill not targeting properly.
- Fixed Racetrack applying it's temporary cost reduction to discarded cards instead of drawn cards.

## Version 0.1.1
- Fixed an issue with certain statuses softlocking the game after a battle.
- Fixed the version not updating properly.
- (Actually remembered to use this changelog).

## Version 0.1.0
- Added Clownpiece.