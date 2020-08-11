using System;
using Vintagestory.API.Client;
using NLua;
using KeraLua;
using Lua = NLua.Lua;

namespace VSLua {
	/// <summary>
	/// simple text field that automatically updates a Lua state as a user types.
	/// It also records what status there was on last update
	/// </summary>
	public class GuiElementLuaInput : GuiElementEditableTextBase {
		private readonly ICoreClientAPI ClientAPI;
		/// <summary>
		/// Called when the Lua state is changed.
		/// </summary>
		public Action<GuiElementLuaInput, Lua> OnLuaChanged;
		/// <summary>
		/// Called when updating the Lua state results in an error.
		/// </summary>
		public Action<GuiElementLuaInput, Lua> OnLuaError;
		private readonly Lua LuaState = VSLuaManager.GetLuaState();

		/// <summary>
		/// The last status of the update.
		/// </summary>
		public LuaStatus Status { get; private set; } = LuaStatus.OK;

		public GuiElementLuaInput(ICoreClientAPI capi, ElementBounds bounds,
			Action<GuiElementLuaInput, Lua> OnLuaChanged, Action<GuiElementLuaInput, Lua> OnLuaError, CairoFont font
		) : base(capi, font, bounds) {
			ClientAPI = capi;
			LuaState["ClientAPI"] = ClientAPI;
			LuaState["element"] = this;
			this.OnLuaChanged = OnLuaChanged;
			this.OnLuaError = OnLuaError;
			OnTextChanged = UpdateLua;
		}

		private void UpdateLua(string source) {
			Status = LuaState.State.LoadString(source);
			if (Status == LuaStatus.OK) Status = LuaState.State.PCall(0, -1, 0);
			if (Status != LuaStatus.OK)
				OnLuaError(this, LuaState);
			else
				OnLuaChanged(this, LuaState);
		}
	}
}
