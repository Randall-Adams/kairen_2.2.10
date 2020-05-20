--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: ProcessOutsideCommands
Code-Type: LUA Class
Code-Version: 1.0
Code-Description: NPC Class
Code-Author: Robert Randazzio
]]--
return (
function (_optionToSet, _optionData)
    --_sender = "ProcessOutsideCommands"
    --co(_sender, "co_enter", "")
    if _optionToSet ~= nil then
        if _optionData ~= nil then
            if FAPI.Options.ProcessOutsideCommands == nil then
                FAPI.Options.ProcessOutsideCommands = {};
            else
                if _optionToSet == "TryFileCommands" then
                    if _optionData == true then
                        FAPI.Options.ProcessOutsideCommands.TryFileCommands = true
                    elseif _optionData == false then
                        FAPI.Options.ProcessOutsideCommands.TryFileCommands = false
                    else
                        --error nonboolean value
                    end
                else
                    --error unsupported command
                end
            end
        else
            --error _optionToSet is set but _optionData is not
        end
    elseif _optionToSet == nil then
        local _commandToTry
        _filepath = RF.EQOA.Temp.self .. "Outside Command" .. Extension_Outside_Command
        if FAPI.IO.File_Exists(_filepath) == true then
            --co(_sender, "co_debugoutput", "File Exists: " .. _filepath)
            commandFile = io.open(_filepath, "r+");
            _outsideCommand = FAPI.IO.ReadNextLine(commandFile, "%-%-")
            if _outsideCommand ~= nil then
                _commandToTry = _outsideCommand
                -----
                if _commandToTry == "Spawn_NPC" then
                    _npc = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    print("0| Command: " .. _commandToTry .. "; NPC: " .. _npc)
                    --Spawn_NPC(_npc)
                    _G.NPCs.Spawn(_npc);
                    
                elseif _commandToTry == "Spawn_NPCs_By_Location" then
                    _location = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    --Spawn_NPCs_By_Location(_location)
                    
                elseif _commandToTry == "Spawn Wall Marker" then
                    _number = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    _x = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    _y = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    _z = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    --_facing = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    --NPC_Maker.SpawnWallMarker(_number, _x, _y, _z)--, _facing)
                    
                elseif _commandToTry == "PrintToConsole" then
                    print("0| ProcessOutsideCommands Print Action: " .. FAPI.IO.ReadNextLine(commandFile, "%-%-"))
                    
                elseif _commandToTry == "OutputPlayerData" then
                    local toggle = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    print("0| ProcessOutsideCommands.OutputPlayerData(" .. toggle .. ")")
                    if toggle == "true" then
                        FAPI.RunOptions.OutputPlayerData.Do = true
                    else
                        FAPI.RunOptions.OutputPlayerData.Do = false
                    end
                elseif _commandToTry == "ConnectToKairen" then
                    _pathFAPIData = RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites
                    --print("0| _pathFAPIData == " .. _pathFAPIData)
                    file2 = io.open(_pathFAPIData,"w+");
                    file2:write("ConnectionAccepted");
                    file2:write("\n");       
                    file2:close();
                    FAPI.RunOptions.Alert_OutputPlayerData.Do = true
                    --FAPI.RunOptions.Alert_ProcessOutsideCommands.Do = true
                    print("0| Attempted to Accept Connection..")
                elseif _commandToTry == "CloseCE" then
                    closeCE();
                elseif _commandToTry == "" then
                    --co(_sender, "co_debugoutput", "_commandToTry Is blank")
                end
                --FAPI.RunOptions.Alert_ProcessOutsideCommands.Try()
                -----
            else
                --if no outside command file
            end
            commandFile:close();
            os.remove(_filepath)
        else
            ----co(_sender, "co_debugoutput", "Notice: File Does Not Exist: " .. _filepath)
            --co(_sender, "co_debugoutput", "No Outside Command to Follow.")
        end
    end
end
);
