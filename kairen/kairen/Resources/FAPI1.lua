--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: FAPI
Code-Type: LUA Code
Code-Version: 2.0
Code-Description: The FAPI Code Loader. This FAPI Module loads the FAPI API code into memory.
Code-Author: Robert Randazzio
]]--

if _G.FAPI.DirectoryLocation ~= nil then
    local _directoryLocation = FAPI.DirectoryLocation
    FAPI = nil
    FAPI = {
        DirectoryLocation = _directoryLocation;
        CommentIndicatorString = "--";
    };
    
    FAPI = {
        RunOptions = {
            New = dofile(FAPI.DirectoryLocation .. "Functions/RunOptions.lua");
        };
        IO = {
            File_Exists = dofile(FAPI.DirectoryLocation .. "IO/File_Exists.lua");
            ReadNextLine = dofile(FAPI.DirectoryLocation .. "IO/ReadNextLine.lua");
            Read_FileToStringArray = dofile(FAPI.DirectoryLocation .. "IO/Read_FileToStringArray.lua");
            Write_StringArrayToFile = dofile(FAPI.DirectoryLocation .. "IO/Write_StringArrayToFile.lua");
        };
        CE = {
            Address = {
                --New = dofile(FAPI.DirectoryLocation .. "CE/Address.New.lua");
            };
            IO = {
                Read_BitArrayToString = dofile(FAPI.DirectoryLocation .. "CE/IO/Read_BitArrayToString.lua");
                Write_StringToBitArrayAddress = dofile(FAPI.DirectoryLocation .. "CE/IO/Write_StringToBitArrayAddress.lua");
            };
        };
        Kanizah = {
            CreateNPCFile = dofile(FAPI.DirectoryLocation .. "Kanizah/CreateNPCFile.lua");
            --CreateAreaFile = dofile(FAPI.DirectoryLocation .. "Kanizah/CreateAreaFile.lua");
            ProcessOutsideCommands = dofile(FAPI.DirectoryLocation .. "Kanizah/ProcessOutsideCommands.lua");
            --ProcessOutsideCommands = {
                --Do = dofile(FAPI.DirectoryLocation .. "Kanizah/ProcessOutsideCommands.lua");
                --Add = (function (_commandToAdd) FAPI.Kanizah.ProcessOutsideCommands.i = FAPI.Kanizah.ProcessOutsideCommands.i + 1; end);
                --NextCommand = (function () FAPI.Kanizah.ProcessOutsideCommands.NextCommand.Do(); end);
                --i = (function () FAPI.Kanizah.ProcessOutsideCommands.NextCommand.Do(); end);
            --};
        };
        Options = {
        };
        EQOA = {
            Player = {
                Location = {
                    X = (function () return readInteger("[pcsx2-r3878.exe+0040239C]+760"); end);
                    Y = (function () return readInteger("[pcsx2-r3878.exe+0040239C]+768"); end);
                    Z = (function () return readInteger("[pcsx2-r3878.exe+0040239C]+764"); end);
                    F = (function () return readInteger("[pcsx2-r3878.exe+0040239C]+730"); end);
                    ZoneFull = (function () return FAPI.CE.IO.Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", "TypeZoneName"); end);
                    Zone = (function () return FAPI.CE.IO.Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", "TypeZoneName"); end);
                    ZoneSub = (function () return FAPI.CE.IO.Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", "TypeZoneName"); end);
                };
            Group = {
                isInGroup = false;
            };
            Stats = {
                Stamina = "55";
                LR = "";
                AC = "";
            };
            Armor = {
            };
            Inventory = {
            };
            };
        };
    };
    
    print("0| FAPI Loaded.")
elseif _G.FAPI == nil then
    print("0| FAPI Not Loaded - FAPI.DirectoryLocation == nil")
else
    print("0| FAPI Not Loaded - Unknown Error")
end
