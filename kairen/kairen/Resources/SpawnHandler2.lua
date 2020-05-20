--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: SpawnHandler
Code-Type: LUA Class
Code-Version: 1.0
Code-Description: The code that cohesively spawns NPCs in the game. EG: in camps or the wildlife or bosses.
Code-Author: Robert Randazzio
]]--
-- 001 - _G.SpawnHandler.UpdateEverything
-- 002 - _G.SpawnHandler.GetNextNestIndex
-- 003 - SpawnNests[i].UpdateSpawnCamps()
-- 004 - SpawnNests[].SpawnCamps[i].UpdateSpawnCamps()
-- 005 - SpawnNests[].SpawnCamps[].UpdateSpawnPoints()
-- 006 - SpawnNests[].SpawnCamps[].SpawnPoints[i].SpawnNextNPC()
-- 007 - NPCs.Spawn(self.SpawnList[self.CurrentIndex]) ; self. = SpawnNests[].SpawnCamps[].SpawnPoints[i].
_G.SpawnHandler = {
    NewSpawnPoint = (function(_nameSafe)
      --print("Creating Spawn Point --")
        if _nameSafe == nil then return nil; end;
        if _G.FAPI.IO.File_Exists(RF.EQOA.Custom_Data.Spawn_Points.self .. _nameSafe .. Extension_SpawnPoints) == false then
          --print("Spawn Point Creation Failed -- Spawn Point File doesn't exist.")
            return nil;
        end;
        local self = {};
        self.nameSafe = _nameSafe
        self.FilePath = RF.EQOA.Custom_Data.Spawn_Points.self .. _nameSafe .. Extension_SpawnPoints
        self.SpawnList = {};
        self.SpawnListIndex = 0;
        self.CurrentIndex = 0;
        function self.AddNPC(_npcToAdd)
          --print("Adding NPC (" .. _npcToAdd .. ") to spawn point.")
            self.SpawnListIndex = self.SpawnListIndex + 1
          --print("SpawnListIndex == " .. self.SpawnListIndex)
            self.SpawnList[self.SpawnListIndex] = _npcToAdd
          --print("SpawnList[" .. self.SpawnListIndex .. "] == " .. self.SpawnList[self.SpawnListIndex])
            if self.CurrentIndex == 0 then self.CurrentIndex = 1 end
        end
        function self.SpawnNextNPC()
          --print("Spawning Next NPC --")
            if self.CurrentIndex == 0 then
                --error no npcs added yet
              --print("Spawning Next NPC Failed -- No NPCs in spawn point.")
                return nil;
            else
              --print("NPCs detected in spawn point.")
                --for key,value in pairs(self.SpawnList) do
                --  --print(key .. ": " .. value)
                --end
              --print("CurrentIndex == " .. self.CurrentIndex .. " / SpawnListIndex == " .. self.SpawnListIndex)
                if self.CurrentIndex > self.SpawnListIndex then self.CurrentIndex = 1 end
              --print("pre spawn npc line")
                NPCs.Spawn(self.SpawnList[self.CurrentIndex])
              --print("post spawn npc line")
              --print("Looking for: " .. self.SpawnList[self.CurrentIndex])
                if NPCs.Find("nameSafe", self.SpawnList[self.CurrentIndex]) ~= nil then
                  --print("Spawning Next NPC -- NPC detected to have spawned somewhere.")
                else
                  --print("Spawning Next NPC -- NPC not detected to have spawned.")
                end
                self.CurrentIndex = self.CurrentIndex + 1
            end
        end
        function self.SpawnAllNPCs()
            if self.CurrentIndex == 0 then
                --error no npcs added yet
                return nil;
            else
                if self.CurrentIndex > self.SpawnListIndex then self.CurrentIndex = 1; end;
                NPCs.Spawn(self.SpawnList[self.CurrentIndex])
                self.CurrentIndex = self.CurrentIndex + 1
                for key,value in pairs(self.SpawnList) do
                    _G.NPCs.Spawn(value)
                end
            end
        end
        function self.RemoveNPC(_npcToRemove)
            --error no npcs added
            if self.SpawnListIndex == 0 then return nil; end
            local i = 1
            while i <= self.SpawnListIndex and continue == true do
                if self.SpawnList[i] == _npcToRemove.nameSafe then
                    --don't add npc flag
                    continue = false
                    self.SpawnListIndex = self.SpawnListIndex - 1
                    if i < self.CurrentIndex then 
                        self.CurrentIndex = self.CurrentIndex - 1
                    end
                else
                    i = i + 1
                end
            end                
            local i2 = 1
            while i2 <= self.SpawnListIndex do
                if i2 ~= i then
                    if i2 < i then
                        SpawnList2[i2] = self.SpawnList[i2]
                    elseif i2 > i then
                        SpawnList2[i2 - 1] = self.SpawnList[i2]
                    end
                end
                i2 = i2 + 1
            end
            self.SpawnList = SpawnList2
        end
        function self.ApplyFileData()
          --print("Applying spawn point File Data -- ")
            local spawnPointFile = io.open(self.FilePath,"r+"); -- This is checked for existence above..
            local FileVersion = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
          --print("Applying Data -- Spawn Point File Version == " .. FileVersion)
            if FileVersion == "1.0" then
              --print("Applying Data -- Loading File")
                self.nameSafe = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                self.Xmin = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                self.Xmax = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                self.Ymin = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                self.Ymax = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                self.Zmin = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                self.Zmax = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                npcline = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                while npcline ~= nil do
                  --print("Applying Data -- Trying to add NPC to spawn point.")
                    self.AddNPC(npcline)
                    npcline = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                end
            elseif FileVersion == "1.1" then
              --print("Applying Data -- Loading File")
                self.nameSafe = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                npcline = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                if npcline == nil then
                  --print("Applying Data -- First NPC Line is nil")
                end
                while npcline ~= nil do
                  --print("Trying to add NPC to spawn point.")
                    self.AddNPC(npcline)
                    npcline = FAPI.IO.ReadNextLine(spawnPointFile, "%-%-")
                end
            else
              --print("Applying Data Failed -- Unrecognized Spawn Points file version.")
                --error file version not recognized
            end
            spawnPointFile:close();
        end
        self.ApplyFileData()
      --print("Spawn Point Created -- Passing self so it can be set to a variable")
        return self;
    end
    );
    NewSpawnCamp = (function(_nameSafe)
    --print("Creating Spawn Camp --")
        if _nameSafe == nil then return nil; end;
        if _G.FAPI.IO.File_Exists(RF.EQOA.Custom_Data.Spawn_Camps.self .. _nameSafe .. Extension_SpawnCamps) == false then
        --print("Creating Spawn Camp Faiiled -- Spawn Camp file doesn't exist.")
            return nil;
        end
        local self = {};
        self.nameSafe = _nameSafe
        self.FilePath = RF.EQOA.Custom_Data.Spawn_Camps.self .. _nameSafe .. Extension_SpawnCamps
        self.SpawnPoints = {};
        self.SpawnPointsIndex = 0;

        function self.UpdateSpawnPoints()
        --print("Updating Spawn Points --")
            for key,value in pairs(self.SpawnPoints) do
              --print(key .. ": " .. value.nameSafe)
                value.SpawnNextNPC()
            end
        end
        function self.AddSpawnPoint(_nameSafe)
        --print("Adding Spawn Point --")
            if _nameSafe == nil then return nil; end;
            local tempSpawnPoint = _G.SpawnHandler.NewSpawnPoint(_nameSafe)
            if tempSpawnPoint == nil then return nil; end;
          --print("Adding Spawn Point: " .. _nameSafe .. " To Spawn Camp: " .. self.nameSafe)
            self.SpawnPointsIndex = self.SpawnPointsIndex + 1
            self.SpawnPoints[self.SpawnPointsIndex] = tempSpawnPoint
        end
        function self.RemoveSpawnPoint(_spawnpointToRemove)
            --error no spawn points added
            if self.SpawnPointsIndex == 0 then return nil; end
            local i = 1
            while i <= self.SpawnPointsIndex and continue == true do
                if self.SpawnPoints[i] == _spawnpointToRemove.nameSafe then
                    --don't add spawn point flag
                    continue = false
                    self.SpawnPointsIndex = self.SpawnPointsIndex - 1
                else
                    i = i + 1
                end
            end                
            local i2 = 1
            while i2 <= self.SpawnPointsIndex do
                if i2 ~= i then
                    if i2 < i then
                        SpawnPoints2[i2] = self.SpawnPoints[i2]
                    elseif i2 > i then
                        SpawnPoints2[i2 - 1] = self.SpawnPoints[i2]
                    end
                end
                i2 = i2 + 1
            end
            self.SpawnPoints = SpawnPoints2
        end        
        function self.ApplyFileData()
        --print("Applying spawn camp File Data --")
            --co(_sender, "co_debugoutput", "Setting Spawn Camp Data..")
            local campFile = io.open(self.FilePath,"r+"); -- This is checked for existence above..
            local cs = FAPI.CommentIndicatorString
            --local rnl = FAPI.IO.ReadNextLine
            local FileVersion = FAPI.IO.ReadNextLine(campFile, cs)
            --co(_sender, "co_debugoutput", "File Version is: " .. FileVersion)
        --print("Applying Data -- Spawn Camp FileVersion == " .. FileVersion)
            if FileVersion == "0.1.0" then
            --print("Applying Data -- Loading File")
                self.nameSafe = FAPI.IO.ReadNextLine(campFile, cs)
                self.nameGame = FAPI.IO.ReadNextLine(campFile, cs)
                local nextLine = FAPI.IO.ReadNextLine(campFile, cs)
                while nextLine ~= nil do
                    self.AddSpawnPoint(nextLine)
                    nextLine = FAPI.IO.ReadNextLine(campFile, cs)
                end
            else
            --print("Applying Data Failed -- Unrecognized File Version")
                -- error unrecognized version
            end
            campFile:close();
        end
        self.ApplyFileData();
    --print("Spawn Camp Created -- Passing self so it can be set to a variable")
        return self;
    end
    );
    NewSpawnNest = (function(_filepath)
    --print("Creating Spawn Nest --")
      --print("  " .. _filepath)
        if _filepath == nil then return nil; end;
        if _G.FAPI.IO.File_Exists(_filepath) == false then
        --print("Creating Nest Failed -- Nest File Not Found")
            return nil;
        end
        local self = {};
        self.FilePath = _filepath
        self.SpawnCamps = {};
        self.SpawnCampsIndex = 0;
        function self.UpdateSpawnCamps()
        --print("Updating Spawn Camps --")
            for key,value in pairs(self.SpawnCamps) do
            --print(key .. ": " .. value.nameSafe)
                value.UpdateSpawnPoints()
            end
        end
        function self.AddSpawnCamp(_nameSafe)
        --print("Adding Spawn Camp --")
            if _nameSafe == nil then return nil; end;
            local tempSpawnCamp = _G.SpawnHandler.NewSpawnCamp(_nameSafe)
            if tempSpawnCamp == nil then return nil; end;
          --print("Adding Spawn Camp: " .. _nameSafe .. " To Spawn Nest: " .. self.nameSafe)
            self.SpawnCampsIndex = self.SpawnCampsIndex + 1
            self.SpawnCamps[self.SpawnCampsIndex] = tempSpawnCamp
        end
        function self.RemoveSpawnCamp(_spawncampToRemove) -- not updated --
            --error no spawn points added
            if self.SpawnPointsIndex == 0 then return nil; end
            local i = 1
            while i <= self.SpawnPointsIndex and continue == true do
                if self.SpawnPoints[i] == _spawnpointToRemove.nameSafe then
                    --don't add spawn point flag
                    continue = false
                    self.SpawnPointsIndex = self.SpawnPointsIndex - 1
                else
                    i = i + 1
                end
            end                
            local i2 = 1
            while i2 <= self.SpawnPointsIndex do
                if i2 ~= i then
                    if i2 < i then
                        SpawnPoints2[i2] = self.SpawnPoints[i2]
                    elseif i2 > i then
                        SpawnPoints2[i2 - 1] = self.SpawnPoints[i2]
                    end
                end
                i2 = i2 + 1
            end
            self.SpawnPoints = SpawnPoints2
        end        
        function self.ApplyFileData()
        --print("Applying spawn nest File Data")
            --co(_sender, "co_debugoutput", "Setting Spawn Nest Data..")
          --print("  " .. self.FilePath)
            local nestFile = io.open(self.FilePath,"r+"); -- This is checked for existence above..
            local cs = FAPI.CommentIndicatorString
            --local rnl = FAPI.IO.ReadNextLine
            local FileVersion = FAPI.IO.ReadNextLine(nestFile, cs)
            --co(_sender, "co_debugoutput", "File Version is: " .. FileVersion)
        --print("Applying Data -- Spawn Nest FileVersion == " .. FileVersion)
            if FileVersion == "0.1.0" then
            --print("Applying Data -- Loading File")
                self.nameSafe = FAPI.IO.ReadNextLine(nestFile, cs)
                self.nameGame = FAPI.IO.ReadNextLine(nestFile, cs)
                local nextLine = FAPI.IO.ReadNextLine(nestFile, cs)
                while nextLine ~= nil do
                    self.AddSpawnCamp(nextLine)
                    nextLine = FAPI.IO.ReadNextLine(nestFile, cs)
                end
            else
            --print("Applying Data Failed -- Unrecognized SpawnNest File Version")
                -- error unrecognized version
            end
            nestFile:close();
        end
        self.ApplyFileData();
    --print("Spawn Nest Created -- Passing self so it can be set to a variable")
        return self;
    end
    );
    SpawnNests = {};
    SpawnNestEmptyMarker = {};
    SpawnNestIndex = 0;
    GetSpawnNestCoord = (function(_gameWorldCoord)
        return ((math.floor(_gameWorldCoord / 500)) * 500);
    end
    );
    UpdateSpawnNests = (function() -- Optionally Returns if the passed in nest is spawned or not; boolean
    --print("Updating Spawn Nests --")
        local _myX = _G.SpawnHandler.GetSpawnNestCoord(_G.EQOA.Player.Location.X.Value())
        local _myY = _G.SpawnHandler.GetSpawnNestCoord(_G.EQOA.Player.Location.Y.Value())
        local _filepath = RF.EQOA.Custom_Data.Spawn_Nests.self .. _myX .. "/" .. _myY .. Extension_SpawnNests
        local i = 1
        local _currentNestIsSpawned = false
        if _G.SpawnHandler.SpawnNestIndex == 0 then
        --print("Updating Spawn Nests -- No Spawn Nests Created yet")
        --print("Updating Spawn Nests -- The Current Nest should try to spawn next")
        else
        --print("Updating Spawn Nests -- Spawn Nests already spawned, updating them.")
            while i <= _G.SpawnHandler.SpawnNestIndex do
                if _G.SpawnHandler.SpawnNestEmptyMarker[i] == 1 then
                --print("Updating Spawn Nests -- Updating Spawn Nest #" .. i)
                    _G.SpawnHandler.SpawnNests[i].UpdateSpawnCamps()
                end
                if _G.SpawnHandler.SpawnNests[i] ~= nil and _G.SpawnHandler.SpawnNests[i].FilePath == _filepath then
                --print("Updating Spawn Nests -- Current Nest Detected as spawned, marking.")
                    _currentNestIsSpawned = true
                end
                i = i + 1
            end
        end
        
        -- Spawn Current Nest if not marked as spawned
        if _currentNestIsSpawned == false then
        --print("Updating Spawn Nests -- Attempting to spawn current nest")
            if _G.FAPI.IO.File_Exists(_filepath) == true then
            --print("Updating Spawn Nests -- Current Spawn Nest file found, spawning..")
                i = _G.SpawnHandler.GetNextNestIndex() --disabled for release, so that only 1 nest will be spawned
              --print("Next Nest Index: " .. i)
              --print("Assigning Spawn Nest " .. _filepath .. " to index " .. i .. ".")
                _G.SpawnHandler.SpawnNests[i] = _G.SpawnHandler.NewSpawnNest(_filepath)
                _G.SpawnHandler.SpawnNests[i].UpdateSpawnCamps();
                _G.SpawnHandler.SpawnNestIndex = _G.SpawnHandler.SpawnNestIndex - 1 -- spoof
            else
            --print("Updating Spawn Nests -- Spawn Nest (" .. _myX .. ", " .. _myY .. ") attempted to Spawn but was not found.")
              --print("(" .. _filepath .. ")")
            end
        else
        --print("Updating Spawn Nests --  Current Nest was already detected so it was not spawned again")
        end
    end
    );
    GetNextNestIndex = (function()
    --print("Getting Next Nest Index")
        for key,value in pairs(_G.SpawnHandler.SpawnNestEmptyMarker) do
            if value == 0 then
            --print("Getting Next Nest Index -- Found Unused Nest Index: " .. key)
                value = 1 -- mark this index as not empty
                return value;
            else
              --print("Getting Next Nest Index -- Found Used Nest Index: " .. key)
            end
        end
    --print("Getting Next Nest Index -- Creating Next Nest Index")
        _G.SpawnHandler.SpawnNestIndex = _G.SpawnHandler.SpawnNestIndex + 1
        _G.SpawnHandler.SpawnNestEmptyMarker[_G.SpawnHandler.SpawnNestIndex] = 1
        return _G.SpawnHandler.SpawnNestIndex;
    end
    );
};
