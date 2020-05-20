--[[ Meta-Data
Meta-Data-Version: 1.0

Code-Name: ConsoleOutput
Code-Type: LUA Class
Code-Version: 1.2
Code-Description: Handles outputting to the console.
Code-Author: Robert Randazzio
]]--
co2 = print
co2i = 0
print = function(_passedarg)
    if SilentMode == nil then SilentMode = false end
    if SilentMode == false then
        if _passedarg == nil then _passedarg = "" end;
        co2i = co2i + 1
        co2(co2i .. "| " .. _passedarg)
        if string.sub(_passedarg, 1, 2) == "0|" then
            local calling_script = debug.getinfo(2).short_src
            print(" -- Print called by: " .. calling_script)
            print(" -- The above script needs updated for use with ConsoleOutput.lua -- ")
            --if mode == "DeveloperMode" then
            --    os.execute(calling_script)
            --end
        end
    end
end