using System;
using System.Collections.Generic;

namespace ProjectA
{

/// <summary>
/// Обозначаем переменные.
/// Value стрингового значения.
/// val с плавающей точкой.
/// </summary>
	internal class Value : Node
	{

		public Value(string s)
		{
			val = ValueParser.Parse(s);
			
		}

		public Value(double val)
		{
			this.val = val;
		}

		public double val;
		private bool isInverted;

		public override int Priority => 4;

		public new double Val => isInverted ? -val : val;

		public void InvertValue()
		{
			isInverted = !isInverted;
		}
	}
	
	internal class InverValue : Node
	{
		public override int Priority => 4;
		public new double Val => throw new NotImplementedException();
	}


	
	internal abstract class OperationBase : Node
	{

		public abstract string Name { get; }

		public new double Val => throw new InvalidOperationException();
		protected abstract OperationBase CloneFactory();

		public Node Clone()
		{
			return CloneFactory();
		}

		internal abstract Value Accept(Stack<Value> nodes);
	}


	/// <summary>
	/// Все бинарные и унарные операции класса хронятся в классе OperationBase.
	/// Задаем символ(Name), приоритет, закидываем в стек, вычисляем.
	/// Приоритет унарных операций должен быть выше бинарных
	/// </summary>
	internal class Add : OperationBase
	{
		public override string Name => "+";
		public override int Priority => 1;

		protected override OperationBase CloneFactory()
		{
			return new Add();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(nodes.Pop().Val + nodes.Pop().Val);
		}
	}

	internal class Subtract : OperationBase
	{
		public override string Name => "-";
		public override int Priority => 1;

		protected override OperationBase CloneFactory()
		{
			return new Subtract();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			
			return new Value(nodes.Pop().Val - nodes.Pop().Val);
		}
	}


	internal class Multiply : OperationBase
	{
		public override string Name => "*";

		public override int Priority => 2;

		protected override OperationBase CloneFactory()
		{

			return new Multiply();
		}
		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(nodes.Pop().Val * nodes.Pop().Val);
		}

	}


	internal class Divide : OperationBase
	{
		public override string Name => "/";
		public override int Priority => 2;

		protected override OperationBase CloneFactory()
		{
			return new Divide();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			var rigth = nodes.Pop().Val;
			return new Value(nodes.Pop().Val / (rigth * 1d));
		}
	}

	internal class Pow : OperationBase
	{
		public override string Name => "^";
		public override int Priority => 3;

		protected override OperationBase CloneFactory()
		{
			return new Pow();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			var rigth = nodes.Pop().Val;
			return new Value(Math.Pow(nodes.Pop().Val, rigth));
		}
	}


	internal class Cos : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "cos";

		protected override OperationBase CloneFactory()
		{
			return new Cos();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(Math.Cos(nodes.Pop().Val));
		}
	}


	internal class Sin : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "sin";

		protected override OperationBase CloneFactory()
		{
			return new Sin();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(Math.Sin(nodes.Pop().Val));
		}
	}

	internal class Tan : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "tg";

		protected override OperationBase CloneFactory()
		{
			return new Tan();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(Math.Tan(nodes.Pop().Val));
		}
	}


	internal class Ctg : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "ctg";

		protected override OperationBase CloneFactory()
		{
			return new Ctg();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(1 / Math.Tan(nodes.Pop().Val));
		}
	}


	internal class Log : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "log";

		protected override OperationBase CloneFactory()
		{
			return new Log();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(Math.Log(nodes.Pop().Val));
		}
	}

	internal class Pi : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "pi";

		protected override OperationBase CloneFactory()
		{
			return new Pi();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(Math.PI);
		}
	}

	internal class E : OperationBase
	{
		public override int Priority => 4;
		public override string Name => "e";

		protected override OperationBase CloneFactory()
		{
			return new E();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			return new Value(Math.E);
		}
	}


	internal class RigthBracket : OperationBase
	{
		public override string Name => ")";
		public override int Priority => 0;

		protected override OperationBase CloneFactory()
		{
			return new RigthBracket();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			throw new NotImplementedException();
		}

	}

	internal class LeftBracket : OperationBase
	{
		public override string Name => "(";
		public override int Priority => 0;

		protected override OperationBase CloneFactory()
		{
			return new LeftBracket();
		}

		internal override Value Accept(Stack<Value> nodes)
		{
			throw new NotImplementedException();
		}
	}

	


	/// <summary>
	/// Парсинг чисел
	/// </summary>
	internal static class ValueParser
	{
		public static double Parse(string value)
		{ 
		//Парсим hex.
	    //Создаем массив, для того чтобы скеился символ "0x" с числом идущем после, далее парсим это это число целиком
			if (value.StartsWith("0x"))
			{
				char[] _trim_hex = new char[] { '0', 'x' };
				
				if (int.TryParse(value.TrimStart(_trim_hex), System.Globalization.NumberStyles.HexNumber, null, out var resultValue))
				{
					return resultValue;
				}
			}

			if (value.EndsWith("h"))
			{
				char[] _trim_hex = new char[] { 'h' };

				if (int.TryParse(value.Trim(_trim_hex), System.Globalization.NumberStyles.HexNumber, null, out var resultValue))
				{
					return resultValue;
				}
			}


			//тоже самое и для двоичной системы, только числа уже впереди а за ним символ "b"
			if (value.EndsWith("b"))
			{
				char[] _trim_bin = new char[] { 'b' };

				return Convert.ToInt32(value.Trim(_trim_bin), 2);			

			}
			//Конвертируем число из строки в double, где точка меняется на запятую
			value = value.Replace('.', ',');
			if (double.TryParse(value, out var val2)) return val2;
			throw new ArgumentException();
		}

		
	}
}
