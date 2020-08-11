using System;
using System.Collections.Generic;
using System.Linq;
using KeraLua;
using NLua;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Lua = NLua.Lua;
using LuaFunction = NLua.LuaFunction;

namespace VSLua {
	// all the stuff to do with functions
	public static partial class VSLuaManager {

		/// <summary>
		/// Creates a function.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="status"></param>
		/// <param name="factory"></param>
		/// <returns>The new function. If an error is thrown while creating the function, null is returned
		/// and status is set accordingly (if applicable).</returns>
		public static LuaFunction CreateFunction(string source, ref LuaStatus status, Lua factory) {
			var state = factory.State;
			var global = (LuaTable) factory.GetObjectFromPath("_G");
			var globalKeysBefore = (ICollection<object>) global.Keys;
			// compiles and pushes to the stack as a function
			// if theres an error then it pushes that error to the stack
			// we need to clean that up if that happens
			status = state.LoadString(source);
			if (status != LuaStatus.OK) {
				state.Pop(1);
				return null;
			}
			status = state.PCall(0, 0, 0); // run the function on the stack, ie the chunk
			if (status != LuaStatus.OK) return null;

			var globalKeysAfter = (ICollection<object>) global.Keys;
			var difference = Enumerable.Except(globalKeysAfter, globalKeysBefore);
			Console.WriteLine(difference.Count());
			var first = difference.First();
			Console.WriteLine(first);
			return (LuaFunction) global[first];
		}
		/// <summary>
		/// Creates a function given source and factory.
		/// </summary>
		public static LuaFunction CreateFunction(string source, Lua factory) {
			var o = LuaStatus.ErrRun;
			return CreateFunction(source, ref o, factory);
		}

		/// <summary>
		/// Creates a function given source.
		/// </summary>
		public static LuaFunction CreateFunction(string source) => CreateFunction(source, defaultLuaFactory);
		/// <summary>
		/// Creates a function given source and sets status ref.
		/// </summary>
		public static LuaFunction CreateFunction(string source, ref LuaStatus status) =>
			CreateFunction(source, ref status, defaultLuaFactory);

		/// <summary>
		/// Creates a function from whatever is in the GuiElementTextBase, sets status ref, and uses factory.
		/// This is client-side only!
		/// </summary>
		public static LuaFunction CreateFunctionFromTextField(
			GuiElementTextBase field, ref LuaStatus status, Lua factory
			) => CreateFunction(field.GetText(), ref status, factory);
		/// <summary>
		/// Creates a function from whatever is in the GuiElementTextBase.
		/// </summary>
		public static LuaFunction CreateFunctionFromTextField(GuiElementTextBase field) => CreateFunction(field.GetText());
		/// <summary>
		/// Creates a function from whatever is in the GuiElementTextBase and factory.
		/// </summary>
		public static LuaFunction CreateFunctionFromTextField(GuiElementTextBase field, Lua factory) =>
			CreateFunction(field.GetText(), factory);
		/// <summary>
		/// Creates a function from whatever is in the GuiElementTextBase and sets status ref.
		/// </summary>
		public static LuaFunction CreateFunctionFromTextField(
			GuiElementTextBase field, ref LuaStatus status
			) => CreateFunction(field.GetText(), ref status);

		public static ClientChatLineDelegate MakeDelegate(LuaFunction lua) {
			return (int groupId, ref string message, ref EnumHandling handled) => {
				lua.Call(groupId, message, handled);
			};
		}
	}
}

