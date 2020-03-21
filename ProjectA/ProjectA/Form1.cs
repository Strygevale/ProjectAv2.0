using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectA
{
    public partial class Form1 : Form
    {
		
        public Form1()
        {
            InitializeComponent();

			
				this.AcceptButton = this.button1;
	
		}

	/// <summary>
	/// Добавляем метод ConvertToPostfixNaotation. Проверяем остались ли числа или операции в стеке, если остались ошибка иначе результат.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

			double resultValue = 0f;

			try
			{
				var stack = new Stack<Value>();
				var parsedExpression = new PostfixNotationExpression().ConvertToPostfixNotation(textBox1.Text);
				var queue = new Queue<Node>(parsedExpression);

				while (queue.Count > 0)
				{
					var node = queue.Dequeue();
					stack.Push(!(node is OperationBase) ? node as Value : (node as OperationBase).Accept(stack));
				}

				if (stack.Count != 1 || !(stack.Peek() is Value))

					label1.Text = "Выражение составлено неверно";

				 resultValue = stack.Pop().Val;
				label1.Text = resultValue.ToString();
			}
			catch (Exception)
			{
				label1.Text = "Ошибка";
			}
			
			//Перевод resultValue в 2 и 16 систему
			try
			{
			
				int numconverter = Convert.ToInt32(resultValue);
				

				binary.Text = Convert.ToString(numconverter, 2);

				hexadecimal.Text = Convert.ToString(numconverter, 16);


				// var bytes = BitConverter.GetBytes(resultValue);
				//  var i = BitConverter.ToInt32(bytes, 0);

				// hexadecimal.Text = "0x" + i.ToString("X8");
			//	string s = "";
			//	foreach (byte b in BitConverter.GetBytes(resultValue))
			//	{
			//		s += Convert.ToString(b, 2).PadLeft(8, '0'); // for hex. For binary, use 2 and 8. For octal, use 8 and 3
					
			//	}
			//	binary.Text = s;
			}

			catch (Exception)
			{

				binary.Text = "Выражение составлено неверно";

			}

		}


	







		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			

		}

		

		private void binary_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			
		}

		

		
	}
}
