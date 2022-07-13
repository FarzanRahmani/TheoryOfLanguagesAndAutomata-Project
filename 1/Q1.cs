using System;
using System.Collections.Generic;
using System.Linq;

namespace _1
{
    // class Transition
    // {
    //     public string alphabet;
    //     public string preState;
    //     public string nextState;

    //     public Transition(string alphabet, string preState, string nextState)
    //     {
    //         this.alphabet = alphabet;
    //         this.preState = preState;
    //         this.nextState = nextState;
    //     }
    // }
    // class State
    // {

    //     public bool isFinal;
    //     public string name;
    //     // List<State> NextStates;

    //     public State(string name)
    //     {
    //         this.name = name;
    //         this.isFinal = false;
    //         // NextStates = new List<State>();
    //     }
    // }
    class Q1
    {
        static void Main1(string[] args)
        {
            string[] temp = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');
            State[] states = new State[temp.Length];
            for (int i = 0; i < temp.Length; i++)
                states[i] = new State(temp[i]);
            State initialState = states[0];

            string[] alphabet = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');

            temp = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');
            for (int i = 0; i < temp.Length; i++)
                states.Where(x => x.name == temp[i]).First().isFinal = true;

            int numOfTransitions = int.Parse(Console.ReadLine());
            Transition[] trans = new Transition[numOfTransitions];
            for (int i = 0; i < numOfTransitions; i++)
            {
                temp = Console.ReadLine().Split(',');
                trans[i] = new Transition(temp[1], temp[0], temp[2]);
            }

            string input = Console.ReadLine();
            foreach (char tran in input)
            {
                Transition t = trans.Where(x => initialState.name == x.preState && x.alphabet == tran.ToString()).FirstOrDefault();
                if (t == null)
                {
                    if (nfaChecking(states, alphabet, trans, input))
                    {
                        Console.WriteLine("Accepted");
                        return;
                    }
                    Console.WriteLine("Rejected");
                    return;
                }
                initialState = states.Where(x => x.name == t.nextState).First();
            }
            if (initialState.isFinal)
            {
                Console.WriteLine("Accepted");
            }
            else
            {
                Console.WriteLine("Rejected");
            }
        }

        private static bool nfaChecking(State[] states, string[] alphabet, Transition[] trans, string input)
        {
            List<State> traverseStates = new List<State>();
            traverseStates.Add(states[0]);
            foreach (Transition t in trans)
            {
                if (t.alphabet == "$") // tajayee ke momkene nemire
                    if (t.preState == states[0].name)
                        traverseStates.Add(states.Where(x => x.name == t.nextState).First());
            }
            foreach (char tran in input)
            {
                List<State> NextStates = new List<State>();
                foreach (State s in traverseStates)
                {
                    Transition[] ts = trans.Where(x => s.name == x.preState && x.alphabet == tran.ToString()).ToArray();
                    foreach (Transition t1 in ts)
                    {
                        NextStates.Add(states.Where(x => x.name == t1.nextState).First());
                    }
                }

                List<State> temp = new List<State>(NextStates);
                foreach (State s in NextStates)
                    foreach (Transition t1 in trans)
                    {
                        if (t1.alphabet == "$")
                            if (t1.preState == s.name)
                                temp.Add(states.Where(x => x.name == t1.nextState).First());
                    }

                traverseStates = temp;
            }
            foreach (State s in traverseStates)
            {
                if (s.isFinal)
                    return true;
            }
            return false;

        }
    }
}
