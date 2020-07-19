using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NLua;

namespace VSLua {
	public static class VSLuaManager {
		public static readonly Lua luaFunctionFactory = new Lua();
		private static readonly Regex functionName = new Regex(@"function\s+(\w+)\s*\(");

		/// <summary>
		/// Returns a new Lua state.
		/// </summary>
		/// <returns></returns>
		public static Lua GetLuaState() => new Lua();

		/// <summary>
		/// Creates a single function.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="factory">Optional factory parameter, defaults to the internal factory.</param>
		/// <returns>The new function. If an error is thrown while creating the function, null is returned.</returns>
		public static LuaFunction CreateFunction(string source, Lua factory = null) {
			if (factory is null) factory = luaFunctionFactory;
			factory.DoStringSafe(source, out Exception ex);
			if (!(ex is null)) {
				var name = functionName.Match(source).Groups[1].Value;
				return factory[name] as LuaFunction;
			} else
				return null;
		}

		/// <summary>
		/// Creates and returns all functions in the source string.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="factory">Optional factory parameter, defaults to the internal factory.</param>
		/// <returns>A dictionary of functions keyed by the parsed function names.
		/// If an error is thrown while creating the function, null is returned.</returns>
		public static Dictionary<string, LuaFunction> CreateSeveralFunctions(string source, Lua factory = null) {
			if (factory is null) factory = luaFunctionFactory;
			var funcs = new Dictionary<string, LuaFunction>();

			factory.DoStringSafe(source, out Exception ex);
			if (!(ex is null)) {
				foreach (Match match in functionName.Matches(source)) {
					var name = match.Groups[1].Value;
					var function = factory[name] as LuaFunction;
					funcs.Add(name, function);
				}
				return funcs;
			} else
				return null;
		}

	}

	public static class LuaExtentions {
		/// <summary>
		/// Prevents errors from being thrown and always returns at least an empty object array.
		/// </summary>
		/// <param name="lua"></param>
		/// <param name="chunk"></param>
		/// <param name="exception">The exception, if one occurs.</param>
		/// <returns></returns>
		public static object[] DoStringSafe(this Lua lua, string chunk, out Exception exception) {
			object[] vals = null;
			exception = null;
			try {
				vals = lua.DoString(chunk);
			} catch (Exception ex) {
				exception = ex;
			}
			return vals ?? new object[] { };
		}
	}
}

