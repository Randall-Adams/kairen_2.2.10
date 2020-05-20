    --[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Read_BitArrayToString
Code-Type: LUA Function
Code-Version: 1.2
Code-Description: Reads a CE Bit Array into a single String.
Code-Author: Robert Randazzio
]]--
return (
function(BitArrayAddress, LengthtoRead, LengthIsManuallyEntered, _type)
    --_sender = "Read_BitArrayToString"
    --co(_sender, "co_enter", "BitArrayAddress, LengthtoRead, LengthIsManuallyEntered")    
    if LengthIsManuallyEntered ~= nil and LengthIsManuallyEntered == true then
        --The length is manually entered, so LengthtoRead doesn't need changed
            --co(_sender, "co_debugoutput", "Manual Read: " .. LengthtoRead)
    elseif (LengthIsManuallyEntered ~= nil and LengthIsManuallyEntered == false) or (LengthIsManuallyEntered == nil) then
        --The length is being referred to by a definition, so look up the definition and change 
        --  LengthtoRead to whatever is defined
        LengthtoReadName = LengthtoRead
        if LengthtoRead == "TypeZoneName" then -- This creates a new group type called TypeZoneName
            LengthtoRead = 50 -- This is twice the number of possible characters.
            _type = 2
        elseif LengthtoRead == "TypeCharacterName" then -- When you add your types, copy this line because of the "elseif"
            LengthtoRead = 24 -- So you only need to copy this line and the above line to add your own types
            _type = 1
        elseif LengthtoRead == "TypePlayerName" then -- When you add your types, copy this line because of the "elseif"
            LengthtoRead = 24 -- So you only need to copy this line and the above line to add your own types
            _type = 1
        elseif LengthtoRead == "TypeGameChat" then
            LengthtoRead = 127
            _type = 2
        else
            print("Read_BitArrayToString Error: Undefined Type Passed.")
        end
        --co(_sender, "co_debugoutput", LengthtoReadName .. ": " .. LengthtoRead)
    end
    ValueAsUnicodeBitArray = readBytes(BitArrayAddress, LengthtoRead, true)
    local i = 1
    local ValueAsString
    ValueAsString = string.char(ValueAsUnicodeBitArray[i])
    if _type == nil then
    
    elseif _type == 2 then
        i = i + 2
        while ValueAsUnicodeBitArray[i] ~= nil and ValueAsUnicodeBitArray[i] ~= 0 do
            --co(_sender, "co_debugoutput", "3: " .. i .. " = " .. ValueAsUnicodeBitArray[i])
            ValueAsString = ValueAsString .. string.char(ValueAsUnicodeBitArray[i])
            i = i + 2
        end
    elseif _type == 1 then
        i = i + 1
        while ValueAsUnicodeBitArray[i] ~= nil and ValueAsUnicodeBitArray[i] ~= 0 do
            --co(_sender, "co_debugoutput", "3: " .. i .. " = " .. ValueAsUnicodeBitArray[i])
            ValueAsString = ValueAsString .. string.char(ValueAsUnicodeBitArray[i])
            i = i + 1
        end
    end
    return ValueAsString
end
);
