--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: HEX_DEC
Code-Type: LUA Function
Code-Version: 1.0
Code-Description: Converts a HEX value into a DEC value
Code-Author: Robert Randazzio
]]--
return (
function (_input)
    return tonumber(_input, 16)
end
);
