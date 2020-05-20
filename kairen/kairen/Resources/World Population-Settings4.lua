print("Loading Settings")
GameLoop.et.check("SETUP", mode, 100)

-- Process Outside Commands
FAPI.RunOptions.ProcessOutsideCommands = FAPI.RunOptions.New(FAPI.Kanizah.ProcessOutsideCommands, true)
FAPI.RunOptions.Alert_ProcessOutsideCommands = FAPI.RunOptions.New(FAPI.Kanizah.Alert_ProcessOutsideCommands, false)

-- Output PlayerData
FAPI.RunOptions.OutputPlayerData = FAPI.RunOptions.New(_G.FAPI.Kanizah.OutputPlayerData, false, "OutputPlayerData", 1000)
FAPI.RunOptions.Alert_OutputPlayerData = FAPI.RunOptions.New(FAPI.Kanizah.Alert_OutputPlayerData, false)
FAPI.RunOptions.Alert_OutputPlayerData_ConsoleOutput = FAPI.RunOptions.New(FAPI.Kanizah.Alert_OutputPlayerData_ConsoleOutput, false)

-- Handle Automatic NPC Spawning
FAPI.RunOptions.UpdateSpawnNests = FAPI.RunOptions.New(_G.SpawnHandler.UpdateSpawnNests, true, "UpdateSpawnNests", 1000)

-- From Dev Main
dofile(RF.EQOA.LUAs.Modules.FAPI.self .. "Classes/Kanizah.lua")
--SquareIsPressed = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+975", "Byte", "SquareIsPressed")
CrossIsPressed = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+976", "Integer", "CrossIsPressed")
SquareIsPressed = FAPI.CE.Address.New("[pcsx2-r3878.exe+0040239C]+975", "Integer", "SquareIsPressed")
TargetID = FAPI.CE.Address.New("[pcsx2-r3878.exe+004023B0]+B90", "Integer", "TargetID", 4)
function CrossPressCheck()
    if CrossIsPressed.Value() == 1 then
        if PopupBox.TextBoxOn.Value() == 1 then
            PopupBox.Close()
        end
    end
end
function SquarePressCheck()
    if SquareIsPressed.Value() == 1 then
        if PopupBox.TextBoxOn.Value() == 0 then
            --NPCs.HailPlayer(TargetID.Value())
            NPCs.Interact(TargetID.Value(), "StartDialogue")
        end
    end
end
PopupBox = {};
PopupBox.TextBox = FAPI.CE.Address.New("[pcsx2-r3878.exe+00400C60]+578", "String", "TextBox", "57", "TypeGameChat");
PopupBox.TextBoxOn = FAPI.CE.Address.New("[pcsx2-r3878.exe+00400C60]+3EC", "Integer", "TextBoxOn", "1");
PopupBox.UKTextBox = FAPI.CE.Address.New("[pcsx2-r3878.exe+00400B40]+6B0", "String", "UKTextBox", "57", "TypeGameChat");
PopupBox.Show = (function(_message)
        PopupBox.TextBox.Value(_message)
        PopupBox.TextBoxOn.Value(1)
    end
    );
PopupBox.Close = (function()
        PopupBox.TextBoxOn.Value(0)
    end
    );
PopupBox.Show2 = (function(_message, _channel, _speaker)
        local ChannelMessage
        if _channel == "Say" then
            ChannelMessage = " says: "
        elseif _channel == "Shout" then
            ChannelMessage = " shouts at you: "
        elseif _channel == "Tell" then
            ChannelMessage = " whispers to you: "
        end
        PopupBox.Show(_speaker .. ChannelMessage .. _message)
        --PopupBox.TextBoxOn.Value(1)
    end
    );

FAPI.RunOptions.CrossPressCheck = FAPI.RunOptions.New(CrossPressCheck, true, "CrossPressCheck", 100)
FAPI.RunOptions.SquarePressCheck = FAPI.RunOptions.New(SquarePressCheck, true, "SquarePressCheck", 100)

-- From Dev World Pop
_G.Kanizah.AddOutputByAddressName("MyX");
_G.Kanizah.AddOutputByAddressName("MyY");
_G.Kanizah.AddOutputByAddressName("MyZ");
_G.Kanizah.AddOutputByAddressName("MyF");
_G.Kanizah.AddOutputByFunctionReference("MyZone", EQOA.Player.Location.Zone);
_G.Kanizah.AddOutputByDefinition("MyNestX");
_G.Kanizah.AddOutputByDefinition("MyNestY");
_G.Kanizah.AddOutputByDefinition("MyRow");
_G.Kanizah.AddOutputByDefinition("MyColumn");
local i = 1
local imax = 50
while i <= imax do
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].Name(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "Name"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].X(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "X"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].Y(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "Y"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].Z(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "Z"
    _G.Kanizah.CurrentIndex = _G.Kanizah.CurrentIndex + 1
    _G.Kanizah.OutputArray[_G.Kanizah.CurrentIndex] = (function() if _G.NPCs[i] == nil then return "nil" else return _G.NPCs[i].F(); end end);
    _G.Kanizah.AdditionalDataArray[_G.Kanizah.CurrentIndex] = "NPC" .. i .. "F"
    i = i + 1
end
i = nil
imax = nil

_G.FAPI.RunOptions.UpdateVisualNPCs = FAPI.RunOptions.New(_G.NPCs.UpdateVisualNPCs, true, "UpdateVisualNPCs", 100);
--_G.FAPI.RunOptions.UpdateKanizah = FAPI.RunOptions.New(_G.Kanizah.UpdateOutput, true, "UpdateKanizah", 1000);
_G.FAPI.RunOptions.UpdateKanizah = FAPI.RunOptions.New(_G.Kanizah.Update, true, "UpdateKanizah", 1000);
NPCs.Spawn("Randall_Adams")
print("All Settings Loaded.")
