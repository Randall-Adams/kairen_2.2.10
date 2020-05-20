--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: ProcessOutsideCommands
Code-Type: LUA Class
Code-Version: 1.1
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
        local _filepath = RF.EQOA.Temp.self .. "Outside Command" .. Extension_Outside_Command
        if FAPI.IO.File_Exists(_filepath) == true then
            --co(_sender, "co_debugoutput", "File Exists: " .. _filepath)
            commandFile = io.open(_filepath, "r+");
            _outsideCommand = FAPI.IO.ReadNextLine(commandFile, _G.FAPI.CommentIndicatorString)
            if _outsideCommand ~= nil then
                _commandToTry = _outsideCommand
                -----
                if _commandToTry == "Spawn_NPC" then
                    _npc = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    print("Command: " .. _commandToTry .. " -- NPC: " .. _npc)
                    _G.NPCs.Spawn(_npc);

                elseif _commandToTry == "PrintToConsole" then
                    print("ProcessOutsideCommands Print Action: " .. FAPI.IO.ReadNextLine(commandFile, "%-%-"))

                elseif _commandToTry == "OutputPlayerData" then
                    local toggle = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    print("ProcessOutsideCommands.OutputPlayerData(" .. toggle .. ")")
                    print("ALERT: This method of toggling the option is Deprycated and should be updated.")
                    if toggle == "true" then
                        FAPI.RunOptions.OutputPlayerData.Do = true
                    else
                        FAPI.RunOptions.OutputPlayerData.Do = false
                    end

                elseif _commandToTry == "ConnectToKairen" then
                    --this needs moved into a class
                    --probably the kanizah class
                    _pathFAPIData = RF.EQOA.Net_Streams.o.self .. "FAPI Data" .. Extension_ReadWrites
                    --print("_pathFAPIData == " .. _pathFAPIData)
                    file2 = io.open(_pathFAPIData,"w+");
                    file2:write("ConnectionAccepted");
                    file2:write("\n");       
                    file2:close();
                    FAPI.RunOptions.Alert_OutputPlayerData.Do = true
                    --FAPI.RunOptions.Alert_ProcessOutsideCommands.Do = true
                    print("Attempted to Accept Connection..")
                    
                elseif _commandToTry == "ToggleOption" then
                    local OptionToToggle = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    local OptionToToggle_ToggleMode = FAPI.IO.ReadNextLine(commandFile, "%-%-")
                    print("ProcessOutsideCommands.ToggleOption." .. OptionToToggle .. " = " .. OptionToToggle_ToggleMode)
                    --this needs put into runoptions and then runoptions needs a togglefunction so 
                    -- it's not rewritten longhand a thousand times
                    --next add the option "Alert_ProcessOutsideCommands"
                    --[re]add the option to display your location to the lua console
                    if OptionToToggle == "OutputPlayerData" then
                        if OptionToToggle_ToggleMode == "true" then
                            FAPI.RunOptions.OutputPlayerData.Do = true
                        elseif OptionToToggle_ToggleMode == "false" then
                            FAPI.RunOptions.OutputPlayerData.Do = false
                        elseif OptionToToggle_ToggleMode == "toggle" then
                            if FAPI.RunOptions.OutputPlayerData.Do == true then
                                FAPI.RunOptions.OutputPlayerData.Do = false
                            else
                                FAPI.RunOptions.OutputPlayerData.Do = true
                            end
                        else
                            -- error unrecognized toggle mode selection
                        end
                    else
                        -- error unrecognized option
                    end

                elseif _commandToTry == "CloseCE" then
                    -- this command is special and does things not normally done
                    commandFile:close();
                    GameLoop.LogicLoop.ClearOldFiles();
                    closeCE();
                
                elseif _commandToTry == "Create Address List" then
                    _G.AddressLists.NewList(FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"))
                
                elseif _commandToTry == "Output Address List" then
                    _G.AddressLists.OutputAddressList_ByName(FAPI.IO.ReadNextLine(commandFile, "%-%-"))
                
                elseif _commandToTry == "Add Address To List" then
                    _G.AddressLists.AddAddressToList_ByListName(FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"), FAPI.IO.ReadNextLine(commandFile, "%-%-"))
                
                elseif _commandToTry == "DevModeCommand" then
                    DevModeCommand(commandFile);
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
