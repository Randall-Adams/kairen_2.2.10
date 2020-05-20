--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Alert_OutputPlayerData_ConsoleOutput
Code-Type: LUA Class
Code-Version: 1.0
Code-Description: Alerts in the console that Player Data was Output
Code-Author: Robert Randazzio
]]--
return (
function ()
    --This alerts the user that player data was output
    _pathFAPIData = RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites
    print("0| Alert_OutputPlayerData: _pathFAPIData == " .. _pathFAPIData)
end
);