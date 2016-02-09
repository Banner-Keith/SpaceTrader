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
    public class PoliticalSystem
    {
        #region Member Declarations

        #endregion

        #region Methods

        public PoliticalSystem(PoliticalSystemType type, int reactionIllegal, Activity activityPolice, Activity activityPirates,
            Activity activityTraders, TechLevel minTechLevel, TechLevel maxTechLevel, int bribeLevel, bool drugsOk,
            bool firearmsOk, TradeItemType wanted)
        {
            Type = type;
            ReactionIllegal = reactionIllegal;
            ActivityPolice = activityPolice;
            ActivityPirates = activityPirates;
            ActivityTraders = activityTraders;
            MinimumTechLevel = minTechLevel;
            MaximumTechLevel = maxTechLevel;
            BribeLevel = bribeLevel;
            DrugsOk = drugsOk;
            FirearmsOk = firearmsOk;
            Wanted = wanted;
        }

        public bool ShipTypeLikely(ShipType shipType, OpponentType oppType)
        {
            bool likely = false;
            int diffMod = Math.Max(0, (int)Game.CurrentGame.Difficulty - (int)Difficulty.Normal);

            switch (oppType)
            {
                case OpponentType.Pirate:
                    likely = (int)ActivityPirates + diffMod >= (int)Consts.ShipSpecs[(int)shipType].Pirates;
                    break;
                case OpponentType.Police:
                    likely = (int)ActivityPolice + diffMod >= (int)Consts.ShipSpecs[(int)shipType].Police;
                    break;
                case OpponentType.Trader:
                    likely = (int)ActivityTraders + diffMod >= (int)Consts.ShipSpecs[(int)shipType].Traders;
                    break;
            }

            return likely;
        }

        #endregion

        #region Properties

        public Activity ActivityPirates { get; }

        public Activity ActivityPolice { get; }

        public Activity ActivityTraders { get; }

        public int BribeLevel { get; }

        public bool DrugsOk { get; }

        public bool FirearmsOk { get; }

        public TechLevel MaximumTechLevel { get; }

        public TechLevel MinimumTechLevel { get; }

        public string Name => Strings.PoliticalSystemNames[(int)Type];

        public int ReactionIllegal { get; }

        public PoliticalSystemType Type { get; }

        public TradeItemType Wanted { get; }

        #endregion
    }
}
