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
using System.Collections;

namespace SpaceTrader
{
    public class StarSystem : STSerializableObject
    {
        #region Member Declarations

        private SpecialResource _specialResource;

        #endregion

        #region Methods

        public StarSystem(StarSystemId id, int x, int y, Size size, TechLevel techLevel,
            PoliticalSystemType politicalSystemType, SystemPressure systemPressure, SpecialResource specialResource)
        {
            Id = id;
            X = x;
            Y = y;
            Size = size;
            TechLevel = techLevel;
            PoliticalSystemType = politicalSystemType;
            SystemPressure = systemPressure;
            _specialResource = specialResource;

            InitializeTradeItems();
        }

        public StarSystem(Hashtable hash) : base(hash)
        {
            Id = (StarSystemId)GetValueFromHash(hash, "_id", Id);
            X = (int)GetValueFromHash(hash, "_x", X);
            Y = (int)GetValueFromHash(hash, "_y", Y);
            Size = (Size)GetValueFromHash(hash, "_size", Size);
            TechLevel = (TechLevel)GetValueFromHash(hash, "_techLevel", TechLevel);
            PoliticalSystemType = (PoliticalSystemType)GetValueFromHash(hash, "_politicalSystemType", PoliticalSystemType);
            SystemPressure = (SystemPressure)GetValueFromHash(hash, "_systemPressure", SystemPressure);
            _specialResource = (SpecialResource)GetValueFromHash(hash, "_specialResource", _specialResource);
            SpecialEventType = (SpecialEventType)GetValueFromHash(hash, "_specialEventType", SpecialEventType);
            TradeItems = (int[])GetValueFromHash(hash, "_tradeItems", TradeItems);
            CountDown = (int)GetValueFromHash(hash, "_countDown", CountDown);
            Visited = (bool)GetValueFromHash(hash, "_visited", Visited);
            ShipyardId = (ShipyardId)GetValueFromHash(hash, "_shipyardId", ShipyardId);
        }

        public void InitializeTradeItems()
        {
            for (int i = 0; i < Consts.TradeItems.Length; i++)
            {
                if (!ItemTraded(Consts.TradeItems[i]))
                {
                    TradeItems[i] = 0;
                }
                else
                {
                    TradeItems[i] = ((int)this.Size + 1) * (Functions.GetRandom(9, 14) -
                        Math.Abs(Consts.TradeItems[i].TechTopProduction - this.TechLevel));

                    // Because of the enormous profits possible, there shouldn't be too many robots or narcotics available.
                    if (i >= (int)TradeItemType.Narcotics)
                        TradeItems[i] = ((TradeItems[i] * (5 - (int)Game.CurrentGame.Difficulty)) / (6 - (int)Game.CurrentGame.Difficulty)) + 1;

                    if (this.SpecialResource == Consts.TradeItems[i].ResourceLowPrice)
                        TradeItems[i] = TradeItems[i] * 4 / 3;

                    if (this.SpecialResource == Consts.TradeItems[i].ResourceHighPrice)
                        TradeItems[i] = TradeItems[i] * 3 / 4;

                    if (this.SystemPressure == Consts.TradeItems[i].PressurePriceHike)
                        TradeItems[i] = TradeItems[i] / 5;

                    TradeItems[i] = TradeItems[i] - Functions.GetRandom(10) + Functions.GetRandom(10);

                    if (TradeItems[i] < 0)
                        TradeItems[i] = 0;
                }
            }
        }

        public bool ItemTraded(TradeItem item)
        {
            return ((item.Type != TradeItemType.Narcotics || PoliticalSystem.DrugsOk) &&
                (item.Type != TradeItemType.Firearms || PoliticalSystem.FirearmsOk) &&
                TechLevel >= item.TechProduction);
        }

        public bool ItemUsed(TradeItem item)
        {
            return ((item.Type != TradeItemType.Narcotics || PoliticalSystem.DrugsOk) &&
                (item.Type != TradeItemType.Firearms || PoliticalSystem.FirearmsOk) &&
                TechLevel >= item.TechUsage);
        }

        public override Hashtable Serialize()
        {
            Hashtable hash = base.Serialize();

            hash.Add("_id", (int)Id);
            hash.Add("_x", X);
            hash.Add("_y", Y);
            hash.Add("_size", (int)Size);
            hash.Add("_techLevel", (int)TechLevel);
            hash.Add("_politicalSystemType", (int)PoliticalSystemType);
            hash.Add("_systemPressure", (int)SystemPressure);
            hash.Add("_specialResource", (int)_specialResource);
            hash.Add("_specialEventType", (int)SpecialEventType);
            hash.Add("_tradeItems", TradeItems);
            hash.Add("_countDown", CountDown);
            hash.Add("_visited", Visited);
            hash.Add("_shipyardId", (int)ShipyardId);

            return hash;
        }

        public bool ShowSpecialButton()
        {
            Game game = Game.CurrentGame;
            bool show = false;

            switch (SpecialEventType)
            {
                case SpecialEventType.Artifact:
                case SpecialEventType.Dragonfly:
                case SpecialEventType.Experiment:
                case SpecialEventType.Jarek:
                    show = game.Commander.PoliceRecordScore >= Consts.PoliceRecordScoreDubious;
                    break;
                case SpecialEventType.ArtifactDelivery:
                    show = game.Commander.Ship.ArtifactOnBoard;
                    break;
                case SpecialEventType.CargoForSale:
                    show = game.Commander.Ship.FreeCargoBays >= 3;
                    break;
                case SpecialEventType.DragonflyBaratas:
                    show = game.QuestStatusDragonfly > SpecialEvent.StatusDragonflyNotStarted &&
                                    game.QuestStatusDragonfly < SpecialEvent.StatusDragonflyDestroyed;
                    break;
                case SpecialEventType.DragonflyDestroyed:
                    show = game.QuestStatusDragonfly == SpecialEvent.StatusDragonflyDestroyed;
                    break;
                case SpecialEventType.DragonflyMelina:
                    show = game.QuestStatusDragonfly > SpecialEvent.StatusDragonflyFlyBaratas &&
                                    game.QuestStatusDragonfly < SpecialEvent.StatusDragonflyDestroyed;
                    break;
                case SpecialEventType.DragonflyRegulas:
                    show = game.QuestStatusDragonfly > SpecialEvent.StatusDragonflyFlyMelina &&
                                    game.QuestStatusDragonfly < SpecialEvent.StatusDragonflyDestroyed;
                    break;
                case SpecialEventType.DragonflyShield:
                case SpecialEventType.ExperimentFailed:
                case SpecialEventType.Gemulon:
                case SpecialEventType.GemulonFuel:
                case SpecialEventType.GemulonInvaded:
                case SpecialEventType.Lottery:
                case SpecialEventType.ReactorLaser:
                case SpecialEventType.PrincessQuantum:
                case SpecialEventType.SculptureHiddenBays:
                case SpecialEventType.Skill:
                case SpecialEventType.SpaceMonster:
                case SpecialEventType.Tribble:
                    show = true;
                    break;
                case SpecialEventType.EraseRecord:
                case SpecialEventType.Wild:
                    show = game.Commander.PoliceRecordScore < Consts.PoliceRecordScoreDubious;
                    break;
                case SpecialEventType.ExperimentStopped:
                    show = game.QuestStatusExperiment > SpecialEvent.StatusExperimentNotStarted &&
                                    game.QuestStatusExperiment < SpecialEvent.StatusExperimentPerformed;
                    break;
                case SpecialEventType.GemulonRescued:
                    show = game.QuestStatusGemulon > SpecialEvent.StatusGemulonNotStarted &&
                                    game.QuestStatusGemulon < SpecialEvent.StatusGemulonTooLate;
                    break;
                case SpecialEventType.Japori:
                    show = game.QuestStatusJapori == SpecialEvent.StatusJaporiNotStarted &&
                                    game.Commander.PoliceRecordScore >= Consts.PoliceRecordScoreDubious;
                    break;
                case SpecialEventType.JaporiDelivery:
                    show = game.QuestStatusJapori == SpecialEvent.StatusJaporiInTransit;
                    break;
                case SpecialEventType.JarekGetsOut:
                    show = game.Commander.Ship.JarekOnBoard;
                    break;
                case SpecialEventType.Moon:
                    show = game.QuestStatusMoon == SpecialEvent.StatusMoonNotStarted &&
                                    game.Commander.Worth > SpecialEvent.MoonCost * .8;
                    break;
                case SpecialEventType.MoonRetirement:
                    show = game.QuestStatusMoon == SpecialEvent.StatusMoonBought;
                    break;
                case SpecialEventType.Princess:
                    show = game.Commander.PoliceRecordScore >= Consts.PoliceRecordScoreLawful &&
                                    game.Commander.ReputationScore >= Consts.ReputationScoreAverage;
                    break;
                case SpecialEventType.PrincessCentauri:
                    show = game.QuestStatusPrincess >= SpecialEvent.StatusPrincessFlyCentauri &&
                                    game.QuestStatusPrincess <= SpecialEvent.StatusPrincessFlyQonos;
                    break;
                case SpecialEventType.PrincessInthara:
                    show = game.QuestStatusPrincess >= SpecialEvent.StatusPrincessFlyInthara &&
                                    game.QuestStatusPrincess <= SpecialEvent.StatusPrincessFlyQonos;
                    break;
                case SpecialEventType.PrincessQonos:
                    show = game.QuestStatusPrincess == SpecialEvent.StatusPrincessRescued &&
                        !game.Commander.Ship.PrincessOnBoard;
                    break;
                case SpecialEventType.PrincessReturned:
                    show = game.Commander.Ship.PrincessOnBoard;
                    break;
                case SpecialEventType.Reactor:
                    show = game.QuestStatusReactor == SpecialEvent.StatusReactorNotStarted &&
                                    game.Commander.PoliceRecordScore < Consts.PoliceRecordScoreDubious &&
                                    game.Commander.ReputationScore >= Consts.ReputationScoreAverage;
                    break;
                case SpecialEventType.ReactorDelivered:
                    show = game.Commander.Ship.ReactorOnBoard;
                    break;
                case SpecialEventType.Scarab:
                    show = game.QuestStatusScarab == SpecialEvent.StatusScarabNotStarted &&
                                    game.Commander.ReputationScore >= Consts.ReputationScoreAverage;
                    break;
                case SpecialEventType.ScarabDestroyed:
                case SpecialEventType.ScarabUpgradeHull:
                    show = game.QuestStatusScarab == SpecialEvent.StatusScarabDestroyed;
                    break;
                case SpecialEventType.Sculpture:
                    show = game.QuestStatusSculpture == SpecialEvent.StatusSculptureNotStarted &&
                                    game.Commander.PoliceRecordScore < Consts.PoliceRecordScoreDubious &&
                                    game.Commander.ReputationScore >= Consts.ReputationScoreAverage;
                    break;
                case SpecialEventType.SculptureDelivered:
                    show = game.QuestStatusSculpture == SpecialEvent.StatusSculptureInTransit;
                    break;
                case SpecialEventType.SpaceMonsterKilled:
                    show = game.QuestStatusSpaceMonster == SpecialEvent.StatusSpaceMonsterDestroyed;
                    break;
                case SpecialEventType.TribbleBuyer:
                    show = game.Commander.Ship.Tribbles > 0;
                    break;
                case SpecialEventType.WildGetsOut:
                    show = game.Commander.Ship.WildOnBoard;
                    break;
                default:
                    break;
            }

            return show;
        }

        #endregion

        #region Properties

        public int CountDown { get; set; } = 0;

        public bool DestOk
        {
            get
            {
                Commander comm = Game.CurrentGame.Commander;
                return this != comm.CurrentSystem && (Distance <= comm.Ship.Fuel ||
                    Functions.WormholeExists(comm.CurrentSystem, this));
            }
        }

        public int Distance => Functions.Distance(this, Game.CurrentGame.Commander.CurrentSystem);

        public StarSystemId Id { get; }

        public CrewMember[] MercenariesForHire
        {
            get
            {
                Commander cmdr = Game.CurrentGame.Commander;
                CrewMember[] mercs = Game.CurrentGame.Mercenaries;
                ArrayList forHire = new ArrayList(3);

                for (int i = 1; i < mercs.Length; i++)
                {
                    if (mercs[i].CurrentSystem == cmdr.CurrentSystem && !cmdr.Ship.HasCrew(mercs[i].Id))
                        forHire.Add(mercs[i]);
                }

                return (CrewMember[])forHire.ToArray(typeof(CrewMember));
            }
        }

        public string Name => Strings.SystemNames[(int)Id];

        public PoliticalSystem PoliticalSystem => Consts.PoliticalSystems[(int)PoliticalSystemType];

        public PoliticalSystemType PoliticalSystemType { get; set; }

        public Shipyard Shipyard => (ShipyardId == ShipyardId.Na ? null : Consts.Shipyards[(int)ShipyardId]);

        public ShipyardId ShipyardId { get; set; } = ShipyardId.Na;

        public Size Size { get; }

        public SpecialEvent SpecialEvent => (SpecialEventType == SpecialEventType.Na ? null : Consts.SpecialEvents[(int)SpecialEventType]);

        public SpecialEventType SpecialEventType { get; set; } = SpecialEventType.Na;

        public SpecialResource SpecialResource => Visited ? _specialResource : SpecialResource.Nothing;

        public SystemPressure SystemPressure { get; set; }

        public TechLevel TechLevel { get; set; }

        public int[] TradeItems { get; } = new int[10];

        public bool Visited { get; set; } = false;

        public int X { get; set; }

        public int Y { get; set; }

        #endregion
    }
}
