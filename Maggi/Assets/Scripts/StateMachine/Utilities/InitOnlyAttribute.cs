using System;
using UnityEngine;

namespace Maggi.StateMachine
{
	[AttributeUsage(AttributeTargets.Field)]
	public class InitOnlyAttribute : PropertyAttribute { }
}
