--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: CE.Address.New
Code-Type: LUA Function
Code-Version: 1.3
Code-Description: Represents a CE Address through a LUA Class. This creates an instance of the class.
Code-Author: Robert Randazzio
]]--
--_G.CE = {};
--_G.CE.Address = {};
--it would be nice if this was better equipped to output nonstring values to a print function properly
return (
function(_Address0x, _Type, _Name, _Length, _SubType)
    local self = {};
    self.vAddress0x = _Address0x
    self.vType = _Type
    self.vName = _Name
    self.vLength = nil
    self.vSubType = nil
    self.ListIndex = nil
    if AddressList == nil then
        _G.AddressList = {}
        AddressList[_Name] = self
    else
        if AddressList[_Name] ~= nil then
            --address already created
            --print("Returning preexisting Address [" .. _Name .. "] instead of creating a new reference.")
            return AddressList[_Name]
        else
            AddressList[_Name] = self
        end
    end
    function self.Address0x()
        return self.vAddress0x
    end
    function self.Type()
        return self.vType
    end

    if self.vType == "Byte" then
        self.vType = "Bytes"
        _Length = 1
    end

    if self.vType == "Bytes" then
        self.vLength = _Length
        function self.Length()
            return self.vLength
        end
        function self.Value(_new)
            if _new == nil then
            --print("reading " .. self.vAddress0x .. " of " ..vLength)
                return readBytes(self.vAddress0x, self.vLength)
            else
                writeBytes(self.vAddress0x, _new)
            end
        end
        
    elseif self.vType == "Integer" then    
        function self.Value(_new)
            if _new == nil then
                return readInteger(self.vAddress0x)
            else
                writeInteger(self.vAddress0x, _new)
            end
        end
    elseif self.vType == "Float" then    
        function self.Value(_new)
            if _new == nil then
                return readFloat(self.vAddress0x)
            else
                writeFloat(self.vAddress0x, _new)
            end
        end
        
    elseif self.vType == "Double" then    
        function self.Value(_new)
            if _new == nil then
                return readDouble(self.vAddress0x)
            else
                writeDouble(self.vAddress0x, _new)
            end
        end
        
    elseif self.vType == "String" then
        self.vLength = _Length
        self.vSubType = _SubType
        function self.Length()
            return self.vLength
        end
        function self.SubType()
            return self.vSubType
        end
        if self.vSubType == "Normal" then
            function self.Value(_new)
                if _new == nil then
                    return readString(self.vAddress0x, self.vLength)
                else
                    writeString(self.vAddress0x, _new)
                end
            end
        elseif self.vSubType == "Wide" then
            function self.Value(_new)
                if _new == nil then
                    return readString(self.vAddress0x, self.vLength, true)
                else
                    writeString(self.vAddress0x, _new)
                end
            end
        elseif self.vSubType == "1" then
            function self.Value(_new)
                if _new == nil then
                    return FAPI.CE.IO.Read_BitArrayToString(self.vAddress0x, self.vLength, true, 1)
                else
                    _G.FAPI.CE.IO.Write_StringToBitArrayAddress(_new, self.vAddress0x, 1)
                end
            end
        elseif self.vSubType == "2" then
            function self.Value(_new)
                if _new == nil then
                    return FAPI.CE.IO.Read_BitArrayToString(self.vAddress0x, self.vLength, true, 2)
                else
                    _G.FAPI.CE.IO.Write_StringToBitArrayAddress(_new, self.vAddress0x, 2)
                end
            end
        elseif self.vSubType ~= nil then
            function self.Value(_new)
                if _new == nil then
                    return FAPI.CE.IO.Read_BitArrayToString(self.vAddress0x, self.vSubType)
                else
                    _G.FAPI.CE.IO.Write_StringToBitArrayAddress(_new, self.vAddress0x, self.vSubType)
                end
            end
        end
    else
        function self.Value(_new)
            print("Address.lua - Generic Address Type Warning")
            if _new == nil then
                return self.vValue
            else
                self.vValue = _new
            end
        end
    end
    function self.Name()
        return self.vName
    end
    --print("Address [" .. _Name .. "] Registered.") 
    return self;
end
);
--[[
CE.Address[1] = CE.Address.New("[pcsx2-r3878.exe+0040239C]+760", integer, "MyX")
CE.Address[2] = CE.Address.New("Brad")

local i = 1
CE.Address[i].Address0x("[pcsx2-r3878.exe+0040239C]+760")
print(CE.Address[i].Name() .. "'s Address: " .. CE.Address[i].Address0x())
i = i + 1
print(CE.Address[i].Name() .. "'s Address: " .. CE.Address[i].Address0x())
]]--
