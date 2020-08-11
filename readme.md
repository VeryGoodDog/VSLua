### VSLua

VSLua is a, currently, small library mod that allows you to use Lua in Vintage Story. This should only be used for in-game user edited scripts and not actual production because thats what C# is for.

#### Features
- Easily create Lua states.
- Create functions from plain strings, and from text GUI elements.
- A `GuiElementLuaInput` class that streamlines getting user input and updating a Lua state. Currently WIP.

#### Known bugs and issues
- NLua likes to throw a lot of errors so pretty much everything not implemented by VSLua needs to be in a try catch.
- She's very fragile please be nice to her.

#### Planned features
- ~~Arbitrarily using `LuaFunction` objects as delegates.~~ This seems to be impossible. MAYBE NOT! :)
- Saving Lua states with protobuf or something for persistence between sessions.

#### Building
Before building, make sure to install the NuGet packages, and you will need to change the paths of the libs in VSLua.csproj.

Also, apparently the lua54.dll that is built into the mod crashes the game, so be sure to delete that after building. I know I should make a post-build command or whatever.