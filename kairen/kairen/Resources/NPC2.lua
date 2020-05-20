--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: NPCs
Code-Type: LUA Class
Code-Version: 4.0
Code-Description: NPC Class
Code-Author: Robert Randazzio
]]--
-- NPC Class
--NPCs.NPC[NPCs.index()] = NPCs.NewNPC("Coachman_Ronks")
--.NPC[] class holds the NPC.Spawn, etc, data
_G.NPCs = {
    New = (function(_nameSafe)
        if _G.FAPI.IO.File_Exists(RF.EQOA.Custom_Data.NPC_Maker.self .. _nameSafe .. Extension_NPCFiles) == false then
          --print(_nameSafe .. " File not found.")
            return nil;
        end
      --print("Making NPC: " .. _nameSafe)
        local self = {};
        self.exData = {};
        self.nameSafe = _nameSafe
        self.FilePath = RF.EQOA.Custom_Data.NPC_Maker.self .. _nameSafe .. Extension_NPCFiles
        self.myGhost = _G.NPCs.Ghosts.AssignNextGhost();
        function self.Name(_value)
            if _value ~= nil then
                self.myGhost.Name(_value)
            else
                --local value = FAPI.CE.IO.Read_BitArrayToString(self.myGhost.nameOverhead, "TypeCharacterName")
                --return value
                return self.myGhost.Name();
            end
        end
        function self.ghostID(_value)
            if _value ~= nil then
               --co("self.ghostIDSet", "co_enter", _value)
                writeString(self.myGhost.ghostID, _value)
               --co("self.ghostIDSet","co_exit","")
            else
               --co("self.ghostIDGet","co_enter","")
                local value = readString(self.myGhost.ghostID)
               --co("self.ghostIDGet","co_debugoutput","value = " .. value)
               --co("self.ghostIDGet","co_exit","")
                return value
            end
        end
        function self.nameOverheadColor(_value)
            if _value ~= nil then
               --co("self.nameOverheadColorSet", "co_enter", _value)
                writeBytes(self.myGhost.nameOverheadColor, _value)
               --co("self.nameOverheadColorSet","co_exit","")
            else
               --co("self.nameOverheadColorGet","co_enter","")
                local value = readInteger(self.myGhost.nameOverheadColor)
               --co("self.nameOverheadColorGet","co_debugoutput","value = " .. value)
               --co("self.nameOverheadColorGet","co_exit","")
                return value
            end
        end
        function self.X(_value)
            if _value ~= nil then
               --co("self.XSet", "co_enter", _value)
               --co("self.XSet", "co_debugoutput", self.myGhost.X)
                writeFloat(self.myGhost.X, _value)
               --co("self.XSet","co_exit","")
            else
               --co("self.XGet","co_enter","")
                local value = readFloat(self.myGhost.X)
               --co("self.XGet","co_debugoutput","value = " .. value)
               --co("self.XGet","co_exit","")
                return value
            end
        end
        function self.Y(_value)
            if _value ~= nil then
               --co("self.YSet", "co_enter", _value)
                writeFloat(self.myGhost.Y, _value)
               --co("self.YSet","co_exit","")
            else
               --co("self.YGet","co_enter","")
                local value = readFloat(self.myGhost.Y)
               --co("self.YGet","co_debugoutput","value = " .. value)
               --co("self.YGet","co_exit","")
                return value
            end
        end        
        function self.Z(_value)
            if _value ~= nil then
           --co("self.ZSet", "co_enter", _value)
            writeFloat(self.myGhost.Z, _value)
           --co("self.ZSet","co_exit","")
            else
               --co("self.ZGet","co_enter","")
                local value = readFloat(self.myGhost.Z)
               --co("self.ZGet","co_debugoutput","value = " .. value)
               --co("self.ZGet","co_exit","")
                return value
            end
        end
        function self.F(_value)
            if _value ~= nil then
               --co("self.FSet", "co_enter", _value)
                writeFloat(self.myGhost.F, _value)
               --co("self.FSet","co_exit","")
            else
               --co("self.FGet","co_enter","")
                local value = readFloat(self.myGhost.F)
               --co("self.FGet","co_debugoutput","value = " .. value)
               --co("self.FGet","co_exit","")
                return value
            end
        end
        function self.level(_value)
            if _value ~= nil then
               --co("self.levelSet", "co_enter", _value)
                writeBytes(self.myGhost.level, _value)
               --co("self.levelSet","co_exit","")
            else
               --co("self.levelGet","co_enter","")
                local value = readBytes(self.myGhost.level)
               --co("self.levelGet","co_debugoutput","value = " .. value)
               --co("self.levelGet","co_exit","")
                return value
            end
        end
        function self.hp(_value)
            if _value ~= nil then
           --co("self.hpSet", "co_enter", _value)
            writeBytes(self.myGhost.hp, _value)
           --co("self.hpSet","co_exit","")
            else
           --co("self.hpGet","co_enter","")
            local value = readBytes(self.myGhost.hp)
           --co("self.hpGet","co_debugoutput","value = " .. value)
           --co("self.hpGet","co_exit","")
            return value
            end
        end

        function self.SpawnPoint(_value)
            if _value ~= nil then
               self.SpawnPoint = _value
            else
                return value.nameSafe
            end
        end
        self.exData.SpeakChannel = "Say"
        function self.Speak(_message, _channel)
            local useChannel
            if _message == nil and _channel ~= nil then
                self.exData.SpeakChannel = _channel
            elseif _message == nil and _channel == nil then
                --error no arguments provided
              --print("Error: No Arguments Provided")
            elseif _message ~= nil then
                local speakHeader
                if _channel == nil then
                    useChannel = self.exData.SpeakChannel
                elseif _channel ~= nil then
                    useChannel = _channel
                end
                if useChannel == "Say" then
                    speakHeader = self.name() .. " says: "
                elseif useChannel == "Shout" then
                    speakHeader = self.name() .. " shouts: "
                else
                    speakHeader = self.name() .. " says: "
                end
                if _message == "Name" then
                    speakMessage = self.name()
                elseif _message == "Location" then
                    speakMessage = "X: " .. self.X() .. ", Y: " .. self.Y() .. ", Z: " .. self.Z()
                elseif _message == "Zone" then
                    speakMessage = "Well, I don't actually know what zone I'm in."
                end
                
                print(speakHeader .. speakMessage)
            end
        end
        function self.ApplyFileData()
          --print("_G.NPCs.New.ApplyFileData()")
            --co(_sender, "co_debugoutput", "Setting NPC's Stats..")
            npcfile = io.open(self.FilePath,"r+"); -- This is checked for existence above..
            FileVersion = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
            --co(_sender, "co_debugoutput", "File Version is: " .. FileVersion)
                if FileVersion == "0.1.1" then
                  --print("NPC Version Matched")
                    self.FileVersion_Loaded = "0.1.1"
                    self.nameSafe = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    self.Name(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    self.X(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    self.Y(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    self.Z(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    self.F(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Race = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Gender = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Class = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")        
                    self.level(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    self.hp(_G.FAPI.IO.ReadNextLine(npcfile, "%-%-"))
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].MP = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].AC = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Strength = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Stamina = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Agility = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Dexterity = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Wisdom = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Intelligence = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].Charisma = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].FR = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].CR = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].LR = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].AR = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].PR = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --_G.FAPI.IO.ReadNextLine(npcfile, "%-%-")--        NPCs[i].DR = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
                    --co(_sender, "co_debugoutput", "All NPC Values Set")
                else
                  --print("Unrecognized NPC File Version")
                    --co(_sender, "co_debugoutput", "File Version Not Supported")
                end
            npcfile:close();
        end
        --set npc file data to npc ram data
      --print("Applying NPC File Data")
        self.ApplyFileData();
        return self;
    end
    );
    Ghosts = {
        NumberofGhosts = 0;
        index = -1; -- -1 for not setup, 0, and higher for actual index position
        AssignNextGhost = (function()
            if _G.NPCs.Ghosts.index == -1 then
                _G.NPCs.Ghosts.SetupGhosts();
            end
            --get free ghost and return it
            if _G.NPCs.Ghosts.index == _G.NPCs.Ghosts.NumberofGhosts then
                _G.NPCs.Ghosts.index = 1
                --_G.NPCs.Ghosts.NewGheist()
            else
                _G.NPCs.Ghosts.index = _G.NPCs.Ghosts.index + 1                
            end
            return _G.NPCs.Ghost[_G.NPCs.Ghosts.index];
        end
        );
        SetupGhosts = (function()
            --local _sender = "setupGhosts"
            --co(_sender, "co_enter", "")
            local i = 0
            name = {};
            if SaveState == nil or SaveState == "Corsten" then
                --if SaveState == nil then co(_sender, "co_debugoutput", "Notice: SaveState variable is nil") end
                i = i + 1
                name[i] = "Coachman_Ronks"
                i = i + 1
                name[i] = "Aloj_Tilsteran"
                i = i + 1
                name[i] = "Merchant_Kari"
                i = i + 1
                name[i] = "Guard_Saolen"
                i = i + 1
                name[i] = "Guard_Jahn"
                i = i + 1
                name[i] = "Bowyer_Koll"
                i = i + 1
                name[i] = "Tailor_Bariston"
                i = i + 1
                name[i] = "Tailor_Zixar"
                i = i + 1
                name[i] = "Guard_Serenda"
                i = i + 1
                name[i] = "Finalquestt"
                i = i + 1
                name[i] = "Guard_Perinen"
                i = i + 1
                name[i] = "Manoarmz"
                i = i + 1
                name[i] = "Marona_Jofranka"
                i = i + 1
                name[i] = "Merchant_Ahkham"
                i = i + 1
                name[i] = "Nukenurplace"
                i = i + 1
                name[i] = "Raam"
                i = i + 1
                name[i] = "Royce_Tilsteran"
                i = i + 1
                --name[i] = "Dr_Killian"
                --i = i + 1
                name[i] = "a_badger"
                i = i + 1
                name[i] = "an_undead_mammoth"
                i = i + 1
                name[i] = "Arch_Familiar"
                i = i + 1
                --name[i] = "Angry_Patron"
                --i = i + 1
                --name[i] = "sign_post"
                _G.NPCs.Ghost = {};
                i = 1
                while name[i] ~= nil do
                    _G.NPCs.Ghost[i] = _G.NPCs.Ghosts.New(name[i])
                    _G.NPCs.Ghost[i].isFree = true
                    i = i + 1
                end
                _G.NPCs.Ghosts.NumberofGhosts = i
                _G.NPCs.Ghosts.index = 1
            elseif SaveState == "Lostologist" then    
                name[i] = "elder_spirit"
                i = i + 1
                name[i] = "evil_head"
                i = i + 1
                name[i] = "kappa_drudge1"
                i = i + 1
                name[i] = "kappa_drudge2"
                i = i + 1
                name[i] = "kappa_drudge3"
                i = i + 1
                name[i] = "kappa_drudge5"
                i = i + 1
                name[i] = "phantom1"
                i = i + 1
                name[i] = "phantom2"
                i = i + 1
                name[i] = "phantom3"
                i = i + 1
                name[i] = "phantom4"
                i = i + 1
                name[i] = "phantom5"
                i = i + 1
                name[i] = "phantom6"
                i = i + 1
                name[i] = "phantom7"
                i = i + 1
                name[i] = "phantom8"
                i = i + 1
                name[i] = "phantom9"
                i = i + 1
                name[i] = "Spectral_Servant"
                i = i + 1
                name[i] = "spirit1"
                i = i + 1
                name[i] = "spirit2"
                i = i + 1
                name[i] = "spirit3"
                i = i + 1
                name[i] = "spirit4"
                i = i + 1
                name[i] = "spirit5"
                i = i + 1
                name[i] = "spirit6"
                i = i + 1
                name[i] = "spirit7"
                _G.NPCs.Ghost = {};
                i = 1
                while name[i] ~= nil do
                    _G.NPCs.Ghost[i] = _G.NPCs.Ghosts.New(name[i])
                    _G.NPCs.Ghost[i].isFree = true
                    i = i + 1
                end
                _G.NPCs.Ghosts.NumberofGhosts = i
                _G.NPCs.Ghosts.index = 1
            else
                --co(_sender, "co_debugoutput", "Error: No Ghosts code for the SaveState: " .. SaveState)
            end
          --print("Number of Ghosts Loaded = " .. _G.NPCs.Ghosts.NumberofGhosts)
            
            --co(_sender, "co_debugoutput", "Number of Ghosts Loaded = " .. _G.NPCs.Ghosts.NumberofGhosts)
            --co(_sender, "co_exit", "")
        end
        );
        New = (function(_nameSafe, _folderToUse)
            if _nameSafe == nil then
                local self = {};
              --print("No Ghost File Supplied - Ghosting as LUA instead")
                
                self.name = ""
                self.X = ""
                self.Y = ""
                self.Z = ""
                self.F = ""
                self.level = ""
                self.hp = ""
                
                return self;
            else
                if _folderToUse == nil then 
                    fileOfGhost = RF.EQOA.Game_Data.Ghosts.self .. _nameSafe .. Extension_GhostFiles
                else
                    fileOfGhost = _folderToUse .. _nameSafe .. Extension_GhostFiles
                end
                if FAPI.IO.File_Exists(fileOfGhost) == false then
                  --print("Error: Ghost File Does Not Exist: " .. fileOfGhost)
                    --co(_sender, "co_debugoutput", "Error: File Version Not Supported : " .. fileversion)
                    return nil;
                else
                    local self = {};
                    file = io.open(fileOfGhost,"r+");
                    -- Sets This Ghost to point to the Pointers
                    local fileversion
                    fileversion = FAPI.IO.ReadNextLine(file, "%-%-")
                    if fileversion == "0.1.1" then
                        self.ghostID = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.nameOverhead = FAPI.IO.ReadNextLine(file, "%-%-") 
                        self.nameOverheadColor = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.nameTarget = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.X = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.Y = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.Z = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.F = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.level = FAPI.IO.ReadNextLine(file, "%-%-")
                        self.hp = FAPI.IO.ReadNextLine(file, "%-%-")

                        function self.Name(_value)
                            if _value ~= nil then
                                FAPI.CE.IO.Write_StringToBitArrayAddress(_value, self.nameOverhead, 1)
                                FAPI.CE.IO.Write_StringToBitArrayAddress(_value, self.nameTarget, 1)
                            else
                                return FAPI.CE.IO.Read_BitArrayToString(self.nameTarget, "TypeCharacterName")
                                --return self.nameTarget;
                            end
                        end
                    else
                      --print("Error: Unsupported Ghost Version: " .. fileversion)
                    end
                    file:close();
                    return self;
                end
            end
        end
        );
        index = -1; -- -1 for not setup, 0, and higher for actual index position
        NewGheist = (function() return nil; end);
    };
    index = 0;
    Spawn = (function(_nameSafe)
        if _G.FAPI.IO.File_Exists(RF.EQOA.Custom_Data.NPC_Maker.self .. _nameSafe .. Extension_NPCFiles) == false then
          --print("NPC file not found, cancelling spawn.")
            return nil;
        end
      --print("NPCs.Spawn(" .. _nameSafe .. ")")
        local foundNPC
        foundNPC = _G.NPCs.Find("nameSafe", _nameSafe)
        if foundNPC ~= nil then
          --print("Found current NPC to spawn.")
            foundNPC.ApplyFileData()
        else
          --print("No current NPC found to spawn, assigning next npc.")
          --print("_nameSafe == " .. _nameSafe)
            local _tempnpc 
            _tempnpc = _G.NPCs.New(_nameSafe)
          --print("_tempnpc.Name() == " .. _tempnpc.Name())
            _G.NPCs.AssignNextNPC(_tempnpc)
        end
        return nil;
    end
    );
    AssignNextNPC = (function(_npcToAssign)
      --print("NPCs.AssignNextNPC()")
        if _G.NPCs.index == 0 then
            _G.NPCs.index = 1
        elseif _G.NPCs.index == _G.NPCs.Ghosts.NumberofGhosts then
            _G.NPCs.index = 1
            --_G.NPCs.Ghosts.NewGheist()
        else
            _G.NPCs.index = _G.NPCs.index + 1
        end
      --print("type(_npcToAssign) == " .. type(_npcToAssign))
        _G.NPCs[_G.NPCs.index] = _npcToAssign
        if _G.NPCs.NumberofNPCs < _G.NPCs.index then
            _G.NPCs.NumberofNPCs = _G.NPCs.index
          --print("type(_G.NPCs[_G.NPCs.index]) == " .. type(_G.NPCs[_G.NPCs.index]))
          --print("NPCs[1].Name() == " .. NPCs[1].Name())
            return _G.NPCs[_G.NPCs.index];
        end
      --print("error no return")
    end
    );
    NumberofNPCs = 0;
    Find = (function(_by, _data)
        if _by == "Name" then
            i = 1
            while i <= _G.NPCs.Ghosts.NumberofGhosts do
                if i > _G.NPCs.NumberofNPCs then return nil; end
                if _G.NPCs[i].Name() == _data then
                    return _G.NPCs[i];
                else
                    i = i + 1
                end
            end
            -- No NPC Found
            return nil;
        elseif _by == "nameSafe" then
            i = 1
            while i <= _G.NPCs.Ghosts.NumberofGhosts do
                if i > _G.NPCs.NumberofNPCs then return nil; end
                if _G.NPCs[i].nameSafe == _data then
                    return _G.NPCs[i];
                else
                    i = i + 1
                end
            end
            -- No NPC Found
            return nil;
        else
            -- Unrecognized Find Type
            return nil;
        end
    end
    );
    ListAllNPCs_byName = (function()
            for key,value in pairs(_G.NPCs) do
                print(_G.NPCs[_G.NPCs.key].Name)
            end
        end
    );
    StableLocation = {
        [1] = {
        X = "1187342748";
        Y = "1182091322";
        Z = "1113096453";
        F = "1076335644";
        };
        [2] = {
        X = "";
        Y = "";
        Z = "";
        F = "1076335644";
        };
        [3] = {
        X = "";
        Y = "";
        Z = "";
        F = "1076335644";
        };
        [4] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [5] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [6] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [7] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [8] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [9] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [10] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [11] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [12] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [13] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [14] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [15] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [16] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [17] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [18] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [19] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [20] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [21] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [22] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [23] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [24] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
    };
};
