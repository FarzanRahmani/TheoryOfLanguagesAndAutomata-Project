using System;
using System.Collections.Generic;
using System.Linq;

namespace _1
{
    class NextState
    {
        public State next;
        public string alphab;

        public NextState(State next, string alphab)
        {
            this.next = next;
            this.alphab = alphab;
        }
    }
    class Transition
    {
        public string alphabet;
        public string preState;
        public string nextState;

        public Transition(string alphabet, string preState, string nextState)
        {
            this.alphabet = alphabet;
            this.preState = preState;
            this.nextState = nextState;
        }
    }
    class State
    {

        public bool isFinal;
        public bool isReachable;
        public string name;
        public List<NextState> NextStates;

        public State(string name)
        {
            this.name = name;
            this.isFinal = false;
            this.isReachable = false;
            NextStates = new List<NextState>();
        }
    }
    class Q3
    {
        static int numOfGroups;
        static void Main3(string[] args)
        {
            string[] temp = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');
            State[] dfaStates = new State[temp.Length];
            for (int i = 0; i < temp.Length; i++)
                dfaStates[i] = new State(temp[i]);
            State nfaInitialState = dfaStates[0];

            string[] alphabet = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');

            temp = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');
            for (int i = 0; i < temp.Length; i++)
                dfaStates.Where(x => x.name == temp[i]).First().isFinal = true;

            int numOfDfaTransitions = int.Parse(Console.ReadLine());
            Transition[] trans = new Transition[numOfDfaTransitions];
            for (int i = 0; i < numOfDfaTransitions; i++)
            {
                temp = Console.ReadLine().Split(',');
                trans[i] = new Transition(temp[1], temp[0], temp[2]);
                State pre = dfaStates.Where(x => x.name == temp[0]).FirstOrDefault();
                State nex = dfaStates.Where(x => x.name == temp[2]).FirstOrDefault();
                pre.NextStates.Add(new NextState(nex, temp[1]));
            }

            Traverse(dfaStates);
            List<State> reachableStates = new List<State>();
            foreach (State s in dfaStates)
            {
                if (s.isReachable == true)
                    reachableStates.Add(s);
            }

            Dictionary<State, int> groups = new Dictionary<State, int>();
            foreach (State s in reachableStates)
            {
                if (s.isFinal == false)
                    groups[s] = 1;
                else
                    groups[s] = 2;
            }
            numOfGroups = 2;

            bool divided = true;
            while (divided) // max = reachableStates.Count
            {
                Dictionary<State, int> temporary = new Dictionary<State, int>(groups);
                divided = false;
                int flag = numOfGroups;
                for (int i = 1; i <= flag; i++)
                {
                    var pivot = groups.Where(p => p.Value == i).First();
                    bool isDividedGroups = false;
                    foreach (var g in groups.Where(p => p.Value == i))
                    {
                        foreach (string alpha in alphabet)
                        {
                            if (groups[g.Key.NextStates.Where(n => n.alphab == alpha).First().next]
                            != groups[pivot.Key.NextStates.Where(n => n.alphab == alpha).First().next])
                            {
                                temporary[g.Key] = numOfGroups + 1;
                                isDividedGroups = true;
                                divided = true;
                                break;
                            }
                        }
                    }
                    if (isDividedGroups)
                        numOfGroups++;
                }
                groups = temporary;
            }

            Console.WriteLine(numOfGroups);
            Console.ReadKey();
        }


        private static void Traverse(State[] dfaStates)
        {
            dfaStates[0].isReachable = true;
            Explore(dfaStates[0]);
        }

        private static void Explore(State state)
        {
            state.isReachable = true;
            foreach (var n in state.NextStates)
            {
                if (n.next.isReachable == false)
                {
                    Explore(n.next);
                }
            }
        }
    }
}