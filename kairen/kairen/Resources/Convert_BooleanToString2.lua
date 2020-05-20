--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: Convert_BooleanToString
Code-Type: LUA Function
Code-Version: 1.0
Code-Description: Converts a boolean value into it's string equivalent.
Code-Author: Robert Randazzio
]]--
return (
function (_value)
    if _value == true then
        return "true";
    elseif _value == false then
        return "false"
    elseif _value == nil then
        return "nil"
    end
    return nil;
end
);
