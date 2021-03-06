/*******************************************************************************
 *
 * Space Trader for Windows 1.00
 *
 * Copyright (C) 2016 Keith Banner, All Rights Reserved
 *
 * Port to Windows by David Pierron & Jay French
 * Original coding by Pieter Spronck, Sam Anderson, Samuel Goldstein, Matt Lee
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option) any
 * later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * If you'd like a copy of the GNU General Public License, go to
 * http://www.gnu.org/copyleft/gpl.html.
 *
 * You can contact the author at spacetrader@kb4000.com
 *
 ******************************************************************************/

using System;

namespace SpaceTrader
{
	public class Reputation
	{
		#region Member Declarations

		private ReputationType	_type;

	    #endregion

		#region Methods

		public Reputation(ReputationType type, int minScore)
		{
			_type			= type;
			MinScore	= minScore;
		}

		public static Reputation GetReputationFromScore(int ReputationScore)
		{
			int i;
			for (i = 0; i < Consts.Reputations.Length && Game.CurrentGame.Commander.ReputationScore >= Consts.Reputations[i].MinScore; i++);
			return Consts.Reputations[Math.Max(0, i - 1)];
		}

		#endregion

		#region Properties

		public int MinScore { get; }

	    public string Name => Strings.ReputationNames[(int)_type];

	    public ReputationType Type => _type;

	    #endregion
	}
}
