--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Kanizah
Code-Type: LUA Class
Code-Version: 1.1
Code-Description: Outputs the player's data to Kairen.
Code-Author: Robert Randazzio
]]--
_G.Kanizah = {
    index = 0;
    GetIndex = (function()
        _G.KanizahList.index = _G.KanizahList.index + 1
        _G.KanizahList.UsedMarker[_G.KanizahList.index] = 1
        return _G.KanizahList.index;
    end
    );
    ProcessCommandFile = (function()
        local _FAPIDataFilePath = RF.EQOA.Net_Streams.i.self .. "FAPI Data2" .. Extension_ReadWrites
        if FAPI.IO.File_Exists(_FAPIDataFilePath) then
            local cs = _G.FAPI.CommentIndicatorString
            print(_FAPIDataFilePath)
            local FDi = io.open(_FAPIDataFilePath,"r+")
            local nl
            nl = FAPI.IO.ReadNextLine(FDi, cs)
            while nl ~= nil do
                if nl ~= "" then
                    print("FAPI Data line: " .. nl)
                    if nl == "Add" then
                        _G.KanizahList[_G.KanizahList.GetIndex] = FAPI.IO.ReadNextLine(FDi, cs)
                    end
                    --if type(nl) == "table" then
                    --    local _table, _key = GetTableAndKey(nl)
                    --    _table[key]
                    --end
                end
                nl = FAPI.IO.ReadNextLine(FDi, cs)
            end
            FDi:close();
            --os.remove(_FAPIDataFilePath);
        else
            print("No FAPI Data File")
        end
    end
    );
    CurrentIndex = 0;
    OutputArray = {};
    AdditionalDataArray = {};
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
        if _G.Kanizah.OutputArray == nil then return nil; end
        local _FAPIDataFilePath = RF.EQOA.Net_Streams.o.self .. "FAPI Data2" .. Extension_ReadWrites
        --print("making fd2: " .. RF.EQOA.Net_Streams.o.self .. "FAPI Data2" .. Extension_ReadWrites)
        local cs = _G.FAPI.CommentIndicatorString
        --print(_FAPIDataFilePath)
        local updateCurrent = true
        local FDo = io.open(_FAPIDataFilePath,"w+")
        if FDo ~= nil then
            for key,value in pairs(_G.Kanizah.OutputArray) do
                --print("key: " .. key .. " / value: " .. value)
                FDo:write(value());
                FDo:write("\n");
                FDo:write(_G.Kanizah.AdditionalDataArray[key]);
                FDo:write("\n");
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
        for key1,value1_list in pairs(OutputList) do
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
        if AddressList == nil then
            return nil;
        else
            if AddressList[_addressName] == nil then
                print("Address.lua:AddOutputByAddressName -- Address [" .. _addressName .. "] does not exist.")
                return nil;
            else
                Kanizah.CurrentIndex = Kanizah.CurrentIndex + 1
                _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() return AddressList[_addressName].Value(); end);
                _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = _addressName
            end
        end
    end
    );
    AddOutputByFunctionReference = (function(_referenceName, _reference)
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
        else
        
        end
    end
    );
};
