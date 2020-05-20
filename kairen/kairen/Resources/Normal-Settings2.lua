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
FAPI.RunOptions.UpdateSpawnNests = FAPI.RunOptions.New(_G.SpawnHandler.UpdateSpawnNests, true, "UpdateSpawnNests", 1000)

print("All Settings Loaded.")
