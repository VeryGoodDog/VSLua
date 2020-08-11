using System.Reflection;
using System.Text.RegularExpressions;
using NLua;
using Lua = NLua.Lua;

namespace VSLua {
	// Core parts of the class
	public static partial class VSLuaManager {
		/// <summary>
		/// The Lua instance used by default in all methods that have optional factories.
		/// </summary>
		public static readonly Lua defaultLuaFactory = GetLuaState();
		/// <summary>
		/// The ObjectTranslator instance from defaultLuaFactory.
		/// </summary>
		public static readonly ObjectTranslator defaultObjectTranslator = GetObjectTranslator(defaultLuaFactory);
		

		static VSLuaManager() {
		}

		/// <summary>
		/// Returns the value of the supplied Lua object's private "_translator" field.
		/// </summary>
		/// <param name="factory"></param>
		/// <returns></returns>
		public static ObjectTranslator GetObjectTranslator(Lua factory) {
			var objField = typeof(Lua).GetField("_translator", BindingFlags.NonPublic | BindingFlags.Instance);
			return (ObjectTranslator) objField.GetValue(factory);
		}

		/// <summary>
		/// Returns a new Lua state.
		/// </summary>
		/// <returns></returns>
		public static Lua GetLuaState() => new Lua();
	}
}

