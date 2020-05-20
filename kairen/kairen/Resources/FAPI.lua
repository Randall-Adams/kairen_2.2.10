--[[ Meta-Data
Version: 1.0
]]--
  -- -- FAPI Classes -- --
dofile("../EQOA/luas/Modules/FAPI/Classes/Area.lua")
dofile("../EQOA/luas/Modules/FAPI/Classes/Ghost.lua")
dofile("../EQOA/luas/Modules/FAPI/Classes/NPC.lua")
-- FAPI Simple Classes --
--Ghosts = { };
--NPCs = { };
--Areas = { };
--My = {}; -- This is your own info
--WasUpdated = {}; -- Keeps track of when things have changed
_G.My = {}; -- This is your own info
_G.WasUpdated = {}; -- Keeps track of when things have changed


  -- -- FAPI Functions -- --
-- FAPI Game Functions --
-- FAPI Game:Main Functions
function Spawn_NPCs_By_Location(_Area)
    _sender = "Spawn_NPCs_By_Location"
    co(_sender, "co_enter", "_Area")
    local continue = false
    -- Reads NPC Area from file
    _filepath = _Area.filepath  .. Extension_NPC_Area_Maker
    if File_Exists(_filepath) == true then
        --co(_sender, "co_debugoutput", "File Exists == True")
        _file = io.open(_filepath);
        fileVersion = IO_ReadNextLine(_file, "%-%-")
        if fileVersion == "1.0" then
            IO_ReadNextLine(_file, "%-%-") -- SafeName of Area
            IO_ReadNextLine(_file, "%-%-") -- Name of Zone
            _xmin = IO_ReadNextLine(_file, "%-%-")
            _xmax = IO_ReadNextLine(_file, "%-%-")
            _ymin = IO_ReadNextLine(_file, "%-%-")
            _ymax = IO_ReadNextLine(_file, "%-%-")
            _zmin = IO_ReadNextLine(_file, "%-%-")
            _zmax = IO_ReadNextLine(_file, "%-%-")
            --NPCToSpawn = IO_ReadNextLine(_file, "%-%-")
            continue = true
            --co(_sender, "co_debugoutput", "No More NPCs to Spawn")
        elseif fileVersion == "1.1" then
            IO_ReadNextLine(_file, "%-%-") -- SafeName of Area
            IO_ReadNextLine(_file, "%-%-") -- Name of Zone
            IO_ReadNextLine(_file, "%-%-") -- Name of Sub Zone
            _xmin = IO_ReadNextLine(_file, "%-%-")
            _xmax = IO_ReadNextLine(_file, "%-%-")
            _ymin = IO_ReadNextLine(_file, "%-%-")
            _ymax = IO_ReadNextLine(_file, "%-%-")
            _zmin = IO_ReadNextLine(_file, "%-%-")
            _zmax = IO_ReadNextLine(_file, "%-%-")
            --NPCToSpawn = IO_ReadNextLine(_file, "%-%-")
            continue = true
        else
            co(_sender, "co_debugoutput", "Error: File Version " .. fileVersion .. " Not Supported.")
        end
        if continue == true then
            NPCToSpawn = IO_ReadNextLine(_file, "%-%-")
            while NPCToSpawn ~= nil do
                --co(_sender, "co_debugoutput", "NPCToSpawn == " .. tostring(NPCToSpawn))
                Spawn_NPC(NPCToSpawn)
                NPCToSpawn = IO_ReadNextLine(_file, "%-%-")
            end
        end
        _file:close();
    else
        co(_sender, "co_debugoutput", "Error: File Does Not Exist: " .. _filepath)
    end
    
    co(_sender, "co_exit", "")
end
function Spawn_NPC(_NPCFileName, _owner, _forceRespawn) -- Optional: Returns the NPC[] Used
    local _sender = "Spawn_NPC"
    if _owner == nil then _owner = "free" end
    co(_sender, "co_enter", _NPCFileName .. ", " .. _owner)-- .. ", _ghost.ID: " .. _ghost.ghostIDGet
    --This function should create a new NPC class, and then fill it's values
    --This function does the following:
    -- Finds the NPC File
    -- Finds the Ghost Reference
    -- Creates a new NPC Reference
    -- Fills the NPC file data into the NPC Reference
    
    local _npc
    local continue
    local _folderLocation
    continue = true
    if continue == true then
        continue = false
        local loops
        loops = 1
        while loops <= NumberofGhosts and NPCs[loops] ~= nil do
            _npc = NPCs[loops]
            if _npc.nameSafe == _NPCFileName then -- if this NPC is spawned already [by file name]
                co(_sender, "co_debugoutput", _npc.nameSafe .. " Spawned Already [by file name].")
                if _forceRespawn == true then
                    break
                else
                end
                    continue = false -- don't spawn NPC
                loops = 25
            end
            if loops ~= 25 then
                loops = loops + 1
            end
        end
        if loops ~= 25 and (loops == (NumberofGhosts + 1) or NPCs[loops] == nil) then -- if NPC not spawned
            continue = true -- do spawn NPC
        end
        if loops ~= 25 then
            loops = 0
        end
    end
    loops = 0
    if continue == true or _forceRespawn == true then
        if _forceRespawn == true then co(_sender, "co_debugoutput", "Force Respawn == True") end
        continue = false
        -- Check if NPC File Exists, if it does set it for use
        if File_Exists(Folder_NPCs .. _NPCFileName .. Extension_NPCFiles) == true then
            if ReadFromNPCMaker == true then co(_sender, "co_debugoutput", "File is in Normal Folder") end
            _folderLocation = "Normal"
            fileOfNPC = Folder_NPCs .. _NPCFileName .. Extension_NPCFiles
            ----doRead = true
            continue = true
        elseif ReadFromNPCMaker == true and File_Exists(Folder_NPC_Maker_Output .. _NPCFileName .. Extension_NPC_Maker) == true then
            co(_sender, "co_debugoutput", "File is in Custom Folder")
            _folderLocation = "Custom"
            fileOfNPC = Folder_NPC_Maker_Output .. _NPCFileName .. Extension_NPC_Maker
            ----doRead = true
            continue = true
        else
            co(_sender, "co_debugoutput", "NPC File does not exist or is unable to be read from.")
            continue = false
        end
    end
    if continue == true then
        continue = false
        -- Find Free NPC
        co(_sender, "co_debugoutput", "Creating New NPC Class..")
        local i_npcs
        i_npcs = 1
        local loops
        loops = 1
        while loops <= NumberofGhosts do
        --check around here for _force respawn and if so use _npc
            if NPCs[i_npcs] == nil then
                co(_sender, "co_debugoutput", "NPCs[" .. i_npcs .. "] == Nil and will be used.")
                NPCs[i_npcs] = NPC.new(_NPCFileName, _owner)
                _npc = NPCs[i_npcs]
                _npc.folderLocation = _folderLocation
                loops = 25
                continue = true
                co(_sender, "co_debugoutput", "Ghost Set to: " .. _npc.myGhost.ghostID)
            elseif NPCs[i_npcs].isFreeOrReplaceable == true then
                co(_sender, "co_debugoutput", "NPC[" .. i_npcs .. "]isFreeOrReplaceable == True and will be used.")
                _npc = NPCs[i_npcs]
                NPCs[i_npcs].isFreeOrReplaceable = false -- replace with "_npc."
                NPCs[i_npcs].Owner = _owner
                _npc.folderLocation = _folderLocation
                loops = 25
                continue = true
                co(_sender, "co_debugoutput", "Ghost Set to: " .. _npc.myGhost.ghostID)
            else
                i_npcs = i_npcs + 1
            end
            if loops ~= 25 then
                loops = loops + 1
            end
        end
        if loops == (NumberofGhosts + 1) then
            co(_sender, "co_debugoutput", "No Free NPCs[] Slots")
        end
    end
    if continue == true then
        continue = false -- Now will be set to true if you did spawn an NPC
    -- Set NPC Variables to New NPC
    --if File_Exists(fileOfNPC) == true and _npc.isFreeOrReplaceable == true then
        co(_sender, "co_debugoutput", "Setting NPC's Stats..")
        file = io.open(fileOfNPC,"r+"); -- This is checked for existence above..
        FileVersion = IO_ReadNextLine(file, "%-%-")
        co(_sender, "co_debugoutput", "File Version is: " .. FileVersion)
            if FileVersion == "0.1.1" then
                _npc.FileVersion_Loaded = "0.1.1"
                _npc.nameSafe = IO_ReadNextLine(file, "%-%-")
                _npc.nameSet(IO_ReadNextLine(file, "%-%-"))
                _npc.XSet(IO_ReadNextLine(file, "%-%-"))
                _npc.YSet(IO_ReadNextLine(file, "%-%-"))
                _npc.ZSet(IO_ReadNextLine(file, "%-%-"))
                _npc.FSet(IO_ReadNextLine(file, "%-%-"))
                IO_ReadNextLine(file, "%-%-")--        NPCs[i].Race = IO_ReadNextLine(file, "%-%-")
                IO_ReadNextLine(file, "%-%-")--        NPCs[i].Gender = IO_ReadNextLine(file, "%-%-")
                IO_ReadNextLine(file, "%-%-")--        NPCs[i].Class = IO_ReadNextLine(file, "%-%-")        
                _npc.levelSet(IO_ReadNextLine(file, "%-%-"))
                _npc.hpSet(IO_ReadNextLine(file, "%-%-"))
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].MP = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].AC = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Strength = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Stamina = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Agility = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Dexterity = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Wisdom = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Intelligence = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].Charisma = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].FR = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].CR = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].LR = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].AR = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].PR = IO_ReadNextLine(file, "%-%-")
                --IO_ReadNextLine(file, "%-%-")--        NPCs[i].DR = IO_ReadNextLine(file, "%-%-")
                continue = true
                co(_sender, "co_debugoutput", "All NPC Values Set")
            else
                co(_sender, "co_debugoutput", "File Version Not Supported")
                continue = false
            end
        file:close();
    end
    
    if continue == false then -- if no NPC was spawned then
        co(_sender, "co_debugoutput", "Did not spawn any NPC.")
        return nil
    elseif continue == true then
        co(_sender, "co_debugoutput", "Spawned an NPC. Returning _npc[.nameGet]: " .. _npc.nameGet())
        return _npc
    end

--end

    co(_sender, "co_exit", _NPCFileName)-- .. ", _ghostID: " .. _ghost.ghostID)
end
function Spawn_Character(_name, _x, _y, _z, _f, _owner)
    local _sender = "Spawn_Character"
    if _owner == nil then _owner = "free" end
    co(_sender, "co_enter", _name .. ", " .. _owner)
    --This function should create a new NPC class, and then fill it's values
    --This function does the following:
    -- Finds the Ghost Reference
    -- Creates a new NPC Reference
    -- Fills the NPC data into the NPC Reference

    local continue
    continue = true
    -- find a free NPCs[] and use it.. some of my code needs rewritten i see..
    co(_sender, "co_debugoutput", "Creating New NPC Class..")
    local i_npcs
    i_npcs = 1
    local loops
    loops = 1
    while loops <= NumberofGhosts do
        if NPCs[i_npcs] == nil then
            co(_sender, "co_debugoutput", "NPCs[" .. i_npcs .. "] == Nil and will be used.")
            NPCs[i_npcs] = NPC.new(_NPCFileName, _owner)
            _npc = NPCs[i_npcs]
            loops = 25
            continue = true
            co(_sender, "co_debugoutput", "Ghost Set to: " .. _npc.myGhost.ghostID)
        elseif NPCs[i_npcs].isFreeOrReplaceable == true then
            co(_sender, "co_debugoutput", "NPC[" .. i_npcs .. "]isFreeOrReplaceable == True and will be used.")
            _npc = NPCs[i_npcs]
            NPCs[i_npcs].isFreeOrReplaceable = false
            NPCs[i_npcs].Owner = _owner
            loops = 25
            continue = true
            co(_sender, "co_debugoutput", "Ghost Set to: " .. _npc.myGhost.ghostID)
        else
            i_npcs = i_npcs + 1
        end
        if loops ~= 25 then
            loops = loops + 1
        end
    end
    if loops == (NumberofGhosts + 1) then
        co(_sender, "co_debugoutput", "No Free NPCs[] Slots")
    end
end

-- FAPI Game:Update Functions
function updateAreas()
    local _sender = "updateAreas"
    co(_sender, "co_enter", "")
    local i
    i = 1
    --If you moved..'
    local _folderLocation
    if WasUpdated["my_Location"] == true then
        --co(_sender, "co_debugoutput", "You Moved, so figuring out what to do..")
        --If you moved to a new Zone or SubZone..
        if WasUpdated["my_ZoneFull"] == true then
            --co(_sender, "co_debugoutput", "You changed zones or sub zones...")
            --Clear old Area Data from memory
            while Areas[i] ~= nil do
                Areas[i] = nil
                i = i + 1
            end
            i = 1
            
            filepath_main = Folder_Area_Zones .. My["my_Zone"] .. "/" .. My["my_ZoneSub"] .. "/"
            filepath_indexmain = filepath_main .. "index" .. Extension_NPC_Area_Maker
            
            filepath_custom = Folder_NPC_Area_Maker_Zones .. My["my_Zone"] .. "/" .. My["my_ZoneSub"] .. "/"
            filepath_indexcustom = filepath_custom .. "index" .. Extension_NPC_Area_Maker
            continue = false
            --If there is an index file present for the SubZone
            if File_Exists(filepath_indexmain) == true then
                co(_sender, "co_debugoutput", "File is in Main Folder")
                _folderLocation = "Normal"
                filepath_base = filepath_main
                continue = true
            elseif File_Exists(filepath_indexcustom) == true then
                co(_sender, "co_debugoutput", "File is in Custom Folder")
                _folderLocation = "Custom"
                filepath_base = filepath_custom
                continue = true
            else
                co(_sender, "co_debugoutput", "Notice: SubZone Index File does not exist: /" .. My["my_Zone"] .. "/" .. My["my_ZoneSub"] .. "/index.html")
                co(_sender, "co_debugoutput", "(This means there are no files for this entire subzone.)")
            end
            if continue == true then
                --Load each area in this SubZone
                filepath_index = filepath_base .. "index" .. Extension_NPC_Area_Maker
                
                --co(_sender, "co_debugoutput", "SubZone Index File does exist: " .. filepath_index)
                file_Index = io.open(filepath_index, "r+");
                areafileread = IO_ReadNextLine(file_Index, "%-%-")
                filename = filepath_base .. areafileread .. Extension_NPC_Area_Maker
                --Load the area file specified by the index file into memory
                --Load all Areas in this SubZone
            end

            while file_Index ~= nil and areafileread ~= nil and File_Exists(filename) == true do
                --co(_sender, "co_lineofcode", "Area" .. i .. " = Area.new(\"" .. areafileread .. "\");")
                --co(_sender, "co_debugoutput", "Area File exists, loading " .. areafileread .. " into memory.")
                Areas[i] = Area.new(filepath_base .. areafileread); -- Makes Area
                Areas[i].folderLocation = _folderLocation
                Areas[0] = Areas[i]
                i = i + 1
                areafileread = IO_ReadNextLine(file_Index, "%-%-")
            end
            if file_Index ~= nil then
                file_Index:close();
            end
        else
            --co(_sender, "co_debugoutput", "You are in the same zone and subzone, do nothing..")
        end
        
        --co(_sender, "co_debugoutput", "If any areas are in memory, check if you are in one..")
        i = 1
        --While there is an Area in memory to iterate through
        if Areas[i] == nil then
            --co(_sender, "co_debugoutput", "No areas are loaded...")
        end
        offsetZmin = 0
        offsetZmax = 0
        while Areas[i] ~= nil do
            --If you are in Area[i]..
            if My["my_X"] >= tonumber(Areas[i].Xmin) and My["my_X"] <= tonumber(Areas[i].Xmax)
            and My["my_Y"] >= tonumber(Areas[i].Ymin) and My["my_Y"] <= tonumber(Areas[i].Ymax) 
            and My["my_Z"] >= tonumber(Areas[i].Zmin - offsetZmin) and My["my_Z"] <= tonumber(Areas[i].Zmax + offsetZmax) then
                --If my old area is not my present area..
                if My["my_Area"] == nil or My["my_Area"] ~= Areas[i].nameSafe then
                    --co(_sender, "co_debugoutput", "You are in area: " .. Areas[i].nameSafe)
                    My["my_Area"] = Areas[i].nameSafe
                    Areas[0] = Areas[i]
                    WasUpdated["my_Area"] = true
                    Spawn_NPCs_By_Location(Areas[i])
                    break
                end
            else
                My["my_Area"] = nil
                Areas[0] = nil
                --co(_sender, "co_debugoutput", "You are not in area: " .. Areas[i].nameSafe)
            end
            i = i + 1
        end
    end

    co(_sender, "co_exit", "")
end
function updateValues()
    local _sender = "updateValues"
    co(_sender, "co_enter", "")
    local tempvar, tempvar2
    -- Update Location Information
    -- updating zone name is done after checking if you've even changed locations.
    tempvar = readInteger("[pcsx2-r3878.exe+0040239C]+760")
    checkUpdated(tempvar, "my_X", "my_X")
    --co(_sender, "co_debugoutput", "X: " .. tempvar)
    tempvar = readInteger("[pcsx2-r3878.exe+0040239C]+768")
    checkUpdated(tempvar, "my_Y", "my_Y")
    --co(_sender, "co_debugoutput", "Y: " .. tempvar)
    tempvar = readInteger("[pcsx2-r3878.exe+0040239C]+764")
    checkUpdated(tempvar, "my_Z", "my_Z")
    --co(_sender, "co_debugoutput", "Z: " .. tempvar)
    tempvar = readInteger("[pcsx2-r3878.exe+0040239C]+730")
    checkUpdated(tempvar, "my_F", "my_F")
    --co(_sender, "co_debugoutput", "F: " .. tempvar)
    tempvar = ""
    --If your location changed..
    if WasUpdated["my_X"] == true or WasUpdated["my_Y"] == true or WasUpdated["my_Z"] == true then
        --co(_sender, "co_debugoutput", "My X, Y or Z changed..")
        WasUpdated["my_Location"] = true
        -- Update Current Zone Information
        --Update Full Zone Variable
        tempvar = Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", "TypeZoneName")
        checkUpdated(tempvar, "my_ZoneFull", "my_ZoneFull")
        tempvar = ""
        --Update SubZone Information
        --This code could be updated to account for varying lengths between the ()
        if WasUpdated["my_ZoneFull"] == true then
            --co(_sender, "co_debugoutput", "My[\"my_ZoneFull\"] = " .. My["my_ZoneFull"])
            tempvar = Left(Right(My["my_ZoneFull"], 3), 1)
            tempvar2 = Left(Right(My["my_ZoneFull"], 4), 1)
            if tempvar == "(" then
                My["my_ZoneSub"] = Right(My["my_ZoneFull"], 3)
                My["my_Zone"] = Left(My["my_ZoneFull"], string.len(My["my_ZoneFull"]) - 4)
                --co(_sender, "co_debugoutput", "\"" .. My["my_Zone"] .. "\"")
                --co(_sender, "co_debugoutput", "\"" .. My["my_ZoneSub"] .. "\"")
            elseif tempvar2 == "(" then
                My["my_ZoneSub"] = Right(My["my_ZoneFull"], 4)
                My["my_Zone"] = Left(My["my_ZoneFull"], string.len(My["my_ZoneFull"]) - 5)
                --co(_sender, "co_debugoutput", "\"" .. My["my_Zone"] .. "\"")
                --co(_sender, "co_debugoutput", "\"" .. My["my_ZoneSub"] .. "\"")
            end
        end
    end



    -- Update Input Data Here
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

    --value_Start = readBytes("[pcsx2-r3878.exe+0040239C]+980")
    --value_Select = readBytes("[pcsx2-r3878.exe+0040239C]+981")
    co(_sender, "co_exit", "")
end
function checkUpdated(_currentvalue, _oldvalue, _UpdateKey)
    local _sender = "checkUpdated"
    --co(_sender, "co_enter", "_currentvalue, _oldvalue, _UpdateKey")
    if My[_oldvalue] == nil and _currentvalue == nil then
        --co(_sender, "co_debugoutput", "Returning False .. Nil is still Nil")
        return false
    elseif My[_oldvalue] == nil and _currentvalue ~= nil then
        --co(_sender, "co_debugoutput", "Returning True .. Nil is no longer Nil")
        My[_oldvalue] = _currentvalue
        WasUpdated[_UpdateKey] = true
        return true
    elseif My[_oldvalue] ~= nil and _currentvalue == nil then
        --co(_sender, "co_debugoutput", "Returning True .. Not Nil is now Nil")
        My[_oldvalue] = _currentvalue
        WasUpdated[_UpdateKey] = true
        return true
    elseif My[_oldvalue] ~= _currentvalue then
        My[_oldvalue] = _currentvalue
        WasUpdated[_UpdateKey] = true
        --co(_sender, "co_debugoutput", "Returning True .. Old and New values no longer match")
        return true
    elseif My[_oldvalue] == _currentvalue then
        --co(_sender, "co_debugoutput", "Returning False .. Old and New values still match")
        return false
    else
        --co(_sender, "co_debugoutput", "Returning False .. YOU HIT THE ELSE STATEMENT AND YOU SHOULDN'T DO THAT.")
        return false
    end
end
function ProcessOutsideCommands()
    _sender = "ProcessOutsideCommands"
    co(_sender, "co_enter", "")
    _filepath = Folder_Temp .. "Outside Command" .. Extension_Outside_Command
    if File_Exists(_filepath) == true then
        co(_sender, "co_debugoutput", "File Exists: " .. _filepath)
        file = io.open(_filepath, "r+");
        _outsideCommand = IO_ReadNextLine(file, "%-%-")
        if _outsideCommand ~= nil then
            co(_sender, "co_debugoutput", "_outsideCommand == " .. _outsideCommand)
            if _outsideCommand == "Spawn_NPC" then
                _npc = IO_ReadNextLine(file, "%-%-")
                Spawn_NPC(_npc)
            elseif _outsideCommand == "Spawn_NPCs_By_Location" then
                _location = IO_ReadNextLine(file, "%-%-")
                Spawn_NPCs_By_Location(_location)
            elseif _outsideCommand == "Spawn Wall Marker" then
                _number = IO_ReadNextLine(file, "%-%-")
                _x = IO_ReadNextLine(file, "%-%-")
                _y = IO_ReadNextLine(file, "%-%-")
                _z = IO_ReadNextLine(file, "%-%-")
                --_facing = IO_ReadNextLine(file, "%-%-")
                NPC_Maker.SpawnWallMarker(_number, _x, _y, _z)--, _facing)
            elseif _outsideCommand == "" then
                co(_sender, "co_debugoutput", "_outsideCommand Is blank")
            end
        end
        file:close();
        os.remove(_filepath)
    else
        --co(_sender, "co_debugoutput", "Notice: File Does Not Exist: " .. _filepath)
        co(_sender, "co_debugoutput", "No Outside Command to Follow.")
    end
    co(_sender, "co_exit", "")
end

-- FAPI Game:Helper Functions
function outputLocation(shouldtoggle)
    _sender = "outputLocation"
    if doOutputLocation == true then
        co(_sender, "co_enter","")
    end

    if shouldtoggle == true then
        co(_sender, "co_debugoutput", "shouldtoggle is True")
        if doOutputLocation == true then
            co(_sender, "co_debugoutput", "doOutputLocation == True --> False")
            doOutputLocation = false
        elseif doOutputLocation == false then
            co(_sender, "co_debugoutput", "doOutputLocation == False --> True")
            doOutputLocation = true
        end
    elseif shouldtoggle == false then
        co(_sender, "co_debugoutput", "shouldtoggle is False")
        -- Should never be here?
    else
        if doOutputLocation == true then
            co(_sender, "co_debugoutput", "My[\"my_X\"] = " .. tostring(My["my_X"]))
            co(_sender, "co_debugoutput", "My[\"my_Y\"] = " .. tostring(My["my_Y"]))
            co(_sender, "co_debugoutput", "My[\"my_Z\"] = " .. tostring(My["my_Z"]))
            co(_sender, "co_debugoutput", "My[\"my_F\"] = " .. tostring(My["my_F"]))
        else

        end
    end
    
    if doOutputLocation == true then
        co(_sender, "co_exit", "")
    end
end

-- FAPI System Functions --
-- FAPI IO Functions
function File_Exists(_file)
   local f=io.open(_file,"r")
   if f~=nil then io.close(f) return true else return false end
end
function IO_ReadNextLine(_ioObject, _commentString)
    _line = _ioObject:read() -- reads line for first processing
    if _line ~= nil then
        if _commentString ~= false then -- if comment removal should happen
            redoRead = true
            while redoRead == true do
                _start, _end = string.find(_line, _commentString) -- checks if comment
                if _start == 1 and _end == 2 then -- if is a comment then ..
                    redoRead = true -- read the next line.
                    _line = _ioObject:read() -- reads next line for processing
                else -- if not a comment ..
                    redoRead = false -- don't read more lines, and return current line.
                end
            end
        end
    end
    if _line == nil then
        return nil
    else
        return _line
    end
end
function Read_FileToStringArray(_filepath, _commentString)
    if File_Exists(_filepath) then
        _file = io.open(_filepath, "r+");
        i = 1
        if _commentString == nil then
            _lins[i] = _file:read()
            while _lines[i] ~= nil do
                i = i + 1
                _lines[i] = _file:read()
            end
        else        
            _lins[i] = IO_ReadNextLine(_file, _commentString)
            while _lines[i] ~= nil do
                i = i + 1
                _lines[i] = IO_ReadNextLine(_file, _commentString)
            end
        end
        _file:close();
        return _lines
    else
        -- File Does Not Exist
        return nil
    end
end
function Write_StringArrayToFile(_filepath, _stringArray, _keepFileOpen)
    if _stringArray ~= nil then
        i = 1
        _file = io.open(_filepath,"w+");
        while _stringArray[i] ~= nil do
            _file:write(_stringArray[i]);
            _file:write("\n");
            i = i + 1
        end
        if _keepFileOpen ~= nil and _keepFileOpen == true then
            return _file
        else
            _file:close();            
        end
    end
end

-- FAPI CE IO Functions --
function Write_StringToBitArrayAddress(_string, _bitArrayAddress, _type)
    _sender = "Write_StringToBitArrayAddress"
    --co(_sender, "co_enter", "_string, _bitArrayAddress, _type == " .. _type)
    local sp
    local i
    local i2
    sp = 1
    i = 1
    StringAsArrayofBits = {};
    if _type == 1 then
        while sp <= string.len(_string) do
            StringAsArrayofBits[i] = string.byte(_string, sp)
             --co(_sender, "co_debugoutput", StringAsArrayofBits[i])
            sp = sp + 1
            i = i + 1
        end
        StringAsArrayofBits[i] = 00
        StringAsArrayofBits[i + 1] = 00
    elseif _type == 2 then
        i2 = i + 1
        while sp <= string.len(String) do
            StringAsArrayofBits[i] = string.byte(_string, sp)
            StringAsArrayofBits[i2] = 00
            --co(_sender, "co_debugoutput", StringAsArrayofBits[i])
            sp = sp + 1
            i = i + 2
            i2 = i + 1
        end
        StringAsArrayofBits[i] = 00
        StringAsArrayofBits[i2] = 00
    else
        
    end
    writeBytes(_bitArrayAddress, StringAsArrayofBits)
end
function Read_BitArrayToString(BitArrayAddress, LengthtoRead, LengthIsManuallyEntered, _type)
    --_sender = "Read_BitArrayToString"
    --co(_sender, "co_enter", "BitArrayAddress, LengthtoRead, LengthIsManuallyEntered")
    local i
    
    if LengthIsManuallyEntered ~= nil and LengthIsManuallyEntered == true then
        --The length is manually entered, so LengthtoRead doesn't need changed
            --co(_sender, "co_debugoutput", "Manual Read: " .. LengthtoRead)
    elseif (LengthIsManuallyEntered ~= nil and LengthIsManuallyEntered == false) or (LengthIsManuallyEntered == nil) then
        --The length is being referred to by a definition, so look up the definition and change 
        --  LengthtoRead to whatever is defined
        LengthtoReadName = LengthtoRead
        if LengthtoRead == "TypeZoneName" then -- This creates a new group type called TypeZoneName
            LengthtoRead = 50 -- This is twice the number of possible characters.
            _type = 1
        elseif LengthtoRead == "TypeCharacterName" then -- When you add your types, copy this line because of the "elseif"
            LengthtoRead = 24 -- So you only need to copy this line and the above line to add your own types
            _type = 2
        end
        --co(_sender, "co_debugoutput", LengthtoReadName .. ": " .. LengthtoRead)
    end
    ValueAsUnicodeBitArray = readBytes(BitArrayAddress, LengthtoRead, true)
    i = 1
    local ZoneAsString
    ZoneAsString = string.char(ValueAsUnicodeBitArray[i])
    if _type == nil then
    
    elseif _type == 1 then
        i = i + 2
        while ValueAsUnicodeBitArray[i] ~= nil and ValueAsUnicodeBitArray[i] ~= 0 do
            --co(_sender, "co_debugoutput", "3: " .. i .. " = " .. ValueAsUnicodeBitArray[i])
            ZoneAsString = ZoneAsString .. string.char(ValueAsUnicodeBitArray[i])
            i = i + 2
        end
    elseif _type == 2 then
        i = i + 1
        while ValueAsUnicodeBitArray[i] ~= nil and ValueAsUnicodeBitArray[i] ~= 0 do
            --co(_sender, "co_debugoutput", "3: " .. i .. " = " .. ValueAsUnicodeBitArray[i])
            ZoneAsString = ZoneAsString .. string.char(ValueAsUnicodeBitArray[i])
            i = i + 1
        end
    end
    return ZoneAsString
end

-- FAPI Core Functions --
function co(_sender, _action, _data) -- console output
    local lineoutput = ""
    local dopad_pre = false
    local dopad_post = false
    local do_output = true
    local dofileoutput = false
    if _action == "set_fileoutput" then
        doCOOutput = false -- do not do console output since this was a command and not output
        output_file_lines[0] = _data
    elseif _action == "do_fileoutput" then
        doCOOutput = false -- do not do console output since this was a command and not output
        dofileoutput = true
    elseif _action == "do_fileoutput" then
        doCOOutput = false -- do not do console output since this was a command and not output
        dofileoutput = true
    end
    if (doCOOutput == true) or (doCOOutput == false and _action == "co_error") then
        if _action == "co_pad" then
            lineoutput = " "
        elseif _action == "co_enter" then
            dopad_pre = true
            lineoutput = _sender .. "(" .. _data .. ") Initiated.."
        elseif _action == "co_exit" then
            dopad_post = true
            lineoutput =_sender .. "(" .. _data .. ") Ending.."
        elseif _action == "co_comment" then
            lineoutput = "--" .. _data
        elseif _action == "co_debugoutput" then
            lineoutput = " :: " .. _data
        elseif _action == "co_lineofcode" then
            lineoutput = " :: " .. _data
        elseif _action == "co_linesofcode_start" then
            dopad_pre = true
            lineoutput = " :: " .. _data .. ": Started" 
        elseif _action == "co_linesofcode_end" then
            dopad_post = true
            lineoutput = " :: " .. _data .. ": Ended"
        elseif _action == "co_error" then
            lineoutput = " :: Error: " .. _data
            local _filepath
            _filepath = Folder_Temp .. "Error Output" .. ".txt"
            if File_Exists(_filepath) then
                -- Read Current Error File
                _lines = Read_FileToStringArray(_filepath)
                _file = Write_StringArrayToFile(_filepath, _lines, true)
                _file:write((output_line_number  + 1) .. "| " .. lineoutput);
                _file:write("\n");
                _file:close();
            else
                
            end
        else
            lineoutput = " : _sender = " .. _sender .. " : _action = " .. _action .. " : " .. _data
        end

        _start, _end = string.find(_sender, "self.")
        if _start == 1 and _end == 5 then
            do_output = false
        end

        if do_output == true then
            if dopad_pre == true then
                output_line_number = output_line_number + 1
                print(output_line_number .. "| ")
                if output_file_lines ~= 0 then
                    output_file_lines[output_line_number] = output_line_number .. "| "
                end
            end
            output_line_number = output_line_number + 1
            print(output_line_number .. "| " .. lineoutput)
            if output_file_lines ~= 0 then
                output_file_lines[output_line_number] = output_line_number .. "| " .. lineoutput
            end
            if dopad_post == true then
                output_line_number = output_line_number + 1
                print(output_line_number .. "| ")
                if output_file_lines ~= 0 then
                    output_file_lines[output_line_number] = output_line_number .. "| "
                end
            end
        end
        if output_file_lines[0] ~= 0 then
            if output_file_lines[0] == output_line_number then
                dofileoutput = true
            end
        end
    elseif doCOOutput == false then
    end
    if dofileoutput == true then
        local _filepath
        _filepath = Folder_Temp .. "Console Output As File" .. ".txt"
        local i
        i = 1
        --if File_Exists(_filepath) == true then
            -- Print That You Are Outputting A File And Shutting Off Output
            -- just print it
            print((output_line_number + 1) .. "| " .. "Saving Output to a file and turning off further output.")
            print((output_line_number + 2) .. "| " .. "File is: " .. _filepath)
            -- Print It With co, using "co" as sender
            --co("co", "co_debugoutput", "Saving Output to a file and turning off further output.")
            --co("co", "co_debugoutput", "File is: " .. _filepath)
            -- Print It With co, using "_sender" as sender
            --co(_sender, "co_debugoutput", "Saving Output to a file and turning off further output.")
            --co(_sender, "co_debugoutput", "File is: " .. _filepath)
            doCOOutput = false
            file = io.open(_filepath, "w+");
            while output_file_lines[i] ~= nil do
                file:write(output_file_lines[i]);
                file:write("\n");
                i = i + 1
            end
            file:close();
        --else
        --    co(_sender, "co_debugoutput", "Error: File Does Not Exist: " .. _filepath)
        --end
    end
end

-- FAPI Helper Functions
function Right(_string, _spaces) -- returns right half of a string, starting at _spaces
    placetostart = string.len(_string) - _spaces + 1
    output = string.sub(_string, placetostart)
    return output
end
function Left(_string, _spaces) -- returns left half of a string, ending at _spaces
    output = string.sub(_string, 1, _spaces)
    return output
end

