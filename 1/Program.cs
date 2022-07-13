using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimizeDFA
{
    /// <summary>
	/// Represents a partition of elements into sets.  Based on algrothm presented in
	/// "Fast brief practical DFA minimization" by Antti Valmari 2011.  Here elements
	/// are representing as integers from 0 to N-1
	/// </summary>
	public class Partition
	{
		private int setCount; // z - the number of sets
		private readonly int[] elements; // E[f], E[f+1],...E[p-1] The elements of the set s where f=first[s] and p=past[s]
		private readonly int[] location; // L[e] the location of element e
		// TODO change setOf to int? since not all elements are in a set
		private readonly int[] setOf; // S[e] the set the element e belongs to
		private readonly int[] first; // F[s] the first element of set s
		private readonly int[] past; // P[s] the element past the end of set s

		// For simplicity we do not share the next two data structures
		private readonly int[] marked; // M[s] the number of marked elements in set s
		private readonly List<int> touched; // W touched (i.e. contain marked) sets, replaces W[] and w in the paper

		public Partition(int elementCount)
		{
			elements = new int[elementCount];
			location = new int[elementCount];
			setOf = new int[elementCount];
			first = new int[elementCount];
			past = new int[elementCount];
			marked = new int[elementCount];
			touched = new List<int>(elementCount); // capacity so we never have to worry about resize

			for(var i = 0; i < elementCount; ++i)
				elements[i] = location[i] = i;

			if(elementCount > 0)
			{
				setCount = 1;
				first[0] = 0;
				past[0] = elementCount;
			}
			else
				setCount = 0;
		}

		public int SetCount => setCount;

		public void Mark(int element)
		{
			var set = setOf[element];
			if(set == -1) return; // not in any set
			var elementIndex = location[element];
			var firstUnmarked = first[set] + marked[set];
			if(elementIndex < firstUnmarked) return; // already marked

			// swap element and the first unmarked in elements, updating location appropriately
			elements[elementIndex] = elements[firstUnmarked];
			location[elements[elementIndex]] = elementIndex;
			elements[firstUnmarked] = element;
			location[element] = firstUnmarked;

			// track how many marked in the set, and if it was touched.
			if(marked[set]++ == 0)
				touched.Add(set);
		}

		/// <summary>
		/// Split each set into the marked and unmarked subsets
		/// </summary>
		public void SplitSets()
		{
			foreach(var set in touched)
			{
				var firstUnmarked = first[set] + marked[set];

				// if the whole set was marked
				if(firstUnmarked == past[set])
				{
					// just unmark it and do nothing
					marked[set] = 0;
					continue;
				}

				// Make the smaller half a new set
				// If same size, then make a new set out of unmarked
				if(marked[set] <= past[set] - firstUnmarked)
				{
					first[setCount] = first[set];
					past[setCount] = first[set] = firstUnmarked;
				}
				else
				{
					past[setCount] = past[set];
					first[setCount] = past[set] = firstUnmarked;
				}

				// mark the elements as members of the new set
				for(var i = first[setCount]; i < past[setCount]; ++i)
					setOf[elements[i]] = setCount;

				// clear marks on old and new set
				marked[set] = marked[setCount] = 0;

				// increase set count
				setCount++;
			}
			touched.Clear();
		}

		/// <summary>
		/// Discard the unmarked from each set, they are no longer in any set (-1)
		/// </summary>
		public void DiscardUnmarked()
		{
			foreach(var set in touched)
			{
				var firstUnmarked = first[set] + marked[set];

				// if the whole set was marked
				if(firstUnmarked == past[set])
				{
					// just unmark it and do nothing
					marked[set] = 0;
					continue;
				}

				var pastUnmarked = past[set];
				past[set] = firstUnmarked;

				// mark the elements as members no set
				for(var i = firstUnmarked; i < pastUnmarked; ++i)
					setOf[elements[i]] = -1;

				// clear mark on set
				marked[set] = 0;
			}
			touched.Clear();
		}

		public IEnumerable<int> MarkedInSet(int set)
		{
			// The algorithm relies on the fact that you can mark nodes and they will be added to this IEnumerable
			var firstOfSet = first[set];
			for(var i = firstOfSet; i < firstOfSet + marked[set]; i++)
				yield return elements[i];
		}

		public IEnumerable<int> Set(int set)
		{
			// Assumes set is >=0 and < setCount
			var firstOfSet = first[set];
			return Enumerable.Range(firstOfSet, past[set] - firstOfSet).Select(i => elements[i]);
		}

		public int SetOf(int element)
		{
			return setOf[element];
		}

		public void PartitionBy(Func<int, int> partitionFunc)
		{
			if(elements.Length == 0) return;

			// Wipes out any existing sets
			setCount = marked[0] = 0;

			// Sort them by the partition func so they will be together
			var partition = elements.Select(partitionFunc).ToArray();
			Array.Sort(partition, elements);

			// Create sets for each partition
			first[0] = 0; // The first set starts at 0
			var currentPartition = partition[0];
			for(var i = 0; i < elements.Length; ++i)
			{
				var element = elements[i];
				if(partition[i] != currentPartition)
				{
					currentPartition = partition[i];
					past[setCount++] = i;
					first[setCount] = i;
					marked[setCount] = 0;
				}
				setOf[element] = setCount;
				location[element] = i;
			}
			past[setCount++] = elements.Length;
		}

		public int SomeElementOf(int set)
		{
			return elements[first[set]];
		}
	}
    public struct Transition
	{
		public readonly int From;
		public readonly int OnInput;
		public readonly int To;

		public Transition(int @from, int onInput, int to)
		{
			From = @from;
			OnInput = onInput;
			To = to;
		}
	}
    public class AdjacentTransitions
	{
		private readonly int[] adjacent; // transition indexes grouped by state they are adjacent to
		private readonly int[] offset; // offsets into adjacent list for a given state

		public AdjacentTransitions(int stateCount, IList<Transition> transitions, Func<Transition, int> getState)
		{
			adjacent = new int[transitions.Count];
			offset = new int[stateCount + 1];

			// Count transitions per state
			foreach(var transition in transitions)
				++offset[getState(transition)];

			// Running addition
			for(var state = 0; state < stateCount; ++state)
			{
				offset[state + 1] += offset[state];
			}

			// Place transitions, and correct offsets
			for(var transition = transitions.Count - 1; transition >= 0; transition--)
				adjacent[--offset[getState(transitions[transition])]] = transition;
		}

		public IEnumerable<int> this[int state]
		{
			get
			{
				for(var i = offset[state]; i < offset[state + 1]; ++i)
					yield return adjacent[i];
			}
		}
	}

    public class DFA
	{
		private readonly List<Transition> transitions = new List<Transition>();
		private readonly HashSet<int> finalStates = new HashSet<int>();

		public DFA(int stateCount, int startState)
		{
			StateCount = stateCount;
			StartState = startState;
		}

		public int StateCount { get; }
		public int StartState { get; }
		public IReadOnlyList<Transition> Transitions => transitions;
		public IReadOnlyCollection<int> FinalStates => finalStates;

		public void AddTransition(int fromState, int onInput, int toState)
		{
			transitions.Add(new Transition(fromState, onInput, toState));
		}
		public void AddFinalState(int state)
		{
			finalStates.Add(state);
		}

		public DFA Minimize()
		{
			// We will be modifying this list of transitions and we don't want to mess up our own
			var transitions = new List<Transition>(this.transitions);

			var blocks = new Partition(StateCount);

			// Reachable from start
			blocks.Mark(StartState);

			DiscardNotReachable(blocks, transitions, t => t.From, t => t.To);

			// Reachable from final
			foreach(var finalState in finalStates)
				blocks.Mark(finalState);

			DiscardNotReachable(blocks, transitions, t => t.To, t => t.From);

			// Split final states from non-final
			foreach(var finalState in finalStates)
				blocks.Mark(finalState);

			blocks.SplitSets();

			// Cords partition to manage transitions
			var cords = new Partition(transitions.Count);

			// Split transitions by input
			cords.PartitionBy(transition => transitions[transition].OnInput);

			//Split blocks and cords
			var adjacentTransitions = new AdjacentTransitions(StateCount, transitions, t => t.To);
			var blockSet = 1;
			for(var cordSet = 0; cordSet < cords.SetCount; cordSet++)
			{
				foreach(var transition in cords.Set(cordSet))
					blocks.Mark(transitions[transition].From);

				blocks.SplitSets();

				for(; blockSet < blocks.SetCount; blockSet++)
				{
					foreach(var state in blocks.Set(blockSet))
						foreach(var transition in adjacentTransitions[state])
							cords.Mark(transition);

					cords.SplitSets();
				}
			}

			// Generate minimized DFA
			var minDFA = new DFA(blocks.SetCount, blocks.SetOf(StartState));

			// Set Final States
			foreach(var finalState in finalStates)
			{
				var set = blocks.SetOf(finalState);
				if(set != -1) // not all final states may have been reachable
					minDFA.AddFinalState(set);
			}

			// Create transitions
			for(var set = 0; set < cords.SetCount; set++)
			{
				var transition = transitions[cords.SomeElementOf(set)];
				var @from = blocks.SetOf(transition.From);
				var to = blocks.SetOf(transition.To);
				minDFA.AddTransition(@from, transition.OnInput, to);
			}

			return minDFA;
		}

		private void DiscardNotReachable(Partition blocks, List<Transition> transitions, Func<Transition, int> getFrom, Func<Transition, int> getTo)
		{
			var adjacentTransitions = new AdjacentTransitions(StateCount, transitions, getFrom);

			foreach(var state in blocks.MarkedInSet(0))
				foreach(var transition in adjacentTransitions[state])
					blocks.Mark(getTo(transitions[transition]));

			blocks.DiscardUnmarked();

			transitions.RemoveAll(transition => blocks.SetOf(getFrom(transition)) == -1);
		}
	}

    class Program
    {
        static void Main(string[] args)
        {
            var states = Console.ReadLine().Split(new char[]{'{', ',', '}'}, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> int_states = new Dictionary<string, int>();
            for(int i = 0; i < states.Length; i++)
                int_states.Add(states[i], i);
            var initialState = int_states[states[0]];
            var alphabets = Console.ReadLine().Split(new char[]{'{', ',', '}'}, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> int_alphabets = new Dictionary<string, int>();
            for(int i = 0; i < alphabets.Length; i++)
                int_alphabets.Add(alphabets[i], i);

            var finalStates = Console.ReadLine().Split(new char[]{'{', ',', '}'}, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> int_finalStates = new Dictionary<string, int>();
            for(int i = 0; i < finalStates.Length; i++)
                int_finalStates.Add(finalStates[i], int_states[finalStates[i]]);
            var transitionCount = int.Parse(Console.ReadLine());
			var finalStateCount = finalStates.Length;
			// Create DFA
			var dfa = new DFA(states.Length, initialState);

			// Read Transitions
			for(var i = 0; i < transitionCount; i++)
			{
				var transition = Console.ReadLine().Split(new char[]{','});
				int fromState = int_states[transition[0]];
				int input = int_alphabets[transition[1]];
				int toState = int_states[transition[2]];
				dfa.AddTransition(fromState, input, toState);
			}

            for(var i = 0; i < finalStateCount; i++)
			{
				int state = int_finalStates[finalStates[i]];
				dfa.AddFinalState(state);
            }

            var minDfa = dfa.Minimize();
            System.Console.WriteLine(minDfa.StateCount);
        }
    }
}
