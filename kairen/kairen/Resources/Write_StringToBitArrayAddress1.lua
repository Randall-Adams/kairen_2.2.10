--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Write_StringToBitArrayAddress
Code-Type: LUA Class
Code-Version: 1.0
Code-Description: Writes a String as bytes starting at a CE Address.
Code-Author: Robert Randazzio
]]--
return(
function (_string, _bitArrayAddress, _type)
    _sender = "Write_StringToBitArrayAddress"
    --co(_sender, "co_enter", "_string, _bitArrayAddress, _type == " .. _type)
    local sp
    local i
    local i2
    sp = 1
    i = 1
    StringAsArrayofBits = {};
    if _type == 1 then
        while sp <= string.len(_string) do
            StringAsArrayofBits[i] = string.byte(_string, sp)
             --co(_sender, "co_debugoutput", StringAsArrayofBits[i])
            sp = sp + 1
            i = i + 1
        end
        StringAsArrayofBits[i] = 00
        StringAsArrayofBits[i + 1] = 00
    elseif _type == 2 then
        i2 = i + 1
        while sp <= string.len(String) do
            StringAsArrayofBits[i] = string.byte(_string, sp)
            StringAsArrayofBits[i2] = 00
            --co(_sender, "co_debugoutput", StringAsArrayofBits[i])
            sp = sp + 1
            i = i + 2
            i2 = i + 1
        end
        StringAsArrayofBits[i] = 00
        StringAsArrayofBits[i2] = 00
    else
        
    end
    writeBytes(_bitArrayAddress, StringAsArrayofBits)
end
);
