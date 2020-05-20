--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Kanizah
Code-Type: LUA Class
Code-Version: 1.2
Code-Description: Outputs the player's data to Kairen.
Code-Author: Robert Randazzio
]]--
_G.Kanizah = {
    Update = (function()
        _G.Kanizah.ProcessCommandFile()
        _G.Kanizah.UpdateOutput()
        return nil;
    end
    );
    index = 0;
    GetIndex = (function()
        _G.KanizahList.index = _G.KanizahList.index + 1
        _G.KanizahList.UsedMarker[_G.KanizahList.index] = 1
        return _G.KanizahList.index;
    end
    );
    ProcessCommandFile = (function()
        local _FAPIDataFilePath = RF.EQOA.Net_Streams.i.self .. "FAPI Data Request" .. Extension_ReadWrites
        if FAPI.IO.File_Exists(_FAPIDataFilePath) then
            local cs = _G.FAPI.CommentIndicatorString
            --print(_FAPIDataFilePath)
            local FDi = io.open(_FAPIDataFilePath,"r+")
            local nl
            nl = FAPI.IO.ReadNextLine(FDi, cs)
            while nl ~= nil do
                if nl ~= "" then
                    -- Update requests are for constant updates, Output requests are for one-time updates
                    --print("FAPI Data line: " .. nl)
                    if nl == "AddOutputByAddressName" then
                        --print("Kanizah.AddOutputByAddressName()")
                        --_G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi, cs)
                        --tells fapi to output an address from AddressList[], where the .Name is the additional data
                        Kanizah.AddOutputByAddressName(FAPI.IO.ReadNextLine(FDi, cs))
                    --elseif nl == "AddOutputByVariableName" then
                    --    print("Kanizah.AddOutputByVariableName()")
                    --    --_G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi, cs)
                    --    Kanizah.AddOutputByVariableName(FAPI.IO.ReadNextLine(FDi, cs))

                    elseif nl == "UpdateVariableByVariableName" then
                        --print("Kanizah.UpdateVariableByVariableName()")
                        --_G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi, cs)
                        Kanizah.UpdateVariableByVariableName(FAPI.IO.ReadNextLine(FDi, cs), FAPI.IO.ReadNextLine(FDi, cs))
                        
                    elseif nl == "OutputAddressValueByAddressName" then
                        --print("Kanizah.OutputAddressValueByAddressName()")
                        --_G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi, cs)
                        Kanizah.OutputAddressValueByAddressName(FAPI.IO.ReadNextLine(FDi, cs))

                    elseif nl == "PrintToConsole" then
                        print("PrintToConsole Message: " .. FAPI.IO.ReadNextLine(commandFile, "%-%-"))

                    elseif nl == "UpdateAddressByAddressName" then
                        --print("Kanizah.UpdateAddressByAddressName()")
                        --_G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi, cs
                        local val1 = FAPI.IO.ReadNextLine(FDi, cs)
                        local val2 = FAPI.IO.ReadNextLine(FDi, cs)
                        if val1 ~= nil then
                            if val2 ~= nil then
                                --print("Kanizah.UpdateAddressByAddressName(" .. val1 .. ", " .. val2 .. ")")
                            end
                        end
                        Kanizah.UpdateAddressByAddressName(val1, val2)

                    elseif nl == "TurnHailsOff" then
                        FAPI.RunOptions.SquarePressCheck.Do = false
                        print("Turning Hails off.")
                        
                    else
                        print("Kanizah.ProcessCommandFile(): Invalid Command Issued")
                        FDi:close();
                        os.remove(_FAPIDataFilePath);
                        return -1
                    end
                    --if type(nl) == "table" then
                    --    local _table, _key = GetTableAndKey(nl)
                    --    _table[key]
                    --end
                end
                nl = FAPI.IO.ReadNextLine(FDi, cs)
            end
            FDi:close();
            os.remove(_FAPIDataFilePath);
        else
            --print("No FAPI Data File")
        end
    end
    );
    CurrentIndex = 0;
    OutputArray = {};
    AdditionalDataArray = {};
    OutputArray_OneTime = {};
    AdditionalDataArray_OneTime = {};
    GetFreeIndex = (function()
        local i = 1
        while OutputArray[i] ~= nil do
            if OutputArray[i] == 1 then
                return i;
            end
            i = i + 1
        end
        return i;
    end
    );
    RemoveItemByAdditionalData = (function(valueToRemove)
        for key,value in pairs(_G.Kanizah.AdditionalDataArray) do
            if value == valueToRemove then
                _G.Kanizah.OutputArray[i] = nil
                _G.Kanizah.AdditionalDataArray[i] = nil
            end
        end
    end
    );
    UpdateItemByAdditionalData = (function(valueToUpdate, newOutputMethod)
        for key,value in pairs(_G.Kanizah.AdditionalDataArray) do
            if value == valueToUpdate then
                _G.Kanizah.OutputArray[key] = newOutputMethod
            end
        end
    end
    );
    UpdateOutput = (function()
        local exitmarker = true
        if _G.Kanizah.OutputArray ~= nil then exitmarker = false; end
        if _G.Kanizah.OutputArray_OneTime ~= nil then exitmarker = false; end
        if exitmarker == true then return -1; end
        local _FAPIDataFilePath = RF.EQOA.Net_Streams.o.self .. "FAPI Data2" .. Extension_ReadWrites
        --print("making fd2: " .. RF.EQOA.Net_Streams.o.self .. "FAPI Data2" .. Extension_ReadWrites)
        local cs = _G.FAPI.CommentIndicatorString
        --print(_FAPIDataFilePath)
        local updateCurrent = true
        local FDo = io.open(_FAPIDataFilePath,"w+")
        if FDo ~= nil then
            if _G.Kanizah.OutputArray ~= nil then
                for key,value in pairs(_G.Kanizah.OutputArray) do
                    if value ~= nil then
                        --print("key: " .. key .. " / value: " .. value)
                        FDo:write(value());
                        FDo:write("\n");
                        FDo:write(_G.Kanizah.AdditionalDataArray[key]);
                        FDo:write("\n");
                    end
                end
            end
            if _G.Kanizah.OutputArray_OneTime ~= nil then
                for key,value in pairs(_G.Kanizah.OutputArray_OneTime) do
                    if value ~= nil then
                        --print("key: " .. key .. " / value: " .. value)
                        FDo:write(value());
                        FDo:write("\n");
                        FDo:write(_G.Kanizah.AdditionalDataArray_OneTime[key]);
                        FDo:write("\n");
                    end
                end
            _G.Kanizah.OutputArray_OneTime = {};
            _G.Kanizah.AdditionalDataArray_OneTime = {};
            end
        end
        FDo:close();
    end
    );
    OutputList = {};
    OutputListAdditionalData = {};
    UpdateByOutputList = (function()
        local cs = _G.FAPI.CommentIndicatorString
        local _lFdo
        for key1,value1_list in pairs(OutputList) do -- this doesn't reference the output list in kanizah..
            for key2_list,value2 in pairs(value1_list) do
                if key2 == 1 then
                    _lFdo = io.open(value2,"w+")
                else
                        FDo:write(value2());
                        FDo:write("\n");
                        FDo:write(_G.Kanizah.OutputListAdditionalData.key1.key2_list);
                        FDo:write("\n");
                end
                _lFdo:close();
            end
        end
    
        local _FAPIDataFilePath = RF.EQOA.Net_Streams.o.self .. "FAPI Data2" .. Extension_ReadWrites
        --print("making fd2: " .. RF.EQOA.Net_Streams.o.self .. "FAPI Data2" .. Extension_ReadWrites)
        local cs = _G.FAPI.CommentIndicatorString
        --print(_FAPIDataFilePath)
        local updateCurrent = true
        local FDo = io.open(_FAPIDataFilePath,"w+")
        for key,value in pairs(_G.Kanizah.OutputArray) do
            --print("key: " .. key .. " / value: " .. value)
            FDo:write(value());
            FDo:write("\n");
            FDo:write(_G.Kanizah.AdditionalDataArray[key]);
            FDo:write("\n");
        end
        FDo:close();
    end
    );
    AddOutputByAddressName = (function(_addressName)
        if _addressName == nil then
            _addressName = ""
        end
        if AddressList == nil then
            return nil;
        else
            if AddressList[_addressName] == nil then
                print("Address.lua:AddOutputByAddressName -- Address [" .. _addressName .. "] does not exist.")
                return nil;
            else
                if Kanizah.CheckOutputStatusByAdditionalData(_addressName) then return nil; end;
                Kanizah.CurrentIndex = Kanizah.CurrentIndex + 1
                --print("Address.lua AddOutputByAddressName._addressName: " .. _addressName)
                _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() return AddressList[_addressName].Value(); end);
                _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = _addressName
                
            end
        end
    end
    );
    AddOutputByVariableName = (function(_variableName)
        if _variableName == nil then
            return nil;
        else
        if Kanizah.CheckOutputStatusByAdditionalData(_variableName) then return nil; end;
            for key,value in pairs(_G) do
                if key == _variableName then
                    if value ~= nil then
                        Kanizah.CurrentIndex = Kanizah.CurrentIndex + 1
                        --print("Address.lua AddOutputByVariableName._variableName: " .. _variableName)
                        _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() return value; end);
                        _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = _variableName
                    else
                        -- value is nil, exit
                        --print("Address.lua:AddOutputByVariableName -- _G. " .. _variableName .. " does not exist.")
                        return nil;
                    end
                end
            end
        end
    end
    );
    AddOutputByFunctionReference = (function(_referenceName, _reference)
        if Kanizah.CheckOutputStatusByAdditionalData(_referenceName) then return nil; end;
        Kanizah.CurrentIndex = Kanizah.CurrentIndex + 1
        _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = _reference;
        _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = _referenceName;
    end
    );
    AddOutputByDefinition = (function(_definition)
        _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
        --print("Adding definition: " .. _definition)
        if _definition == "MyZone" then
            _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() return EQOA.Player.Location.Zone(); end);
            _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "MyZone"
        elseif _definition == "MyRow" then
            _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() 
                local _rowcolumn
                _rowcolumn = EQOA.HelperFunctions.GetZoneRowColumn_ByCoords(EQOA.Player.Location.X.Value(), EQOA.Player.Location.Y.Value())
                return _rowcolumn[1];
            end
            );
            _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "MyRow"
        elseif _definition == "MyColumn" then
            _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() 
                local _rowcolumn
                _rowcolumn = EQOA.HelperFunctions.GetZoneRowColumn_ByCoords(EQOA.Player.Location.X.Value(), EQOA.Player.Location.Y.Value())
                return _rowcolumn[2];
            end
            );
            _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "MyColumn"
        elseif _definition == "MyNestX" then
            _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() return _G.SpawnHandler.GetSpawnNestCoord(_G.EQOA.Player.Location.X.Value()); end);
            _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "MyNestX"
        elseif _definition == "MyNestY" then
            _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() return _G.SpawnHandler.GetSpawnNestCoord(_G.EQOA.Player.Location.Y.Value()); end);
            _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "MyNestY"
        elseif _definition == "ChatValues" then
            Kanizah.AddOutputByAddressName("Chat_2-1")
            Kanizah.AddOutputByAddressName("Chat_3-2")
            Kanizah.AddOutputByAddressName("Chat_4-3")
            Kanizah.AddOutputByAddressName("Chat_5-4")
            Kanizah.AddOutputByAddressName("Chat_6-5")
            Kanizah.AddOutputByAddressName("Chat_7-6")
            Kanizah.AddOutputByAddressName("Chat_8-7")
            Kanizah.AddOutputByAddressName("Chat_9-8")
            Kanizah.AddOutputByAddressName("Chat_10-9")
            Kanizah.AddOutputByAddressName("Chat_11-10")
            Kanizah.AddOutputByAddressName("Chat_12-11")
            Kanizah.AddOutputByAddressName("Chat_13-12")
            Kanizah.AddOutputByAddressName("Chat_14-13")
            Kanizah.AddOutputByAddressName("Chat_15-14")
            Kanizah.AddOutputByAddressName("Chat_16-15")
            Kanizah.AddOutputByAddressName("Chat_17-16")
            Kanizah.AddOutputByAddressName("Chat_18-17")
            Kanizah.AddOutputByAddressName("Chat_19-18")
            Kanizah.AddOutputByAddressName("Chat_20-19")
            Kanizah.AddOutputByAddressName("Chat_21-20")
            Kanizah.AddOutputByAddressName("Chat_22-21")
            Kanizah.AddOutputByAddressName("Chat_23-22")
            Kanizah.AddOutputByAddressName("Chat_24-23")
            Kanizah.AddOutputByAddressName("Chat_25-24")
            Kanizah.AddOutputByAddressName("Chat_26-25")
            Kanizah.AddOutputByAddressName("Chat_27-26")
            Kanizah.AddOutputByAddressName("Chat_28-27")
            Kanizah.AddOutputByAddressName("Chat_29-28")
            Kanizah.AddOutputByAddressName("Chat_30-29")
            Kanizah.AddOutputByAddressName("Chat_31-30")
            Kanizah.AddOutputByAddressName("Chat_0-31")
            Kanizah.AddOutputByAddressName("Chat_1-0")
            Kanizah.AddOutputByAddressName("SlotInd1")
            Kanizah.AddOutputByAddressName("SlotInd2")
            Kanizah.AddOutputByAddressName("OpenChatBox")
            Kanizah.AddOutputByAddressName("ChatIsOpen1")
            Kanizah.AddOutputByAddressName("ChatIsOpen2")
            else

        end
    end
    );
    OutputAddressValueByAddressName = (function(_addressName)
        if AddressList == nil then
            return nil;
        else
            if AddressList[_addressName] == nil then
                print("Address.lua:OutputAddressValueByAddressName -- Address [" .. _addressName .. "] does not exist.")
                return nil;
            else
                Kanizah.CurrentIndex = Kanizah.CurrentIndex + 1
                --print("Address.lua OutputAddressValueByAddressName._addressName: " .. _addressName)
                _G.Kanizah.OutputArray_OneTime[_G.Kanizah.CurrentIndex] = (function() return AddressList[_addressName].Value(); end);
                _G.Kanizah.AdditionalDataArray_OneTime[_G.Kanizah.CurrentIndex] = _addressName
            end
        end
    end
    );
    CheckOutputStatusByAdditionalData = (function(_additionalData)
        if _additionalData == nil then return nil; end -- would it be safer to return true so it doesn't try to output broken stuff?
        for key,value in pairs(_G.Kanizah.AdditionalDataArray) do
            if value == _additionalData then
                --this item is being output
                return true;
            end
        end
        return false
    end
    );
    -- input
    UpdateInput = (function()
        local _FAPIDataInputFilePath = RF.EQOA.Net_Streams.i.self .. "FDi_Unreliable" .. Extension_ReadWrites
        --print("making FDi: " .. RF.EQOA.Net_Streams.i.self .. "FDi" .. Extension_ReadWrites)
        local cs = _G.FAPI.CommentIndicatorString
        --print(_FAPIDataInputFilePath)
        local updateCurrent = true
        local FDi_Unreliable = io.open(_FAPIDataInputFilePath,"r+")
        if FDi_Unreliable ~= nil then
        local nl
            nl = FAPI.IO.ReadNextLine(FDi_Unreliable, cs)
            while nl ~= nil do
                if nl ~= "" then
                    print("FAPI Data Input line: " .. nl)
                    if nl == "UpdateVariableByVariableName" then
                        print("Kanizah.UpdateVariableByVariableName()")
                        --_G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi_Unreliable, cs)
                        local r1 = FAPI.IO.ReadNextLine(FDi_Unreliable, cs)
                        local r2 = FAPI.IO.ReadNextLine(FDi_Unreliable, cs)
                        print(r1)
                        print(r2)
                        Kanizah.UpdateVariableByVariableName(r1, r2)
                    else
                        print("Kanizah.UpdateInput(): Invalid Command Issued")
                        FDi_Unreliable:close();
                        os.remove(_FAPIDataInputFilePath);
                        return -1
                    end
                    --if type(nl) == "table" then
                    --    local _table, _key = GetTableAndKey(nl)
                    --    _table[key]
                    --end
                end
                nl = FAPI.IO.ReadNextLine(FDi_Unreliable, cs)
            end
            FDi_Unreliable:close();
            os.remove(_FAPIDataInputFilePath);
        else
            --print("No FAPI Data Input File")
        end
    end
    );
    UpdateVariableByVariableName = (function(_variableNewValue, _variableToUpdate)
        for key,value in pairs(_G) do
            if key == _variableToUpdate then
                if value ~= nil then
                    value = _variableNewValue
                    return 1
                else
                    return -1
                end
            end
        end
        return 0
        --return -5
        --if _variableToUpdate == nil then return -1 end
        --if _variableNewValue == nil then return -2 end
        --print(_variableToUpdate .. "'s new value should be: " .. _variableNewValue)
        --if _G.[_variableToUpdate] ~=  then
            --_G.[_variableToUpdate] = _variableNewValue
        --end
    end
    );
    UpdateAddressByAddressName = (function(_addressName, _newvalue)
        if AddressList == nil then
            return nil;
        else
            if AddressList[_addressName] == nil then
                --print("Address.lua:UpdateAddressByAddressName -- Address [" .. _addressName .. "] does not exist.")
                return nil;
            else
                if _newvalue == nil then
                    --print("Address.lua:UpdateAddressByAddressName -- _newvalue is nil.")
                    return nil;
                else
                    --print("Address.lua UpdateAddressByAddressName._addressName: " .. _addressName)
                    AddressList[_addressName].Value(_newvalue);
                end
            end
        end
    end
    );
};
