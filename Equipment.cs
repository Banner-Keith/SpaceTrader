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

using System.Collections;

namespace SpaceTrader
{
    public abstract class Equipment : STSerializableObject
    {
        #region Member Declarations

        protected EquipmentType _equipType;
        protected int _price;
        protected TechLevel _minTech;
        protected int _chance;

        #endregion

        #region Methods

        public Equipment(EquipmentType type, int price, TechLevel minTechLevel, int chance)
        {
            _equipType = type;
            _price = price;
            _minTech = minTechLevel;
            _chance = chance;
        }

        public Equipment(Hashtable hash) : base(hash)
        {
            _equipType = (EquipmentType)GetValueFromHash(hash, "_equipType");
            _price = (int)GetValueFromHash(hash, "_price");
            _minTech = (TechLevel)GetValueFromHash(hash, "_minTech");
            _chance = (int)GetValueFromHash(hash, "_chance");
        }

        public abstract Equipment Clone();

        public override Hashtable Serialize()
        {
            Hashtable hash = base.Serialize();

            hash.Add("_equipType", (int)_equipType);
            hash.Add("_price", _price);
            hash.Add("_minTech", (int)_minTech);
            hash.Add("_chance", _chance);

            return hash;
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract bool TypeEquals(object type);

        #endregion

        #region Properties

        protected int BaseImageIndex
        {
            get
            {
                int baseImageIndex = 0;

                switch (EquipmentType)
                {
                    case EquipmentType.Gadget:
                        baseImageIndex = Strings.WeaponNames.Length + Strings.ShieldNames.Length;
                        break;
                    case EquipmentType.Shield:
                        baseImageIndex = Strings.WeaponNames.Length;
                        break;
                    case EquipmentType.Weapon:
                        // baseImageIndex should be 0
                        break;
                }

                return baseImageIndex;
            }
        }

        public int Chance => _chance;

        public EquipmentType EquipmentType => _equipType;

        public System.Drawing.Image Image => Game.CurrentGame.ParentWindow.EquipmentImages.Images[BaseImageIndex + (int)SubType];

        public TechLevel MinimumTechLevel => _minTech;

        public virtual string Name => "";

        public int Price
        {
            get
            {
                Commander cmdr = Game.CurrentGame.Commander;
                int price = 0;

                if (cmdr != null && cmdr.CurrentSystem.TechLevel >= MinimumTechLevel)
                    price = (_price * (100 - cmdr.Ship.Trader)) / 100;

                return price;
            }
        }

        public int SellPrice => _price * 3 / 4;

        public virtual object SubType => 0;

        // The cost to transfer is 10% of the item worth. This is changed
        // from actually PAYING the buyer about 8% to transfer items. - JAF
        public int TransferPrice => SellPrice * 110 / 90;

        #endregion
    }
}
