
using System;
using System.Collections.Generic;
using System.Linq;


namespace ProjectA
{


	/// <summary>
	/// Если текущий элемент — цифра или переменная, то положим в стек значение этого числа/переменной.
	/// Если текущий элемент — открывающая скобка, то положим её в стек.
	/// Если текущий элемент — закрывающая скобка, то будем выталкивать из стека и выполнять все операции до тех пор, пока мы не извлечём открывающую скобку (т.е встречая закрывающую скобку, мы выполняем все операции, находящиеся внутри этой скобки). 
	/// Если текущий элемент — операция, то, пока на вершине стека находится операция с таким же или большим приоритетом, будем выталкивать и выполнять её.
	/// После того, как мы обработаем всю строку, в стеке операций ещё могут остаться некоторые операции, которые ещё не были вычислены, и нужно выполнить их все(т.е.действуем аналогично случаю, когда встречаем закрывающую скобку).
	/// </summary>
    

	public class PostfixNotationExpression
	{



        /// <summary>
        ///Получаем все доступные операторы OperationBase
        /// </summary>
		public PostfixNotationExpression()
		{
			operators = GetType().Assembly.DefinedTypes.Where(it => typeof(OperationBase).IsAssignableFrom(it)
			&& !it.IsAbstract).Select(Activator.CreateInstance).Cast<OperationBase>().ToList();

		}

		private readonly List<OperationBase> operators;



        /// <summary>
        /// Проверяем число или оператор посимвольно. Создаем коллекцию.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        
		private List<string> Separate(string input)
		{

            var allowChars = new List<Char> {

                'A','B', 'C', 'D','E','F','a','b', 'c', 'd','e','f','x','b','h','.',','
            };
            var resultCollection = new List<String>();
			var pos = 0;
			while (pos < input.Length)
			{
				var s = string.Empty + input[pos];
                //считываем операторы из OperationBase
				if (operators.All(it => it.Name != input[pos].ToString()))
				{

                    //склеивает числа в одно число и если символы 'A','B', 'C', 'D','E','F','a','b', 'c', 'd','e','f','x','b','h','.',','
                    if (char.IsDigit(input[pos]))
                    {
                        for (var i = pos + 1; i < input.Length && (char.IsDigit(input[i]) || allowChars.Contains(input[i])); i++)
                        {
                            s += input[i];
                        }
                    }

                    //склеиваются буквы с символами
                    else if (char.IsLetter(input[pos]))
                    {
                        for (var i = pos + 1; i < input.Length && (char.IsLetter(input[i]) || char.IsDigit(input[i])); i++)
                        {
                            s += input[i];
                        }
                    }
				}

				resultCollection.Add(s);
				pos += s.Length;
			}

            return resultCollection;
		}

         /// <summary>
         /// Создаем стек
         /// </summary>
         /// <param name="input"></param>
         /// <returns></returns>

		internal List<Node> ConvertToPostfixNotation(string input)
		{
            var outputSeparated = new List<Node>();
            var stack = new Stack<Node>();
            var mayUnary = true;

           //Склеиваем числа обращаясь к методу Separate
            var separated = Separate(input.Replace(" ", ""));

           
            foreach (var c in separated)
            {
                //Обращаемся к числу или операции, если операция, то смотрим приоритет, число закидываем в стек
                var ops = operators.Where(it => it.Name == c).ToList();
                if (ops.Any())
                {
                    OperationBase op = null;
                    if (ops.Count > 1)
                        op = (OperationBase)(mayUnary ? ops.OrderByDescending(it => it.Priority).First().Clone() : ops.OrderBy(it => it.Priority).First().Clone());
                          
                    else
                       op = ops[0];
                    //проверяем наличие скобок
                    if (stack.Count > 0 && !(op.Name == "("))
                        if (op.Name == ")")
                        {
                            var s = stack.Pop();
                            while (!(s is LeftBracket))
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }

                            mayUnary = false;
                        }
                    //если приоритет выше - проталкиваем элементы дальше, иначе если стек заполнен и приоритет меньше либо равен элементу лежащему в стеке, у которого приоритет больше - выталкиваем его
                        else if (op.Priority > stack.Peek().Priority)
                        {
                            stack.Push(op.Clone());
                            mayUnary = true;
                        }
                        else
                        {
                            while (stack.Count > 0 && op.Priority <= stack.Peek().Priority)
                                outputSeparated.Add(stack.Pop());
                            stack.Push(op.Clone());
                            mayUnary = true;
                        }
                    else
                    {
                        stack.Push(op.Clone());
                        mayUnary = true;
                    }
                }
                else
                {
                    outputSeparated.Add(new Value(c));
                    mayUnary = false;
                }
            }

            if (stack.Count > 0)
                foreach (var c in stack)
                    outputSeparated.Add(c);

            return outputSeparated;
        }
	}
}
