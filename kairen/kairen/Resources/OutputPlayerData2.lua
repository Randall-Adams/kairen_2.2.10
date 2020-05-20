--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: OutputPlayerData
Code-Type: LUA Class
Code-Version: 1.0
Code-Description: Outputs the player's data to Kairen.
Code-Author: Robert Randazzio
]]--
return (
function ()
    _filepath = RF.EQOA.Net_Streams.o.self .. "Player Data" .. Extension_ReadWrites
    file = io.open(_filepath,"w+");
    file:write(EQOA.Player.Location.X.Value());
    file:write("\n");
    file:write(EQOA.Player.Location.Y.Value());
    file:write("\n");
    file:write(EQOA.Player.Location.Z.Value());
    file:write("\n");
    file:write(EQOA.Player.Location.F.Value());
        local _rowcolumn
        _columnrow = EQOA.HelperFunctions.GetZoneRowColumn_ByCoords(EQOA.Player.Location.X.Value(), EQOA.Player.Location.Y.Value())
    file:write("\n");
    file:write(_columnrow[1]);
    file:write("\n");
    file:write(_columnrow[2]);
    file:write("\n");
        local _myX = _G.SpawnHandler.GetSpawnNestCoord(_G.EQOA.Player.Location.X.Value())
        local _myY = _G.SpawnHandler.GetSpawnNestCoord(_G.EQOA.Player.Location.Y.Value())
    file:write(_myX);
    file:write("\n");
    file:write(_myY);
    file:write("\n");
    file:write(EQOA.Player.Location.Zone());
    file:write("\n");
    file:write(EQOA.Player.Location.StartMenuZoneFull.Value());
    file:write("\n");
    file:write(EQOA.Player.Location.StartMenuZoneName());
    file:write("\n");
    file:write(EQOA.Player.Location.StartMenuZoneSub());
    file:write("\n");
    file:close();
    FAPI.RunOptions.Alert_OutputPlayerData.Try()
end
);