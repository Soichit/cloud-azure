using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;





namespace WebRole1
{
    public class Trie
    {
        private static TrieNode root;
        private static int hybridCapacity = 20;
        private static int levenshteinDistance = 1;

        /// constructor
        public Trie()
        {
            root = new TrieNode();
        }

        public String insert(String word)
        {
            if (word == null || word.Length == 0)
            {
                return "can't do empty";
            }
            return rearrange(root, word);
        }

        public String rearrange(TrieNode current, String tempWord)
        {
            current.HybridList.Add(tempWord);
            if (current.HybridList.Count <= hybridCapacity)
            {
                return "done";
            }
            else
            {
                foreach (String item in current.HybridList)
                {
                    //get first letter in item
                    char ch = item[0];
                    TrieNode node;

                    //check that first letter is in the dictionary key
                    if (current.Dict.ContainsKey(ch))
                    {
                        //if it does, pass that word into that node
                        node = current.Dict[ch];
                    }
                    else
                    {
                        //if it doesn't, create a new node with that letter and add the rest of the letters into the hybrid
                        node = new TrieNode();
                        current.Dict.Add(ch, node);
                    }
                    //if it's the end of the word
                    if (item.Length == 1)
                    {
                        node.SetEndOfWord(true);
                    }

                    if (item.Length >= 2)
                    {
                        rearrange(node, item.Substring(1, item.Length - 1));
                    }
                }
                current.HybridList.Clear();
                return "done";
            }
        }



        public List<String> search(String input)
        {
            List<String> result = searchPrefix(input);

            if (result.Count < 10)
            {
                result = searchMistakes(root, "", input, result);
            }
            return result;
        }


        public List<String> searchPrefix(String input)
        {
            // redundant variables
            List<String> result = new List<String>();
            TrieNode current = root;
            String tempWord = "";
            char ch = '_';
            if (input == null || input.Length == 0)
            {
                return result;
            }


            for (int i = 0; i < input.Length; i++)
            {
                foreach (String word in current.HybridList)
                {
                    String subtractedWord = input;
                    if (tempWord != "")
                    {
                        subtractedWord = input.Replace(tempWord, "");
                    }
                    if (word.StartsWith(subtractedWord))
                    {
                        result.Add(tempWord + word);
                        if (result.Count == 10)
                        {
                            return result;
                        }
                    }
                }
                ch = input[i];
                if (!current.Dict.ContainsKey(ch))
                {
                    return result;
                }
                else
                {
                    tempWord += ch;
                    current = current.Dict[ch];
                }
            }
            return searchSuggestions(current, tempWord, input, result);
        }

        private List<String> searchSuggestions(TrieNode current, String tempWord, String input, List<String> result)
        {
            if (result.Count == 10)
            {
                return result;
            }
            else
            {
                TrieNode node = current;
                if (current.EndOfWord)
                {
                    if (result.Count < 10)
                    {
                        result.Add(tempWord);
                    }
                }

                foreach (KeyValuePair<char, TrieNode> item in node.Dict)
                {
                    char ch = item.Key;
                    TrieNode nextNode = item.Value;
                    result = searchSuggestions(nextNode, tempWord + ch, input, result);
                    if (result.Count == 10)
                    {
                        return result;
                    }
                }

                foreach (String word in current.HybridList)
                {
                    result.Add(tempWord + word);
                    if (result.Count == 10)
                    {
                        return result;
                    }
                }
                return result;
            }
        }


        private List<String> searchMistakes(TrieNode current, String tempWord, String input, List<String> result)
        {
            if (result.Count == 10)
            {
                return result;
            }
            else
            {
                TrieNode node = current;
                if (current.EndOfWord)
                {
                    if (result.Count < 10)
                    {
                        if (levenshtein(tempWord, input) <= levenshteinDistance)
                        {
                            result.Add(tempWord);
                        }
                    }
                }

                foreach (KeyValuePair<char, TrieNode> item in node.Dict)
                {
                    char ch = item.Key;
                    TrieNode nextNode = item.Value;
                    result = searchMistakes(nextNode, tempWord + ch, input, result);
                    if (result.Count == 10)
                    {
                        return result;
                    }
                }

                foreach (String word in current.HybridList)
                {
                    String addedWord = tempWord + word;
                    if (levenshtein(addedWord, input) <= levenshteinDistance)
                    {
                        result.Add(addedWord);
                        if (result.Count == 10)
                        {
                            return result;
                        }
                    }
                }
                return result;
            }
        }


        private int levenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
