print("0| Loading Settings")
GameLoop.et.check("SETUP", mode, 3000)
--dofile("../EQOA/luas/Modules/FAPI/Classes/NPC.lua")
--dofile("../EQOA/luas/Modules/FAPI/Classes/Ghost.lua")
--dofile("../EQOA/luas/Modules/FAPI/Classes/SpawnPoint.lua")
dofile("../EQOA/luas/Modules/FAPI/Classes/NPC.lua")

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
FAPI.RunOptions.CorstenTest = FAPI.RunOptions.New(CorstenTest, true)
if CorstenTest() == false then
    print("0| Sorry, you are not connected. Exiting.")
    closeCE()
end
--[[et("SETUP", "TFT", 9000)
-- Optional Functions
--doUpdateValues = true -- Update LUA Variables
--doUpdateAreas = true -- Spawn NPCs
--ReadFromNPCMaker = false -- Spawn /Custom Data/ NPCs
--doOutputLocation = false -- Output your location to the console
--doOutsideCommands = false -- Process commands from other programs
-- Options for Automatic Functions
]]--doCOOutput = false -- Enable Console output

function NPCTest()
--setup ghosts class to read file and point to each address for each line
--setup npc class to point to the ghost class, which points to a file
    --_G.Ghosts = {};
    _G.Ghosts[1] = _G.Ghosts.New("Coachman_Ronks")
    NPCs[1] = NPCs.New("Coachman_Ronks")    
    SpawnPoints[1] = SpawnPoints.New("Freeport_Coach2")    
    --NPCs[1].myGhost = _G.Ghosts[1]
    --NPCs[1].Spawn()
    NPCs[1].Speak(nil, "Say")
    NPCs[1].Speak("Name")
    NPCs[1].Speak("Location", "Shout")
    NPCs[1].Speak("Zone")    
    FAPI.RunOptions.NPCTest.Do = false
end

function SpawnNPCCode()
    FAPI.RunOptions.SpawnNPCCode.Do = false
    --local _sender = "setupGhosts"
    --co(_sender, "co_enter", "")
    local i
    name = {};
    i = 1
    if SaveState == nil or SaveState == "Corsten" then
        --if SaveState == nil then co(_sender, "co_debugoutput", "Notice: SaveState variable is nil") end
        name[1] = "Coachman_Ronks"
        name[2] = "Aloj_Tilsteran"
        name[3] = "Merchant_Kari"
        name[4] = "Dr_Killian"
        name[5] = "Guard_Saolen"
        name[6] = "Guard_Jahn"
        name[7] = "Bowyer_Koll"
        name[8] = "Tailor_Bariston"
        name[9] = "Tailor_Zixar"
        name[10] = "Guard_Serenda"
        name[11] = "a_badger"
        name[12] = "an_undead_mammoth"
        name[13] = "Angry_Patron"
        name[14] = "Arch_Familiar"
        name[15] = "Finalquestt"
        name[16] = "Guard_Perinen"
        name[17] = "Manoarmz"
        name[18] = "Marona_Jofranka"
        name[19] = "Merchant_Ahkham"
        name[20] = "Nukenurplace"
        name[21] = "Raam"
        name[22] = "Royce_Tilsteran"
        --name[23] = "sign_post"
        while name[i] ~= nil do
            Ghosts[i] = Ghosts.New(name[i])
            Ghosts[i].isFree = true
            i = i + 1
        end
        NumberofGhosts = i
    elseif SaveState == "Lostologist" then    
        name[1] = "elder_spirit"
        name[2] = "evil_head"
        name[3] = "kappa_drudge1"
        name[4] = "kappa_drudge2"
        name[5] = "kappa_drudge3"
        name[6] = "kappa_drudge5"
        name[7] = "phantom1"
        name[8] = "phantom2"
        name[9] = "phantom3"
        name[10] = "phantom4"
        name[11] = "phantom5"
        name[12] = "phantom6"
        name[13] = "phantom7"
        name[14] = "phantom8"
        name[15] = "phantom9"
        name[16] = "Spectral_Servant"
        name[17] = "spirit1"
        name[18] = "spirit2"
        name[19] = "spirit3"
        name[20] = "spirit4"
        name[21] = "spirit5"
        name[22] = "spirit6"
        name[23] = "spirit7"
        while name[i] ~= nil do
            Ghosts[i] = Ghosts.New(name[i])
            Ghosts[i].isFree = true
            i = i + 1
        end
        NumberofGhosts = i
    else
        --co(_sender, "co_debugoutput", "Error: No Ghosts code for the SaveState: " .. SaveState)
    end
    print("0| Number of Ghosts Loaded = " .. NumberofGhosts)
    
    --co(_sender, "co_debugoutput", "Number of Ghosts Loaded = " .. NumberofGhosts)
    --co(_sender, "co_exit", "")
end


--"TestFunctionThree = nil" can be done to erase the original function as the RunOptions function seems 
--to copy it, and not reference it
--or just name the original function the fapi.ro.whatev and then rewrite it later it?

function OutputPlayerData()
    _filepath = RF.EQOA.Net_Streams.o.self .. "Player Data" .. Extension_ReadWrites
    file = io.open(_filepath,"w+");
    file:write(FAPI.EQOA.Player.Location.X());
    file:write("\n");
    file:write(FAPI.EQOA.Player.Location.Y());
    file:write("\n");
    file:write(FAPI.EQOA.Player.Location.Z());
    file:write("\n");
    file:write(FAPI.EQOA.Player.Location.F());
    file:write("\n");
    file:write(FAPI.EQOA.Player.Location.ZoneFull());
    file:write("\n");
    file:write(FAPI.EQOA.Player.Location.Zone());
    file:write("\n");
    file:write(FAPI.EQOA.Player.Location.ZoneSub());
    file:write("\n");       
    file:close();
    FAPI.RunOptions.Alert_OutputPlayerData.Try()
end

FAPI.RunOptions.OutputPlayerData = FAPI.RunOptions.New(OutputPlayerData, false, "OutputPlayerData", 2000)
FAPI.RunOptions.NPCTest = FAPI.RunOptions.New(NPCTest, false)
FAPI.RunOptions.SpawnNPCCode = FAPI.RunOptions.New(SpawnNPCCode, false)

print("0| All Settings Loaded.")