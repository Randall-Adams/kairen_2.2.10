--co(_sender,"co_debugoutput", "Elapsed Time: " .. elapsed_time)
FAPI.RunOptions.LogicLoopOneTest.Try()
FAPI.RunOptions.TickLoop_OneLogic.Try()
FAPI.RunOptions.TickLoop_ThirtySeconds.Try()

FAPI.RunOptions.NPCTest.Try()
FAPI.RunOptions.SpawnNPCCode.Try()
FAPI.RunOptions.OutputPlayerData.Try()
--FAPI.RunOptions.Alert_ProcessOutsideCommands.Try()
FAPI.RunOptions.ProcessOutsideCommands.Try()

--[[
FAPI.LoopManager.
FAPI.LoopManager.co()
FAPI.EQOA = {};
FAPI.EQOA.NPCs.NPC[i]
FAPI.EQOA.NPCs.NPC[i].Spawn()
FAPI.EQOA.NPCs.Spawn_NPC("")
FAPI.EQOA.NPCs.Spawn_NPCs_By_Location("")
FAPI.EQOA.?.Spawn_Character("")
FAPI.EQOA.Player
FAPI.EQOA.?.outputLocation()
FAPI.Kairen.WriteData()
FAPI.Kairen.ReadData()
FAPI.Kairen.ProcessOutsideCommands()
FAPI.LUA.StringFunctions.Left("")
FAPI.LUA.StringFunctions.Right("")
FAPI.IO.File_Exists("")
FAPI.IO.Read_BitArrayToString("")
FAPI.IO.Read_FileToStringArray("")
FAPI.IO.Write_StringArrayToFile("")
FAPI.IO.Write_StringToBitArrayAddress("")
FAPI.IO.IO_ReadNextLine("")
FAPI.CE.

updateAreas
updateValues
checkUpdated
]]--
