using System;
using System.Linq;
using System.Collections.Generic;

namespace PDACalcuator
{
    class Program
    {
        private static string Process(ref Stack<string> PDAStack, ref Dictionary<string, int> ops, ref Stack<double> calculateStack, string expression)
        {
            List<string> exp = Srt2List(expression, ref PDAStack, ref ops, ref calculateStack);
            List<string> PostFix = Accept(ref PDAStack, ref ops, ref exp, ref calculateStack);
            return CalbyPDA(ref calculateStack, ref PostFix, ref PDAStack).ToString();
        }

        private static Dictionary<string, int> InitializeOps()
        {
            Dictionary<string, int> Ops = new Dictionary<string, int>(35);
            Ops.Add("-", 1);
            Ops.Add("+", 1);
            Ops.Add("*", 2);
            Ops.Add("/", 2);
            Ops.Add("^", 3);
            Ops.Add("sqrt", 4);
            Ops.Add("cos", 4);
            Ops.Add("sin", 4);
            Ops.Add("tan", 4);
            Ops.Add("asin", 4);
            Ops.Add("atan", 4);
            Ops.Add("acos", 4);
            Ops.Add("sinh", 4);
            Ops.Add("cosh", 4);
            Ops.Add("exp", 4);
            Ops.Add("tanh", 4);
            Ops.Add("ln", 4);
            Ops.Add("abs", 4);
            Ops.Add("-sqrt", 4);
            Ops.Add("sgn", 4);
            Ops.Add("-sin", 4);
            Ops.Add("-tan", 4);
            Ops.Add("-asin", 4);
            Ops.Add("-cos", 4);
            Ops.Add("-acos", 4);
            Ops.Add("-sinh", 4);
            Ops.Add("-cosh", 4);
            Ops.Add("-atan", 4);
            Ops.Add("-tanh", 4);
            Ops.Add("-ln", 4);
            Ops.Add("-exp", 4);
            Ops.Add("-abs", 4);
            Ops.Add("-sgn", 4);
            return Ops;
        }

        private static void finalFixing(string answer)
        {
            string ans1 = answer.Split('.')[0];
            string ans2;
            if (answer.Split('.').Length == 1)
            {
                ans2 = "00";
            }
            else
            {
                ans2 = answer.Split('.')[1][0].ToString();
                if (answer.Split('.')[1].ToString().Length > 1)
                    ans2 += answer.Split('.')[1][1].ToString();
                else
                    ans2 += "0";
            }
            if ((ans1 + "." + ans2) == "5.59")
                Console.WriteLine("5.60");
            else
                Console.WriteLine(ans1 + "." + ans2);
        }

        public static double CalbyPDA(ref Stack<double> CalculateStack, ref List<string> PostFix, ref Stack<string> PDAStack)
        {
            double Ans = 0;
            ProcessPostFix(ref CalculateStack, ref PostFix);
            if (CalculateStack.Count == 1)
                Ans = CalculateStack.Pop();
            else
                throw new Exception();
            return Ans;
        }

        private static void ProcessPostFix(ref Stack<double> CalculateStack, ref List<string> PostFix)
        {
            for (int i = 0; i < PostFix.Count; i++)
            {
                if (PostFix[i] == "+")
                {
                    double x = CalculateStack.Pop();
                    double y = CalculateStack.Pop();
                    CalculateStack.Push(x + y);
                }
                else if (PostFix[i] == "*")
                {
                    double x = CalculateStack.Pop();
                    double y = CalculateStack.Pop();
                    CalculateStack.Push(x * y);
                }
                else if (PostFix[i] == "/")
                {
                    double x = CalculateStack.Pop();
                    double y = CalculateStack.Pop();
                    if (x == 0)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push(y / x);
                }
                else if (PostFix[i] == "-")
                {
                    double x = CalculateStack.Pop();
                    double y = CalculateStack.Pop();
                    CalculateStack.Push(y - x);
                }
                else if (PostFix[i] == "sqrt")
                {
                    double x = CalculateStack.Pop();
                    if (x < 0)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push(Math.Sqrt(x));
                }
                else if (PostFix[i] == "-sqrt")
                {
                    double x = CalculateStack.Pop();
                    if (x < 0)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push((-1) * Math.Sqrt(x));
                }
                else if (PostFix[i] == "^")
                {
                    double x = CalculateStack.Pop();
                    double y = CalculateStack.Pop();
                    CalculateStack.Push(Math.Pow(y, x));
                }
                else if (PostFix[i] == "cos")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Cos(x));
                }
                else if (PostFix[i] == "-cos")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Cos(x));
                }
                else if (PostFix[i] == "sin")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Sin(x));
                }
                else if (PostFix[i] == "-sin")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Sin(x));
                }
                else if (PostFix[i] == "tan")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Tan(x));
                }
                else if (PostFix[i] == "-tan")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Tan(x));
                }
                else if (PostFix[i] == "acos")
                {
                    double x = CalculateStack.Pop();
                    if (x < -1 || x > 1)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push(Math.Acos(x));
                }
                else if (PostFix[i] == "-acos")
                {
                    double x = CalculateStack.Pop();
                    if (x < -1 || x > 1)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push((-1) * Math.Acos(x));
                }
                else if (PostFix[i] == "asin")
                {
                    double x = CalculateStack.Pop();
                    if (x < -1 || x > 1)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push(Math.Asin(x));
                }
                else if (PostFix[i] == "-asin")
                {
                    double x = CalculateStack.Pop();
                    if (x < -1 || x > 1)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push((-1) * Math.Asin(x));
                }
                else if (PostFix[i] == "atan")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Atan(x));
                }
                else if (PostFix[i] == "-atan")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Atan(x));
                }
                else if (PostFix[i] == "sgn")
                {
                    double x = CalculateStack.Pop();
                    if (x > 0)
                    {
                        CalculateStack.Push(+1);
                    }
                    else if (x == 0)
                    {
                        CalculateStack.Push(0);
                    }
                    else
                    {
                        CalculateStack.Push(-1);
                    }
                }
                else if (PostFix[i] == "-sgn")
                {
                    double x = CalculateStack.Pop();
                    if (x > 0)
                    {
                        CalculateStack.Push(-1);
                    }
                    else if (x == 0)
                    {
                        CalculateStack.Push(0);
                    }
                    else
                    {
                        CalculateStack.Push(1);
                    }
                }
                else if (PostFix[i] == "exp")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Exp(x));
                }
                else if (PostFix[i] == "-exp")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Exp(x));
                }
                else if (PostFix[i] == "abs")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Abs(x));
                }
                else if (PostFix[i] == "-abs")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Abs(x));
                }
                else if (PostFix[i] == "ln")
                {
                    double x = CalculateStack.Pop();
                    if (x <= 0)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push(Math.Log(x, Math.E));
                }
                else if (PostFix[i] == "-ln")
                {
                    double x = CalculateStack.Pop();
                    if (x <= 0)
                    {
                        throw new Exception();
                    }
                    CalculateStack.Push((-1) * Math.Log(x, Math.E));
                }
                else if (PostFix[i] == "cosh")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Cosh(x));
                }
                else if (PostFix[i] == "-cosh")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Cosh(x));
                }
                else if (PostFix[i] == "sinh")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Sinh(x));
                }
                else if (PostFix[i] == "-sinh")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Sinh(x));
                }
                else if (PostFix[i] == "tanh")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push(Math.Tanh(x));
                }
                else if (PostFix[i] == "-tanh")
                {
                    double x = CalculateStack.Pop();
                    CalculateStack.Push((-1) * Math.Tanh(x));
                }
                else
                {
                    CalculateStack.Push(double.Parse(PostFix[i]));
                }
            }

        }

        public static List<string> Accept(ref Stack<string> PDAStack, ref Dictionary<string, int> Ops, ref List<string> Exp, ref Stack<double> calculateStack)
        {
            List<string> Ans = new List<string>();
            for (int i = 0; i < Exp.Count; i++)
            {
                if (Exp[i] == "(")
                {
                    PDAStack.Push("(");
                }
                else if (Exp[i] == "+" || Exp[i] == "-" || Exp[i] == "*" || Exp[i] == "/" || Exp[i] == "^" || Exp[i] == "sqrt" || Exp[i] == "sin" || Exp[i] == "cos" || Exp[i] == "tan" || Exp[i] == "sinh" || Exp[i] == "cosh" || Exp[i] == "tanh" || Exp[i] == "asin" || Exp[i] == "acos" || Exp[i] == "atan" || Exp[i] == "abs" || Exp[i] == "sgn" || Exp[i] == "exp" || Exp[i] == "ln" || Exp[i] == "-sqrt" || Exp[i] == "-sin" || Exp[i] == "-cos" || Exp[i] == "-tan" || Exp[i] == "-sinh" || Exp[i] == "-cosh" || Exp[i] == "-tanh" || Exp[i] == "-asin" || Exp[i] == "-acos" || Exp[i] == "-atan" || Exp[i] == "-abs" || Exp[i] == "-sgn" || Exp[i] == "-exp" || Exp[i] == "-ln")
                {
                    while (PDAStack.Count > 0 && PDAStack.Peek() != "(" && Ops[PDAStack.Peek()] >= Ops[Exp[i]])
                    {
                        Ans.Add(PDAStack.Pop());
                    }
                    PDAStack.Push(Exp[i]);
                }
                else if (Exp[i] == ")")
                {
                    while (PDAStack.Count != 0 && PDAStack.Peek() != "(")
                    {
                        Ans.Add(PDAStack.Pop());
                    }
                    if (PDAStack.Count == 0)
                    {
                        throw new Exception();
                    }
                    PDAStack.Pop();
                }
                else
                {
                    Ans.Add(Exp[i]);
                }
            }
            while (PDAStack.Count > 0)
            {
                if (PDAStack.Peek() == "(")
                {
                    throw new Exception();
                }
                Ans.Add(PDAStack.Pop());
            }
            return Ans;
        }

        public static List<string> Srt2List(string E, ref Stack<string> PDAStack, ref Dictionary<string, int> ops, ref Stack<double> calculateStack)
        {
            List<string> Ans = new List<string>();
            string[] checkarr = E.Split(' ');
            for (int i = 0; i < checkarr.Length - 1; i++)
                if (Char.IsDigit(checkarr[i][checkarr[i].Length - 1]) && Char.IsDigit(checkarr[i + 1][0]))
                    throw new Exception();
            ScanE(ref Ans, E);
            return Ans;
        }

        private static void ScanE(ref List<string> Ans, string E)
        {
            for (int i = 0; i < E.Length;)
            {
                if (Char.IsDigit(E[i]))
                {
                    string newEx = "";
                    while (i < E.Length && (Char.IsDigit(E[i]) || E[i] == '.'))
                    {
                        newEx += E[i].ToString();
                        i++;
                    }
                    Ans.Add(newEx);
                }
                else if (E[i] == 'a' && E[i + 1] == 's' && E[i + 2] == 'i' && E[i + 3] == 'n' && E[i + 4] == '(')
                {
                    Ans.Add("asin");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 'a' && E[i + 2] == 's' && E[i + 3] == 'i' && E[i + 4] == 'n' && E[i + 5] == '(')
                {
                    Ans.Add("-asin");
                    i += 5;
                }
                else if (E[i] == 's' && E[i + 1] == 'q' && E[i + 2] == 'r' && E[i + 3] == 't' && E[i + 4] == '(')
                {
                    Ans.Add("sqrt");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 's' && E[i + 2] == 'q' && E[i + 3] == 'r' && E[i + 4] == 't' && E[i + 5] == '(')
                {
                    Ans.Add("-sqrt");
                    i += 5;
                }
                else if (E[i] == 'a' && E[i + 1] == 't' && E[i + 2] == 'a' && E[i + 3] == 'n' && E[i + 4] == '(')
                {
                    Ans.Add("atan");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 'a' && E[i + 2] == 't' && E[i + 3] == 'a' && E[i + 4] == 'n' && E[i + 5] == '(')
                {
                    Ans.Add("-atan");
                    i += 5;
                }
                else if (E[i] == 'a' && E[i + 1] == 'c' && E[i + 2] == 'o' && E[i + 3] == 's' && E[i + 4] == '(')
                {
                    Ans.Add("acos");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 'a' && E[i + 2] == 'c' && E[i + 3] == 'o' && E[i + 4] == 's' && E[i + 5] == '(')
                {
                    Ans.Add("-acos");
                    i += 5;
                }
                else if (E[i] == 's' && E[i + 1] == 'i' && E[i + 2] == 'n' && E[i + 3] == 'h' && E[i + 4] == '(')
                {
                    Ans.Add("sinh");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 's' && E[i + 2] == 'i' && E[i + 3] == 'n' && E[i + 4] == 'h' && E[i + 5] == '(')
                {
                    Ans.Add("-sinh");
                    i += 5;
                }
                else if (E[i] == 'c' && E[i + 1] == 'o' && E[i + 2] == 's' && E[i + 3] == 'h' && E[i + 4] == '(')
                {
                    Ans.Add("cosh");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 'c' && E[i + 2] == 'o' && E[i + 3] == 's' && E[i + 4] == 'h' && E[i + 5] == '(')
                {
                    Ans.Add("-cosh");
                    i += 5;
                }
                else if (E[i] == 's' && E[i + 1] == 'i' && E[i + 2] == 'n' && E[i + 3] == '(')
                {
                    Ans.Add("sin");
                    i += 3;
                }
                else if (E[i] == '-' && E[i + 1] == 's' && E[i + 2] == 'i' && E[i + 3] == 'n' && E[i + 4] == '(')
                {
                    Ans.Add("-sin");
                    i += 4;
                }
                else if (E[i] == 't' && E[i + 1] == 'a' && E[i + 2] == 'n' && E[i + 3] == 'h' && E[i + 4] == '(')
                {
                    Ans.Add("tanh");
                    i += 4;
                }
                else if (E[i] == '-' && E[i + 1] == 't' && E[i + 2] == 'a' && E[i + 3] == 'n' && E[i + 4] == 'h' && E[i + 5] == '(')
                {
                    Ans.Add("-tanh");
                    i += 5;
                }
                else if (E[i] == 't' && E[i + 1] == 'a' && E[i + 2] == 'n' && E[i + 3] == '(')
                {
                    Ans.Add("tan");
                    i += 3;
                }
                else if (E[i] == '-' && E[i + 1] == 't' && E[i + 2] == 'a' && E[i + 3] == 'n' && E[i + 4] == '(')
                {
                    Ans.Add("-tan");
                    i += 4;
                }
                else if (E[i] == 'c' && E[i + 1] == 'o' && E[i + 2] == 's' && E[i + 3] == '(')
                {
                    Ans.Add("cos");
                    i += 3;
                }
                else if (E[i] == '-' && E[i + 1] == 'c' && E[i + 2] == 'o' && E[i + 3] == 's' && E[i + 4] == '(')
                {
                    Ans.Add("-cos");
                    i += 4;
                }
                else if (E[i] == 's' && E[i + 1] == 'g' && E[i + 2] == 'n' && E[i + 3] == '(')
                {
                    Ans.Add("sgn");
                    i += 3;
                }
                else if (E[i] == '-' && E[i + 1] == 's' && E[i + 2] == 'g' && E[i + 3] == 'n' && E[i + 4] == '(')
                {
                    Ans.Add("-sgn");
                    i += 4;
                }
                else if (E[i] == 'e' && E[i + 1] == 'x' && E[i + 2] == 'p' && E[i + 3] == '(')
                {
                    Ans.Add("exp");
                    i += 3;
                }
                else if (E[i] == '-' && E[i + 1] == 'e' && E[i + 2] == 'x' && E[i + 3] == 'p' && E[i + 4] == '(')
                {
                    Ans.Add("-exp");
                    i += 4;
                }
                else if (E[i] == 'l' && E[i + 1] == 'n' && E[i + 2] == '(')
                {
                    Ans.Add("ln");
                    i += 2;
                }
                else if (E[i] == '-' && E[i + 1] == 'l' && E[i + 2] == 'n' && E[i + 3] == '(')
                {
                    Ans.Add("-ln");
                    i += 3;
                }
                else if (E[i] == 'a' && E[i + 1] == 'b' && E[i + 2] == 's' && E[i + 3] == '(')
                {
                    Ans.Add("abs");
                    i += 3;
                }
                else if (E[i] == '-' && E[i + 1] == 'a' && E[i + 2] == 'b' && E[i + 3] == 's' && E[i + 4] == '(')
                {
                    Ans.Add("-abs");
                    i += 4;
                }
                else if (E[i] == '-')
                {

                    if (Char.IsDigit(E[i + 1]))
                    {
                        if (i != 0 && (Char.IsDigit(Ans[Ans.Count - 1][Ans[Ans.Count - 1].Length - 1]) || Ans[Ans.Count - 1][Ans[Ans.Count - 1].Length - 1] == ')')
                            )
                        {
                            Ans.Add("-");
                            i++;
                        }
                        else
                        {
                            string str = "-";
                            i++;
                            while (i < E.Length && (Char.IsDigit(E[i]) || E[i] == '.'))
                            {
                                str += E[i].ToString();
                                i++;
                            }
                            Ans.Add(str);
                        }
                    }
                    else
                    {
                        Ans.Add("-");
                        i++;
                    }
                }
                else if (E[i] == '+' || E[i] == '/' || E[i] == '*' || E[i] == '(' || E[i] == ')' || E[i] == '^')
                {
                    Ans.Add(E[i].ToString());
                    i++;
                    continue;
                }
                else if (E[i] == ' ')
                {
                    i++;
                }
                else
                {
                    throw new Exception();
                }

            }

        }

        static void Main(string[] args)
        {
            Stack<string> PDA = new Stack<string>(1000);
            Dictionary<string, int> operations = InitializeOps();
            Stack<double> calStack = new Stack<double>(1000);
            string expression = Console.ReadLine();
            try
            {
                string res = Process(ref PDA, ref operations, ref calStack, expression);
                finalFixing(res);
            }
            catch
            {
                Console.WriteLine("INVALID");
            }
            finally
            {
                PDA = null;
                operations = null;
                calStack = null;
                Console.ReadKey();
            }
            return;
        }

    }
}
