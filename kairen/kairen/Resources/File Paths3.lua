--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: File Paths
Code-Type: LUA Code
Code-Version: 1.2
Code-Description: The main file containing all of the FAPI File Path References for easy changing.
Code-Author: Robert Randazzio
]]--
--TrainerOrigin
print("Root Folder *should* be: \"" .. string.gsub(TrainerOrigin , "Cheat Engine\\","") .. "\"")
--print("Root Folder *should* be: \"" .. os.getenv("appdata") .. "\\Rundatshityo\\\"")
print("Use the \"/\" slash with the LUA. Windows uses the \"\\\" slash, but the LUA does not.")
_G.RF = {
    self = "";
    NewFolder = function(_folderName, _parentFolderClass)
            _self = {}
            _self.self = _parentFolderClass .. _folderName .. "/"
            print('Path Setup: "' .. _self.self .. '"')
            return _self;
        end;
    };
--local thestring = debug.getinfo(1).source:match("@(.*)$")
--local ts = string.gsub(string.gsub(thestring , "/LUAs/File Paths.lua",""), "../", "")
--local tslen = string.len(thestring ) + 6
--local ts3 = string.sub(ts, 1, tslen)
--RF.EQOA = RF.NewFolder(ts3, RF.self)
--RF.EQOA = RF.NewFolder("EQOA_Syba", RF.self)
RF.EQOA = RF.NewFolder("..", RF.self)
RF.EQOA.LUAs = RF.NewFolder("LUAs", RF.EQOA.self)
RF.EQOA.LUAs.Modes = RF.NewFolder("Modes", RF.EQOA.LUAs.self)
RF.EQOA.LUAs.Modules = RF.NewFolder("Modules", RF.EQOA.LUAs.self)
RF.EQOA.LUAs.Modules.FAPI = RF.NewFolder("FAPI", RF.EQOA.LUAs.Modules.self)
RF.EQOA.Game_Data = RF.NewFolder("Game Data", RF.EQOA.self)
RF.EQOA.Game_Data.Ghosts = RF.NewFolder("Ghosts", RF.EQOA.Game_Data.self)
RF.EQOA.Game_Data.Ghosts.Corsten = RF.NewFolder("Corsten", RF.EQOA.Game_Data.Ghosts.self)
RF.EQOA.Game_Data.Ghosts.Lostologist = RF.NewFolder("Lostologist", RF.EQOA.Game_Data.Ghosts.self)
RF.EQOA.Game_Data.Spawn_Points = RF.NewFolder("Spawn Points", RF.EQOA.Game_Data.self)
RF.EQOA.Game_Data.Spawn_Camps = RF.NewFolder("Spawn Camps", RF.EQOA.Game_Data.self)
RF.EQOA.Game_Data.Spawn_Nests = RF.NewFolder("Spawn Nests", RF.EQOA.Game_Data.self)
RF.EQOA.Game_Data.Address_Files = RF.NewFolder("Address Files", RF.EQOA.Game_Data.self)
RF.EQOA.Custom_Data = RF.NewFolder("Custom Data", RF.EQOA.self)
RF.EQOA.Custom_Data.NPC_Maker = RF.NewFolder("NPC Maker", RF.EQOA.Custom_Data.self)
RF.EQOA.Custom_Data.Spawn_Points = RF.NewFolder("Spawn Points", RF.EQOA.Custom_Data.self)
RF.EQOA.Custom_Data.Spawn_Camps = RF.NewFolder("Spawn Camps", RF.EQOA.Custom_Data.self)
RF.EQOA.Custom_Data.Spawn_Nests = RF.NewFolder("Spawn Nests", RF.EQOA.Custom_Data.self)
RF.EQOA.Temp = RF.NewFolder("Temp", RF.EQOA.self)
RF.EQOA.Net_Streams = RF.NewFolder("Net Streams", RF.EQOA.self)
RF.EQOA.Net_Streams.i = RF.NewFolder("i", RF.EQOA.Net_Streams.self)
RF.EQOA.Net_Streams.o = RF.NewFolder("o", RF.EQOA.Net_Streams.self)

Extension_NPCFiles = ".txt"
Extension_GhostFiles = ".txt"
Extension_ReadWrites = ".txt"
Extension_DBData = ".txt"
Extension_NPC_Maker = ".txt"
Extension_NPC_Area_Maker = ".txt"
Extension_Options = ".txt"
Extension_Outside_Command = ".txt"
Extension_SpawnPoints = ".txt"
Extension_SpawnCamps = ".txt"
Extension_SpawnNests = ".txt"
Extension_AddressFiles = ".txt"
