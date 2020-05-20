-- Standard Folder & File Definitions
Folder_Main = "../EQOA/"
-- /EQOA/
Folder_Temp = Folder_Main .. "Temp/"
Folder_Custom_Data = Folder_Main .. "Custom Data/"
Folder_Net_Streams = Folder_Main .. "net streams/"
Folder_My_Data = Folder_Main .. "My Data/" -- things like option saves
Folder_LUAs = Folder_Main .. "LUAs/"

-- /EQOA/Temp/ : Folder_Temp
Folder_NPC_Maker_Temp = Folder_Temp .. "NPC Maker/"
Folder_NPC_Area_Maker_Temp = Folder_Temp .. "NPC Areas/"

-- /EQOA/Game Data/
Folder_NPCs = Folder_Main .. "Game Data/NPCs/"
Folder_Areas = Folder_Main .. "Game Data/Areas/"
Folder_Area_Zones = Folder_Areas .. "Zones/"
Folder_Ghosts = Folder_Main .. "Game Data/Ghosts/"

-- /EQOA/net streams/ : Folder_Net_Streams
Folder_Reads = Folder_Net_Streams .. "reads/"
Folder_Writes = Folder_Net_Streams .. "writes/"

-- /EQOA/player data/
Folder_DBData = Folder_Main .. "player data/Dualbox Data/"

-- /EQOA/Custom Data/ : Folder_Custom_Data
Folder_NPC_Maker_Output = Folder_Custom_Data .. "NPC Maker/" -- note is not "/NPCs/"
Folder_NPC_Area_Maker_Output = Folder_NPC_Maker_Output .. "NPC Areas/" -- is a part of NPC Maker
Folder_NPC_Area_Maker_Zones = Folder_NPC_Area_Maker_Output .. "Zones/"

-- /EQOA/LUAs/
Folder_LUA_Modes = Folder_LUAs .. "Modes/"
Folder_LUA_Modules = Folder_LUAs .. "Modules/"

Extension_NPCFiles = ".txt"
Extension_GhostFiles = ".txt"
Extension_ReadWrites = ".txt"
Extension_DBData = ".txt"
Extension_NPC_Maker = ".txt"
Extension_NPC_Area_Maker = ".txt"
Extension_Options = ".txt"
Extension_Outside_Command = ".txt"