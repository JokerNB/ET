using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  TWS
    /// Date    2025.8.12
    /// Desc    
    /// </summary>
    
    [ChildOf(typeof(RoleManagerComponent))]
	public class RoleChild : Entity, IAwake<int>
	{
		public int ConfigId;
		public RoleConfig roleConfig => RoleConfigCategory.Instance.Get(this.ConfigId);
	}
}