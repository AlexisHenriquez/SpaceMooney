using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InsuficientBudgetException : Exception
{
	public InsuficientBudgetException() { }
	public InsuficientBudgetException(string message) : base(message) { }
	public InsuficientBudgetException(string message, Exception inner) : base(message, inner) { }
	protected InsuficientBudgetException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context)
		: base(info, context) { }
}