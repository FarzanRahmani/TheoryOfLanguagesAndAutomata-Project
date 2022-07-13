using System;
using System.Collections.Generic;
using System.Linq;

namespace _1
{
    // class NextState
    // {
    //     public State next;
    //     public string alphab;

    //     public NextState(State next, string alphab)
    //     {
    //         this.next = next;
    //         this.alphab = alphab;
    //     }
    // }
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
    //     public List<NextState> NextStates;

    //     public State(string name)
    //     {
    //         this.name = name;
    //         this.isFinal = false;
    //         NextStates = new List<NextState>();
    //     }
    // }
    class Q2
    {
        static void Main2(string[] args)
        {
            string[] temp = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');
            State[] nfaStates = new State[temp.Length];
            for (int i = 0; i < temp.Length; i++)
                nfaStates[i] = new State(temp[i]);
            State nfaInitialState = nfaStates[0];

            string[] alphabet = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');

            temp = Console.ReadLine().Trim(new char[] { '{', '}' }).Split(',');
            for (int i = 0; i < temp.Length; i++)
                nfaStates.Where(x => x.name == temp[i]).First().isFinal = true;

            int numOfDfaTransitions = int.Parse(Console.ReadLine());
            Transition[] trans = new Transition[numOfDfaTransitions];
            for (int i = 0; i < numOfDfaTransitions; i++)
            {
                temp = Console.ReadLine().Split(',');
                trans[i] = new Transition(temp[1], temp[0], temp[2]);
                State pre = nfaStates.Where(x => x.name == temp[0]).FirstOrDefault();
                State nex = nfaStates.Where(x => x.name == temp[2]).FirstOrDefault();
                pre.NextStates.Add(new NextState(nex, temp[1]));
            }


            Queue<State> unProcessed = new Queue<State>();

            string g = nfaInitialState.name;
            char[] charR = g.ToCharArray();
            Array.Sort(charR);
            g = "";
            foreach (var c in charR)
            {
                g += c.ToString();
            }
            nfaInitialState.name = g;

            unProcessed.Enqueue(nfaInitialState);
            List<State> dfaNewStates = new List<State>();
            dfaNewStates.Add(nfaInitialState);
            while (unProcessed.Count > 0)
            {
                State tmp = unProcessed.Dequeue();
                foreach (string a in alphabet)
                {
                    List<State> traverseStates = new List<State>();
                    List<State> reachedStates = new List<State>();
                    /// x
                    foreach (var n in tmp.NextStates)
                    {
                        if (n.alphab == a)
                            if (!reachedStates.Contains(n.next))
                                reachedStates.Add(n.next);
                    }
                    //// x$
                    List<State> temporary = new List<State>(reachedStates);
                    foreach (var t in reachedStates)
                    {
                        foreach (var n in t.NextStates)
                        {
                            if (n.alphab == "$")
                                if (!temporary.Contains(n.next))
                                    temporary.Add(n.next);
                        }
                    }
                    reachedStates = temporary;
                    //// x$$
                    temporary = new List<State>(reachedStates);
                    foreach (var t in reachedStates)
                    {
                        foreach (var n in t.NextStates)
                        {
                            if (n.alphab == "$")
                                if (!temporary.Contains(n.next))
                                    temporary.Add(n.next);
                        }
                    }
                    reachedStates = temporary;
                    //// $x 
                    foreach (var n in tmp.NextStates)
                    {
                        if (n.alphab == "$")
                            traverseStates.Add(n.next);
                    }

                    // $$x
                    temporary = new List<State>(traverseStates);
                    foreach (var n in traverseStates)
                    {
                        foreach (var l in n.NextStates)
                        {
                            if (l.alphab == "$")
                                if (!temporary.Contains(l.next))
                                    temporary.Add(l.next);
                        }
                    }
                    traverseStates = temporary;

                    foreach (var t in traverseStates)
                    {
                        foreach (var n in t.NextStates)
                        {
                            if (n.alphab == a)
                                if (!reachedStates.Contains(n.next))
                                    reachedStates.Add(n.next);
                        }
                    }

                    string res = ""; // q2q4 q4q2
                    List<NextState> resNext = new List<NextState>();
                    foreach (State s in reachedStates)
                    {
                        res += s.name;
                        resNext.AddRange(s.NextStates);
                    }
                    // res.OrderBy(a => a.ToString());
                    // Array.Sort(res);
                    // String.Concat(res.OrderBy(c => c));
                    char[] charRes = res.ToCharArray();
                    Array.Sort(charRes);
                    res = "";
                    foreach (var c in charRes)
                    {
                        res += c.ToString();
                    }


                    bool found = false;
                    for (int i = 0; i < dfaNewStates.Count; i++)
                    {
                        if (dfaNewStates[i].name == res)
                            found = true;
                    }
                    if (!found)
                    {
                        State addingState = new State(res);
                        addingState.NextStates = resNext;
                        dfaNewStates.Add(addingState);
                        unProcessed.Enqueue(addingState);
                    }

                }


            }

            System.Console.WriteLine(dfaNewStates.Count);
            Console.ReadKey();
        }
    }
}