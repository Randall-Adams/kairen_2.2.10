--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: FAPI
Code-Type: LUA Code
Code-Version: 2.3
Code-Description: The FAPI Code Loader. This FAPI Module loads the FAPI API code into memory.
Code-Author: Robert Randazzio
]]--

if _G.FAPI.DirectoryLocation ~= nil then
    print("Loading FAPI.")
    local _directoryLocation = FAPI.DirectoryLocation
    FAPI = nil
    FAPI = {
        DirectoryLocation = _directoryLocation;
        CommentIndicatorString = "%-%-";
    };

    FAPI = {
        DirectoryLocation = _directoryLocation;
        CommentIndicatorString = FAPI.CommentIndicatorString;
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
                New = dofile(FAPI.DirectoryLocation .. "CE/Address.lua");
            };
            IO = {
                Read_BitArrayToString = dofile(FAPI.DirectoryLocation .. "CE/IO/Read_BitArrayToString.lua");
                Write_StringToBitArrayAddress = dofile(FAPI.DirectoryLocation .. "CE/IO/Write_StringToBitArrayAddress.lua");
            };
        };
        --PCSX2 = {
            --SaveStateSlot = FAPI.CE.Address.New("[pcsx2-r3878.exe+19CF0F0]", "Byte", "SaveStateSlot");
        --};
        Kanizah = {
            CreateNPCFile = dofile(FAPI.DirectoryLocation .. "Kanizah/CreateNPCFile.lua");
            --CreateAreaFile = dofile(FAPI.DirectoryLocation .. "Kanizah/CreateAreaFile.lua");
            ProcessOutsideCommands = dofile(FAPI.DirectoryLocation .. "Kanizah/ProcessOutsideCommands.lua");
            OutputPlayerData = dofile(FAPI.DirectoryLocation .. "Kanizah/OutputPlayerData.lua");
            Alert_OutputPlayerData = dofile(FAPI.DirectoryLocation .. "Kanizah/Alert_OutputPlayerData.lua");
            Alert_OutputPlayerData_ConsoleOutput = dofile(FAPI.DirectoryLocation .. "Kanizah/Alert_OutputPlayerData_ConsoleOutput.lua");
            Alert_ProcessOutsideCommands = dofile(FAPI.DirectoryLocation .. "Kanizah/Alert_ProcessOutsideCommands.lua");
        };
        Options = {
        };
        HelperFunctions = {
            Right = dofile(FAPI.DirectoryLocation .. "Helper Functions/Right.lua");
            Left = dofile(FAPI.DirectoryLocation .. "Helper Functions/Left.lua");
            Convert_BooleanToString = dofile(FAPI.DirectoryLocation .. "Helper Functions/Convert_BooleanToString.lua");
        };
    };
    EQOA = {
        StartMenu = {
            Class = FAPI.CE.Address.New("[pcsx2-r3878.exe+004023B0]+D48", "Byte", "StartMenuClass");
            Race = FAPI.CE.Address.New("[pcsx2-r3878.exe+004023B0]+D4C", "Byte", "StartMenuRace");
            Zone = FAPI.CE.Address.New("[pcsx2-r3878.exe+00400B28]+D30", "String", "StartMenuZone", 50, "TypeZoneName");
        };
        SquareMenu = {
            Race = FAPI.CE.Address.New("[pcsx2-r3878.exe+003FDCA0]+B08", "Byte", "SquareMenuRace");
            Class = FAPI.CE.Address.New("[pcsx2-r3878.exe+003FDCA0]+B09", "Byte", "SquareMenuClass");
        };
        Player = {
            Profile = {
                Name = {};
                Class = {};
                Gender = {};
                Guild = {};
                Race = {};
            };
            Location = {
                X = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+760", "Float", "MyX");
                Y = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+768", "Float", "MyY");
                Z = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+764", "Float", "MyZ");
                F = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+730", "Float", "MyF");
                Zone = (function()
                    local _rowcolumn
                    _columnrow = EQOA.HelperFunctions.GetZoneRowColumn_ByCoords(EQOA.Player.Location.X.Value(), EQOA.Player.Location.Y.Value())
                    return EQOA.HelperFunctions.GetZoneName_ByColumnRow(_columnrow[1], _columnrow[2]);
                end
                );
                StartMenuZoneFull = FAPI.CE.Address.New("[pcsx2-r3878.exe+00400B28]+D30", "String", "MyZone", 50, "TypeZoneName");
                --StartMenuZoneFull = (function () return FAPI.CE.IO.Read_BitArrayToString("[pcsx2-r3878.exe+00400B28]+D30", "TypeZoneName"); end);
                --StartMenuZoneFull = _G.FAPI.CE.Address.New("[pcsx2-r3878.exe+00400B28]+D30", "String", "MyZone", 50, "TypeZoneName");
                StartMenuZoneName = (function ()
                    local tempvar = FAPI.HelperFunctions.Left(FAPI.HelperFunctions.Right(EQOA.Player.Location.StartMenuZoneFull.Value(), 3), 1)
                    local tempvar2 = FAPI.HelperFunctions.Left(FAPI.HelperFunctions.Right(EQOA.Player.Location.StartMenuZoneFull.Value(), 4), 1)
                    if tempvar == "(" then
                        return FAPI.HelperFunctions.Left(EQOA.Player.Location.StartMenuZoneFull.Value(), string.len(EQOA.Player.Location.StartMenuZoneFull.Value()) - 4);
                    elseif tempvar2 == "(" then
                        return FAPI.HelperFunctions.Left(EQOA.Player.Location.StartMenuZoneFull.Value(), string.len(EQOA.Player.Location.StartMenuZoneFull.Value()) - 5);
                    end
                end);
                StartMenuZoneSub = (function () 
                    local tempvar = FAPI.HelperFunctions.Left(FAPI.HelperFunctions.Right(EQOA.Player.Location.StartMenuZoneFull.Value(), 3), 1)
                    local tempvar2 = FAPI.HelperFunctions.Left(FAPI.HelperFunctions.Right(EQOA.Player.Location.StartMenuZoneFull.Value(), 4), 1)
                    if tempvar == "(" then
                        return FAPI.HelperFunctions.Right(EQOA.Player.Location.StartMenuZoneFull.Value(), 3);
                    elseif tempvar2 == "(" then
                        return FAPI.HelperFunctions.Right(EQOA.Player.Location.StartMenuZoneFull.Value(), 4);
                    end
                end);
            };
            Group = {
                isInGroup = false;
            };
            Stats = {
                Strength = "";
                Stamina = "55";
                Agility = "";
                Dexterity = "";
                Wisdom = "";
                Intelligence = "";
                Charisma = "";
                FR = "";
                CR = "";
                LR = "";
                AR = "";
                PR = "";
                DR = "";
                AC = "";
                HP = "";
                MP = "";
            };
            BackendStats = {
                XPBars = FAPI.CE.Address.New("[pcsx2-r3878.exe+004023B0]+D54", "Bytes", "XPBars", 4);
                BreathBar = FAPI.CE.Address.New("[pcsx2-r3878.exe+004023B0]+D5C", "Bytes", "BreathBar", 1);
            };
            Armor = {
            };
            Inventory = {
            };
        };
        HelperFunctions = {
            GetZoneRowColumn_ByCoords = (function(_eqoaXloc, _eqoaZloc)
                if _eqoaZloc == nil then
                    if type(_eqoaXloc) == "table" then
                        _eqoaZloc = _eqoaXloc[2]
                        _eqoaXloc = _eqoaXloc[1]
                    end
                end
                local zonelength
                zonelength = 2000
                local columnposition
                columnposition = 0
                local rowposition
                rowposition = 0
                local column
                column = -1
                local row
                row = -1
                local i
                i = 1
                while column == -1 and i < 30 do
                    if _eqoaXloc >= columnposition and _eqoaXloc < (columnposition + zonelength) then
                        column = i
                    end
                    columnposition = columnposition + zonelength
                    i = i + 1
                end
                i = 1
                while row == -1 and i < 30 do
                    if _eqoaZloc >= rowposition and _eqoaZloc < (rowposition + zonelength) then
                        row = i
                    end
                    rowposition = rowposition + zonelength
                    i = i + 1
                end
                local _columnrow = {}
                _columnrow[1] = column
                _columnrow[2] = row
                return _columnrow;
            end
            );
            GetZoneName_ByColumnRow = (function(column, row, continent)
                if continent == nil then
                    continent = "Tunaria"
                end
                if continent == "Tunaria" then
                    if column == 1 and row == 1 then
                        return "Empty"
                        
                    elseif column == 2 and row == 2 then
                        return "Empty"
                        
                    elseif column == 3 and row == 3 then
                        return "Permafrost"
                    elseif column == 3 and row == 4 then
                        return "Zentar's Keep"
                    elseif column == 3 and row == 5 then
                        return "Bogman Village"
                    elseif column == 3 and row == 6 then
                        return "Mariel Village"
                    elseif column == 3 and row == 7 then
                        return "Wyndhaven"
                    elseif column == 3 and row == 8 then
                        return "Whale Hill"
                    elseif column == 3 and row == 9 then
                        return "Qeynos"
                    elseif column == 3 and row == 10 then
                        return "Qeynos Prison"
                    elseif column == 3 and row == 11 then
                        return "Highbourne"
                    elseif column == 3 and row == 12 then
                        return "Ocean Continuation Piece"
                    elseif column == 3 and row == 12 then
                        return "Empty"
                    elseif column == 3 and row == 13 then
                        return "Empty"
                    elseif column == 3 and row == 14 then
                        return "Empty"
                    elseif column == 3 and row == 15 then
                        return "Empty"
                    elseif column == 3 and row == 16 then
                        return "Empty"
                    elseif column == 3 and row == 17 then
                        return "Empty"
                        
                    elseif column == 4 and row == 3 then
                        return "Snowblind Plains"
                    elseif column == 4 and row == 4 then
                        return "Unkempt North"
                    elseif column == 4 and row == 5 then
                        return "Unkempt Glade"
                    elseif column == 4 and row == 6 then
                        return "Twisted Tower"
                    elseif column == 4 and row == 7 then
                        return "Jethro's Cast"
                    elseif column == 4 and row == 8 then
                        return "Crethley Manor"
                    elseif column == 4 and row == 9 then
                        return "Hagley"
                    elseif column == 4 and row == 10 then
                        return "Druid's Watch"
                    elseif column == 4 and row == 11 then
                        return "Stoneclaw"
                    elseif column == 4 and row == 12 then
                        return "Empty"
                    elseif column == 4 and row == 13 then
                        return "Empty"
                    elseif column == 4 and row == 14 then
                        return "Empty"
                    elseif column == 4 and row == 15 then
                        return "Empty"
                    elseif column == 4 and row == 16 then
                        return "Empty"
                    elseif column == 4 and row == 17 then
                        return "Empty"
                        
                    elseif column == 5 and row == 3 then
                        return "Anu Village"
                    elseif column == 5 and row == 4 then
                        return "North Wilderlands"
                    elseif column == 5 and row == 5 then
                        return "Salisearaneen"
                    elseif column == 5 and row == 6 then
                        return "Murnf"
                    elseif column == 5 and row == 7 then
                        return "Surefall Glade"
                    elseif column == 5 and row == 8 then
                        return "Fog Marsh"
                    elseif column == 5 and row == 9 then
                        return "Bear Cave"
                    elseif column == 5 and row == 10 then
                        return "Spider Mine"
                    elseif column == 5 and row == 11 then
                        return "Aviak Village"
                    elseif column == 5 and row == 12 then
                        return "Empty"
                    elseif column == 5 and row == 13 then
                        return "Empty"
                    elseif column == 5 and row == 14 then
                        return "Empty"
                    elseif column == 5 and row == 15 then
                        return "Empty"
                    elseif column == 5 and row == 16 then
                        return "Empty"
                    elseif column == 5 and row == 17 then
                        return "Empty"
                        
                    elseif column == 6 and row == 3 then
                        return "Frosteye Valley"
                    elseif column == 6 and row == 4 then
                        return "Guardian Forest"
                    elseif column == 6 and row == 5 then
                        return "Gramash Ruins"
                    elseif column == 6 and row == 6 then
                        return "Spirit Talker's Wood"
                    elseif column == 6 and row == 7 then
                        return "Wymondham"
                    elseif column == 6 and row == 8 then
                        return "Al Karad Ruins"
                    elseif column == 6 and row == 9 then
                        return "Blakedown"
                    elseif column == 6 and row == 10 then
                        return "Mayfly Glade"
                    elseif column == 6 and row == 11 then
                        return "Urglunt's Wall"
                    elseif column == 6 and row == 12 then
                        return "Empty"
                    elseif column == 6 and row == 13 then
                        return "Empty"
                    elseif column == 6 and row == 14 then
                        return "Empty"
                    elseif column == 6 and row == 15 then
                        return "Empty"
                    elseif column == 6 and row == 16 then
                        return "Empty"
                    elseif column == 6 and row == 17 then
                        return "Empty"
                        
                    elseif column == 7 and row == 3 then
                        return "Halas"
                    elseif column == 7 and row == 4 then
                        return "Freezeblood Valley"
                    elseif column == 7 and row == 5 then
                        return "Diren Village"
                    elseif column == 7 and row == 6 then
                        return "Mt. Hatespike"
                    elseif column == 7 and row == 7 then
                        return "Blackburrow"
                    elseif column == 7 and row == 8 then
                        return "Jared's Blight"
                    elseif column == 7 and row == 9 then
                        return "Alseop's Wall"
                    elseif column == 7 and row == 10 then
                        return "Forkwatch"
                    elseif column == 7 and row == 11 then
                        return "South Crossroads"
                    elseif column == 7 and row == 12 then
                        return "Widow's Peak"
                    elseif column == 7 and row == 13 then
                        return "Keliner"
                    elseif column == 7 and row == 14 then
                        return "Gerntar Mines"
                    elseif column == 7 and row == 15 then
                        return "Oggok Gate"
                    elseif column == 7 and row == 16 then
                        return "Mila's Reef"
                    elseif column == 7 and row == 17 then
                        return "Open Sea"
                        
                    elseif column == 8 and row == 3 then
                        return "Snowfist"
                    elseif column == 8 and row == 4 then
                        return "Goldfeather Eyrie"
                    elseif column == 8 and row == 5 then
                        return "Moradhim"
                    elseif column == 8 and row == 6 then
                        return "Baga Village"
                    elseif column == 8 and row == 7 then
                        return "Merry-by-Water"
                    elseif column == 8 and row == 8 then
                        return "Bandit Hills"
                    elseif column == 8 and row == 9 then
                        return "Strag's Rest"
                    elseif column == 8 and row == 10 then
                        return "Salt Mine"
                    elseif column == 8 and row == 11 then
                        return "Centaur Valley"
                    elseif column == 8 and row == 12 then
                        return "Brog Fens"
                    elseif column == 8 and row == 13 then
                        return "Fort Alliance"
                    elseif column == 8 and row == 14 then
                        return "Elephant Graveyard"
                    elseif column == 8 and row == 15 then
                        return "Kerplunk Outpost"
                    elseif column == 8 and row == 16 then
                        return "Cazic Thule"
                    elseif column == 8 and row == 17 then
                        return "Open Sea"
                        
                    elseif column == 9 and row == 3 then
                        return "Greyvax's Caves"
                    elseif column == 9 and row == 4 then
                        return "Snafitzer's House"
                    elseif column == 9 and row == 5 then
                        return "Shon-To Monastery"
                    elseif column == 9 and row == 6 then
                        return "Misty Thicket"
                    elseif column == 9 and row == 7 then
                        return "Runnyeye"
                    elseif column == 9 and row == 8 then
                        return "Highpass Hold"
                    elseif column == 9 and row == 9 then
                        return "Trail's End"
                    elseif column == 9 and row == 10 then
                        return "Dshinn's Redoubt"
                    elseif column == 9 and row == 11 then
                        return "Wktaan's 4th Talon"
                    elseif column == 9 and row == 12 then
                        return "Serpent Hills"
                    elseif column == 9 and row == 13 then
                        return "Tak 'Xiz"
                    elseif column == 9 and row == 14 then
                        return "Tak 'Xiz South"
                    elseif column == 9 and row == 15 then
                        return "Lake Noregard"
                    elseif column == 9 and row == 16 then
                        return "Dinbak"
                    elseif column == 9 and row == 17 then
                        return "Open Sea"
                        
                    elseif column == 10 and row == 3 then
                        return "Fayspire Gate"
                    elseif column == 10 and row == 4 then
                        return "Fayspires, Tethelin"
                    elseif column == 10 and row == 5 then
                        return "Thedruk"
                    elseif column == 10 and row == 6 then
                        return "Rivervale"
                    elseif column == 10 and row == 7 then
                        return "Moss Mouth Cavern"
                    elseif column == 10 and row == 8 then
                        return "Bastable Village"
                    elseif column == 10 and row == 9 then
                        return "Ferran's Hope"
                    elseif column == 10 and row == 10 then
                        return "Desert Hate"
                    elseif column == 10 and row == 11 then
                        return "Deathfist Horde"
                    elseif column == 10 and row == 12 then
                        return "Chiktar Hive"
                    elseif column == 10 and row == 13 then
                        return "Tak 'Xiv"
                    elseif column == 10 and row == 14 then
                        return "Takish'Hiz"
                    elseif column == 10 and row == 15 then
                        return "Burial Mounds"
                    elseif column == 10 and row == 16 then
                        return "Stone Watchers"
                    elseif column == 10 and row == 17 then
                        return "Open Sea"
                        
                    elseif column == 11 and row == 2 then
                        return "Ocean For Pathing 2"
                    elseif column == 11 and row == 3 then
                        return "NE Mountain Boundary"
                    elseif column == 11 and row == 4 then
                        return "Kara Village"
                    elseif column == 11 and row == 5 then
                        return "Castle Felstar"
                    elseif column == 11 and row == 6 then
                        return "North Kithicor"
                    elseif column == 11 and row == 7 then
                        return "Saerk Towers"
                    elseif column == 11 and row == 8 then
                        return "Tomb of Kings"
                    elseif column == 11 and row == 9 then
                        return "Deathfist Forge"
                    elseif column == 11 and row == 10 then
                        return "Deathfist Citadel"
                    elseif column == 11 and row == 11 then
                        return "Box Canyons"
                    elseif column == 11 and row == 12 then
                        return "Eternal Desert"
                    elseif column == 11 and row == 13 then
                        return "Oasis"
                    elseif column == 11 and row == 14 then
                        return "Sea of Lions"
                    elseif column == 11 and row == 15 then
                        return "Ant Colonies"
                    elseif column == 11 and row == 16 then
                        return "Brokenskull Rock"
                    elseif column == 11 and row == 17 then
                        return "Empty"
                        
                    elseif column == 12 and row == 2 then
                        return "W Dread Sea"
                    elseif column == 12 and row == 3 then
                        return "SW Dread Sea"
                    elseif column == 12 and row == 4 then
                        return "Klick'Anon"
                    elseif column == 12 and row == 5 then
                        return "Collonridge Cemetary"
                    elseif column == 12 and row == 6 then
                        return "The Green Rift"
                    elseif column == 12 and row == 7 then
                        return "Mu Lin's Reach"
                    elseif column == 12 and row == 8 then
                        return "Temple of Light"
                    elseif column == 12 and row == 9 then
                        return "Northwestern Ro" 
                    elseif column == 12 and row == 10 then
                        return "Muniel's Tea Garden"
                    elseif column == 12 and row == 11 then
                        return "Al Farak Ruins"
                    elseif column == 12 and row == 12 then
                        return "Sycamore Joy's Rest"
                    elseif column == 12 and row == 13 then
                        return "Great Waste"
                    elseif column == 12 and row == 14 then
                        return "Slithtar Hive"
                    elseif column == 12 and row == 15 then
                        return "Sslathis"
                    elseif column == 12 and row == 16 then
                        return "Basher's Enclave"
                    elseif column == 12 and row == 17 then
                        return "Open Sea"
                        
                    elseif column == 13 and row == 2 then
                        return "Isle of Dread"
                    elseif column == 13 and row == 3 then
                        return "S Dread Sea"
                    elseif column == 13 and row == 4 then
                        return "Rogue Clockworks"
                    elseif column == 13 and row == 5 then
                        return "Neriak, Nektulos"
                    elseif column == 13 and row == 6 then
                        return "Bobble-by-Water"
                    elseif column == 13 and row == 7 then
                        return "Hodstock and Temby"
                    elseif column == 13 and row == 8 then
                        return "Freeport"
                    elseif column == 13 and row == 9 then
                        return "Northern Ro"
                    elseif column == 13 and row == 10 then
                        return "Open Sea"
                    elseif column == 13 and row == 11 then
                        return "Open Sea"
                    elseif column == 13 and row == 12 then
                        return "Open Sea"
                    elseif column == 13 and row == 13 then
                        return "Elemental Towers"
                    elseif column == 13 and row == 14 then
                        return "Hazinak"
                    elseif column == 13 and row == 15 then
                        return "Guk"
                    elseif column == 13 and row == 16 then
                        return "Grobb"
                    elseif column == 13 and row == 17 then
                        return "Open Sea"
                        
                    elseif column == 15 and row == 2 then
                        return "E Dread Sea"
                    elseif column == 15 and row == 3 then
                        return "SE Dread Sea"
                    elseif column == 15 and row == 4 then
                        return "NE Boundary"
                    elseif column == 15 and row == 5 then
                        return "Open Sea"
                    elseif column == 15 and row == 6 then
                        return "Open Sea"
                    elseif column == 15 and row == 7 then
                        return "Open Sea"
                    elseif column == 15 and row == 8 then
                        return "Open Sea"
                    elseif column == 15 and row == 9 then
                        return "Open Sea"
                    elseif column == 15 and row == 10 then
                        return "Open Sea"
                    elseif column == 15 and row == 11 then
                        return "Open Sea"
                    elseif column == 15 and row == 12 then
                        return "Open Sea"
                    elseif column == 15 and row == 13 then
                        return "Open Sea"
                    elseif column == 15 and row == 14 then
                        return "Open Sea"
                    elseif column == 15 and row == 15 then
                        return "Open Sea"
                    elseif column == 15 and row == 16 then
                        return "Open Sea"
                    elseif column == 15 and row == 17 then
                        return "Open Sea"
                    end

                    return "NONETHINGLESS";
                end
            end
            );
        };
    };
    --FAPI.EQOA.Player.Location.ZoneFull = _G.FAPI.CE.Address.New("[pcsx2-r3878.exe+00400B28]+D30", "String", "MyZone", 50, "TypeZoneName");
    print("FAPI Loaded.")
elseif _G.FAPI == nil then
    print("FAPI Not Loaded - FAPI.DirectoryLocation == nil")
else
    print("FAPI Not Loaded - Unknown Error")
end
