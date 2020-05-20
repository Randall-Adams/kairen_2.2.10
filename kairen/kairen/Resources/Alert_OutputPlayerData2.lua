--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Alert_OutputPlayerData
Code-Type: LUA Class
Code-Version: 1.0
Code-Description: Outputs the player's data to Kairen.
Code-Author: Robert Randazzio
]]--
return (
function ()
    --This alerts @@@ Kairen @@@ that Player Data was output.
    _pathFAPIData = RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites
    --print("0| Alert_OutputPlayerData: _pathFAPIData == " .. _pathFAPIData)
    FAPI.RunOptions.Alert_OutputPlayerData_ConsoleOutput.Try()
    file2 = io.open(_pathFAPIData,"w+");
    file2:write("UpdateKairen");
    file2:write("\n");
    file2:write("OutputPlayerData");
    file2:write("\n");
    file2:close();
end
);