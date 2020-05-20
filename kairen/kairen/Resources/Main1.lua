--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Main
Code-Type: LUA Code
Code-Version: 2.0
Code-Description: The Main LUA Loop that pulls the FAPI framework together into something executable.
Code-Author: Robert Randazzio
]]--
--Code By Robert Randazzio
--Help By Dustin Faxon, Daniel Wallace and internet searches
--Testing By Jeremiah Johnson and Daniel Wallace

print("SCRIPT STARTED")
print("mode == " .. mode)
dofile("../EQOA/LUAs/File Paths.lua")
_G.FAPI = {DirectoryLocation = RF.EQOA.LUAs.Modules.FAPI.self;};
dofile(FAPI.DirectoryLocation .. "FAPI.lua")


--output_line_number = 0 -- do not change
--output_file_lines = {0 = "ll5"}
--above can equal a number of lines, or a number of logic loops, 
-- ex: "ll200", for 200 logic loops, or "700" for 700 lines.
--[[
-- Temporary, Unconverted Global Script Variables Below
Thisbox = "Box1"
    value_Triangle = readBytes("[pcsx2-r3878.exe+0040239C]+974")
    value_Square = readBytes("[pcsx2-r3878.exe+0040239C]+975")
    value_X = readBytes("[pcsx2-r3878.exe+0040239C]+976")
    value_Circle = readBytes("[pcsx2-r3878.exe+0040239C]+977")
    --value_L1 = readBytes("[pcsx2-r3878.exe+0040239C]+97C")
    value_L2 = readBytes("[pcsx2-r3878.exe+0040239C]+97D")
    --value_L3 = readBytes("[pcsx2-r3878.exe+0040239C]+982")
    --value_R1 = readBytes("[pcsx2-r3878.exe+0040239C]+97E")
    value_R2 = readBytes("[pcsx2-r3878.exe+0040239C]+97F")
    --value_R3 = readBytes("[pcsx2-r3878.exe+0040239C]+983")
    --value_Up = readBytes("[pcsx2-r3878.exe+]+")
    --value_Right = readBytes("[pcsx2-r3878.exe+]+")
    --value_Down = readBytes("[pcsx2-r3878.exe+]+")
    --value_Left = readBytes("[pcsx2-r3878.exe+]+")
    value_Start = readBytes("[pcsx2-r3878.exe+0040239C]+980")
    value_Select = readBytes("[pcsx2-r3878.exe+0040239C]+981")
]]--


GameLoop = {};
GameLoop.et = {
    elapsed_times = {};
    elapsed_times_last = {};
    elapsed_time = 0; -- How much time has passed since you started counting
    check = function (_time, _mode, _threshold)
        if _time == "SETUP" then
            GameLoop.et.elapsed_times[_mode] = _threshold
            GameLoop.et.elapsed_times_last[_mode] = 0
        elseif GameLoop.et.elapsed_time - GameLoop.et.elapsed_times_last[_time] > GameLoop.et.elapsed_times[_time] then
            GameLoop.et.elapsed_times_last[_time] = GameLoop.et.elapsed_time
            return true;
        elseif GameLoop.et.elapsed_time - GameLoop.et.elapsed_times_last[_time] < GameLoop.et.elapsed_times[_time] then
            return false;
        end
    end;
};
GameLoop.LogicLoop = {};
GameLoop.LogicLoop.RunSettingsFile = function ()

            if _G.mode == "" then
                print("0| Error: No Mode Set -- Setting Console Mode")
                _G.mode = "Console Mode"
            end
            if FAPI.IO.File_Exists(RF.EQOA.LUAs.Modes.self .. mode .. "/" .. mode .. "-Settings" .. ".lua") then 
                dofile(RF.EQOA.LUAs.Modes.self .. mode .. "/" .. mode .. "-Settings" .. ".lua")
            else
                print("0| Error: No Settings File Found -- Using Console Mode Settings")
                _G.mode = "Console Mode"
                et.check("SETUP", mode, 1000)
            end
        end;
GameLoop.LogicLoop.SettingsLoopFile = RF.EQOA.LUAs.Modes.self .. mode .. "/" .. mode .. "-Settings" .. ".lua";
GameLoop.LogicLoop.GameLogicLoopFile = RF.EQOA.LUAs.Modes.self .. mode .. "/" .. mode .. ".lua";
GameLoop.LogicLoop.Started = 0;
GameLoop.LogicLoop.Ended = 0;
GameLoop.LogicLoop.DoNormalOutput = false;
GameLoop.LogicLoop.DoErrorOutput = true;
GameLoop.LogicLoop.Start = (function ()
            if GameLoop.LogicLoop.Started ~= GameLoop.LogicLoop.Ended then
                --co(_sender, "co_debugoutput", "Error: The Last Logic Loop Did Not Complete.")
                if DoErrorOutput ~= false then
                    print("Error: The Last Logic Loop Did Not Complete.");
                end
                GameLoop.LogicLoop.Ended = GameLoop.LogicLoop.Ended + 1
            end
            GameLoop.LogicLoop.Started = GameLoop.LogicLoop.Started + 1
            if GameLoop.LogicLoop.DoNormalOutput ~= false then
                --co(_sender, "co_comment", "Started Logic Loop " .. LogicLoopsStarted)
                print("Started Logic Loop " .. GameLoop.LogicLoop.Started);
            end
        end);
GameLoop.LogicLoop.End = (function ()
            GameLoop.LogicLoop.Ended = GameLoop.LogicLoop.Ended + 1
            --co(_sender, "co_comment", "Completed Logic Loop " .. LogicLoopsEnded)
            if GameLoop.LogicLoop.DoNormalOutput ~= false then
                print("Completed Logic Loop " .. GameLoop.LogicLoop.Ended);
            end
        end);
GameLoop.LogicLoop.LoopFunction = (function ()
                GameLoop.LogicLoop.Start()
                if FAPI.IO.File_Exists(GameLoop.LogicLoop.GameLogicLoopFile) == true then
                    dofile(GameLoop.LogicLoop.GameLogicLoopFile)
                else
                    --co(_sender, "co_debugoutput", "Error: No Mode File Found")
                    print("0| Error: No Mode File Found")
                end
                GameLoop.LogicLoop.End()
        end);
GameLoop.ConsoleOutput = {};
GameLoop.MyTimer = {};
GameLoop.MyTimer.interval_timer = 100;
GameLoop.MyTimer.timer = createTimer(nil);
GameLoop.MyTimer.timer.Interval = GameLoop.MyTimer.interval_timer;
GameLoop.MyTimer.timer.OnTimer = function()
        if GameLoop.et.check(mode) then
            GameLoop.LogicLoop.LoopFunction()
        end
        GameLoop.et.elapsed_time = GameLoop.et.elapsed_time + GameLoop.MyTimer.interval_timer
    end;
    
GameLoop.LogicLoop.RunSettingsFile()
GameLoop.MyTimer.timer.Enabled = true;



--settings()
--(){
---update loop() -- read updates from ss through ce
---game loop() -- do game logic code
---draw loop() -- display the changes on the screen
--}

--[[ ghosts making
doUpdateValues = false -- Updates Core Script Values to Equal the Game's
doUpdateAreas = false -- Spawn NPCs
doOutputLocation = false -- Output your location to console
doOutsideCommands = false -- Outside Commands
--Variable Options
doCOOutput = true -- Console Output

--LogicLoopsStarted = 0
--LogicLoopsEnded = 0
--NumberofGhosts = 0

--Setup Functions
function setupGhosts()
    local _sender = "setupGhosts"
    co(_sender, "co_enter", "")
    local i
    name = {};
    i = 1
    if SaveState == nil or SaveState == "Corsten" then
        if SaveState == nil then co(_sender, "co_debugoutput", "Notice: SaveState variable is nil") end
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
            Ghosts[i] = Ghost.new(name[i])
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
            Ghosts[i] = Ghost.new(name[i])
            Ghosts[i].isFree = true
            i = i + 1
        end
        NumberofGhosts = i
    else
        co(_sender, "co_debugoutput", "Error: No Ghosts code for the SaveState: " .. SaveState)
    end
    
    co(_sender, "co_debugoutput", "Number of Ghosts Loaded = " .. NumberofGhosts)
    co(_sender, "co_exit", "")
end
]]--


--[[ old ll stuff
LL = {
    Started = 0;
    Ended = 0;
    DoNormalOutput = false;
    DoErrorOutput = true;
    Start = 
        function ()
            if LL.Started ~= LL.Ended then
                --co(_sender, "co_debugoutput", "Error: The Last Logic Loop Did Not Complete.")
                if DoErrorOutput ~= false then
                    print("Error: The Last Logic Loop Did Not Complete.");
                end
                LL.Ended = LL.Ended + 1
            end
            LL.Started = LL.Started + 1
            if LL.DoNormalOutput ~= false then
                --co(_sender, "co_comment", "Started Logic Loop " .. LogicLoopsStarted)
                print("Started Logic Loop " .. LL.Started);
            end
        end;
    End = 
        function ()
            LL.Ended = LL.Ended + 1
            --co(_sender, "co_comment", "Completed Logic Loop " .. LogicLoopsEnded)
            if LL.DoNormalOutput ~= false then
                print("Completed Logic Loop " .. LL.Ended);
            end
        end;
};


 function LoopFunction()
   
    -- Current Mode's PreCode Runs Now
    --dofile(RF.EQOA.LUAs.Modes.self .. mode .. "/PreCode.lua")
    
    -- Add ordering schema to below, so you can inject your own processes where you need to.
    if doUpdateValues then updateValues() end
    if doUpdateAreas then updateAreas() end
    if doOutputLocation then outputLocation() end
    if doOutsideCommands then ProcessOutsideCommands() end

    -- Clear Updated Flags
    WasUpdated["my_Location"] = false
    WasUpdated["my_X"] = false
    WasUpdated["my_Y"] = false
    WasUpdated["my_Z"] = false
    WasUpdated["my_F"] = false
    WasUpdated["my_ZoneFull"] = false
    WasUpdated["my_Zone"] = false
    WasUpdated["my_ZoneSub"] = false
    --Clear WasUpdated[] to false ---
    --for key,value in pairs(WasUpdated) do
    --    WasUpdated[key] = false
    --end
    -- Current Mode's PostCode Runs Now
    --dofile(RF.EQOA.LUAs.Modes.self .. mode .. "/PostCode.lua")
  
    -- If co() is supposed to do file output based on logic loops, trigger that now
    if Left(output_file_lines[0], 2) == "ll" then
        _loopstodo = tonumber(Right(output_file_lines[0], string.len(output_file_lines[0]) - 2))
        if LogicLoopsEnded == _loopstodo then
            co(_sender, "do_fileoutput")
            output_file_lines[0] = 0
        elseif LogicLoopsEnded > _loopstodo then
            _loopsdifference = LogicLoopsEnded - _loopstodo
            co(_sender, "co_debugoutput", "Error: Console Outputting " .. _loopsdifference .. " Logic Loops Past Due")
            co(_sender, "co_debugoutput", "  This is caused by a code error that prevented the previous loop(s) from finishing.")
            co(_sender, "do_fileoutput")
            output_file_lines[0] = 0
        end
    end

end

        -- Read Start and Select, if both are pressed Toggle Console Output on or off.
        value_Start = readBytes("[pcsx2-r3878.exe+0040239C]+980")
        value_Select = readBytes("[pcsx2-r3878.exe+0040239C]+981")
        if value_Select == 1 and value_Start == 1 then
            if doCOOutput == true then
                print("Shutting off Console Output...")
                doCOOutput = false
            elseif doCOOutput == false then
                print("Turning on Console Output...")
                doCOOutput = true
            end
]]--
--[[ To [Maybe] Do List
Millisecond Converter Function
File Reader Function
File Writer Function
Add Arguments to Force The Use of a Specified Old File Version
Add NPCs to where walls are when creating them.
-Add these NPCs to your group so you can target them easily.

]]--

            --[[ -- Other Write String to Bit Array Function Versions
function currentWrite_StringToBitArrayAddress(_string, BitArrayAddress)
    --_sender = "Write_StringToBitArrayAddress"
    --co(_sender, "co_enter", "String, BitArrayAddress")
    local sp
    local i
    local i2
    sp = 1
    i = 1
    --i2 = i + 1
    StringAsArrayofBits = {};
    while sp <= string.len(_string) do
        StringAsArrayofBits[i] = string.byte(_string, sp)
        --StringAsArrayofBits[i2] = 00
        co(_sender, "co_debugoutput", StringAsArrayofBits[i])
        sp = sp + 1
        i = i + 1
        --i = i + 2
        --i2 = i + 1
    end
    StringAsArrayofBits[i] = 00
    StringAsArrayofBits[i + 1] = 00
    --StringAsArrayofBits[i2] = 00
    writeBytes(BitArrayAddress, StringAsArrayofBits)
    
    if Read_BitArrayToString(BitArrayAddress, "TypeZoneName") == _string then
        co(_sender, "co_debugoutput", _sender .. " thinks it worked. Did it?..")
    else
        co(_sender, "co_debugoutput", _sender .. " has detected that it didn't work. Did it?..")
    end    
end
function newWrite_StringToBitArrayAddress(String, BitArrayAddress)
    _sender = "Write_StringToBitArrayAddress"
    co(_sender, "co_enter", "String, BitArrayAddress")
    local sp
    local i
    local i2
    sp = 1
    i = 1
    i2 = i + 1
    StringAsArrayofBits = {};
    while sp <= string.len(String) do
        StringAsArrayofBits[i] = string.byte(String, sp)
        StringAsArrayofBits[i2] = 00
        co(_sender, "co_debugoutput", StringAsArrayofBits[i])
        sp = sp + 1
        i = i + 2
        i2 = i + 1
    end
    StringAsArrayofBits[i] = 00
    StringAsArrayofBits[i2] = 00
    writeBytes(BitArrayAddress, StringAsArrayofBits)
    
        
    if Read_BitArrayToString(BitArrayAddress, "TypeZoneName") == _string then
        co(_sender, "co_debugoutput", _sender .. " thinks it worked. Did it?..")
    else
        co(_sender, "co_debugoutput", _sender .. " has detected that it didn't work. Did it?..")
    end
    
    end
function oldWrite_StringToBitArrayAddress(String, BitArrayAddress)
    --_sender = "Write_StringToBitArrayAddress"
    --co(_sender, "co_enter", "String, BitArrayAddress")
    local sp
    local i
    local i2
    sp = 1
    i = 1
    --i2 = i + 1
    StringAsArrayofBits = {};
    while sp <= string.len(String) do
        StringAsArrayofBits[i] = string.byte(String, sp)
        --StringAsArrayofBits[i2] = 00
        --co(_sender, "co_debugoutput", StringAsArrayofBits[i])
        sp = sp + 1
        i = i + 1
        --i = i + 2
        --i2 = i + 1
    end
    StringAsArrayofBits[i] = 00
    StringAsArrayofBits[i + 1] = 00
    --StringAsArrayofBits[i2] = 00
    writeBytes(BitArrayAddress, StringAsArrayofBits)
end]]--
            --[[-- Zone Manipulation Here
            -- Zone Manipulation Here
            --
            local ZoneAsString
            --ZoneAsString = Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", 32, true)
            ZoneAsString = Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", "TypeZoneName")
            co(_sender, "co_debugoutput", "Zone: " .. ZoneAsString .. ":")
            
            if Left(ZoneAsString, 4) ~= "New " then
                NewZoneName = "New " .. ZoneAsString
                co(_sender, "co_debugoutput", "Changing Zone to " .. NewZoneName)
                Write_StringToBitArrayAddress(NewZoneName, "[pcsx2-r3878.exe+00400B28]+D30", 2)
            end
            --
            -- 
            --]]--
