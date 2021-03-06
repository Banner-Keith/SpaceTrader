/*******************************************************************************
 *
 * Space Trader for Windows File Converter 2.00
 *
 * Copyright (C) 2005 Jay French, All Rights Reserved
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
 ******************************************************************************/
using System;
using System.Collections;

namespace Fryz.Apps.SpaceTrader
{
	[Serializable()]
	public abstract class Equipment: STSerializableObject
	{
		#region Member Declarations

		protected EquipmentType	_equipType	= EquipmentType.Gadget;
		protected int						_price			= 0;
		protected TechLevel			_minTech		= TechLevel.HiTech;
		protected int						_chance			= 0;

		#endregion

		#region Methods

		public override Hashtable Serialize()
		{
			Hashtable	hash	= base.Serialize();

			hash.Add("_equipType",	(int)_equipType);
			hash.Add("_price",			_price);
			hash.Add("_minTech",		(int)_minTech);
			hash.Add("_chance",			_chance);

			return hash;
		}

		#endregion
	}
}
