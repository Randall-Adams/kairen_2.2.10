print("0| Loading Settings")
GameLoop.et.check("SETUP", mode, 3000)
dofile("../EQOA/luas/Modules/FAPI/Classes/NPC.lua")
--dofile("../EQOA/luas/Modules/FAPI/Classes/Ghost.lua")
--dofile("../EQOA/luas/Modules/FAPI/Classes/SpawnPoint.lua")

--nsbegin
function ClearOldFiles()
    if FAPI.IO.File_Exists(RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites) == true then
        os.remove(RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites)
    end
    if FAPI.IO.File_Exists(RF.EQOA.Net_Streams.o.self .. "Player Data" .. Extension_ReadWrites) == true then
        os.remove(RF.EQOA.Net_Streams.o.self .. "Player Data" .. Extension_ReadWrites)
    end
    if FAPI.IO.File_Exists(RF.EQOA.Temp.self .. "Outside Command" .. Extension_Outside_Command) == true then
        os.remove(RF.EQOA.Temp.self .. "Outside Command" .. Extension_Outside_Command)
    end
end

function LogicLoopOneTest()
    print("0| Logic Loop One Successfully Started.")
    FAPI.RunOptions.LogicLoopOneTest.Do = false
end
function TickLoop_OneLogic()
    print("0| Logic Loop 1 Logic Tick")
end
function TickLoop_ThirtySeconds()
    print("0| Logic Loop 30 Second Tick")
end

function Alert_OutputPlayerData()
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
function Alert_OutputPlayerData_ConsoleOutput()
    _pathFAPIData = RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites
    print("0| Alert_OutputPlayerData: _pathFAPIData == " .. _pathFAPIData)
end
function Alert_ProcessOutsideCommands()
    _pathFAPIData = RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites
    print("0| Alert_ProcessOutsideCommands: _pathFAPIData == " .. _pathFAPIData)
    file2 = io.open(_pathFAPIData,"w+");
    file2:write("UpdateKairen");
    file2:write("\n");
    file2:write("UpdateCommandFile");
    file2:write("\n");
    file2:close();
end


function CorstenTest()
    FAPI.RunOptions.CorstenTest.Do = false
    if getOpenedProcessID() ~= nil and readInteger("kernel32.dll") == nil then
        print("0| Not Attached.")
        return false;
    else
        print("0| Attached.")
        return true;
    end

    --[[FAPI.RunOptions.CorstenTest.Do = false
    if readBytes("[pcsx2-r3878.exe+0040239C]+980") == nil then
        print("0| Not Connected to the Save State.")
        FAPI.RunOptions.CorstenTest.Connected = false
    else
        print("0| Connected to the Save State.")
        FAPI.RunOptions.CorstenTest.Connected = true
    end]]--
end

FAPI.RunOptions.LogicLoopOneTest = FAPI.RunOptions.New(LogicLoopOneTest, true)
FAPI.RunOptions.TickLoop_OneLogic = FAPI.RunOptions.New(TickLoop_OneLogic, false)
FAPI.RunOptions.TickLoop_ThirtySeconds = FAPI.RunOptions.New(TickLoop_ThirtySeconds, false, "TickLoop_ThirtySeconds", 30000)

FAPI.RunOptions.ProcessOutsideCommands = FAPI.RunOptions.New(FAPI.Kanizah.ProcessOutsideCommands, true)

FAPI.RunOptions.Alert_OutputPlayerData = FAPI.RunOptions.New(Alert_OutputPlayerData, false)
FAPI.RunOptions.Alert_OutputPlayerData_ConsoleOutput = FAPI.RunOptions.New(Alert_OutputPlayerData_ConsoleOutput, false)
FAPI.RunOptions.Alert_ProcessOutsideCommands = FAPI.RunOptions.New(Alert_ProcessOutsideCommands, false)
--nsend
--dofile("../EQOA/luas/Modules/NewScripts.lua")
ClearOldFiles()
--et("SETUP", "TFT", 9000)
-- Optional Functions
--doUpdateValues = true -- Update LUA Variables
--doUpdateAreas = true -- Spawn NPCs
--ReadFromNPCMaker = false -- Spawn /Custom Data/ NPCs
--doOutputLocation = false -- Output your location to the console
--doOutsideCommands = false -- Process commands from other programs
-- Options for Automatic Functions
--doCOOutput = false -- Enable Console output

--"TestFunctionThree = nil" can be done to erase the original function as the RunOptions function seems 
--to copy it, and not reference it
--or just name the original function the fapi.ro.whatev and then rewrite it later it?

function OutputPlayerData()
    _filepath = RF.EQOA.Net_Streams.o.self .. "Player Data" .. Extension_ReadWrites
    file = io.open(_filepath,"w+");
    file:write("x438748932743");
    file:write("\n");
    file:write("y465273895948");
    file:write("\n");
    file:write("z485734895734");
    file:write("\n");
    file:write("f457489579871");
    file:write("\n");
    file:write("zonefullFreepo't Centrall");
    file:write("\n");
    file:write("zonenameFreepo't");
    file:write("\n");
    file:write("subCentrall");
    file:write("\n");       
    file:close();
    FAPI.RunOptions.Alert_OutputPlayerData.Try()
end
FAPI.RunOptions.CorstenTest = FAPI.RunOptions.New(CorstenTest, true)
FAPI.RunOptions.OutputPlayerData = FAPI.RunOptions.New(OutputPlayerData, false, "OutputPlayerData", 2000)
print("0| All Settings Loaded.")
