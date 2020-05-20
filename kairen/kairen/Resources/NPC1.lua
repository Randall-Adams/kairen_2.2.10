--[[ Meta-Data
Meta -Data-Version: 1.0

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
        if _G.FAPI.IO.File_Exists(RF.EQOA.Custom_Data.NPC_Maker.self .. _nameSafe .. Extension_NPCFiles) == false then return nil; end;
        print("0| Making NPC: " .. _nameSafe)
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
                writeInteger(self.myGhost.X, _value)
               --co("self.XSet","co_exit","")
            else
               --co("self.XGet","co_enter","")
                local value = readInteger(self.myGhost.X)
               --co("self.XGet","co_debugoutput","value = " .. value)
               --co("self.XGet","co_exit","")
                return value
            end
        end
        function self.Y(_value)
            if _value ~= nil then
               --co("self.YSet", "co_enter", _value)
                writeInteger(self.myGhost.Y, _value)
               --co("self.YSet","co_exit","")
            else
               --co("self.YGet","co_enter","")
                local value = readInteger(self.myGhost.Y)
               --co("self.YGet","co_debugoutput","value = " .. value)
               --co("self.YGet","co_exit","")
                return value
            end
        end        
        function self.Z(_value)
            if _value ~= nil then
           --co("self.ZSet", "co_enter", _value)
            writeInteger(self.myGhost.Z, _value)
           --co("self.ZSet","co_exit","")
            else
               --co("self.ZGet","co_enter","")
                local value = readInteger(self.myGhost.Z)
               --co("self.ZGet","co_debugoutput","value = " .. value)
               --co("self.ZGet","co_exit","")
                return value
            end
        end
        function self.F(_value)
            if _value ~= nil then
               --co("self.FSet", "co_enter", _value)
                writeInteger(self.myGhost.F, _value)
               --co("self.FSet","co_exit","")
            else
               --co("self.FGet","co_enter","")
                local value = readInteger(self.myGhost.F)
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
                local value = self.SpawnPoint._nameSafe
                return value
            end
        end
        self.exData.SpeakChannel = "Say"
        function self.Speak(_message, _channel)
            local useChannel
            if _message == nil and _channel ~= nil then
                self.exData.SpeakChannel = _channel
            elseif _message == nil and _channel == nil then
                --error no arguments provided
                print("0| Error: No Arguments Provided")
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
                
                print("0| " .. speakHeader .. speakMessage)
            end
        end
        function self.ApplyFileData()
            --co(_sender, "co_debugoutput", "Setting NPC's Stats..")
            npcfile = io.open(self.FilePath,"r+"); -- This is checked for existence above..
            FileVersion = _G.FAPI.IO.ReadNextLine(npcfile, "%-%-")
            --co(_sender, "co_debugoutput", "File Version is: " .. FileVersion)
                if FileVersion == "0.1.1" then
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
                    --co(_sender, "co_debugoutput", "File Version Not Supported")
                end
            npcfile:close();
        end
        --set npc file data to npc ram data
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
                _G.NPCs.Ghost = {};
                while name[i] ~= nil do
                    _G.NPCs.Ghost[i] = _G.NPCs.Ghosts.New(name[i])
                    _G.NPCs.Ghost[i].isFree = true
                    i = i + 1
                end
                _G.NPCs.Ghosts.NumberofGhosts = i
                _G.NPCs.Ghosts.index = 1
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
                _G.NPCs.Ghost = {};
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
            print("0| Number of Ghosts Loaded = " .. _G.NPCs.Ghosts.NumberofGhosts)
            
            --co(_sender, "co_debugoutput", "Number of Ghosts Loaded = " .. _G.NPCs.Ghosts.NumberofGhosts)
            --co(_sender, "co_exit", "")
        end
        );
        New = (function(_nameSafe, _folderToUse)
            if _nameSafe == nil then
                local self = {};
                print("0| No Ghost File Supplied - Ghosting as LUA instead")
                
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
                    print("0| Error: Ghost File Does Not Exist: " .. fileOfGhost)
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
                        print("0| Error: Unsupported Ghost Version: " .. fileversion)
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
    NewNPC = (function(_nameSafe)
            if _owner == nil then _owner = "free" end
            local self = {};
            self.exData = {};
            self.nameSafe = _nameSafe
            --if creating an npc from file, then load file
            --if creating an npc from code, then load array
            --then npc class needs to assign to a spawn point

            --find free ghost and assign to that        
            self.myGhost = Ghosts[1]

            function self.name(_value)
                if _value ~= nil then
                    --co("self.nameSet", "co_enter", _value)
                    self.myGhost.Name(_value)
                    --co("self.nameSet", "co_exit", _value)
                else
                    --co("self.nameGet","co_enter","")
                    local value = FAPI.CE.IO.Read_BitArrayToString(self.myGhost.nameOverhead, "TypeCharacterName")
                    --local value = Read_BitArrayToString(self.myGhost.nameTarget, "TypeCharacterName")
                    --co("self.nameGet","co_debugoutput","value = " .. value)
                    --co("self.nameGet","co_exit","")
                    return value
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
                    writeInteger(self.myGhost.X, _value)
                   --co("self.XSet","co_exit","")
                else
                   --co("self.XGet","co_enter","")
                    local value = readInteger(self.myGhost.X)
                   --co("self.XGet","co_debugoutput","value = " .. value)
                   --co("self.XGet","co_exit","")
                    return value
                end
            end
            function self.Y(_value)
                if _value ~= nil then
                   --co("self.YSet", "co_enter", _value)
                    writeInteger(self.myGhost.Y, _value)
                   --co("self.YSet","co_exit","")
                else
                   --co("self.YGet","co_enter","")
                    local value = readInteger(self.myGhost.Y)
                   --co("self.YGet","co_debugoutput","value = " .. value)
                   --co("self.YGet","co_exit","")
                    return value
                end
            end        
            function self.Z(_value)
                if _value ~= nil then
               --co("self.ZSet", "co_enter", _value)
                writeInteger(self.myGhost.Z, _value)
               --co("self.ZSet","co_exit","")
                else
                   --co("self.ZGet","co_enter","")
                    local value = readInteger(self.myGhost.Z)
                   --co("self.ZGet","co_debugoutput","value = " .. value)
                   --co("self.ZGet","co_exit","")
                    return value
                end
            end
            function self.F(_value)
                if _value ~= nil then
                   --co("self.FSet", "co_enter", _value)
                    writeInteger(self.myGhost.F, _value)
                   --co("self.FSet","co_exit","")
                else
                   --co("self.FGet","co_enter","")
                    local value = readInteger(self.myGhost.F)
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
                    local value = self.SpawnPoint._nameSafe
                    return value
                end
            end
            self.exData.SpeakChannel = "Say"
            function self.Speak(_message, _channel)
                local useChannel
                if _message == nil and _channel ~= nil then
                    self.exData.SpeakChannel = _channel
                elseif _message == nil and _channel == nil then
                    --error no arguments provided
                    print("0| Error: No Arguments Provided")
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
                    
                    print("0| " .. speakHeader .. speakMessage)
                end
            end

            return self;
        end
    );
    index = 0;
    Spawn = (function(_safeName)
        local foundNPC
        foundNPC = _G.NPCs.Find("safeName", _safeName)
        if foundNPC ~= nil then
            print("0| Found NPC.")
            foundNPC.ApplyFileData()
        else
            print("0| No found NPC.")
            _G.NPCs.AssignNextNPC(_G.NPCs.New(_safeName))
        end
        return nil;
    end
    );
    AssignNextNPC = (function(_npcToAssign)
        if _G.NPCs.index == 0 then
            _G.NPCs.index = 1
        elseif _G.NPCs.index == _G.NPCs.Ghosts.NumberofGhosts then
            _G.NPCs.index = 1
            --_G.NPCs.Ghosts.NewGheist()
        else
            _G.NPCs.index = _G.NPCs.index + 1
        end
        _G.NPCs[_G.NPCs.index] = _npcToAssign
        if _G.NPCs.NumberofNPCs < _G.NPCs.index then
            _G.NPCs.NumberofNPCs = _G.NPCs.index
            return _G.NPCs[_G.NPCs.index];
        end
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
        elseif _by == "safeName" then
            i = 1
            while i <= _G.NPCs.Ghosts.NumberofGhosts do
                if i > _G.NPCs.NumberofNPCs then return nil; end
                if _G.NPCs[i].safeName == _data then
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
    List = (
        function()
            for index,value in pairs(_G.NPCs) do
                print("0| " .. _G.NPCs[_G.NPCs.index].Name)
            end
        end
    );
    SpawnPoints = {
        NewSpawnPoint = (function() return nil; end);
        index = 0;
    };
    StableLocation = {
        [1] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [2] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
        };
        [3] = {
        X = "";
        Y = "";
        Z = "";
        F = "";
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

function NPCs.new2(_nameSafe, _owner)
    local _sender = "NPC.new"
    if _owner == nil then _owner = "free" end
    co(_sender, "co_enter", _nameSafe .. ", " .. _owner)
    -- This class should set up references to _ghost ONLY
    -- Spawn_NPC should fill this class, in turn filling the ghost.
    local self = {};
    
    
    --Find Usable Ghost and assign it for use
    local i_ghosts
    i_ghosts = 1
    loops = 1
    co(_sender, "co_debugoutput", "Looping for Free Ghost...")
    while loops <= NumberofGhosts do
        if Ghosts[i_ghosts].isFree == true then
            Ghosts[i_ghosts].isFree = false
            _ghost = Ghosts[i_ghosts]
            co(_sender, "co_debugoutput", "Will use Ghosts[" .. i_ghosts .. "]")
            loops = 25
        else
            i_ghosts = i_ghosts + 1
        end
        loops = loops + 1
    end
    if loops == (NumberofGhosts + 1) then
        co(_sender, "co_debugoutput", "No Free Ghost Found to spawn: " .. _nameSafe)
    end
    loops = 0
    
    self.myGhost = _ghost
    
    -- Properties / Variables

    self.supType = "supType NPC"
    self.Type = "baseType NPC" -- This gets overwritten by the subtype
    self.isFreeOrReplaceable = false
    self.Owner = _owner
    self.FileVersion_Coded = "1.0"

    return self;
end

function NPCs.Spawn2(_NPCFileName, _owner, _forceRespawn) -- Optional: Returns the NPC[] Used
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

--[[
_G.NPCs = {};
--NPC = {};
function NPCs.NewNPC(_npcID, _nameSafe, _nameGame, _x, _y, _z, _f, _level)
    local self = {};
    local self.v = {};
    local self.g = {};
    
    -- Properties / Variables
    self.nameSafe = _nameSafe
    --self.isFreeOrReplaceable = false
    --self.Owner = _owner
    --self.FileVersion_Coded = "1.0"
    --self.myGhost = _ghost
    self.v.npcID = _npcID
    self.v.x = _x
    self.v.y = _y
    self.v.z = _z
    self.v.f = _f
    self.v.level = _level

    function self.X(_input)
        co("self.X","co_enter","")
        if _input ~= nil then
            --if has ghost then
            writeInteger(_ghost.X, _value)
            --else
            -- variable = input
            --end
        else
            local value = readInteger(_ghost.X)
            co("self.XGet","co_debugoutput","value = " .. value)
            co("self.X","co_exit","")
            return value
        end

    end
    
    return self;

end
NPCs.NPC[i] = NPCs.NewNPC("1", "Crob", )

NPCs.LoadNPCFile("") --loads into newnpc() sub
NPCs.NewNPC("") --turns argument into npc, so anything can make an npc, not just a file
NPCS.UnloadNPC("")
NPCs.FindByName("")
NPCs.FindByTarget("")
NPCs.NPC[i].Spawn()
NPCs.NPC[i].DeSpawn()
NPCs.NPC[i].PerformDeath()
NPCs.NPC[i].HideGraphic()
NPCs.NPC[i].ShowGraphic()
NPCs.NPC[i].UpdateNPC("UpdateDataAsArray")
NPCs.NPC[i].Talk("message", "display channel as tell say MsgBox questers print()", "real channel")
NPCs.NPC[i].GraphicSequence.Emotes.Wave()
NPCs.NPC[i].GraphicSequence.Motions.Walk()
NPCs.NPC[i].GraphicSequence.Motions.Swim()
NPCs.NPC[i].GraphicSequence.Engagements.KnockedBack()
NPCs.NPC[i].GraphicSequence.Engagements.Attack.1HS()
NPCs.NPC[i].GraphicSequence.Engagements.Casting()
NPCs.NPC[i].GraphicSequence.Clear()
]]--
