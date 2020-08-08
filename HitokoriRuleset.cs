﻿using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Hitokori.Beatmaps;
using osu.Game.Rulesets.Hitokori.Difficulty;
using osu.Game.Rulesets.Hitokori.Mods;
using osu.Game.Rulesets.Hitokori.Scoring;
using osu.Game.Rulesets.Hitokori.Settings;
using osu.Game.Rulesets.Hitokori.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking.Statistics;

namespace osu.Game.Rulesets.Hitokori {
	public class HitokoriRuleset : Ruleset {
		public const string SHORT_NAME = "hitokori";
		public override string Description => SHORT_NAME;
		public override string ShortName => SHORT_NAME;
		public override string PlayingVerb => "Playing with fire";

		public override Drawable CreateIcon ()
			=> new HitokoriIcon();

		public override IBeatmapConverter CreateBeatmapConverter ( IBeatmap beatmap )
			=> new HitokoriBeatmapConverter( beatmap, this );
		public override IBeatmapProcessor CreateBeatmapProcessor ( IBeatmap beatmap )
			=> new HitokoriBeatmapProcessor( beatmap );

		public override IRulesetConfigManager CreateConfig ( SettingsStore settings )
			=> new HitokoriSettingsManager( settings, RulesetInfo );
		public override RulesetSettingsSubsection CreateSettings ()
			=> new HitokoriSettingsSubsection( this );

		public override DifficultyCalculator CreateDifficultyCalculator ( WorkingBeatmap beatmap )
			=> new HitokoriDifficultyCalculator( this, beatmap );
		public override HealthProcessor CreateHealthProcessor ( double drainStartTime )
			=> new HitokoriHealthProcessor();
		public override ScoreProcessor CreateScoreProcessor ()
			=> new HitokoriScoreProcessor();
		public override StatisticRow[] CreateStatisticsForScore ( ScoreInfo score, IBeatmap playableBeatmap )
			=> base.CreateStatisticsForScore( score, playableBeatmap );

		public override DrawableRuleset CreateDrawableRulesetWith ( IBeatmap beatmap, IReadOnlyList<Mod> mods = null )
			=> new HitokoriDrawableRuleset( this, beatmap, mods );

		public override IEnumerable<Mod> GetModsFor ( ModType type ) {
			switch ( type ) {
				case ModType.DifficultyReduction:
					return new Mod[] {
						new HitokoriModSquashed(),
						new HitokoriModEasy(),
						new HitokoriModNoFail(),
						new MultiMod( new HitokoriModHalfTime(), new HitokoriModDaycore() )
					};

				case ModType.DifficultyIncrease:
					return new Mod[] {
						new HitokoriModStretched(),
						new HitokoriModHardRock(), // TODO implement hard rock ( harder patterns )
						new MultiMod( new HitokoriModSuddenDeath(), new HitokoriModPerfect() ),
						new MultiMod( new HitokoriModDoubleTime(), new HitokoriModNightcore() ),
						new HitokoriModHidden(),
						new HitokoriModFlashlight()
					};

				case ModType.Conversion:
					return new Mod[] {
						new HitokoriModExperimental(),
						new HitokoriModDoubleTile()
					};

				case ModType.Automation:
					return new Mod[] {
						new HitokoriModAuto()
					};

				case ModType.Fun:
					return new Mod[] {
						new HitokoriModReverseSpin(), // TODO implement reverse spin ( the world spins instead of hitokori )
						new HitokoriModTriplets() // TODO implement triplets ( 3 orbitals )
					};

				case ModType.System:
					return Array.Empty<Mod>();

				default:
					return Array.Empty<Mod>();
			}
		}

		public override IEnumerable<KeyBinding> GetDefaultKeyBindings ( int variant = 0 ) {
			return new[] {
				new KeyBinding( InputKey.Z, HitokoriAction.Action1 ),
				new KeyBinding( InputKey.MouseLeft, HitokoriAction.Action1 ),

				new KeyBinding( InputKey.X, HitokoriAction.Action2 ),
				new KeyBinding( InputKey.MouseRight, HitokoriAction.Action2 )
			};
		}
	}
}
