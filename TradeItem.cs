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
    public class TradeItem : IComparable
    {
        #region Member Declarations

        #endregion

        #region Methods

        public TradeItem(TradeItemType type, TechLevel techProduction, TechLevel techUsage,
            TechLevel techTopProduction, int piceLowTech, int priceInc, int priceVariance,
            SystemPressure pressurePriceHike, SpecialResource resourceLowPrice, SpecialResource resourceHighPrice,
            int minTradePrice, int maxTradePrice, int roundOff)
        {
            Type = type;
            TechProduction = techProduction;
            TechUsage = techUsage;
            TechTopProduction = techTopProduction;
            PriceLowTech = piceLowTech;
            PriceInc = priceInc;
            PriceVariance = priceVariance;
            PressurePriceHike = pressurePriceHike;
            ResourceLowPrice = resourceLowPrice;
            ResourceHighPrice = resourceHighPrice;
            MinTradePrice = minTradePrice;
            MaxTradePrice = maxTradePrice;
            RoundOff = roundOff;
        }

        public int CompareTo(object value)
        {
            int compared = 0;

            if (value == null)
                compared = 1;
            else
            {
                compared = PriceLowTech.CompareTo(((TradeItem)value).PriceLowTech);
                if (compared == 0)
                    compared = -PriceInc.CompareTo(((TradeItem)value).PriceInc);
            }

            return compared;
        }

        public int StandardPrice(StarSystem target)
        {
            int price = 0;

            if (target.ItemUsed(this))
            {
                // Determine base price on tech level of system
                price = PriceLowTech + (int)target.TechLevel * PriceInc;

                // If a good is highly requested, increase the price
                if (target.PoliticalSystem.Wanted == Type)
                    price = price * 4 / 3;

                // High trader activity decreases prices
                price = price * (100 - 2 * (int)target.PoliticalSystem.ActivityTraders) / 100;

                // Large system = high production decreases prices
                price = price * (100 - (int)target.Size) / 100;

                // Special resources price adaptation
                if (target.SpecialResource == ResourceLowPrice)
                    price = price * 3 / 4;
                else if (target.SpecialResource == ResourceHighPrice)
                    price = price * 4 / 3;
            }

            return price;
        }

        #endregion

        #region Properties

        public bool Illegal => Type == TradeItemType.Firearms || Type == TradeItemType.Narcotics;

        public int MaxTradePrice { get; }

        public int MinTradePrice { get; }

        public string Name => Strings.TradeItemNames[(int)Type];

        public SystemPressure PressurePriceHike { get; }

        public int PriceInc { get; }

        public int PriceLowTech { get; }

        public int PriceVariance { get; }

        public SpecialResource ResourceHighPrice { get; }

        public SpecialResource ResourceLowPrice { get; }

        public int RoundOff { get; }

        public TechLevel TechProduction { get; }

        public TechLevel TechTopProduction { get; }

        public TechLevel TechUsage { get; }

        public TradeItemType Type { get; }

        #endregion
    }
}
