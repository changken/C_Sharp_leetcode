using System.Collections.Generic;
using System.Text;

class MyComparer : IComparer<KeyValuePair<string, int>>
{
    public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
    {
        if (x.Value == y.Value)
        {
            return x.Key.CompareTo(y.Key);
        }
        return y.Value - x.Value;
    }
}

public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int x) { val = x; }
}

class Solution
{
    public int[] FindOrder(int numCourses, int[][] prerequisites)
    {
        Dictionary<int, List<int>> prereq = new Dictionary<int, List<int>>();
        for (int i = 0; i < numCourses; i++)
        {
            prereq.Add(i, new List<int>());
        }

        // 先修的AdjList
        foreach (int[] crs in prerequisites)
        {
            prereq[crs[0]].Add(crs[1]);
        }

        List<int> output = new List<int>();
        HashSet<int> visited = new HashSet<int>();
        HashSet<int> cycle = new HashSet<int>();

        bool dfs(int crs)
        {
            if (cycle.Contains(crs))
            {
                Console.WriteLine("cycle " + crs);
                return false;
            }
            if (visited.Contains(crs))
            {
                Console.WriteLine("visited " + crs);
                //foreach(int e in visited)
                //{
                //    Console.WriteLine(e);
                //}
                return true;
            }

            cycle.Add(crs);
            foreach (int pre in prereq[crs])
            {
                if (!dfs(pre))
                    return false;
            }
            /*
             foreach (int o in output)
                {
                    Console.WriteLine(o);
                }*/
            cycle.Remove(crs);
            visited.Add(crs);
            output.Add(crs);
            return true;
        }

        for (int crs = 0; crs < numCourses; crs++)
        {
            if (!dfs(crs))
                return new int[] { };
        }
        return output.ToArray();
    }
    public IList<string> TopKFrequent(string[] words, int k)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (string w in words)
        {
            if (freq.ContainsKey(w))
            {
                freq[w]++;
            }
            else
            {
                freq[w] = 1;
            }
        }

        PriorityQueue<string, KeyValuePair<string, int>> que
            = new PriorityQueue<string, KeyValuePair<string, int>>(new MyComparer());

        foreach (KeyValuePair<string, int> pair in freq)
        {
            que.Enqueue(pair.Key, pair);
            //if (que.Count > k)
            //{
            //    Console.WriteLine("Dequeue");
            //    que.Dequeue();
            //}
        }

        List<string> res = new List<string>();

        while (k > 0 && que.Count > 0)
        {
            res.Add(que.Dequeue());
            k--;
        }

        return res;
    }

    public string DecodeString(string s)
    {
        Stack<string> st = new Stack<string>();
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != ']')
            {
                st.Push(s[i] + "");
                //Console.WriteLine(st.Count);
            }
            else
            {
                StringBuilder substr = new StringBuilder();
                while (st.Count > 0 && !st.Peek().Equals("["))
                {
                    substr.Insert(0, st.Pop());
                }
                st.Pop(); //將 [ 彈出

                //將數字統計起來
                int l = 0;
                StringBuilder ks = new StringBuilder();
                while (st.Count > 0 && int.TryParse(st.Peek(), out l))
                {
                    ks.Insert(0, st.Pop());
                }

                //將ks變成integer
                int k = int.Parse(ks.ToString());
                StringBuilder substrRepeatK = new StringBuilder();

                //substr重複k次再推入stack
                for (int j = 0; j < k; j++)
                {
                    substrRepeatK.Append(substr);
                }
                st.Push(substrRepeatK.ToString());
            }
        }

        return String.Join("", st.ToArray().Reverse());
    }

    public IList<int> DistanceK(TreeNode root, TreeNode target, int k)
    {
        Dictionary<int, List<int>> adjList = new Dictionary<int, List<int>>();
        adjList.Add(root.val, new List<int>());
        buildGraph(adjList, root, root.left);
        buildGraph(adjList, root, root.right);
        printAdjList(adjList);
        return BFS(adjList, target.val, k);
    }

    void printAdjList(Dictionary<int, List<int>> adjList)
    {
        foreach (int src in adjList.Keys)
        {
            Console.Write(String.Format("key: {0},\t", src));
            foreach (int dst in adjList[src])
            {
                Console.Write(String.Format("{0}\t", dst));
            }
            Console.WriteLine();
        }
    }

    void printQueue(Queue<int> q)
    {
        StringBuilder r = new StringBuilder();
        foreach (int e in q)
        {
            r.Append(String.Format("{0}, ", e));
        }
        Console.WriteLine(r);
    }

    void buildGraph(Dictionary<int, List<int>> adjList, TreeNode parent, TreeNode child)
    {
        if (child == null)
        {
            return;
        }

        adjList[parent.val].Add(child.val);
        adjList.Add(child.val, new List<int>());
        adjList[child.val].Add(parent.val);

        buildGraph(adjList, child, child.left);
        buildGraph(adjList, child, child.right);
    }

    IList<int> BFS(Dictionary<int, List<int>> adjList, int target, int k)
    {
        //BFS
        HashSet<int> visited = new HashSet<int>();
        Queue<int> que = new Queue<int>();
        que.Clear();
        visited.Add(target);
        que.Enqueue(target);

        while (k > 0 && que.Any())
        {
            Console.WriteLine(String.Format("k={0}", k));
            int size = que.Count;
            for(int i = 0; i < size; i++) { 
                int cur = que.Dequeue();
                Console.WriteLine(cur);
                for (int j = 0; j < adjList[cur].Count; j++)
                {
                    if (!visited.Contains(adjList[cur][j]))
                    {
                        visited.Add(adjList[cur][j]);
                        que.Enqueue(adjList[cur][j]);
                    }
                }
                printQueue(que);
            }
            k--;
        }

        return que.ToList();
    }

    public int RomanToInt(string s)
    {
        // 比下一個大+，比下一個小則-
        Dictionary<char, int> roman = new Dictionary<char, int>();
        roman.Add('I', 1);
        roman.Add('V', 5);
        roman.Add('X', 10);
        roman.Add('L', 50);
        roman.Add('C', 100);
        roman.Add('D', 500);
        roman.Add('M', 1000);

        int total = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (i + 1 < s.Length && roman[s[i]] < roman[s[i + 1]])
            {
                total -= roman[s[i]];
            }
            else
            {
                total += roman[s[i]];
            }
        }

        return total;
    }

    /*
    public string DecodeString(string s)
    {
        Stack<string> st = new Stack<string>();
        for(int i=0; i<s.Length; i++)
        {
            if (s[i] != ']')
            {
                st.Append(s[i]  + "");
                Console.WriteLine(st.Count);
            }
            else
            {
                StringBuilder substr = new StringBuilder();
                while (st.Count > 0 && !st.Peek().Equals("["))
                {
                    substr.Insert(0, st.Pop());
                }
                st.Pop(); //將 [ 彈出

                //將數字統計起來
                int l = 0;
                StringBuilder ks = new StringBuilder();
                while(st.Count > 0 && int.TryParse(st.Peek(), out l))
                {
                    ks.Insert(0, st.Pop());
                }

                //將ks變成integer
                int k = int.Parse(ks.ToString());
                StringBuilder substrRepeatK = new StringBuilder();

                //substr重複k次再推入stack
                for(int j=0; j< k; j++)
                {
                    substrRepeatK.Append(substr);
                }
                st.Append(substrRepeatK.ToString());
            }
        }

        return String.Join("", st.ToArray());
    }*/
}

public class Program
{
    public static void Main(string[] args)
    {
        Solution sol = new Solution();
        //int[] res = sol.FindOrder(4, new int[4][] { new int[]{ 1, 0 }, new int[] { 2, 0 }, new int[] { 3, 1 }, new int[] { 3, 2 } });
        //foreach(int e in res)
        //{
        //    Console.WriteLine(e);
        //}

        //#692
        /*
        IList<string> res = sol.TopKFrequent(new string[] { "i", "love", "leetcode", "i", "love", "coding" }, 2);
        foreach(string s in res)
        {
            Console.WriteLine(s);
        }*/

        //string res = sol.DecodeString("2[abc]3[cd]ef");

        //Console.WriteLine(res, " ", res.Equals("abcabccdcdcdef"));
        TreeNode root = new TreeNode(3);
        root.left = new TreeNode(5);
        root.left.left = new TreeNode(6);
        root.left.right = new TreeNode(2);
        root.left.right.left = new TreeNode(7);
        root.left.right.right = new TreeNode(4);

        root.right = new TreeNode(1);
        root.right.left = new TreeNode(0);
        root.right.right = new TreeNode(8);


        StringBuilder sb = new StringBuilder();
        IList<int> res = sol.DistanceK(root, root.left, 2);
        foreach(int i in res)
        {
            sb.Append(i + " ");
        }
        Console.WriteLine(sb.ToString());

        Solution solution = new Solution();
        
        //Console.WriteLine(solution.RomanToInt("MCMXCIV") == 1994);
        //Console.WriteLine(solution.RomanToInt("XVII") == 17);

        Console.WriteLine(solution.DecodeString("2[abc]3[cd]ef").Equals("abcabccdcdef"));
    }
}