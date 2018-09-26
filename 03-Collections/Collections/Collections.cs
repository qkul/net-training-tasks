using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Collections.Tasks
{

    /// <summary>
    ///  Tree node item 
    /// </summary>
    /// <typeparam name="T">the type of tree node data</typeparam>
    public interface ITreeNode<T>
    {
        T Data { get; set; }                             // Custom data
        IEnumerable<ITreeNode<T>> Children { get; set; } // List of childrens
    }

    public class Task
    {

        /// <summary> Generate the Fibonacci sequence f(x) = f(x-1)+f(x-2) </summary>
        /// <param name="count">the size of a required sequence</param>
        /// <returns>
        ///   Returns the Fibonacci sequence of required count
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0</exception>
        /// <example>
        ///   0 => { }  
        ///   1 => { 1 }    
        ///   2 => { 1, 1 }
        ///   12 => { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144 }
        /// </example>
        public static IEnumerable<int> GetFibonacciSequence(int count)
        {
            // TODO : Implement Fibonacci sequence generator
            // return count > 1 ? GetFibonacciSequence(count - 1) + GetFibonacciSequence(count - 2) : count;
            //fix #7
            int resultItem = 0, item1 = 0, item2 = 1;
            if (count < 0)
                throw new ArgumentException();
            if (count > 0)
                yield return 1;
            for (var i = 0; i < count - 1; i++)
            {
                resultItem = item1 + item2;
                yield return resultItem;
                item1 = item2;
                item2 = resultItem;
            }
        }

        /// <summary>
        ///    Parses the input string sequence into words
        /// </summary>
        /// <param name="reader">input string sequence</param>
        /// <returns>
        ///   The enumerable of all words from input string sequence. 
        /// </returns>
        /// <exception cref="System.ArgumentNullException">reader is null</exception>
        /// <example>
        ///  "TextReader is the abstract base class of StreamReader and StringReader, which ..." => 
        ///   {"TextReader","is","the","abstract","base","class","of","StreamReader","and","StringReader","which",...}
        /// </example>
        public static IEnumerable<string> Tokenize(TextReader reader)
        {
            char[] delimeters = new[] { ',', ' ', '.', '\t', '\n' };
            // TODO : Implement the tokenizer
            if (reader == null)
                throw new ArgumentNullException();
            string str = null;
            while ((str = reader.ReadLine()) != null)
            {//The ReadLine method reads a line from the standard input stream
                var words = str.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    yield return word;
                }
            }
        }

        /// <summary>
        ///   Traverses a tree using the depth-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in depth-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  6  7
        ///                  / \     \
        ///                 3   4     8
        ///                     |
        ///                     5   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        ////  IEnumerable<ITreeNode<T>> Children { get; set; } // List of childrens
        public static void AddChildrenStack<T>(ITreeNode<T> root, Stack<ITreeNode<T>> stack)
        {
            if (root.Children == null)
                return;
            List<ITreeNode<T>> listChilderns = new List<ITreeNode<T>>(root.Children);
            // foreach (var item in listChilderns)           
            //    stack.Push(item);
            for (var i = listChilderns.Count - 1; i >= 0; i--)
            {
                stack.Push(listChilderns[i]);
            }
        }
        public static IEnumerable<T> DepthTraversalTree<T>(ITreeNode<T> root)
        {
            // TODO : Implement the tree depth traversal algorithm
            if (root == null)
                throw new ArgumentNullException("root");
            Stack<ITreeNode<T>> stack = new Stack<ITreeNode<T>>();
            stack.Push(root);
            while (stack.Count != 0)
            {
                yield return stack.Peek().Data;
                AddChildrenStack(stack.Pop(), stack);
            }
        }

        /// <summary>
        ///   Traverses a tree using the width-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in width-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  3  4
        ///                  / \     \
        ///                 5   6     7
        ///                     |
        ///                     8   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static void AddChildrenQueues<T>(ITreeNode<T> root, Queue<ITreeNode<T>> queue)
        {
            if (root.Children == null)
                return;

            List<ITreeNode<T>> listChilderns = new List<ITreeNode<T>>(root.Children);
            foreach (var item in listChilderns)
                queue.Enqueue(item);//adds an object to the end of the list

        }
        public static IEnumerable<T> WidthTraversalTree<T>(ITreeNode<T> root)
        {
            // TODO : Implement the tree width traversal algorithm
            if (root == null)
                throw new ArgumentNullException("root");
            Queue<ITreeNode<T>> queues = new Queue<ITreeNode<T>>();
            queues.Enqueue(root);
            while (queues.Count != 0)
            {
                yield return queues.Peek().Data;
                AddChildrenQueues(queues.Dequeue(), queues);
            }
        }

        /// <summary>
        ///   Generates all permutations of specified length from source array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">source array</param>
        /// <param name="count">permutation length</param>
        /// <returns>
        ///    All permuations of specified length
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0 or greater then the source length</exception>
        /// <example>
        ///   source = { 1,2,3,4 }, count=1 => {{1},{2},{3},{4}}
        ///   source = { 1,2,3,4 }, count=2 => {{1,2},{1,3},{1,4},{2,3},{2,4},{3,4}}
        ///   source = { 1,2,3,4 }, count=3 => {{1,2,3},{1,2,4},{1,3,4},{2,3,4}}
        ///   source = { 1,2,3,4 }, count=4 => {{1,2,3,4}}
        ///   source = { 1,2,3,4 }, count=5 => ArgumentOutOfRangeException
        /// </example
        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count)
        {
            // TODO : Implement GenerateAllPermutations method
            if (count > source.Length)
                throw new ArgumentOutOfRangeException();

            for (var i = 0; i < Math.Pow(2, source.Length); i++)
            {//2^source -> количество возможных вариантов 
                List<T> list = new List<T>();
                int counter = 0;
                for (int j = 0; j < source.Length; j++)
                {//нахождения булеана 
                    if (((1 << j) & i) != 0)
                    {
                        list.Add(source[j]);
                        counter++;
                        if (counter > count)
                            break;
                    }
                }
                if (counter == count)
                    yield return list.ToArray();
            }
        }
    }
    public static class DictionaryExtentions
    {

        /// <summary>
        ///    Gets a value from the dictionary cache or build new value
        /// </summary>
        /// <typeparam name="TKey">TKey</typeparam>
        /// <typeparam name="TValue">TValue</typeparam>
        /// <param name="dictionary">source dictionary</param>
        /// <param name="key">key</param>
        /// <param name="builder">builder function to build new value if key does not exist</param>
        /// <returns>
        ///   Returns a value assosiated with the specified key from the dictionary cache. 
        ///   If key does not exist than builds a new value using specifyed builder, puts the result into the cache 
        ///   and returns the result.
        /// </returns>
        /// <example>
        ///   IDictionary<int, Person> cache = new SortedDictionary<int, Person>();
        ///   Person value = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should return a loaded Person and put it into the cache
        ///   Person cached = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should get a Person from the cache
        /// </example>
        public static TValue GetOrBuildValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> builder)
        {
            // TODO : Implement GetOrBuildValue method for cache
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }
            //fix #9
            var temp = builder();
            dictionary.Add(key, temp);
            return temp;
        }

    }
}
