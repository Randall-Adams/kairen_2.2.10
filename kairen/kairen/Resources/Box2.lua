--Code by Robert Randazzio
Folder_Main = "../EQOA/"
Folder_NPCs = Folder_Main .. "game data/NPCs/"
Folder_Ghosts = Folder_Main .. "game data/Ghosts/"
Folder_Net_Streams = Folder_Main .. "net streams/"
Folder_Reads = Folder_Net_Streams .. "reads/"
Folder_Reads_Player_1 = Folder_Reads .. "player 1/"
Folder_Reads_Player_1_Location = Folder_Reads_Player_1 .. "location/"
Folder_Writes = Folder_Net_Streams .. "writes/"
Folder_Writes_Location = Folder_Writes .. "location/"
Extension_NPCFiles = ".txt"
Extension_GhostFiles = ".txt"
Extension_ReadWrites = ".txt"
Folder_DBData = Folder_Main .. "player data/Dualbox Data/"
Extension_DBData = ".txt"
Thisbox = "Box2"
Ghost = {};
function Ghost.new(_nameSafe)
local self = {};
fileOfGhost = Folder_Ghosts .. _nameSafe .. Extension_GhostFiles
if File_Exists(fileOfGhost) == true then
file = io.open(fileOfGhost,"r+");
local fileversion = IO_ReadNextLine(file, "%-%-")
self.ghostID = IO_ReadNextLine(file, "%-%-")
self.nameOverhead = IO_ReadNextLine(file, "%-%-") 
self.nameOverheadColor = IO_ReadNextLine(file, "%-%-")
self.nameTarget = IO_ReadNextLine(file, "%-%-")
self.X = IO_ReadNextLine(file, "%-%-")
self.Y = IO_ReadNextLine(file, "%-%-")
self.Z = IO_ReadNextLine(file, "%-%-")
self.F = IO_ReadNextLine(file, "%-%-")
self.level = IO_ReadNextLine(file, "%-%-")
self.hp = IO_ReadNextLine(file, "%-%-")
file:close();
self.Type = "baseType Ghost"
self.isFreeOrReplaceable = false
end
return self;
end
NPC = {};
function NPC.new(_nameSafe, _ghost)
local self = {};
local _myGhost = _ghost
self.myGhost = _ghost
self.nameSafe = _nameSafe
self.supType = "supType NPC"
self.Type = "baseType NPC"
function self.nameGet()
local value = readString(_ghost.nameTarget)
return value
end
function self.nameSet(_value)
writeString(_ghost.nameOverhead, _value)
writeString(_ghost.nameTarget, _value)
end
function self.ghostIDGet()
local value = readString(_ghost.ghostID)
return value
end
function self.ghostIDSet(_value)
writeString(_ghost.ghostID, _value)
end
function self.nameOverheadColorGet()
local value = readInteger(_ghost.nameOverheadColor)
return value
end
function self.nameOverheadColorSet(_value)
writeBytes(_ghost.nameOverheadColor, _value)
end
function self.XGet()
local value = readInteger(_ghost.X)
return value
end
function self.XSet(_value)
writeInteger(_ghost.X, _value)
end
function self.YGet()
local value = readInteger(_ghost.Y)
return value
end
function self.YSet(_value)
writeInteger(_ghost.Y, _value)
end
function self.ZGet()
local value = readInteger(_ghost.Z)
return value
end
function self.ZSet(_value)
writeInteger(_ghost.Z, _value)
end
function self.FGet()
local value = readInteger(_ghost.F)
return value
end
function self.FSet(_value)
writeInteger(_ghost.F, _value)
end
function self.levelGet()
local value = readByte(_ghost.level)
return value
end
function self.levelSet(_value)
writeBytes(_ghost.level, _value)
end
function self.hpGet()
local value = readByte(_ghost.hp)
return value
end
function self.hpSet(_value)
writeBytes(_ghost.hp, _value)
end
return self;
end
Ghosts = { }
NPCs = { }
function Spawn_NPC(_NPCFileName, _ghost)
if _ghost.isFreeOrReplaceable == true then
fileOfNPC = Folder_NPCs .. _NPCFileName .. Extension_NPCFiles
if File_Exists(fileOfNPC) == true then
NPC1 = NPC.new(_NPCFileName, _ghost);
local i = 1
NPCs[i] = NPC1
file = io.open(fileOfNPC,"r+");
NPCs[i].nameSafe = IO_ReadNextLine(file, "%-%-")
NPCs[i].nameSet(IO_ReadNextLine(file, "%-%-"))
NPCs[i].XSet(IO_ReadNextLine(file, "%-%-"))
NPCs[i].YSet(IO_ReadNextLine(file, "%-%-"))
NPCs[i].ZSet(IO_ReadNextLine(file, "%-%-"))
NPCs[i].FSet(IO_ReadNextLine(file, "%-%-"))
IO_ReadNextLine(file, "%-%-")
IO_ReadNextLine(file, "%-%-")
IO_ReadNextLine(file, "%-%-")
NPCs[i].levelSet(IO_ReadNextLine(file, "%-%-"))
NPCs[i].hpSet(IO_ReadNextLine(file, "%-%-"))
file:close();
end
end
end
function OutputDBData()
local file_name = ""
if Thisbox == "Box1" then
file_name = "Box1"
else
file_name = "Box2"
end
file = io.open(Folder_DBData .. file_name .. Extension_DBData,"w+");
x_output = readInteger("[pcsx2-r3878.exe+0040239C]+760") -- X
y_output = readInteger("[pcsx2-r3878.exe+0040239C]+768") -- Y
z_output = readInteger("[pcsx2-r3878.exe+0040239C]+764") -- Z
f_output = readInteger("[pcsx2-r3878.exe+0040239C]+730") -- F
file:write(x_output);
file:write("\n");
file:write(y_output);
file:write("\n");
file:write(z_output);
file:write("\n");
file:write(f_output);
file:write("\n");
file:close();
end
function InputDBData()
local file_name = ""
if Thisbox == "Box1" then
file_name = "Box2"
else
file_name = "Box1"
end
file = io.open(Folder_DBData .. file_name .. Extension_DBData,"r+");
x_input = file:read()
y_input = file:read()
z_input = file:read()
f_input = file:read()
file:close();
NPCs[1].XSet(x_input)
NPCs[1].YSet(y_input)
NPCs[1].ZSet(z_input)
NPCs[1].FSet(f_input)
end
function File_Exists(_file)
 local f=io.open(_file,"r")
 if f~=nil then io.close(f) return true else return false end
end
function IO_ReadNextLine(_ioObject, _commentString)
_line = _ioObject:read()
if _line ~= nil then
if _commentString ~= false then
redoRead = true
while redoRead == true do
_start, _end = string.find(_line, _commentString)
if _start == 1 and _end == 2 then
redoRead = true
_line = _ioObject:read()
else
redoRead = false
end
end
end
end
return _line
end
function setupGhosts()
Ghost1 = Ghost.new("Coachman_Ronks");
Ghosts[1] = Ghost1
Ghosts[1].isFreeOrReplaceable = true
end
function setupScript()
setupGhosts()
Spawn_NPC("Coachman_Ronks", Ghosts[1])
end
mytimer = nil
if mytimer == nil then
elapsed_time_DB = 1
elapsed_time = 1
interval_timer = 100
mytimer=createTimer(nil)
mytimer.Interval=interval_timer
local value_1 = 0
setupScript()
mytimer.OnTimer=function(t)
if elapsed_time_DB >= 3001 then
elapsed_time_DB = 0
OutputDBData()
InputDBData()
end
elapsed_time_DB = elapsed_time_DB + interval_timer
elapsed_time = elapsed_time + interval_timer
end
end
mytimer.Enabled=true