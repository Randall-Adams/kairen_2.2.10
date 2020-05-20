--[[ Meta-Data
Version: 1.0
]]--
NPC_Maker = {

}
function NPC_Maker.SpawnLastNPC()
    _sender = "NPC_Maker.SpawnLastNPC"
    co(_sender, "co_enter", "")
    _filepath = Folder_NPC_Maker_Temp .. "last file made" .. Extension_NPCFiles
    if File_Exists(_filepath) then
        file = io.open(_filepath, "r+");
        _npcname = file:read()
        _npcpath = file:read()
        file:close();
        npcpath = _npcpath
        co(_sender, "co_lineofcode", "Spawn_NPC(\"" .. npcpath .. "\")--, Ghosts[1])") 
        Spawn_NPC(npcpath)--, Ghosts[1])
    else
        co(_sender, "co_debugoutput", "Notice: File Does Not Exist: " .. _filepath)
    end
    co(_sender, "co_exit", "")
end
function NPC_Maker.SpawnWallMarker(_number, _x, _y, _z)--, _facing)
    _npc = Spawn_NPC("Wall Marker " .. _number, "Spawn Wall Marker" .. _number)
    if _npc ~= nil then
        _npc.XSet = _x
        _npc.YSet = _y
        _npc.ZSet = _z
        --_npc.FSet = _facing
    end
end

--doSpawns()
-- Input Processing Goes Here
if value_L2 == 1 and value_Circle == 1 then -- Spawns Last NPC Made
    co(_sender, "co_enter", "-- L2 + circle --")
    NPC_Maker.SpawnLastNPC()
    co(_sender, "co_comment", "If NPC_Maker Spawned the last NPC, I can start the transition..")
    co(_sender, "co_exit", "-- circle --")
elseif value_L2 == 1 and value_Square == 1 then -- Outputs Your Data for New NPC
    co(_sender, "co_enter", "-- L2 + square --")
    co(_sender, "co_comment", "-- Outputting Data for New NPC --")
    file_name = "New_NPC"
    _filepath = Folder_NPC_Maker_Temp .. file_name .. Extension_NPC_Maker
    --if File_Exists(_filepath) == true then
        file = io.open(_filepath,"w+");

        file:write(My["my_X"]);
        file:write("\n");
        file:write(My["my_Y"]);
        file:write("\n");
        file:write(My["my_Z"]);
        file:write("\n");
        file:write(My["my_F"]);
        file:write("\n");
        file:close();
    --else
    --    co(_sender, "co_debugoutput", "Error: File Does Not Exist: " .. _filepath)
    --end

    co(_sender, "co_exit", "-- square --")
elseif value_L2 == 1 and value_X == 1 then -- Add Targetted NPC To Current Area
    co(_sender, "co_enter", "-- L2 + x --")
    if My["my_Area"] == nil then
        -- You are not in an area.
    else
        if targetisannpcfromfile then
            co(_sender, "co_debugoutput", "Adding NPC to Area: " .. My["my_Area"])
            _filepath = Areas[0].filepath
            _lines = Read_FileToStringArray(_filepath)
            _file = Write_StringArrayToFile(_filepath, _lines, true)
            --_npc = FindNPC("Name", "")
            _file:write(_npc.nameGet());
            _file:write("\n");
            _file:close();
        else
        end
    end
elseif value_L2 == 1 and value_Triangle == 1 then -- Output NPC Spawn Area Wall
    co(_sender, "co_enter", "-- L2 + triangle --")
    co(_sender, "co_comment", "-- Outputting Data For Spawn Area Wall --")
    file_name = "New_Wall"
    _filepath = Folder_NPC_Maker_Temp .. file_name .. Extension_NPC_Maker
    --if File_Exists(_filepath) == true then
        file = io.open(_filepath,"w+");

        file:write(My["my_X"]);
        file:write("\n");
        file:write(My["my_Y"]);
        file:write("\n");
        file:write(My["my_Z"]);
        file:write("\n");
        file:write(My["my_Zone"]);
        file:write("\n");
        file:write(My["my_ZoneSub"]);
        file:write("\n");
        file:close();
        NPC_Maker.SpawnWallMarker("0", My["my_X"], My["my_Y"], My["my_Z"], My["my_F"])
    --else
    --    co(_sender, "co_debugoutput", "Error: File Does Not Exist: " .. _filepath)
    --end
elseif value_R2 == 1 and value_Triangle == 1 then -- Toggle outputLocation
    co(_sender, "co_enter", "-- R2 + triangle --")
    co(_sender, "co_comment", "-- Toggling Output Location --")
    outputLocation(true)
end

