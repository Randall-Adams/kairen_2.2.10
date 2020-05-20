print("Loading Settings")
GameLoop.et.check("SETUP", mode, 1000)

-- Process Outside Commands
FAPI.RunOptions.ProcessOutsideCommands = FAPI.RunOptions.New(FAPI.Kanizah.ProcessOutsideCommands, true)
FAPI.RunOptions.Alert_ProcessOutsideCommands = FAPI.RunOptions.New(FAPI.Kanizah.Alert_ProcessOutsideCommands, false)

-- Output PlayerData
FAPI.RunOptions.OutputPlayerData = FAPI.RunOptions.New(_G.FAPI.Kanizah.OutputPlayerData, false, "OutputPlayerData", 1000)
FAPI.RunOptions.Alert_OutputPlayerData = FAPI.RunOptions.New(FAPI.Kanizah.Alert_OutputPlayerData, false)
FAPI.RunOptions.Alert_OutputPlayerData_ConsoleOutput = FAPI.RunOptions.New(FAPI.Kanizah.Alert_OutputPlayerData_ConsoleOutput, false)

-- Handle Automatic NPC Spawning
FAPI.RunOptions.UpdateSpawnNests = FAPI.RunOptions.New(_G.SpawnHandler.UpdateSpawnNests, false, "UpdateSpawnNests", 1000)

-- New changes: New Kanizah code that has not yet been placed properly.
dofile(RF.EQOA.LUAs.Modules.FAPI.self .. "Classes/Kanizah.lua")
_G.Kanizah.AddOutputByDefinition("MyX");
_G.Kanizah.AddOutputByDefinition("MyY");
_G.Kanizah.AddOutputByDefinition("MyZ");
_G.Kanizah.AddOutputByDefinition("MyF");
_G.Kanizah.AddOutputByDefinition("MyZone");
_G.Kanizah.AddOutputByDefinition("MyNestX");
_G.Kanizah.AddOutputByDefinition("MyNestY");
_G.Kanizah.AddOutputByDefinition("MyRow");
_G.Kanizah.AddOutputByDefinition("MyColumn");
local i = 1
local imax = 50
while i <= imax do
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].Name(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "Name"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].X(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "X"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].Y(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "Y"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].Z(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "Z"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].F(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "F"
    i = i + 1
end
i = nil
imax = nil
-- New Way to "Output FAPI Data"
_G.FAPI.RunOptions.UpdateKanizah = FAPI.RunOptions.New(_G.Kanizah.UpdateOutput, true, "UpdateKanizah", 1000);

-- Handle Showing NPCs Graphically
_G.FAPI.RunOptions.UpdateVisualNPCs = FAPI.RunOptions.New(_G.NPCs.UpdateVisualNPCs, true, "UpdateVisualNPCs", 100);
print("All Settings Loaded.")
