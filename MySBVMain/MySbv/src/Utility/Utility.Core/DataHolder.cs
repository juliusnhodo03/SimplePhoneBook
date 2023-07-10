using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
	public class DataHolder<T>
	{
		public string DataString { get; set; }
		public T Object { get; set; }
	}
}
