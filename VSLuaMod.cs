using Vintagestory.API.Common;

namespace VSLua {
	public class VSLuaMod : ModSystem {

		public override double ExecuteOrder() => 0;

		public override bool ShouldLoad(EnumAppSide forSide) => true;

		public override void Start(ICoreAPI api) {
			VSLuaManager.luaFunctionFactory["CoreAPI"] = api;
		}
	}
}

