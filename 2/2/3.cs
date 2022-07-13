using System;
using System.Collections.Generic;
using System.Linq;
namespace _2
{
    struct Tran
    {
        public int startState;
        public char Read;
        public int EndState;
        public char Write;
        public int LorR; // L --> -1 , R--> +1

        public Tran(int startState, char read, int endState, char write, int lorR)
        {
            this.startState = startState;
            Read = read;
            EndState = endState;
            Write = write;
            LorR = lorR;
        }
    }
    class Program
    {
        public static void Main(string[] Args)
        {
            string turingMachine = Console.ReadLine();
            // blank = 1
            // a = 11
            // b = 111
            // c = 1111
            // L = 1
            // R = 11
            // q1 = 1 --> start
            // q2 = 11
            // qn = 1^n --> final
            // List<string> Transitions = turingMachine.Split("00").ToList(); // 101101011011001010110101
            List<string> Transitions = new List<string>(); // 101101011011001010110101

            int pointer1 = 0;
            int start = 0;
            for (int pointer2 = 1; pointer2 < turingMachine.Length; pointer2++)
            {
                if (turingMachine[pointer1] == '0' && turingMachine[pointer2] == '0')
                {
                    Transitions.Add(turingMachine.Substring(start,pointer1-start));
                    start = pointer2 + 1;
                }
                pointer1++;
            }
            Transitions.Add(turingMachine.Substring(start));

            List<Tran> ControlUnit = new List<Tran>();
            int finalState = 1; // 1 11 111 1111 11111 ...
            foreach (string transition in Transitions)
            {
                List<string> tmp = transition.Split('0').ToList(); // 1,11,1,11,11
                int tmpStartS = int.Parse(tmp[0]);
                int tmpEndS = int.Parse(tmp[2]);
                if (tmpEndS > finalState)
                    finalState = tmpEndS;
                int tmpLorR = 0;
                if (tmp[4] == "1")
                    tmpLorR = -1;
                else
                    tmpLorR = +1;
                char tmpRead = CodeToChar(tmp[1]);
                char tmpWrite = CodeToChar(tmp[3]);
                ControlUnit.Add(new Tran(tmpStartS, tmpRead, tmpEndS, tmpWrite, tmpLorR));
            }

            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string input = Console.ReadLine(); // 11011011
                List<char> tape = new List<char>(){'-','-','-','-','-',
                '-','-','-','-','-',
                '-','-','-','-','-',}; // - = blank // pointer 15 start // q1
                string[] tmp = input.Split('0');
                foreach (string t in tmp)
                {
                    tape.Add(CodeToChar(t));
                }
                tape.AddRange(new List<char>(){'-','-','-','-','-',
                '-','-','-','-','-',
                '-','-','-','-','-',}); // - = blank
                if (Process(tape, ControlUnit,1,finalState,15))
                    System.Console.WriteLine("Accepted");
                else
                    System.Console.WriteLine("Rejected");
            }
        }

        private static bool Process(List<char> tape, List<Tran> controlUnit, int startState, int finalState, int head)
        {
            char currenReadSymbol = tape[head];
            int currentState = startState;
            while (true)
            {
                Tran tmp;
                try 
                {
                    tmp = controlUnit.Where(t => t.startState == currentState && t.Read == currenReadSymbol).First();
                }
                catch (System.Exception)
                {
                    break; // halt in non-final
                }
                if (tmp.EndState == finalState)
                    return true;
                tape[head] = tmp.Write;
                head += tmp.LorR;
                currentState = tmp.EndState;
                currenReadSymbol = tape[head];
            }
            return false;
        }

        private static char  CodeToChar(string codedChar) 
        {
            // 1 : - blank
            // 11 : a
            // 111 : b
            // 1111 : c
            if (codedChar == "")
                return '-';
            string alphabet = "-abcdefghijklmnopqrstuvwxyz";
            return alphabet[codedChar.Length - 1];
        }
    }
}