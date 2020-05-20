--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: NPCs
Code-Type: LUA Class
Code-Version: 4.5
Code-Description: NPC Class
Code-Author: Robert Randazzio
]]--
-- NPC Class
--NPCs.NPC[NPCs.index()] = NPCs.NewNPC("Coachman_Ronks")
--.NPC[] class holds the NPC.Spawn, etc, data
NPC = {};
NPCs = setmetatable(
{
    New = (function(_filePath)
        local self = setmetatable( {}, {__index = function (_table, _key) return nil; end});
        self.FilePath = _filePath

        local i = 1
        local _npcfile
        _npcfile = FAPI.IO.Read_FileToStringArray(self.FilePath, FAPI.CommentIndicatorString)
        if _npcfile[i] == "0.1.1" then
            print("NPC is 0.1.1")
            print("NPC file needs converted to 0.2.0 before it can spawn.")
            return nil;
        elseif _npcfile[i] == "0.1.2" then
            print("NPC is 0.1.2")
            print("NPC file needs converted to 0.2.0 before it can spawn.")
            return nil;
        elseif _npcfile[i] == "0.1.3" then
            print("NPC is 0.1.3")
            print("NPC file needs converted to 0.2.0 before it can spawn.")
            return nil;
        elseif _npcfile[i] == "0.2.0" then
            --print("NPC is 0.2.0")
        else
            print("NPC is unknown version " .. _file[i])
            return nil;
        end

        self.exData = {};
        self.MyData = {};
        self.myGhost = nil;
        self.NeedsGhost = true
        self.SpawnPoint = nil;
        function self.Name(_value)
            if _value ~= nil then
                self.MyData.Name = _value
                if self.myGhost ~= nil then
                    self.myGhost.Name(_value)
                end
            else
                return self.MyData.Name;
            end
        end
        function self.X(_value)
            if _value ~= nil then
                self.MyData.X = _value
                if self.myGhost ~= nil then
                    self.myGhost.X.Value(_value);
                end
            else
                return self.MyData.X;
            end
        end
        function self.Y(_value)
            if _value ~= nil then
                self.MyData.Y = _value
                if self.myGhost ~= nil then
                    self.myGhost.Y.Value(_value);
                end
            else
                return self.MyData.Y;
            end
        end       
        function self.Z(_value)
            if _value ~= nil then
                self.MyData.Z = _value
                if self.myGhost ~= nil then
                    self.myGhost.Z.Value(_value);
                end
            else
                return self.MyData.Z;
            end
        end
        function self.F(_value)
            if _value ~= nil then
                self.MyData.F = _value
                if self.myGhost ~= nil then
                    self.myGhost.F.Value(_value);
                end
            else
                return self.MyData.F;
            end
        end
        function self.Level(_value)
            if _value ~= nil then
                self.MyData.Level = _value
                if self.myGhost ~= nil then
                    self.myGhost.Level.Value(_value);
                end
            else
                return self.MyData.Level;
            end
        end
        function self.HP(_value)
            if _value ~= nil then
                self.MyData.HP = _value
                if self.myGhost ~= nil then
                    self.myGhost.HP.Value(_value);
                end
            else
                return self.MyData.HP;
            end
        end
        function self.nameOverheadColor(_value)
            if _value ~= nil then
                self.MyData.nameOverheadColor = _value
                if self.myGhost ~= nil then
                    self.myGhost.nameOverheadColor.Value(_value);
                end
            else
                return self.MyData.nameOverheadColor;
            end
        end
        function self.Despawn(_removeFromSpawnPointFlag)
            NPCs.Despawn(self, _removeFromSpawnPointFlag)
        end
        function self.Goto()
            NPCs.Goto(self)
        end
        self.exData.SpeakChannel = "Say"
        function self.Speak(_message, _channel, _definedMessage)
            local useChannel
            if _message == nil and _channel ~= nil then
                self.exData.SpeakChannel = _channel
            elseif _message == nil and _channel == nil then
                --error no arguments provided
              --print("Error: No Arguments Provided")
            elseif _message ~= nil then
                local speakHeader
                local speakMessage
                if _channel == nil then
                    useChannel = self.exData.SpeakChannel
                elseif _channel ~= nil then
                    useChannel = _channel
                end
                if useChannel == "Say" then
                    speakHeader = self.Name() .. " says: "
                elseif useChannel == "Shout" then
                    speakHeader = self.Name() .. " shouts: "
                else
                    speakHeader = self.Name() .. " says: "
                end
                if _message == "Name" then
                    speakMessage = self.Name()
                elseif _message == "Location" then
                    speakMessage = "X: " .. self.X() .. ", Y: " .. self.Y() .. ", Z: " .. self.Z()
                elseif _message == "Zone" then
                    speakMessage = "I am in " .. self.Zone() .. "."
                else
                    speakMessage = _message
                end
                
                print(speakHeader .. speakMessage)
            end
        end
        function self.UpdateGhost()
            if self.myGhost ~= nil then
                self.myGhost.Name(self.MyData.Name)
                self.myGhost.X.Value(self.MyData.X);
                self.myGhost.Y.Value(self.MyData.Y);
                self.myGhost.Z.Value(self.MyData.Z);
                self.myGhost.F.Value(self.MyData.F);
                self.myGhost.Level.Value(self.MyData.Level);
                return 1
            else
                return -1
            end
        end
        function self.Interact(_interaction)
            --if _interaction == "Hail Player" then
                --self.Speak("Hail " .. AddressList["PlayerName"].Value())
            --elseif _interaction == "StartDialogue" then
                if self.Name() == "Coachman Ronks" then
                    print("Where ya wanna Coach to, buuuudyyyy?")
                else
                    self.Speak("Hail " .. AddressList["PlayerName"].Value())
                end
            --end
        end
        self.FileVersion_Loaded = _npcfile[i]
        i = i + 1
        self.nameSafe = _npcfile[i]
        i = i + 1
        self.Name(_npcfile[i])
        i = i + 1
        self.X(_npcfile[i])
        i = i + 1
        self.Y(_npcfile[i])
        i = i + 1
        self.Z(_npcfile[i])
        i = i + 1
        self.F(_npcfile[i])
        i = i + 1--_npcfile[i] --        NPC[i].Race = _npcfile[i]
        i = i + 1--_npcfile[i] --        NPC[i].Gender = _npcfile[i]
        i = i + 1 --_npcfile[i] --        NPC[i].Class = _npcfile[i]
        i = i + 1
        self.Level(_npcfile[i])
        i = i + 1
        self.HP(_npcfile[i])
        --_npcfile[i]--        NPC[i].MP = _npcfile[i]
        --_npcfile[i]--        NPC[i].AC = _npcfile[i]
        --_npcfile[i]--        NPC[i].Strength = _npcfile[i]
        --_npcfile[i]--        NPC[i].Stamina = _npcfile[i]
        --_npcfile[i]--        NPC[i].Agility = _npcfile[i]
        --_npcfile[i]--        NPC[i].Dexterity = _npcfile[i]
        --_npcfile[i]--        NPC[i].Wisdom = _npcfile[i]
        --_npcfile[i]--        NPC[i].Intelligence = _npcfile[i]
        --_npcfile[i]--        NPC[i].Charisma = _npcfile[i]
        --_npcfile[i]--        NPC[i].FR = _npcfile[i]
        --_npcfile[i]--        NPC[i].CR = _npcfile[i]
        --_npcfile[i]--        NPC[i].LR = _npcfile[i]
        --_npcfile[i]--        NPC[i].AR = _npcfile[i]
        --_npcfile[i]--        NPC[i].PR = _npcfile[i]
        --_npcfile[i]--        NPC[i].DR = _npcfile[i]
        --print(self.MyData.Name .. " spawned.")
        return self;
    end
    );
    Spawn = (function(_nameSafe, _returnType)
        if _returnType == nil then _returnType = "index" end
        if NPCs.Find("nameSafe", _nameSafe) ~= nil then
            --npc is spawned already
            return nil;
        else
            if FAPI.IO.File_Exists(RF.EQOA.Custom_Data.NPC_Maker.self .. _nameSafe .. Extension_NPCFiles) == true then
                _filepath = RF.EQOA.Custom_Data.NPC_Maker.self .. _nameSafe .. Extension_NPCFiles
                --print(_nameSafe .. " located in " .. RF.EQOA.Custom_Data.NPC_Maker.self .. ".")
            elseif FAPI.IO.File_Exists(RF.EQOA.Game_Data.NPCs.self .. _nameSafe .. Extension_NPCFiles) == true then
                _filepath = RF.EQOA.Game_Data.NPCs.self .. _nameSafe .. Extension_NPCFiles
                print(_nameSafe .. " located in " .. RF.EQOA.Game_Data.NPCs.self .. ".")
            else
                print(_nameSafe .. " File not found.")
                print("NPC unable to be spawned.")
                return nil;
            end
            local _tempnpc = NPCs.New(_filepath)
            if _tempnpc == nil then
                print("NPC unable to be spawned.")
                return nil;
            end
            return NPCs.AssignNextNPC(_tempnpc, _returnType); --returns new npc's assigned index
        end
        print("Error in NPCs.Spawn()")
        return nil;
    end
    );
    NumberofNPCs = 0;
    index = -1;
    Despawn = (function(_npcToDespawn, _removeFromSpawnPointFlag)
        if _npcToDespawn == nil then return nil end
        local npcindex = NPCs.Find("nameSafe", _npcToDespawn.nameSafe, "index")
        --NPC[i].Despawn() --kind of not needed since the ghost should disappear pretty quickly after the npc is marked empty anyway
        local newnpclist = {}
        local i = 0
        for key,value in pairs(NPC) do
            if value == nil then
            elseif value ~= _npcToDespawn then
                --copies old npc to new list
                i = i + 1
                newnpclist[i] = value
            elseif value == _npcToDespawn then
                --if this what is to be removed
                if value.myGhost ~= nil then
                    value.myGhost.myNPC = nil
                    value.myGhost.IsFree = true
                    value.myGhost = nil
                end
                value.NeedsGhost = false
                value.X(0)
                value.Y(0)
                value.Z(0)
                if _removeFromSpawnPointFlag ~= nil then
                    if _removeFromSpawnPointFlag == true then
                        value.SpawnPoint.RemoveNPC(value)
                    end
                end
                value = nil
            end
        end
        NPC = newnpclist
        NPCs.index = i
        NPCs.NumberofNPCs = i
    end
    );
    AssignNextNPC = (function(_npcToAssign, _returnType)
        if _returnType == nil then
            _returnType = "NPC"
        end
        if NPCs.index == -1 then
            NPCs.index = 0
        end
        local i = 1
        while NPC[i] ~= nil do
            i = i + 1
        end
        NPCs.index = NPCs.index + 1
        NPC[NPCs.index] = _npcToAssign
        NPCs.NumberofNPCs = NPCs.NumberofNPCs + 1
        Kanizah.UpdateItemByAdditionalData("NPC" .. NPCs.index .. "Name", NPC[NPCs.index].Name)
        Kanizah.UpdateItemByAdditionalData("NPC" .. NPCs.index .. "X", NPC[NPCs.index].X)
        Kanizah.UpdateItemByAdditionalData("NPC" .. NPCs.index .. "Y", NPC[NPCs.index].Y)
        Kanizah.UpdateItemByAdditionalData("NPC" .. NPCs.index .. "Z", NPC[NPCs.index].Z)
        Kanizah.UpdateItemByAdditionalData("NPC" .. NPCs.index .. "F", NPC[NPCs.index].F)
        if _returnType == "NPC" then
            return NPC[NPCs.index];
        elseif _returnType == "index" then
            return NPCs.index;
        else
            return nil;
        end
    end
    );
    Find = (function(_by, _data, _returnType)
        if _returnType == nil then
            _returnType = "NPC"
        end
        if _by == "Name" then
            i = 1
            while i <= NPCs.NumberofNPCs do
                if NPC[i] == nil then
                    --return nil;
                elseif NPC[i].Name() == _data then
                    if _returnType == "NPC" then
                        return NPC[i];
                    elseif _returnType == "index" then
                        return i;
                    else
                        return nil;
                    end
                else
                    i = i + 1
                end
            end
            -- No NPC Found
            return nil;
        elseif _by == "nameSafe" then
            i = 1
            while i <= NPCs.NumberofNPCs do
                if NPC[i] == nil then
                    --return nil;
                elseif NPC[i].nameSafe == _data then
                    if _returnType == "NPC" then
                        return NPC[i];
                    elseif _returnType == "index" then
                        return i;
                    else
                        return nil;
                    end
                else
                    i = i + 1
                end
            end
            -- No NPC Found
            return nil;
        elseif _by == "NPC" then
            i = 1
            while i <= NPCs.NumberofNPCs do
                if NPC[i] == nil then
                    --return nil;
                elseif NPC[i] == _data then
                    if _returnType == "NPC" then
                        return NPC[i];
                    elseif _returnType == "index" then
                        return i;
                    else
                        return nil;
                    end
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
    UpdateVisualNPCs = (function()
        if Ghosts.NumberofGhosts == 0 then 
            print("Number of Ghosts: " .. Ghosts.SetupGhosts())
        end
        local myX = EQOA.Player.Location.X.Value()
        local myY = EQOA.Player.Location.Y.Value()
        local i = 1
        local npclist = {}
        while NPC[i] ~= nil do
            local xy_distance
            local x_distance = (NPC[i].X() - myX)
            local y_distance = (NPC[i].Y() - myY)
            xy_distance = math.sqrt((x_distance * x_distance) + (y_distance * y_distance))
            npclist[NPC[i]] = xy_distance
            --print(NPC[i].Name() .. " is " .. xy_distance .. " distance away.")
            i = i + 1
        end
        --print("")
        local DistanceList = { } -- Result goes here
        -- Store both key and value as pairs
        for key,value in pairs(npclist) do
          DistanceList[#DistanceList + 1] = { key = key, value = value }
        end
        -- Sort by value
        table.sort(DistanceList, function(lhs, rhs) return lhs.value < rhs.value end) -- the < > symbol changes the ordering
        -- Leave only keys, drop values
        for i2 = 1, #DistanceList do
          DistanceList[i2] = DistanceList[i2].key
        end
        -- Print the result
        --print("")
        Ghosts.AssignGhosts(DistanceList)
        --local ghostindex = 0
        --for i2 = 1, #T do
        --    ghostindex = ghostindex + 1
        --    if ghostindex <= Ghosts.NumberofGhosts then
        --        print("Consider NPC " ..  T[i2].Name() .. " visible.")
        --    else
        --        print("Not visibleing things?")
        --        return nil;
        --    end
        --end
    end
    );
    Goto = (function(_npcToGoto)
        if _npcToGoto == nil then return nil end
        --local nx = 0
        --local py = 0
        --local pz = 0
        --local px = 0
        --local py = 0
        --local pz = 0
        nx = _npcToGoto.X()
        ny = _npcToGoto.Y()
        nz = _npcToGoto.Z()
        --px = EQOA.Player.Location.X.Value()
        --py = EQOA.Player.Location.Y.Value()
        --pz = EQOA.Player.Location.Z.Value()
        --print("NPC.X: " .. nx)
        --print("NPC.Y: " .. ny)
        --print("NPC.Z: " .. nz)
        --print("Player.X " .. px)
        --print("Player.Y " .. py)
        --print("Player.Z " .. pz)
        local doagain = true
        while doagain == true do
            EQOA.Player.Location.Z.Value(nz)
            EQOA.Player.Location.Z2.Value(nz)
            
            EQOA.Player.Location.X.Value(nx)
            EQOA.Player.Location.X2.Value(nx)
            
            EQOA.Player.Location.Y.Value(ny)
            EQOA.Player.Location.Y2.Value(ny)
            
            px = EQOA.Player.Location.X.Value()
            py = EQOA.Player.Location.Y.Value()
            pz = EQOA.Player.Location.Z.Value()
            if px == nx then 
                if py == ny then doagain = false end
            end
        end
        --EQOA.Player.Location.X.Value(nx)
        --EQOA.Player.Location.Y.Value(ny)
        --EQOA.Player.Location.Z.Value(_npcToGoto.Z())
        EQOA.Player.Location.F.Value(_npcToGoto.F())
    end
    );
    GotoKeepForVideoMaking = (function(_npcToGoto)
        if _npcToGoto == nil then return nil end
        local nx = 0
        local ny = 0
        local px = 0
        local py = 0
        nx = _npcToGoto.X()
        ny = _npcToGoto.Y()
        px = EQOA.Player.Location.X.Value()
        py = EQOA.Player.Location.Y.Value()
        print("NPC.X: " .. nx)
        print("NPC.Y: " .. ny)
        print("Player.X " .. px)
        print("Player.Y " .. py)
        local doagain = true
        while doagain == true do
            EQOA.Player.Location.X.Value(nx)
            EQOA.Player.Location.Y.Value(ny)
            if px == nx then 
                if py == ny then doagain = false end
            end
        end
        --EQOA.Player.Location.X.Value(nx)
        --EQOA.Player.Location.Y.Value(ny)
        --EQOA.Player.Location.Z.Value(_npcToGoto.Z())
        EQOA.Player.Location.F.Value(_npcToGoto.F())
    end
    );
    Interact = (function(_npcToHail, _interaction)
        if _npcToHail == nil then return nil end
        if _interaction == nil then return nil end
        local _npcToHail_ghost
        if _npcToHail == "" then
            --_npcToHail_ghost = ""
            return nil
        elseif _npcToHail == 553883 then -- an undead mammoth
            _npcToHail_ghost = "an_undead_mammoth"
        elseif _npcToHail == 84866 then -- corsten
            print("Hail Myself")
            return nil
        elseif _npcToHail == 341354 then -- guard jahn
            _npcToHail_ghost = "Guard_Jahn"
        elseif _npcToHail == 367401 then -- raam
            _npcToHail_ghost = "Raam"
        elseif _npcToHail == 293741 then -- finalquestt
            _npcToHail_ghost = "Finalquestt"
        elseif _npcToHail == 42940 then -- nukenurplace
            _npcToHail_ghost = "Nukenurplace"
        elseif _npcToHail == 559528 then -- manoarms
            _npcToHail_ghost = "Manoarmz"
        elseif _npcToHail == 177266 then -- dr killian
            _npcToHail_ghost = "Dr_Killian"
        elseif _npcToHail == 176874 then -- guard perinen
            _npcToHail_ghost = "Guard_Perinen"
        elseif _npcToHail == 176981 then -- merchant ahkham
            _npcToHail_ghost = "Merchant_Ahkham"
        elseif _npcToHail == 553971 then -- arch familiar
            _npcToHail_ghost = "Arch_Familiar"
        elseif _npcToHail == 177181 then -- morana jofranka
            _npcToHail_ghost = "Marona_Jofranka"
        elseif _npcToHail == 177055 then -- bowyer koll
            _npcToHail_ghost = "Bowyer_Koll"
        elseif _npcToHail == 512367 then -- a badger
            _npcToHail_ghost = "a_badger"
        elseif _npcToHail == 513872 then -- guard serenda
            _npcToHail_ghost = "Guard_Serenda"
        elseif _npcToHail == 176985 then -- taylor bariston
            _npcToHail_ghost = "Tailor_Bariston"
        elseif _npcToHail == 177263 then -- angry patron
            _npcToHail_ghost = "Angry_Patron"
        elseif _npcToHail == 177073 then -- signpost
            _npcToHail_ghost = "sign_post"
        elseif _npcToHail == 176991 then -- taylor zixar
            _npcToHail_ghost = "Tailor_Zixar"
        elseif _npcToHail == 176862 then -- coachman ronks
            _npcToHail_ghost = "Coachman_Ronks"
        elseif _npcToHail == 176964 then -- aloj tilsteran
            _npcToHail_ghost = "Aloj_Tilsteran"
        elseif _npcToHail == 176965 then -- royce tilsteran
            _npcToHail_ghost = "Royce_Tilsteran"
        elseif _npcToHail == 341353 then -- guard saolen
            _npcToHail_ghost = "Guard_Saolen"
        elseif _npcToHail == 176975 then -- merchant kari
            _npcToHail_ghost = "Merchant_Kari"
        elseif _npcToHail == -1 then -- no target
            return nil
        else
            print("Unknown ID: " .. _npcToHail)
            return nil
        end
        local i = 0
        while i < Ghosts.NumberofGhosts do
            i = i + 1
            if Ghost[i].nameSafe == _npcToHail_ghost then
                if Ghost[i].myNPC == nil then return nil end
                Ghost[i].myNPC.Interact(_interaction)
            end
        end
    end
    );
},
{
    __index = function (_table, _key)
        return nil;
    end
    }
);
Ghost = {};
Ghosts = setmetatable({
    NumberofGhosts = 0;
    AssignGhosts = (function(_npcsToAssignToArg)
        -- arg is the array, and will cut off when the array ends or when the max ghosts is reached
        -- remove all npcs in excess of ghosts count
        local listCopy = _npcsToAssignToArg
        _npcsToAssignTo = {};
        local i = 1
        while i <= Ghosts.NumberofGhosts do -- for every ghost..
            if listCopy[i] == nil then
                i = Ghosts.NumberofGhosts --if assigning less npcs than ghosts, exit now
            else
                _npcsToAssignTo[i] = listCopy[i]
            end
            i = i + 1
        end
        listCopy = nil;
        -- mark all ghosts as free
        i = 1
        for index,_ghost in pairs(Ghost) do
            _ghost.IsFree = true
        end
        -- mark all npcs as needing ghosts
        i = 1            
        for index,_npc in pairs(_npcsToAssignTo) do -- for every npcs
            _npcsToAssignTo[i].NeedsGhost = true -- mark it as needing ghost
            i = i + 1
        end
        -- mark unfree ghosts as unfree
        -- mark unneeding npcs as unneeding
        i = 1
        while i <= Ghosts.NumberofGhosts do -- for every ghost..
            if _npcsToAssignTo[i] == nil then
                i = Ghosts.NumberofGhosts + 1 -- if assigning less npcs than ghosts, exit now
            elseif _npcsToAssignTo[i].myGhost ~= nil then -- ..for assinging npc, if it has a ghost
                _npcsToAssignTo[i].myGhost.IsFree = false -- mark assinging ghost's current ghost as taken
                _npcsToAssignTo[i].NeedsGhost = false -- mark assinging npc as having ghost already
            end
             i = i + 1
        end
        --get free ghost and assign it [it won't update here]
        i = 1 -- assinging npc index
        while i <= Ghosts.NumberofGhosts do -- do for only every ghost
            if _npcsToAssignTo[i] == nil then -- do for only every npc
                i = Ghosts.NumberofGhosts -- end of npcs marker
            elseif _npcsToAssignTo[i].NeedsGhost == true then -- if assigning npc needs a ghost..
                local i2 = 1 -- ghost index
                while i2 <= Ghosts.NumberofGhosts do -- for every ghost..
                    if  Ghost[i2] == nil then
                        print(" Ghost[i2] is nil : " .. i2)
                    end
                    if Ghost[i2].IsFree == true then -- if it's free..
                        _npcsToAssignTo[i].myGhost = Ghost[i2] -- current npc gets current ghost
                        Ghost[i2].myNPC = _npcsToAssignTo[i]
                        _npcsToAssignTo[i].NeedsGhost = false -- current npc marked as having ghost
                        Ghost[i2].IsFree = false -- current ghost marked as taken
                        i2 = Ghosts.NumberofGhosts
                    end
                    i2 = i2 + 1
                end
            else
                -- npc kept ghost from previous loop and doesn't need a new one
            end
            i = i + 1
        end
        -- remove ghost from old npcs
        while i <= NPCs.NumberofNPCs do
            if _npcsToAssignTo[i] == nil then
                i = NPCs.NumberofNPCs
            else
                if _npcsToAssignTo[i].myGhost ~= nil then
                    if _npcsToAssignTo[i].NeedsGhost == false then
                        _npcsToAssignTo[i].myGhost.myNPC = nil
                        _npcsToAssignTo[i].myGhost = nil
                    end
                end
            end
            i = i + 1
        end
        -- update ghosted npcs
        i = 1
        while i <= Ghosts.NumberofGhosts do
            if _npcsToAssignTo[i] == nil then
                i = Ghosts.NumberofGhosts
            else
                --print("Updating " .. _npcsToAssignTo[i].Name())
                _npcsToAssignTo[i].UpdateGhost()
            end
            i = i + 1
        end
    end
    );
    SetupGhosts = (function()
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
            name[i] = "Guard_Jahn"--
            --i = i + 1
            --name[i] = "Bowyer_Koll" -- did not spawn for me
            i = i + 1
            name[i] = "Tailor_Bariston"
            i = i + 1
            name[i] = "Tailor_Zixar"
            i = i + 1
            name[i] = "Guard_Serenda"--
            --i = i + 1
            --name[i] = "Finalquestt"
            i = i + 1
            name[i] = "Guard_Perinen"--
            --i = i + 1
            --name[i] = "Manoarmz"
            i = i + 1
            name[i] = "Marona_Jofranka"
            i = i + 1
            name[i] = "Merchant_Ahkham"--
            --i = i + 1
            --name[i] = "Nukenurplace"--
            --i = i + 1
            --name[i] = "Raam"
            i = i + 1
            name[i] = "Royce_Tilsteran"
            --i = i + 1
            --name[i] = "Dr_Killian"
            --i = i + 1
            --name[i] = "a_badger"
            --i = i + 1
            --name[i] = "an_undead_mammoth"--
            --i = i + 1
            --name[i] = "Arch_Familiar"
            --i = i + 1
            --name[i] = "Angry_Patron"
            --i = i + 1
            --name[i] = "sign_post"
            i = 1
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
            i = 1
        else
            --co(_sender, "co_debugoutput", "Error: No Ghosts code for the SaveState: " .. SaveState)
            return nil;
        end
        while name[i] ~= nil do
            Ghost[i] = Ghosts.New(name[i])
            i = i + 1
        end
        i = i - 1
        if i == 0 then
            Ghosts.NumberofGhosts = -1
            return 0;
        else
            Ghosts.NumberofGhosts = i
            return i;
        end
    end
    );
    SetupGhosts1 = (function()
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
            name[i] = "Guard_Jahn"--
            --i = i + 1
            --name[i] = "Bowyer_Koll" -- did not spawn for me
            i = i + 1
            name[i] = "Tailor_Bariston"
            i = i + 1
            name[i] = "Tailor_Zixar"
            i = i + 1
            name[i] = "Guard_Serenda"--
            --i = i + 1
            --name[i] = "Finalquestt"
            i = i + 1
            name[i] = "Guard_Perinen"--
            --i = i + 1
            --name[i] = "Manoarmz"
            i = i + 1
            name[i] = "Marona_Jofranka"
            --i = i + 1
            --name[i] = "Merchant_Ahkham"--
            --i = i + 1
            --name[i] = "Nukenurplace"--
            i = i + 1
            name[i] = "Raam"
            i = i + 1
            name[i] = "Royce_Tilsteran"
            --i = i + 1
            --name[i] = "Dr_Killian"
            --i = i + 1
            --name[i] = "a_badger"
            --i = i + 1
            --name[i] = "an_undead_mammoth"--
            --i = i + 1
            --name[i] = "Arch_Familiar"
            --i = i + 1
            --name[i] = "Angry_Patron"
            --i = i + 1
            --name[i] = "sign_post"
            i = 1
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
            i = 1
        else
            --co(_sender, "co_debugoutput", "Error: No Ghosts code for the SaveState: " .. SaveState)
            return nil;
        end
        while name[i] ~= nil do
            Ghost[i] = Ghosts.New(name[i])
            i = i + 1
        end
        i = i - 1
        if i == 0 then
            Ghosts.NumberofGhosts = -1
            return 0;
        else
            Ghosts.NumberofGhosts = i
            return i;
        end
    end
    );
    SetupGhosts1 = (function()
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
            --i = i + 1
            --name[i] = "Guard_Saolen"
            --i = i + 1
            --name[i] = "Guard_Jahn"--
            i = i + 1
            name[i] = "Bowyer_Koll"
            i = i + 1
            name[i] = "Tailor_Bariston"
            i = i + 1
            name[i] = "Tailor_Zixar"
            --i = i + 1
            --name[i] = "Guard_Serenda"--
            --i = i + 1
            --name[i] = "Finalquestt"
            --i = i + 1
            --name[i] = "Guard_Perinen"--
            --i = i + 1
            --name[i] = "Manoarmz"
            i = i + 1
            name[i] = "Marona_Jofranka"
            --i = i + 1
            --name[i] = "Merchant_Ahkham"--
            --i = i + 1
            --name[i] = "Nukenurplace"--
            --i = i + 1
            --name[i] = "Raam"
            --i = i + 1
            --name[i] = "Royce_Tilsteran"
            --i = i + 1
            --name[i] = "Dr_Killian"
            --i = i + 1
            --name[i] = "a_badger"
            --i = i + 1
            --name[i] = "an_undead_mammoth"--
            --i = i + 1
            --name[i] = "Arch_Familiar"
            --i = i + 1
            --name[i] = "Angry_Patron"
            --i = i + 1
            --name[i] = "sign_post"
            i = 1
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
            i = 1
        else
            --co(_sender, "co_debugoutput", "Error: No Ghosts code for the SaveState: " .. SaveState)
            return nil;
        end
        while name[i] ~= nil do
            Ghost[i] = Ghosts.New(name[i])
            i = i + 1
        end
        i = i - 1
        if i == 0 then
            Ghosts.NumberofGhosts = -1
            return 0;
        else
            Ghosts.NumberofGhosts = i
            return i;
        end
    end
    );
    New = (function(_nameSafe, _folderToUse)
        --print("Creating New Ghost")
        if _nameSafe == nil then
            return nil;
        else
            if _folderToUse == nil then 
                fileOfGhost = RF.EQOA.Game_Data.Ghosts.self .. _nameSafe .. Extension_GhostFiles
            else
                fileOfGhost = _folderToUse .. _nameSafe .. Extension_GhostFiles
            end
            if FAPI.IO.File_Exists(fileOfGhost) == false then
                --error file does not exists
                return nil;
            else
                local self = {};
                file = io.open(fileOfGhost,"r+");
                -- Sets This Ghost to point to the Pointers
                local fileversion
                fileversion = FAPI.IO.ReadNextLine(file, "%-%-")
                if fileversion == "0.1.1" then
                    self.myNPC = nil
                    self.nameSafe = FAPI.IO.ReadNextLine(file, "%-%-")
                    self.nameOverhead = FAPI.IO.ReadNextLine(file, "%-%-") 
                    self.nameOverheadColor = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Bytes",self.nameSafe .. ".nameOverheadColor", 12)
                    self.nameTarget = FAPI.IO.ReadNextLine(file, "%-%-") --FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Bytes",self.nameSafe .. ".nameTarget", 24)
                    self.X = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Float",self.nameSafe .. ".X");
                    self.Y = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Float",self.nameSafe .. ".Y");
                    self.Z = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Float",self.nameSafe .. ".Z");
                    self.F = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Float",self.nameSafe .. ".F");
                    self.Level = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Bytes",self.nameSafe .. ".Level", 1);
                    self.HP = FAPI.CE.Address.New(FAPI.IO.ReadNextLine(file, "%-%-"), "Bytes",self.nameSafe .. ".HP", 1);
                    function self.Name(_value)
                        if _value ~= nil then
                            FAPI.CE.IO.Write_StringToBitArrayAddress(_value, self.nameOverhead, 1)
                            FAPI.CE.IO.Write_StringToBitArrayAddress(_value, self.nameTarget, 1)
                        else
                            return FAPI.CE.IO.Read_BitArrayToString(self.nameTarget, "TypeCharacterName")
                        end
                    end
                else
                  --print("Error: Unsupported Ghost Version: " .. fileversion)
                    return nil;
                end
                file:close();
                self.IsFree = true;
                --local npcyellow = {}
                --npcyellow[1] = FAPI.HelperFunctions.HEX_DEC("D5")
                --npcyellow[2] = FAPI.HelperFunctions.HEX_DEC("D4")
                --npcyellow[3] = FAPI.HelperFunctions.HEX_DEC("54")
                --npcyellow[4] = FAPI.HelperFunctions.HEX_DEC("3F")
                --npcyellow[5] = FAPI.HelperFunctions.HEX_DEC("C1")
                --npcyellow[6] = FAPI.HelperFunctions.HEX_DEC("C0")
                --npcyellow[7] = FAPI.HelperFunctions.HEX_DEC("40")
                --npcyellow[8] = FAPI.HelperFunctions.HEX_DEC("3F")
                --npcyellow[9] = FAPI.HelperFunctions.HEX_DEC("DF")
                --npcyellow[10] = FAPI.HelperFunctions.HEX_DEC("DE")
                --npcyellow[11] = FAPI.HelperFunctions.HEX_DEC("DE")
                --npcyellow[12] = FAPI.HelperFunctions.HEX_DEC("3E")
                --print("Yellownaming " .. self.nameSafe)
                --self.nameOverheadColor.Value(npcyellow)
                return self;
            end
        end
    end
    );
},
{
    __index = function (_table, _key) 
        return nil;
    end}
);
