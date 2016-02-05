/*
 *  This file is part of uEssentials project.
 *      https://uessentials.github.io/
 *
 *  Copyright (C) 2015-2016  Leonardosc
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System.Linq;
using Essentials.Api.Command;
using Essentials.I18n;
using SDG.Unturned;
using Essentials.Api.Command.Source;
using Essentials.Api.Unturned;


// ReSharper disable InconsistentNaming

namespace Essentials.Commands
{
    [CommandInfo(
        Name = "repairvehicle",
        Aliases = new[] {"repairveh"},
        Description = "Repair current/all vehicle",
        Usage = "<all>"
        )]
    public class CommandRepairVehicle : EssCommand
    {
        public override void OnExecute( ICommandSource source, ICommandArgs parameters )
        {
            if ( parameters.IsEmpty )
            {
                if ( source.IsConsole )
                {
                    ShowUsage( source );
                    return;
                }

                var currentVeh = source.ToPlayer().CurrentVehicle;

                if ( currentVeh != null )
                {
                    VehicleManager.sendVehicleHealth( currentVeh, currentVeh.asset.health );

                    EssLang.VEHICLE_REPAIRED.SendTo( source );
                }
                else
                {
                    EssLang.NOT_IN_VEHICLE.SendTo( source );
                }
            }
            else if ( parameters[0].Is( "all" ) )
            {
                var allVehicles = UWorld.Vehicles;

                lock ( allVehicles )
                {
                    allVehicles
                        .Where( veh => !veh.isExploded && !veh.isUnderwater )
                        .ToList()
                        .ForEach( vehicle => {
                            VehicleManager.sendVehicleHealth( vehicle, vehicle.asset.health );
                        } );

                    EssLang.VEHICLE_REPAIRED_ALL.SendTo( source );
                }
            }
        }
    }
}